# project101
Release version of Computer-aided Diagnosis System for Thyroid Nodule Malignancy Stratification

This is an application to diagnose a thyroid nodule malignancy based on 5 characteristics:
1. Margin, then divided into 2 classes: smooth and irregular
2. Shape, then divided into 2 classes: round-to-oval and irregular
3. Orientation, then divided into 2 classes: parallel and non-parallel
4. Echogenity, then divided into 4 classes: anechoic, isoechoic, hypoechoic, and markedly hypoechoic
5. Composition, then divided into 3 classes: solid, complex, and cystic

Each class has their own 'points of malignancy'. 
Every input of thyroid nodule's US image is evaluated to find its characteristics, and the accumulated points of malignancy will be converted into percentage which defines its malignancy.


This process is done by digital image processing, consisting of following steps:
1. Pre-processing: input image enhancement.

  a. Adaptive median filter - to remove US label/marking.
  
  b. Bilateral filter - to smoothen image for better result in segmentation process.
  
2. Feature Extraction: the image's features that defines the nodule's characteristics.

  a. External features (margin, shape, and orientation) include:
  
    1) Geometric features.
    2) Invariant moment.
    3) Zernike's moment.
    
  b. Internal features (echogenity and composition) include:
  
    1) Histogram features.
    2) Law's features.
    3) Texture features.
    
3. Classification: to classify nodule's classes to each characteristics.

  a. Support Vector Machine (SVM) to classify external characteristics.
  
  b. Multi-layer Perceptron (MLP) to classify internal characteristics.
  
  
