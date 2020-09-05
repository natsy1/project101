# project101
> Release version of Computer-aided Diagnosis System for Thyroid Nodule Malignancy Stratification


## Outline
This is an application to help diagnosing a thyroid nodule malignancy using machine learning and image processing based on 5 characteristics:
- **Margin**, divided into 2 classes: smooth and irregular
- **Shape**, divided into 2 classes: round-to-oval and irregular
- **Orientation**, divided into 2 classes: parallel and non-parallel
- **Echogenity**, divided into 4 classes: anechoic, isoechoic, hypoechoic, and markedly hypoechoic
- **Composition**, divided into 3 classes: solid, complex, and cystic

## How it works

Each class has its own _points of malignancy'_. Every input of thyroid nodule's US image is evaluated to find its characteristics, and the accumulated points of malignancy will be converted into percentage which defines its malignancy.

This process is done by digital image processing, consisting of following steps:
1. **Pre-processing**: input image enhancement.
   - `Adaptive median filter` - to remove US label/marking.
   - `Bilateral filter` - to smoothen image for better result in segmentation process.
2. **Feature Extraction**: the image's features that defines the nodule's characteristics.
   - External features (margin, shape, and orientation) includes:
			  - `Geometric features`
			  - `Invariant moment`
			  - `Zernike's moment`
	- Internal features (echogenity and composition) includes:
				- `Histogram features`
				- `Law's features`
				- `Texture features`
3. **Classification**: to classify nodule's classes to each characteristics.
   - `Support Vector Machine` algorithm to classify external characteristics.
   - `Multi-layer Perceptron` algorithm to classify internal characteristics.
