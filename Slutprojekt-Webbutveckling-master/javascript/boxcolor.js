$('.project-box').each(function(index, element) {
	var hue = Math.floor(Math.random() * 360);	
	var pastel = 'hsl(' + hue + ', 100%, 87.5%)';
    $(this).css("background-color", pastel);
});
$('.project-box').each(function(index, element) {
	$(this).hover(function(){
		$(this).animate({opacity: 1}, 300);	
	},function(){
		$(this).animate({opacity: 0.5}, 300)
	})
});
