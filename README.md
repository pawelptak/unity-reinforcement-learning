# Unity Reinforcement Learning

Development of a simple computer game that uses reinforcement learning to teach an agent progressing through the environment.

## Getting Started

### Dependencies
* Python 3.8
* PyTorch 1.7.1 
* ML-agents 0.28.0 
* Unity Editor 2020.3.30r1 with ML Agents 2.0.1 package

### Installing

* Clone the repository
* Create en empty Unity 3D project
* Move the repository files to the project directory
* Add one of the existing scenes from **Assets/Scenes** to the project Hierarchy
* Install Python 3.8
* In the project directory open terminal and create a virtual environment: 
```
python -m venv venv
```
* Activate the environment:
```
venv\Scripts\activate
```
* Install PyTorch 1.7.1
```
pip3 install torch~=1.7.1 -f https://download.pytorch.org/whl/torch_stable.html
```
* Install ML-agents 0.28.0
```
pip install mlagents==0.28.0
```
* Launch the Unity project > **Window** > **Package Manager** > **Unity Registry** > Search for **ML Agents** package > Install **ML Agents 2.0.1**

### Executing program
To use an existing ML model:
* In Unity select the **RollerAgent** object
* In the **Behavour Parameters** module select a model from the **Assets/ML-Models** directory
* Press **Play** in Unity Editor

To train a new model:
* In Unity select the **RollerAgent** object
* In the **Behavour Parameters** module set the **Behavour Type** to **Default**
* Open terminal in the project directory and activate the virtual environment:
```
venv\Scripts\activate
```
* Train a new model:
```
mlagents-learn <path to config file, e.g. config/new_config.yaml> --run-id=<unique name, e.g. run1>
```
* Press **Play** in Unity Editor
* The model file will be saved in **results/_run-id_** directory with **.onnx** extension

## Help

[ML-Agents docummentation](https://github.com/Unity-Technologies/ml-agents/tree/release_19_docs/docs)
