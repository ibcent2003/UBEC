const image = document.querySelectorAll('.image');

for (let i = 0; i < image.length; i++) {
  image[i].addEventListener('click', lightbox);
}

function lightbox() {
  let lightbox = document.getElementById('lightbox'),
      imageSrc = this.getAttribute('src'),
      lightboxImage = document.createElement('img'),
      lightboxCloseBtn = document.createElement('span'),
      lightboxDesc = document.createElement('p');
  lightboxImage.src = imageSrc;
  let orginalWidth = lightboxImage.width;
  window.addEventListener('resize', function(e) {
    let width = e.target.innerWidth;
    if (width < lightboxImage.width) {
      lightboxImage.className = 'mobile';
    }
    if (width > orginalWidth) {
      lightboxImage.className = '';
    }
  });
  lightbox.appendChild(lightboxImage);
  lightboxCloseBtn.className = 'fa fa-times fa-2x';
  lightboxCloseBtn.addEventListener('click', lightboxClose);
  lightbox.appendChild(lightboxCloseBtn);
  lightboxDesc.innerHTML = this.alt;
  lightbox.appendChild(lightboxDesc);
  lightbox.classList.add('active');
  lightbox.addEventListener('click', function(evt) {
    if (evt.target.id == 'lightbox') {
      lightboxClose();
    }
  });
}

function lightboxClose() {
  let lightbox = document.getElementById('lightbox');
  lightbox.classList.remove('active');
  lightbox.innerHTML = '';
}