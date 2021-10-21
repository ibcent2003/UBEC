$(document).ready(function(){  

	$("#navigation1").navigation();
	
	$("#navigation2").navigation({
		effect: "slide"
	});
	
	$("#navigation3").navigation({
		animationOnShow: "zoom-in",
		animationOnHide: "zoom-out"
	});
	
	$("#navigation4").navigation({
		overlayColor: "rgba(233,87,63,0.8)"
	});
	
	$("#navigation5").navigation({
		hidden: true
	});
	$(".btn-show").click(function(){ 
		$("#navigation5").data("navigation").toggleOffcanvas();
	});
	
	$("#navigation6").navigation({
		offCanvasSide: "right"
	});
	
	$("#navigation7").navigation();
		
});