FROM tiangolo/uwsgi-nginx-flask:python3.8
WORKDIR /app

# Update the existing packages, install dependencies and clean-up lists.
RUN apt-get update \
    #&& apt-get install -y gnupg2 \
         #apt-utils \
    && rm -rf /var/lib/apt/lists/*

# Install the 'pip' packages and purge the cache.
RUN pip install --upgrade pip \
    && pip install tensorflow \
        keras \
        numpy \
        matplotlib \
        pillow \
    && pip cache purge

# Preload the SpeechBrain models.
RUN python -c "import tensorflow as tf; print('Num GPUs Available: ', len(tf.config.list_physical_devices('GPU')));"

COPY src/api/app .