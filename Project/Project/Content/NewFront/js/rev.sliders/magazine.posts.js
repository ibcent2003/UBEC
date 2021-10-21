var tpj=jQuery;
			
			var revapi1171;
			tpj(document).ready(function() {
				if(tpj("#rev_slider_1171_1").revolution == undefined){
					revslider_showDoubleJqueryError("#rev_slider_1171_1");
				}else{
					revapi1171 = tpj("#rev_slider_1171_1").show().revolution({
						sliderType:"standard",
jsFileLocation:"revolution/js/",
						sliderLayout:"fullwidth",
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
								swipe_threshold: 75,
								swipe_min_touches: 1,
								swipe_direction: "horizontal",
								drag_block_vertical: false
							}
							,
							arrows: {
								style:"metis",
								enable:true,
								hide_onmobile:true,
								hide_under:768,
								hide_onleave:false,
								tmp:'',
								left: {
									h_align:"left",
									v_align:"center",
									h_offset:0,
									v_offset:0
								},
								right: {
									h_align:"right",
									v_align:"center",
									h_offset:0,
									v_offset:0
								}
							}
						},
						responsiveLevels:[1240,1024,778,480],
						visibilityLevels:[1240,1024,778,480],
						gridwidth:[1400,1200,1000,480],
						gridheight:[1000,900,700,700],
						lazyType:"single",
						parallax: {
							type:"3D",
							origo:"slidercenter",
							speed:400,
							levels:[5,10,15,20,25,30,35,40,-5,-10,-15,-20,-25,-30,-35,55],
							type:"3D",
							ddd_shadow:"off",
							ddd_bgfreeze:"off",
							ddd_overflow:"visible",
							ddd_layer_overflow:"visible",
							ddd_z_correction:65,
							disable_onmobile:"on"
						},
						spinner:"off",
						stopLoop:"on",
						stopAfterLoops:0,
						stopAtSlide:1,
						shuffle:"off",
						autoHeight:"off",
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
				}
			});	/*ready*/