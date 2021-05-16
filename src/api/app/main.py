import os
os.environ["CUDA_DEVICE_ORDER"] = "PCI_BUS_ID"   # see issue #152
os.environ["CUDA_VISIBLE_DEVICES"] = ""
from flask import Flask, flash, request, redirect, url_for, session, send_file
from PIL import Image
from keras.models import load_model
from matplotlib import pyplot
from numpy.random import randn
import numpy as np

# SLERP + Linear Interpolation
from numpy import arccos
from numpy import clip
from numpy import dot
from numpy import sin
from numpy import linspace
from numpy.linalg import norm

ALLOWED_EXTENSIONS = {'wav'}
MODELS_FOLDER = './data/models/'
EMOJIS_FOLDER = './data/emojis/'

app = Flask(__name__)
app.config['MODELS_FOLDER'] = MODELS_FOLDER
app.config['EMOJIS_FOLDER'] = EMOJIS_FOLDER
app.config['LATENT_DIM'] = 100

def generate_latent_points(latent_dim, n_samples):
    # Generate points in latent space
    x_input = randn(latent_dim * n_samples)
    # Reshape into a batch of inputs for the network
    x_input = x_input.reshape(n_samples, latent_dim)
    return x_input

def save_emojis(directory, emojis, n_images):
    # scale from [-1,1] to [0,1]
    emojis = (emojis + 1) / 2.0

    for i in range(n_images):
        pyplot.subplot(n_images, n_images, 1 + i)
        pyplot.axis('off')
        pyplot.imshow(emojis[i])
        
    pyplot.savefig(os.path.join(directory, 'pyplot.png'))
    pyplot.close()

    # Save individual emojis
    # https://stackoverflow.com/questions/10965417/how-to-convert-a-numpy-array-to-pil-image-applying-matplotlib-colormap
    for i in range(n_images):
        img = Image.fromarray(np.uint8(emojis[i] * 255), 'RGB')
        img.save(os.path.join(directory, str(i) + '.png'))

def save_morphs(directory, morphs, n_images):
    # scale from [-1,1] to [0,1]
    morphs = (morphs + 1) / 2.0

    fig, ax = pyplot.subplots(1, n_images)
    for i, ax in enumerate(ax):
        ax.axis('off')
        ax.imshow(morphs[i])

    fig.tight_layout()
    pyplot.savefig(os.path.join(directory, 'morphs.png'), transparent=True, bbox_inches='tight', pad_inches=0)
    pyplot.close()

    # Save individual emojis
    # https://stackoverflow.com/questions/10965417/how-to-convert-a-numpy-array-to-pil-image-applying-matplotlib-colormap
    for i in range(n_images):
        img = Image.fromarray(np.uint8(morphs[i] * 255), 'RGB')
        img.save(os.path.join(directory, str(i) + '.png'))

def allowed_file(filename):
    return '.' in filename and \
           filename.rsplit('.', 1)[1].lower() in ALLOWED_EXTENSIONS

@app.route('/', methods=['GET'])
def home():
    return '''
    <!doctype html>
    <html>
    <head>
        <title>SpeechBrain API - Tests</title>
    </head>
    <body>
        <h1>SpeechBrain API - Tests</h1>
        <p>Upload a voice recording and get the transcript from SpeechBrain.</p>
        <form action="/transcribe" method="post" enctype="multipart/form-data">
           <p>
              <label for="fileVoice">Local File (.wav): </label><br />
              <input type="file" name="fileVoice">
           </p>
           <p>
              <input type="submit" value="Upload">
           </p>
        </form>
    </body>
    </html>
    '''

@app.route('/generate', methods=['POST'])
def generateEmojis():
    n_emojis = 53
    if request.method == 'POST':
        # Ensure that the form value exists.
        if not request.form['id']:
            flash('No "id" found in the request...')
            return redirect(request.url)

        id = request.form['id'];
        unique_folder = os.path.join(app.config['EMOJIS_FOLDER'], id + '/')
        if (not os.path.exists(unique_folder)):
            os.makedirs(unique_folder)

        g_model = load_model(os.path.join(app.config['MODELS_FOLDER'], 'e075_generator.h5'))
        
        # Make sure that the data directories exist
        if (not os.path.exists(app.config['EMOJIS_FOLDER'])):
            os.makedirs(app.config['EMOJIS_FOLDER'])

        # Generate points in latent space
        points = generate_latent_points(app.config['LATENT_DIM'], n_emojis)
        print('Points: ', points.shape)

        # Predict outputs
        emojis = g_model.predict(points)
        print('Emojis: ', emojis.shape)

        # Save the emojis
        save_emojis(unique_folder, emojis, n_emojis)

        for i in range(n_emojis):
            np.save(os.path.join(unique_folder, str(i) + '.npy'), points[i])

        return "Complété!"

    return "PAS COMPRENDU DSL"

# spherical linear interpolation (slerp)
def slerp(val, low, high):
	omega = arccos(clip(dot(low/norm(low), high/norm(high)), -1, 1))
	so = sin(omega)
	if so == 0:
		# L'Hopital's rule/LERP
		return (1.0-val) * low + val * high
	return sin((1.0-val)*omega) / so * low + sin(val*omega) / so * high

# uniform interpolation between two points in latent space
def interpolate_points(p1, p2, n_steps=10):
	# interpolate ratios between the points
	ratios = linspace(0, 1, num=n_steps)
	# linear interpolate vectors
	vectors = list()
	for ratio in ratios:
		v = slerp(ratio, p1, p2)
		vectors.append(v)
	return np.asarray(vectors)

@app.route('/interpolate', methods=['POST'])
def interpolate():
    n_morphs = 10
    if request.method == 'POST':
        # Ensure that the form value exists
        if not request.form['id']:
            flash('No "id" found in the request...')
            return redirect(request.url)
        if not request.form['firstId']:
            flash('No "firstId" found in the request...')
            return redirect(request.url)
        if not request.form['secondId']:
            flash('No "secondId" found in the request...')
            return redirect(request.url)
        
        id = request.form['id'];
        first_id = request.form['firstId'];
        second_id = request.form['secondId'];

        # Ensure that the unique emojis folder exists
        unique_folder = os.path.join(app.config['EMOJIS_FOLDER'], id + '/')
        if (not os.path.exists(unique_folder)):
            flash('The emojis folder for id [' + id + '] doesn\' exist...')
            return redirect(request.url)
        
        first_emoji = np.load(os.path.join(unique_folder, first_id + '.npy'))
        second_emoji = np.load(os.path.join(unique_folder, second_id + '.npy'))
        
        print('first_emoji: ', first_emoji.shape)
        print('second_emoji: ', second_emoji.shape)

        g_model = load_model(os.path.join(app.config['MODELS_FOLDER'], 'e075_generator.h5'))
        
        # interpolate points in latent space
        interpolated = interpolate_points(first_emoji, second_emoji, n_morphs)

        # generate images
        morphs = g_model.predict(interpolated)

        unique_folder = os.path.join(unique_folder, 'morphs')
        if (not os.path.exists(unique_folder)):
            os.makedirs(unique_folder)

        # Save the emojis
        save_morphs(unique_folder, morphs, n_morphs)

        return "Complété!"

    return "PAS COMPRENDU DSL"

if __name__ == "__main__":
    # Only for debugging while developing
    #app.run(host='0.0.0.0', debug=True, port=80)
    app.run()