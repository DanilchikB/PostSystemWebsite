

var cropper=null;
const imageArea = document.getElementById('image-area');
const buttonAddPhoto = document.getElementById('buttonAddAvatar');
const imageAvatar = document.getElementById('imgAvatar');
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
  
  cropper = new Cropper(image, {
    aspectRatio: 1,
    viewMode: 2,
    initialAspectRatio: 1,
    background: false,
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
          $('#modalAddPhoto').modal('hide');
          let path = imageAvatar.src.split('?');
          imageAvatar.src = path[0] + '?' + new Date().getTime();
          $('#popupMiniMessage').toast('show');
        }
    });
  }
}
