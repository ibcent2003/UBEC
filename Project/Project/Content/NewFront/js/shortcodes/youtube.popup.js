$(document).ready(function() {
  var $btnLoadMore = $(
    '<div class="btn-wrapper text-center"><a href="#" class="btn load-more">Load More</a></div>'
  );
  var items = $(".youtube-popup[data-listnum]");
  var count = items.length;
  var slice = 2;
  var current = 0;

  if (items.length > slice) {
    //bind load more event
    $btnLoadMore.on("click", function(e) {
      e.preventDefault();
      loadMoreNews();
    });
    //append load more button
    items.closest(".salvattore-grid").after($btnLoadMore);
  }

  function getItem(listnum) {
    return items
        .filter(function(index) {
          if ($(this).attr("data-listnum") == listnum) {
            return true;
          }
        });
  }
  
  function loadMoreNews() {
    var end = current + slice;
    if (end >= count) {
      end = count;
      $btnLoadMore.hide();
    }
    while (current < end) {
      var listnum = current + 1; //data-listnum : 1-based
      var item = getItem(listnum);
      if (item) {
        item.fadeIn();
      }
      current++;
    }
  }

  //youtube popup
  $(".popup-youtube").magnificPopup({
    type: "iframe",
    removalDelay: 160,
    preloader: false,
    fixedContentPos: false,
    iframe: {
      markup:
        '<div class="mfp-iframe-scaler">' +
          '<div class="mfp-close"></div>' +
          '<iframe class="mfp-iframe" frameborder="0" allowfullscreen></iframe>' +
          "</div>",
      patterns: {
        youtube: {
          index: "youtube.com/",
          id: "v=",
          src: "//www.youtube.com/embed/%id%?autoplay=1&rel=0&showinfo=0"
        }
      },
      srcAction: "iframe_src"
    }
  });

  //init load
  loadMoreNews();
});