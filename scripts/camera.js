var isWindowLoaded = false;
var canvas = null;
var context = null;
var video = null;
const refreshRate = 200;

window.onload = () => { isWindowLoaded = true; };

// Make it runnable via Blazor
window.startVideoOnloaded = () => {
    if (!isWindowLoaded) {
        console.log("On load");
        window.onload = () => { startVideo(); };
    } else {
        console.log("Start video");
        startVideo();
    }
}

window.getFrame = () => {
    return getFrameBase64();
};

function startVideo() {
    console.log('Get canvas');

    canvas = document.querySelector('.canvas');
    console.log(canvas);
    context = canvas.getContext('2d');
    console.log(context);
    video = document.querySelector('.player');
    console.log(video);

    getVideo();
}

function stopVideo() {
    if (!!video && !!video.srcObject) {
        video.srcObject.getTracks().forEach(function (track) {
            track.stop();
        });
        video.srcObject = null;
    }
}

const getFrameBase64 = () => {
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    context.drawImage(video, 0, 0);
    const data = canvas.toDataURL('image/png');
    console.log(data);
    return data;
}

const getFrameBlob = () => {
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    context.drawImage(video, 0, 0);
    const data = toBlob(canvas);
    console.log(data);
    return data;
}

function getVideo() {
    navigator.mediaDevices.getUserMedia({ video: true, audio: false })
        .then(webCam => {
            video.srcObject = webCam;
        })
        .catch(err => {
            console.error('Oh no, you denied the webcam, no fun for you.', err)
        })
}

function toBlobPromise(canvas) {
    return new Promise(function (resolve) {
        var binaryString = window.atob(canvas.toDataURL().split(",")[1]);
        var length = binaryString.length;
        var binaryArray = new Uint8Array(length);

        for (var i = 0; i < length; i++) binaryArray[i] = binaryString.charCodeAt(i);

        resolve(
            new Blob([binaryArray], {
                type: "image/png"
            })
        );
    });
}

function toBlob(canvas) {
    var binaryString = window.atob(canvas.toDataURL().split(",")[1]);
    var length = binaryString.length;
    var binaryArray = new Uint8Array(length);

    for (var i = 0; i < length; i++) binaryArray[i] = binaryString.charCodeAt(i);

    return new Blob([binaryArray], {
        type: "image/png"
    });
}
