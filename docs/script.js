var file = document.getElementById('file');
var message = document.getElementById('message');
var canvas = document.getElementById('canvas');
var xhr;

var showResult = (result, file) => {
    if (typeof result === 'string') {
        canvas.style.display = 'none';
        message.innerText = result;
        return;
    }

    message.innerText = 'Detected ' + result.length + ' face/s';

    alert(file);
    alert(createImageBitmap);
    try {
    createImageBitmap(file).then(b => {
        message.innerText += 'created';
        
        const max = 700;
        var width = b.width;
        var height = b.height;
        if (b.width > max || b.height > max) {
            if (b.width > b.height) {
                width = max;
                height = b.height * width / b.width;
            } else {
                height = max;
                width = b.width * height / b.height;
            }
        }
        canvas.width = width;
        canvas.height = height;
        
        message.innerText += ' canvas ' + width + ' ' + height;

        var context = canvas.getContext('2d');
        context.clearRect(0, 0, width, height);
        context.drawImage(b, 0, 0, width, height);
        context.lineWidth = 2;
        context.strokeStyle = 'red';
        
        message.innerText += ' draw';

        context.scale(width / b.width, height / b.height);
        result
            .filter(r => r.confidence > 0.5)
            .forEach(r => {
            const p = r.rectangle.split(', ', 4);
            context.strokeRect(p[0], p[1], p[2], p[3]);
        });
    });
    } catch (err) {
        alert(err);
    }

    canvas.style.display = 'inline';
}

var uploadFile = (file) => {
    showResult([], file);
    return;
    showResult('Uploading...');

    if (xhr) {
        xhr.abort();
    }
    xhr = new XMLHttpRequest();
    xhr.addEventListener('load', e => {
        if (xhr.status === 200) {
            var result = JSON.parse(xhr.responseText);
            showResult(result, file);
        } else if (xhr.status === 429) {
            showResult('Too many requests. Please retry in few minutes');
        } else {
            showResult('Unexpected result from server');
        }
        xhr = undefined;
    }), false;
    xhr.addEventListener('error', e => {
        showResult('Cannot upload the image');
        xhr = undefined;
    }, false);
    xhr.open('POST', 'https://libfacedetectionnet.azurewebsites.net/api/Detect');
    xhr.send(file);
};

file.addEventListener('change', e => {
    if (file.files.length > 0) {
        // Max 1 MB
        if (file.files.size > 1024 * 1024) {
            showResult('Image size too big')
            return;
        }

        uploadFile(file.files[0]);
        file.value = null;
    }
});
