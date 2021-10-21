var revapi27,
	tpj=jQuery;
			
tpj(document).ready(function() {
	if(tpj("#rev_slider_27_1").revolution == undefined){
		revslider_showDoubleJqueryError("#rev_slider_27_1");
	}else{
		revapi27 = tpj("#rev_slider_27_1").show().revolution({
			sliderType:"standard",
			jsFileLocation:"//tpserver.local/R_5452/wp-content/plugins/revslider/public/assets/js/",
			sliderLayout:"fullscreen",
			dottedOverlay:"none",
			delay:9000,
			navigation: {
				keyboardNavigation:"off",
				keyboard_direction: "horizontal",
				mouseScrollNavigation:"off",
 							mouseScrollReverse:"default",
				onHoverStop:"off",
				touch:{
					touchenabled:"on",
					touchOnDesktop:"off",
					swipe_threshold: 75,
					swipe_min_touches: 1,
					swipe_direction: "horizontal",
					drag_block_vertical: false
				}
			},
			responsiveLevels:[1240,1024,778,480],
			visibilityLevels:[1240,1024,778,480],
			gridwidth:[1240,1024,778,480],
			gridheight:[868,768,960,720],
			lazyType:"none",
			scrolleffect: {
				blur:"on",
				maxblur:"20",
				on_slidebg:"on",
				direction:"top",
				tilt:"10",
				disable_on_mobile:"off",
			},
			parallax: {
				type:"scroll",
				origo:"enterpoint",
				speed:400,
				speedbg:1000,
				speedls:0,
				levels:[5,10,15,20,25,30,35,40,45,46,47,48,49,50,51,55],
			},
			shadow:0,
			spinner:"off",
			stopLoop:"on",
			stopAfterLoops:0,
			stopAtSlide:1,
			shuffle:"off",
			autoHeight:"off",
			fullScreenAutoWidth:"off",
			fullScreenAlignForce:"off",
			fullScreenOffsetContainer: "",
			fullScreenOffset: "60px",
			disableProgressBar:"on",
			hideThumbsOnMobile:"off",
			hideSliderAtLimit:0,
			hideCaptionAtLimit:0,
			hideAllCaptionAtLilmit:0,
			debugMode:false,
			fallbacks: {
				simplifyAll:"off",
				nextSlideOnWindowFocus:"off",
				disableFocusListener:false,
			}
		});
		window.updateWeather();
	}
				/*	jQuery.ajax({
				url : 'http://builder.themepunch.com/wp-admin/admin-ajax.php',
				type : 'post',
				data : {
					action : 'display_slider',
					slider: 'goodnews-testimonials'
				},
				success : function( response ) {
					jQuery("body").append("<div id=new_slider>"+response+"</div>");
					
				},
				error : function ( response ){
					
				}
			}); // End Ajax

			*/
		
});	/*ready*/