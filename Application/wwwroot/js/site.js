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
            element.setAttribute('fill','Red');
            element.dataset.status = 'true';
        }else{
            element.setAttribute('fill','DarkGray');
            element.dataset.status = 'false';
        }
    }
    
}