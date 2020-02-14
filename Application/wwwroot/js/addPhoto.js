

var cropper=null;
const imageArea = document.getElementById('image-area');
const buttonAddPhoto = document.getElementById('buttonAddAvatar');
/*event*/
$('#modalAddPhoto').on('hide.bs.modal', deleteImageArea);
/*end event*/
function addPhoto(element){
    const image = document.createElement('img');
    let reader  = new FileReader();
    /*while(imageArea.firstChild){
      imageArea.removeChild(imageArea.firstChild);
    }*/
    //imageArea.firstChild.remove();
    image.style.maxWidth = "100%";
    image.style.maxHeight = "750px";
    reader.onloadend = function () {
        deleteImageArea();
        image.src = reader.result;
        imageArea.append(image);
        viewCropper(image);
        buttonAddPhoto.style.display="block";
    }
    if (element[0]) {
        reader.readAsDataURL(element[0]);
    }
}

function viewCropper(image){
  
  console.log(cropper)
  cropper = new Cropper(image, {
    aspectRatio: 1,
    viewMode: 2,
    initialAspectRatio: 1,
    background: false,
    
    crop(event) {
      console.log(event.detail.x);
    },
  });

}

function deleteImageArea(){
  if(cropper!=null){
    cropper.destroy();
    while(imageArea.firstChild){
    imageArea.removeChild(imageArea.firstChild);
    }
    cropper = null;
    buttonAddPhoto.style.display="none";
  }
  console.log(window)
}

function addPhotoOnServer(){
  if(cropper!=null){
      cropper.getCroppedCanvas().toBlob(async (blob) => {
        const url = '/Ajax/AddPhoto';
        const formData = new FormData();
        formData.append('Image', blob);
        const response = await fetch(url, {
          method: 'POST', 
          body: formData
        });
        if (response.ok) {
            console.log('Успех');
        }
    });
  }
}
