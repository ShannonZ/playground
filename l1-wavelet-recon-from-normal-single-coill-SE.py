# -*- coding: utf-8 -*-
"""
Created on Tue Jul  2 16:10:00 2019

@author: Administrator
"""

# -*- coding: utf-8 -*-
"""
Created on Mon Jul  1 12:59:09 2019

@author: Administrator
"""
import numpy as np
import sigpy as sp
import sigpy.mri as mr
import sigpy.plot as pl


ksp = np.load('ksp.npy')
#ksp = np.load('data/cartesian_ksp.npy')
#ksp = ksp[0,:,:]
img_shape = ksp.shape

#%%
F = sp.linop.FFT(ksp.shape, axes=(0,1))
pl.ImagePlot(F.H * ksp,  title=r'$F^H y$')

#%%
mask = abs(ksp)>0
pl.ImagePlot(mask, title='Sampling Mask')

P = sp.linop.Multiply(ksp.shape, mask)
#%%
W = sp.linop.Wavelet(img_shape)
wav = W  * F.H * ksp
pl.ImagePlot(wav**0.1, title=r'$W S^H F^H y$')
#%%
A = P * F * W.H

lamda = 0.005
proxg = sp.prox.L1Reg(wav.shape, lamda)
alpha = 1
wav_thresh = proxg(alpha, wav)

pl.ImagePlot(wav_thresh**0.1)

max_iter = 100
alpha = 1

def gradf(x):
    return A.H * (A * x - ksp)

wav_hat = np.zeros(wav.shape, np.complex)
alg = sp.alg.GradientMethod(gradf, wav_hat, alpha, proxg=proxg, max_iter=max_iter)

while not alg.done():
    alg.update()
    print('\rL1WaveletRecon, Iteration={}'.format(alg.iter), end='')

pl.ImagePlot(W.H(wav_hat))