// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//Feedback
async function GetFeedbackData() {
    const feedbackAgree = document.getElementById("f-check");
    let success = document.getElementById("feedbackSuccess"); 
    let errEmail = document.getElementById("f-err-email");
    let errText = document.getElementById("f-err-text");
    let errAgree = document.getElementById("f-err-agree")
    if(feedbackAgree.checked){
        errAgree.textContent = "";
        const FeedbackEmail = document.getElementById("FeedbackEmail").value;
        const FeedbackText = document.getElementById("FeedbackText").value;
        
        const url = '/home/feedback';
        let data = { Email: FeedbackEmail, Text: FeedbackText};
        const response = await fetch(url, {
            method: 'POST', 
            body: JSON.stringify(data),
            headers: {
            'Content-Type': 'application/json'
            }
            
        });
        let messages = await response.json();
        if (response.ok) {   
            
            console.log(response);
            console.log(messages);
            if(messages.Success){
                errEmail.textContent = "";
                errText.textContent = "";
                errAgree.textContent = "";
                success.style.display = "block";
            }else{
                success.style.display = "none";
                errEmail.textContent = messages.ErrEmail;
                errText.textContent = messages.ErrText;
            }
        }else{
        console.log("Fail");
        }
    }else{
        errAgree.textContent = "To send data, you must agree to the processing of personal data";
    }

}
async function ClickLike(element){
    console.log(element.dataset.postId);
    let countLikes = document.getElementById("count-likes");
    const url = '/Ajax/Like';
    let data = { PostId: element.dataset.postId, Status: element.dataset.status};
    const response = await fetch(url, {
        method: 'POST', 
        body: JSON.stringify(data),
        headers: {
        'Content-Type': 'application/json'
        }
    });
    if (response.ok) {
        if(element.dataset.status == 'false'){
            element.nextElementSibling.innerHTML++;
            element.setAttribute('fill','Red');
            element.dataset.status = 'true';
        }else{
            element.nextElementSibling.innerHTML--;
            element.setAttribute('fill','DarkGray');
            element.dataset.status = 'false';
        }
    }
    
}

//add comment
async function addComment(){
    let input = document.getElementById("input-comment");
    const url = '/Ajax/AddComment';
    const data = { PostId: parseInt(input.dataset.postId), Text: input.value};
    const response = await fetch(url, {
        method: 'POST', 
        body: JSON.stringify(data),
        headers: {
        'Content-Type': 'application/json'
        }
    });
    if (response.ok) {
        alert("Ok");
    }
}
//getComment
async function getComments(){
    let commentsBlock = document.getElementById('comment-block');
    const url = '/Ajax/GetComments';
    const data = parseInt(commentsBlock.dataset.postId);
    const response = await fetch(url, {
        method: 'POST', 
        body: JSON.stringify(data),
        headers: {
        'Content-Type': 'application/json'
        }
    });
    let comments = await response.json();
    if (response.ok) {
        return comments;
    }
    return false;
}
//view comments
async function viewComments(){
    let commentsBlock = document.getElementById('comment-block');
    let helpAnimation = document.getElementById('help-animation');
    let buttonView = document.getElementById('button-view');
    buttonView.remove();
    helpAnimation.style.height = 0;
    //comments.insertAdjacentElement('afterbegin', buttonView);
    commentsBlock.insertAdjacentHTML('afterbegin','<h5 class = "ml-1">Comments</h5>');
    commentsBlock.insertAdjacentHTML('beforeend','<div class="input-group"><input type="text" id="input-comment" class="form-control" placeholder="Your comment" data-post-id="'+commentsBlock.dataset.postId+'"> <div class="input-group-append"><button class="btn btn-success" type="button" id="add-comment" onclick="addComment()"> Add </button> </div> </div>');
    let comments = await getComments();
    if(comments.length > 0){
        console.log(comments);
        let commentsArea = document.createElement('div');
        
        commentsArea.id = "comments";
        for(i = 0; i < comments.length; i++){
            let comment = document.createElement('div');
            let userName = document.createElement('a');
            let text = document.createElement('div');
            let line = document.createElement('hr');
            text.className = "comment-text";
            line.className = "mx-2";
            comment.id = "comment";
            comment.className = "ml-4"
            userName.innerHTML = comments[i].userName;
            text.innerHTML = comments[i].text;
            comment.append(userName);
            comment.append(text);
            commentsArea.append(line);
            commentsArea.append(comment);
        }
        commentsBlock.append(commentsArea);
        animate({
            duration: 700,
            timing(timeFraction) {
              return Math.pow(timeFraction, 2);
            },
            draw(progress) {
                helpAnimation.style.height = progress * commentsBlock.offsetHeight + 'px';
            }
          });
          //commentsBlock.style.height = 0;
          console.log(commentsBlock.offsetHeight);
    }else{
        console.log("Нету");
    }
}

//animation
function animate(options) {

    var start = performance.now();
  
    requestAnimationFrame(function animate(time) {
      
      var timeFraction = (time - start) / options.duration;
      if (timeFraction > 1) timeFraction = 1;
  
      
      var progress = options.timing(timeFraction)
      
      options.draw(progress);
  
      if (timeFraction < 1) {
        requestAnimationFrame(animate);
      }
  
    });
  }