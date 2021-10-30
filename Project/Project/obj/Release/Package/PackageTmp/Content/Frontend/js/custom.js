/*

Template: Qwilo - Multipurpose Responsive HTML5 Template
Author: iqonicthemes.in
Version: 3.0

*/

/*----------------------------------------------
Index Of Script
------------------------------------------------

1.Page Loader
2.Back To Top
3.Slick slider
4.Rangslider
5.Shopslider
6.Cart Tooltip
7.Tooltip
8.Jarallax
9.Searchstyle Bar
10.Menu Bar
11.Sidebar Menu
12.Accordion
13.Isotope
14.Masonry
15.Portfolio move
16.Progress Bar
17.Audio video
18.Magnific Popup
20.Countdown
21.widget
22.counter
23.Typer
24.Wow Animation
25.Owl Carousel
26.Contact from
27.Subscription from

------------------------------------------------
Index Of Script
----------------------------------------------*/

$(document).ready(function() {

    /*------------------------
    Page Loader
    --------------------------*/
    jQuery("#load").fadeOut();
    jQuery("#loading").delay(0).fadeOut("slow");



    /*------------------------
    Back To Top
    --------------------------*/
    $('#back-to-top').fadeOut();
    $(window).on("scroll", function() {
        if ($(this).scrollTop() > 250) {
            $('#back-to-top').fadeIn(1400);
        } else {
            $('#back-to-top').fadeOut(400);
        }
    });
    // scroll body to 0px on click
    $('#top').on('click', function() {
        $('top').tooltip('hide');
        $('body,html').animate({
            scrollTop: 0
        }, 800);
        return false;
    });



    /*------------------------
    Slick slider
    --------------------------*/
    if ($('div').hasClass('slider-for')) {
        $('.slider-for').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            fade: true,
            asNavFor: '.slider-nav'
        });
    }
    //if(document.getElementsByClassName("slider-nav") != null) {
    if ($('div').hasClass('slider-nav')) {
        $('.slider-nav').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            asNavFor: '.slider-for',
            dots: true,
            centerMode: true,
            focusOnSelect: true
        });
    }
    // if(document.getElementsByClassName("responsive") != null) {
    if ($('div').hasClass('responsive')) {
        $('.responsive').slick({
            dots: true,
            infinite: false,
            speed: 300,
            slidesToShow: 4,
            slidesToScroll: 1,
            responsive: [{
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: true
                    }
                }, {
                    breakpoint: 600,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 1
                    }
                }, {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }

            ]
        });
    }

    /*------------------------
    Rangslider
    --------------------------*/
    if (document.getElementById("mySlider") != null) {

        $("#mySlider").slider({
            range: true,
            min: 10,
            max: 999,
            values: [200, 500],
            slide: function(event, ui) {
                $("#price").val("$" + ui.values[0] + " - $" + ui.values[1]);
            }
        });

        $("#price").val("$" + $("#mySlider").slider("values", 0) +
            " - $" + $("#mySlider").slider("values", 1));

    }


    /*------------------------
    Shopslider
    --------------------------*/
    $('.indc .increment, .indc1 .increment, .indc2 .increment, .indc3 .increment, .indc4 .increment, .indc5 .increment').click(function(e) {
        e.preventDefault();
        var fieldName = $(this).attr('field');
        var currentVal = parseInt($(this).prev().val());
        if (!isNaN(currentVal)) {
            $(this).prev().val(currentVal + 1);
        } else {
            $(this).prev().val(0);
        }
    });

    $('.indc .decrement, .indc1 .decrement, .indc2 .decrement, .indc3 .decrement, .indc4 .decrement, .indc5 .decrement').click(function(e) {
        e.preventDefault();
        var fieldName = $(this).attr('field');
        var currentVal = parseInt($(this).next().val());
        if (!isNaN(currentVal) && currentVal > 0) {
            $(this).next().val(currentVal - 1);
        } else {
            $(this).next().val(0);
        }
    });


    /*------------------------
    Cart Tooltip
    --------------------------*/
    $("#cart").on("click", function() {
        $(".cart-box").fadeToggle("fast");
    });


    /*------------------------
    Tooltip
    --------------------------*/
    $('[data-toggle="tooltip"]').tooltip()


    /*------------------------
    Jarallax
    --------------------------*/
    $('.jarallax').jarallax({
        speed: 0.2
    });


    /*------------------------
    Searchstyle Bar
    --------------------------*/
    'use strict';
    if ($(".search__input").length > 0) {
        var openCtrl = document.getElementById('btn-search'),
            closeCtrl = document.getElementById('btn-search-close'),
            searchContainer = document.querySelector('.search'),
            inputSearch = searchContainer.querySelector('.search__input');

        function init() {
            initEvents();
        }

        function initEvents() {
            openCtrl.addEventListener('click', openSearch);
            closeCtrl.addEventListener('click', closeSearch);
            document.addEventListener('keyup', function(ev) {
                // escape key.
                if (ev.keyCode === 27) {
                    closeSearch();
                }
            });
        }

        function openSearch() {
            searchContainer.classList.add('search--open');
            inputSearch.focus();
        }

        function closeSearch() {
            searchContainer.classList.remove('search--open');
            inputSearch.blur();
            inputSearch.value = '';
        }

        init();
    }

    // search 2
    $(".iq-search").on('click', function() {
        var checkId = document.getElementsByClassName("search-open");
        if (checkId.length > 0) {
            $('.iq-search').removeClass("search-open");
        } else {
            $('.iq-search').addClass("search-open");
        }
    })


    /*------------------------
    Menu Bar
    --------------------------*/
    jQuery('#menu-1').megaMenu({
        // DESKTOP MODE SETTINGS
        logo_align: 'left', // align the logo left or right. options (left) or (right)
        links_align: 'left', // align the links left or right. options (left) or (right)
        socialBar_align: 'left', // align the socialBar left or right. options (left) or (right)
        searchBar_align: 'right', // align the search bar left or right. options (left) or (right)
        trigger: 'hover', // show drop down using click or hover. options (hover) or (click)
        effect: 'fade', // drop down effects. options (fade), (scale), (expand-top), (expand-bottom), (expand-left), (expand-right)
        effect_speed: 400, // drop down show speed in milliseconds
        sibling: true, // hide the others showing drop downs if this option true. this option works on if the trigger option is "click". options (true) or (false)
        outside_click_close: true, // hide the showing drop downs when user click outside the menu. this option works if the trigger option is "click". options (true) or (false)
        top_fixed: false, // fixed the menu top of the screen. options (true) or (false)
        sticky_header: true, // menu fixed on top when scroll down down. options (true) or (false)
        sticky_header_height: 200, // sticky header height top of the screen. activate sticky header when meet the height. option change the height in px value.
        menu_position: 'horizontal', // change the menu position. options (horizontal), (vertical-left) or (vertical-right)
        full_width: true, // make menu full width. options (true) or (false)
        // MOBILE MODE SETTINGS
        mobile_settings: {
            collapse: true, // collapse the menu on click. options (true) or (false)
            sibling: true, // hide the others showing drop downs when click on current drop down. options (true) or (false)
            scrollBar: true, // enable the scroll bar. options (true) or (false)
            scrollBar_height: 400, // scroll bar height in px value. this option works if the scrollBar option true.
            top_fixed: false, // fixed menu top of the screen. options (true) or (false)
            sticky_header: true, // menu fixed on top when scroll down down. options (true) or (false)
            sticky_header_height: 200 // sticky header height top of the screen. activate sticky header when meet the height. option change the height in px value.
        }
    });
    $('#menu-2').megaMenu({
        // DESKTOP MODE SETTINGS
        logo_align: 'left', // align the logo left or right. options (left) or (right)
        links_align: 'left', // align the links left or right. options (left) or (right)
        socialBar_align: 'left', // align the socialBar left or right. options (left) or (right)
        searchBar_align: 'right', // align the search bar left or right. options (left) or (right)
        trigger: 'hover', // show drop down using click or hover. options (hover) or (click)
        effect: 'fade', // drop down effects. options (fade), (scale), (expand-top), (expand-bottom), (expand-left), (expand-right)
        effect_speed: 400, // drop down show speed in milliseconds
        sibling: true, // hide the others showing drop downs if this option true. this option works on if the trigger option is "click". options (true) or (false)
        outside_click_close: true, // hide the showing drop downs when user click outside the menu. this option works if the trigger option is "click". options (true) or (false)
        top_fixed: false, // fixed the menu top of the screen. options (true) or (false)
        sticky_header: false, // menu fixed on top when scroll down down. options (true) or (false)
        sticky_header_height: 200, // sticky header height top of the screen. activate sticky header when meet the height. option change the height in px value.
        menu_position: 'vertical-right', // change the menu position. options (horizontal), (vertical-left) or (vertical-right)
        full_width: false, // make menu full width. options (true) or (false)
        // MOBILE MODE SETTINGS
        mobile_settings: {
            collapse: true, // collapse the menu on click. options (true) or (false)
            sibling: true, // hide the others showing drop downs when click on current drop down. options (true) or (false)
            scrollBar: true, // enable the scroll bar. options (true) or (false)
            scrollBar_height: 400, // scroll bar height in px value. this option works if the scrollBar option true.
            top_fixed: false, // fixed menu top of the screen. options (true) or (false)
            sticky_header: false, // menu fixed on top when scroll down down. options (true) or (false)
            sticky_header_height: 200 // sticky header height top of the screen. activate sticky header when meet the height. option change the height in px value.
        }
    });
    $('#menu-3').megaMenu({
        // DESKTOP MODE SETTINGS
        logo_align: 'left', // align the logo left or right. options (left) or (right)
        links_align: 'left', // align the links left or right. options (left) or (right)
        socialBar_align: 'left', // align the socialBar left or right. options (left) or (right)
        searchBar_align: 'right', // align the search bar left or right. options (left) or (right)
        trigger: 'hover', // show drop down using click or hover. options (hover) or (click)
        effect: 'fade', // drop down effects. options (fade), (scale), (expand-top), (expand-bottom), (expand-left), (expand-right)
        effect_speed: 400, // drop down show speed in milliseconds
        sibling: true, // hide the others showing drop downs if this option true. this option works on if the trigger option is "click". options (true) or (false)
        outside_click_close: true, // hide the showing drop downs when user click outside the menu. this option works if the trigger option is "click". options (true) or (false)
        top_fixed: false, // fixed the menu top of the screen. options (true) or (false)
        sticky_header: false, // menu fixed on top when scroll down down. options (true) or (false)
        sticky_header_height: 200, // sticky header height top of the screen. activate sticky header when meet the height. option change the height in px value.
        menu_position: 'vertical-left', // change the menu position. options (horizontal), (vertical-left) or (vertical-right)
        full_width: false, // make menu full width. options (true) or (false)
        // MOBILE MODE SETTINGS
        mobile_settings: {
            collapse: true, // collapse the menu on click. options (true) or (false)
            sibling: true, // hide the others showing drop downs when click on current drop down. options (true) or (false)
            scrollBar: true, // enable the scroll bar. options (true) or (false)
            scrollBar_height: 400, // scroll bar height in px value. this option works if the scrollBar option true.
            top_fixed: false, // fixed menu top of the screen. options (true) or (false)
            sticky_header: false, // menu fixed on top when scroll down down. options (true) or (false)
            sticky_header_height: 200 // sticky header height top of the screen. activate sticky header when meet the height. option change the height in px value.
        }
    });

    /*------------------------
    Sidebar Menu
    --------------------------*/
    $(".sider-bt").on("click", function() {
        $(".sider-bt").toggleClass("cross");
        $(".sidebar-menu").toggleClass("sidebar-open");
    });


    /*------------------------
    Accordion
    --------------------------*/
    $('.iq-accordion .iq-ad-block .ad-details').hide();
    $('.iq-accordion .iq-ad-block:first').addClass('ad-active').children().slideDown('slow');
    $('.iq-accordion .iq-ad-block').on("click", function() {
        if ($(this).children('div').is(':hidden')) {
            $('.iq-accordion .iq-ad-block').removeClass('ad-active').children('div').slideUp('slow');
            $(this).toggleClass('ad-active').children('div').slideDown('slow');
        }
    });


    /*------------------------
    Isotope
    --------------------------*/
    $('.isotope').isotope({
        itemSelector: '.iq-grid-item',
    });

    // filter items on button click
    $('.isotope-filters').on('click', 'button', function() {
        var filterValue = $(this).attr('data-filter');
        $('.isotope').isotope({
            resizable: true,
            filter: filterValue
        });
        $('.isotope-filters button').removeClass('active');
        $(this).addClass('active');
    });


    /*------------------------
    Masonry
    --------------------------*/
    var $msnry = $('.iq-masonry-block .iq-masonry');
    if ($msnry) {
        var $filter = $('.iq-masonry-block .isotope-filters');
        $msnry.isotope({
            percentPosition: true,
            resizable: true,
            itemSelector: '.iq-masonry-block .iq-masonry-item',
            masonry: {
                gutterWidth: 0
            }
        });
        // bind filter button click
        $filter.on('click', 'button', function() {
            var filterValue = $(this).attr('data-filter');
            $msnry.isotope({
                filter: filterValue
            });
        });

        $filter.each(function(i, buttonGroup) {
            var $buttonGroup = $(buttonGroup);
            $buttonGroup.on('click', 'button', function() {
                $buttonGroup.find('.active').removeClass('active');
                $(this).addClass('active');
            });
        });
    }


    /*------------------------
    Portfolio move
    --------------------------*/
    $('.iq-portfolio-05').each(function() {
        $(this).hoverdir({});
    });


    /*------------------------
    Progress Bar
    --------------------------*/
    $('.progress-bar').each(function(i, e) {
        var progress_data = $(this);
        var delay_data = progress_data.attr('data-delay') || "100";
        var type_data = progress_data.attr('data-type') || "%";
        var percentage_data = progress_data.attr('data-percent') || "100";
        if (!progress_data.is('progress-animated')) {
            progress_data.css({
                'width': '0%'
            });
        }

        $(e).show(function() {
            setTimeout(function() {
                progress_data.animate({
                    'width': percentage_data + '%'
                }, 'easeInOutCirc').addClass('progress-animated');

                progress_data.delay(delay_data).append('<span class="progress-type">' + type_data + '</span><span class="progress-number">' + percentage_data + '</span>');
            }, delay_data);
        });
    });


    /*------------------------
    Audio video
    --------------------------*/
    if ($(".audio-video").length != 0) {
        $('audio,video').mediaelementplayer();
    }


    /*------------------------
    Magnific Popup
    --------------------------*/
    $('.popup-gallery').magnificPopup({
        delegate: 'a.popup-img',
        tLoading: 'Loading image #%curr%...',
        type: 'image',
        mainClass: 'mfp-img-mobile',
        gallery: {
            navigateByImgClick: true,
            enabled: true,
            preload: [0, 1]
        },
        image: {
            tError: '<a href="%url%">The image #%curr%</a> could not be loaded.'
        }
    });

    $('.popup-youtube, .popup-vimeo, .popup-gmaps').magnificPopup({
        type: 'iframe',
        disableOn: 700,
        mainClass: 'mfp-fade',
        preloader: false,
        removalDelay: 160,
        fixedContentPos: false
    });



    /*------------------------
    Countdown
    --------------------------*/
    $('#countdown').countdown({
        date: '10/01/2019 23:59:59',
        day: 'Day',
        days: 'Days'
    });
    $('#iq-countdown1').countdown({
        date: '10/01/2019 23:59:59',
        day: 'Day',
        days: 'Days'
    });
    $('#iq-countdown2').countdown({
        date: '10/01/2019 23:59:59',
        day: 'Day',
        days: 'Days'
    });
    $('#iq-countdown3').countdown({
        date: '10/01/2019 23:59:59',
        day: 'Day',
        days: 'Days'
    });


    /*------------------------
    widget
    --------------------------*/
    $('.iq-widget-menu > ul > li > a').on('click', function() {
        var checkElement = $(this).next();
        $('.iq-widget-menu li').removeClass('active');
        $(this).closest('li').addClass('active');
        if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
            $(this).closest('li').removeClass('active');
            checkElement.slideUp('normal');
        }
        if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
            $('.iq-widget-menu ul ul:visible').slideUp('normal');
            checkElement.slideDown('normal');
        }
        if ($(this).closest('li').find('ul').children().length === 0) {
            return true;
        } else {
            return false;
        }
    });



    /*------------------------
    counter
    --------------------------*/
    $('.timer').countTo();


    /*------------------------
    Typer
    --------------------------*/
    var win = $(window);
        foo = $('#typer');
    foo.typer(['<h6 class="iq-tw-6"><span class="iq-font-green">Web</span> Developer</h6>', '<h6 class="iq-tw-6">Web <span class="iq-font-green">Designer</span></h6>', '<h6 class="iq-tw-6"><span class="iq-font-green">Frontend</span> Developer</h6>']);


    /*------------------------
    Wow Animation
    --------------------------*/
    var wow = new WOW({
        boxClass: 'wow',
        animateClass: 'animated',
        offset: 0,
        mobile: false,
        live: true
    });
    wow.init();


    /*------------------------
    Owl Carousel
    --------------------------*/
    $('.owl-carousel').each(function() {
        var $carousel = $(this);
        $carousel.owlCarousel({
            items: $carousel.data("items"),
            loop: $carousel.data("loop"),
            margin: $carousel.data("margin"),
            nav: $carousel.data("nav"),
            dots: $carousel.data("dots"),
            autoplay: $carousel.data("autoplay"),
            autoplayTimeout: $carousel.data("autoplay-timeout"),
            navText: ['<i class="fa fa-angle-left fa-2x"></i>', '<i class="fa fa-angle-right fa-2x"></i>'],
            responsiveClass: true,
            responsive: {
                // breakpoint from 0 up
                0: {
                    items: $carousel.data("items-mobile-sm")
                },
                // breakpoint from 480 up
                480: {
                    items: $carousel.data("items-mobile")
                },
                // breakpoint from 786 up
                786: {
                    items: $carousel.data("items-tab")
                },
                // breakpoint from 1023 up
                1023: {
                    items: $carousel.data("items-laptop")
                },
                1199: {
                    items: $carousel.data("items")
                }
            }
        });
    });




    /*------------------------
    Contact from
    --------------------------*/
    $('#contact').submit(function(e) {
        var flag = 0;
        e.preventDefault(); // Prevent Default Submission
        $('.require').each(function() {
            if ($.trim($(this).val()) == '') {
                $(this).css("border", "1px solid red");
                e.preventDefault(); // Prevent Default Submission
                flag = 1;
            } else {
                $(this).css("border", "1px solid grey");
                flag = 0;
            }
        });

        if (grecaptcha.getResponse() == "") {
            flag = 1;
            alert('Please verify Recaptch');

        } else {
            flag = 0;
        }

        if (flag == 0) {
            $.ajax({
                    url: 'contact-form.php',
                    type: 'POST',
                    data: $("#contact").serialize() // it will serialize the form data
                })
                .done(function(data) {
                    $("#result").html('Form was successfully submitted.');
                    $('#contact')[0].reset();
                })
                .fail(function() {
                    alert('Ajax Submit Failed ...');
                });
        }

    });


    /*------------------------
    Subscription from
    --------------------------*/
    $('#subscribe_btn').on('click', function(e) {
        var flag = 0;
        e.preventDefault(); // Prevent Default Submission
        if ($('#subscribe_email').val() == '') {
            $('#subscribe_email').css("border", "1px solid red");
            e.preventDefault(); // Prevent Default Submission
            flag = 1;
        } else {
            $('#subscribe_email').css("border", "1px solid grey");
            flag = 0;
        }
        if (flag == 0) {
            $.ajax({
                    url: 'php/subscribed-form.php',
                    type: 'POST',
                    data: $("#subscribe_form").serialize() // it will serialize the form data
                })
                .done(function(data) {
                    $("#result").html('Form was successfully submitted.');
                    $('#subscribe_form')[0].reset();
                })
                .fail(function() {
                    alert('Ajax Submit Failed ...');
                });
        }

    });

});