"use strict";
$(function(){
	$('.nav-item').click(function(){
		// reset active class
		$('.nav-item').removeClass("active");
		// add active class to selected
		$(this).addClass("active");
		// return needed to make function work
		return false;
	});
	
	
	$(function() {
		// create an empty variable
		var selectedClass = "";
		// call function when item is clicked
		$(".nav-item").click(function(){
			// assigns class to selected item
			selectedClass = $(this).attr("data-rel");
			// fades out all portfolio items
			$(".portfolio li").fadeOut(300);
			// fades in selected category
			$(".portfolio li." + selectedClass).delay(300).fadeIn(300);
		});
	});
  
}); // document ready

