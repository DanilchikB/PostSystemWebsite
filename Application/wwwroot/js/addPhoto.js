//*****modify ******/
function addPhoto(element){
    
    let imageArea = document.getElementById('image-area');
    let image = document.createElement('img');
    let reader  = new FileReader();
    /*while(imageArea.firstChild){
      imageArea.removeChild(imageArea.firstChild);
    }*/
    //imageArea.firstChild.remove();
    image.style.maxWidth = "100%";
    reader.onloadend = function () {
        image.src = reader.result;
        imageArea.append(image);
        cropper(image);
        
    }
    if (element[0]) {
        reader.readAsDataURL(element[0]);
    }
}

function cropper(image){
  Cropper.noConflict();
  let cropperValue;
  console.log(cropperValue)
  cropperValue = new Cropper(image, {
    aspectRatio: 1,
    viewMode: 0,
    initialAspectRatio: 1,
    background: true,
    setData:{
      width:100,
      height: 100
    },
    crop(event) {

    },
  });

}

function deleteImageArea(cropperValue){
  let imageArea = document.getElementById('image-area');
  cropperValue.destroy();
  while(imageArea.firstChild){
    imageArea.removeChild(imageArea.firstChild);
  }
}
//*****modify end******/