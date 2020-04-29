 <style>
        #form {
            background: white;
            border-radius: 5px;
            border: 2px dashed #b5e853;
            border-image: none;
            max-width: 500px;
            margin-left: auto;
            margin-right: auto;
            text-align: center;
        }

        #container {
            text-align: center;
            margin-left: auto;
            margin-right: auto;
        }

        #canvas {
            display: none;
        }
    </style>
    <div id="form">
        <p>Choose image to upload <i>Max 1MB</i></p>
        <input type="file" id="file" accept="image/*" />
        <p>Image will not be stored!</p>
    </div>
    <div id="container">
        <h4 id="message"></h4>
        <canvas id="canvas">

        </canvas>
    </div>
