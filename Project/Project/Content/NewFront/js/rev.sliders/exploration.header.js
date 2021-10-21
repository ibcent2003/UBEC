var tpj=jQuery;
			
			var revapi1164;
			tpj(document).ready(function() {
				if(tpj("#rev_slider_1164_1").revolution == undefined){
					revslider_showDoubleJqueryError("#rev_slider_1164_1");
				}else{
					revapi1164 = tpj("#rev_slider_1164_1").show().revolution({
						sliderType:"standard",
jsFileLocation:"revolution/js/",
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
								swipe_threshold: 75,
								swipe_min_touches: 1,
								swipe_direction: "horizontal",
								drag_block_vertical: false
							}
							,
							arrows: {
								style:"gyges",
								enable:true,
								hide_onmobile:false,
								hide_over:1199,
								hide_onleave:false,
								tmp:'',
								left: {
									h_align:"right",
									v_align:"bottom",
									h_offset:60,
									v_offset:20
								},
								right: {
									h_align:"right",
									v_align:"bottom",
									h_offset:20,
									v_offset:20
								}
							}
							,
							tabs: {
								style:"news-header",
								enable:true,
								width:350,
								height:50,
								min_width:350,
								wrapper_padding:5,
								wrapper_color:"transparent",
								wrapper_opacity:"0.05",
								tmp:'<div class="tp-tab-title">{{title}} <i style="float:right; margin-top:5px;" class="fa-icon-chevron-right"></i></div><div class="tp-tab-desc">{{param1}}</div>',
								visibleAmount: 3,
								hide_onmobile: true,
								hide_under:1200,
								hide_onleave:false,
								hide_delay:200,
								direction:"vertical",
								span:false,
								position:"inner",
								space:25,
								h_align:"right",
								v_align:"bottom",
								h_offset:43,
								v_offset:110
							}
						},
						responsiveLevels:[1240,1024,778,480],
						visibilityLevels:[1240,1024,778,480],
						gridwidth:[1240,1024,778,480],
						gridheight:[868,768,960,720],
						lazyType:"single",
						parallax: {
							type:"scroll",
							origo:"slidercenter",
							speed:400,
							levels:[5,10,15,20,25,30,35,40,45,46,47,48,49,50,51,55],
							type:"scroll",
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
						fullScreenOffsetContainer: ".header",
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
				}
			});	/*ready*/