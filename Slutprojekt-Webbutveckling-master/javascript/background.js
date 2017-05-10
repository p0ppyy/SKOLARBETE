var date = new Date();
var time = date.getSeconds();
$(document).ready(function(e) {
	console.log(time);
	if(time % 2 > 0){
		$("body").css("background-image", "url('bg1.png')" );	
	}else{
		$("body").css("background-image", "url('bg2.png')");
	}
});