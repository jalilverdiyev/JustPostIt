// Special thanks to https://codepen.io/mahtab-alam for photo upload component

let formSubmit = document.getElementById('Register-form');
formSubmit.addEventListener('submit',e =>{
    e.preventDefault();
    let image = document.getElementById('upload_file');
    let username = document.getElementById('username');
    let email = document.getElementById('email');
    let pass = document.getElementById('Pass');
    const formData = new FormData();
    formData.append('Username',username.value);
    formData.append('Email',email.value);
    formData.append('Password',pass.value);
    formData.append('ProfilePhoto',image.files[0]);
    $.ajax({
        url:"https://justpostitapi.azurewebsites.net/api/Authentication/Register",
        type:'POST',
        data: formData,
        contentType:false,
        processData:false,
    }).done(function(response){
        console.log(response);
        if(response){
            window.location.href = "https://justpostit.azurewebsites.net/login";
        }
        else{
            let popup = document.getElementById('popup');
            popup.style.display = 'block';
            let alert = document.createElement('div');
            alert.classList.add('alert');
            alert.classList.add('alert-danger');
            alert.classList.add('d-flex');
            alert.classList.add('flex-justify-between');
            alert.innerText = "There is a user with this e-mail!";
            let x = document.createElement('span');
            x.innerText = 'X';
            x.style.cursor = 'pointer';
            x.addEventListener('click',function (){
                document.getElementById('popup').style.display = 'none';
            });
            alert.appendChild(x);
            popup.appendChild(alert);
        }
    })

});

let btnUpload = $("#upload_file"),
    btnOuter = $(".button_outer");
btnUpload.on("change", function(e){
    let ext = btnUpload.val().split('.').pop().toLowerCase();
    if($.inArray(ext, ['gif','png','jpg','jpeg']) === -1) {
        $(".error_msg").text("Not an Image...");
    } else {
        $(".error_msg").text("");
        btnOuter.addClass("file_uploading");
        setTimeout(function(){
            btnOuter.addClass("file_uploaded");
        },3000);
        let uploadedFile = URL.createObjectURL(e.target.files[0]);
        setTimeout(function(){
            $("#uploaded_view").append('<img alt="" src="'+uploadedFile+'" />').addClass("show");
        },3500);
    }
});
$(".file_remove").on("click", function(e){
    let view = $("#uploaded_view"); 
    view.removeClass("show");
    view.find("img").remove();
    btnOuter.removeClass("file_uploading");
    btnOuter.removeClass("file_uploaded");
});