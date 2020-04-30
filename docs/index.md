<style>
    #form {
        background: white;
        border-radius: 5px;
        border: 2px dashed #2268b2;
        border-image: none;
        max-width: 500px;
        margin-left: auto;
        margin-right: auto;
        text-align: center;
    }

    #container {
        #text-align: center;
        #margin-left: auto;
        #margin-right: auto;
    }

    #canvas {
        #display: none;
    }
    
    .privacy {
        font-size: 12px;
    }
</style>

## Live demo
<div id="form">
    <p>Choose image to upload <i>Max 1MB</i></p>
    <input type="file" id="file" accept="image/*" />
    <p class="privacy">Image will not be stored!</p>
</div>
<div id="container">
    <h4 id="message"></h4>
    <canvas id="canvas">

    </canvas>
</div>
<script src="script.js"></script>
