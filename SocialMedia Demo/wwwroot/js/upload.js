// Special thanks to https://codepen.io/mahtab-alam for photo upload component

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