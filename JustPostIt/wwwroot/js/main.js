function confirm(){
	let confirmPassInput = document.getElementById("confirmPass").value
	let passInput = document.getElementById("Pass").value
	if(passInput === "" || passInput == null)
	{
		document.getElementById("msgAlert").style.display = "none";
	}
	else if(confirmPassInput !== passInput)
	{
		document.getElementById("msgAlert").style.display = "block";
		document.getElementById("btnSubmit").disabled = true;
	}
	else
	{
		document.getElementById("msgAlert").style.display = "none";
		document.getElementById("btnSubmit").disabled = false;
	}
}
function activeCheck() {
	let posts = document.getElementById("posts");
	let friends_posts = document.getElementById("friend's_posts");
	let people = document.getElementById("people");
	// let friends = document.getElementById("navbarDropdown");
	switch (window.location.pathname) {
		case "/":
			posts.style.borderBottom = "3px solid #0e73a7";
			friends_posts.style.borderBottom = "none";
			people.style.borderBottom = "none";
			// friends.style.borderBottom = "none";
			break;
		case "/Post/FriendsPosts":
			posts.style.borderBottom = "none";
			friends_posts.style.borderBottom = "3px solid #0e73a7";
			people.style.borderBottom = "none";
			// friends.style.borderBottom = "none";
			break;
		case "/Person/People":
			posts.style.borderBottom = "none";
			friends_posts.style.borderBottom = "none";
			people.style.borderBottom = "3px solid #0e73a7";
			// friends.style.borderBottom = "none";
			break;
		case "/Person/Friends":
			posts.style.borderBottom = "none";
			friends_posts.style.borderBottom = "none";
			people.style.borderBottom = "none";
			// friends.style.borderBottom = "3px solid #0e73a7";
			break;
		default:
			break;
	}
}

//Ajax requests
function addFriend(id,name,url)
{
	// let profile_photo = document.getElementById(id); after profile photo added
		$.post(url, {
			name:name,
			PersonId:id,
			status: 1
		},function (response){
			$('.popup').html(response);
			document.getElementById('popup').style.display = "block";		
		});
}

function updateFriend(id,name,url,action)
{
	let status = action === 'accept' ? 2 : 3; 
	$.post(url,{
		name : name,
		PersonId: id,
		status: status
	},function(response){
		$('.popup').html(response);
		document.getElementById('popup').style.display = "block";
	});
}

function getFriendPosts(id,url){
	$.post(url,{
		id: id
	},function(response){
		$('.contentpage').html(response);	
	})
}

//Ui matters :)
function hidealert(){
	document.getElementById('popup').style.display = "none";
}
