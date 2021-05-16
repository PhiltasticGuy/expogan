# Import Libraries
import pandas as pd
import scipy.sparse as sparse
import numpy as np
from scipy.sparse.linalg import spsolve
import random
import pickle5
import warnings
from sklearn.metrics.pairwise import cosine_similarity
from sklearn.feature_extraction.text import TfidfVectorizer
import os
import pandas as pd

# path ='./data'

# files = os.listdir(path)
# i = 0
# for index,file in enumerate(files):
# 	os.rename(os.path.join(path, file), os.path.join(path, ''.join([str(i), '.jpg'])))
# 	i+=1


def recommendation(imid, table, model):
	idx = table.index[table['image_id'] == imid][0]

	cos_sim = cosine_similarity(model[idx], model).flatten()

	ls_top = sorted(list(enumerate(cos_sim)), key = lambda x: x[1], reverse = True)[1:6]

	res = {'result': [{'id':int(table.iloc[t_vect[0]][0]),'path':table.iloc[t_vect[0]][5] ,'confidence': round(t_vect[1],1)} for t_vect in ls_top]}
	return res


# table['duplicate_type'] = table['type']

# table_new = table.copy()
# table_new.drop(columns =['path','rating'],inplace = True)

# table_new = table_new.astype(str)
# print(table_new.info())
# corpus = table_new.apply(' '.join, axis = 1)
# print(corpus)

# tfidf_vectorizer_params = TfidfVectorizer(lowercase = True, stop_words ='english', ngram_range = (1,3),max_df = 0.5)

# tfidf_vectorizer = tfidf_vectorizer_params.fit_transform(corpus)

# check = pd.DataFrame(tfidf_vectorizer.toarray(), columns = tfidf_vectorizer_params.get_feature_names())
# print(check.head(5))
# print(check.info())

# vec = cosine_similarity(tfidf_vectorizer,tfidf_vectorizer)
# check = pd.DataFrame(data = vec, index = table['image_id'].astype(str), columns = table['image_id']).head(5)
# print(check.head())
# print(check.info())

