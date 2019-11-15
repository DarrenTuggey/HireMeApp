var randomColor = Math.floor(Math.random()*16777215).toString(16),
canvasBg = 'matrix',
overlay = document.getElementById('overlay');
function init() {
document.body.style.opacity = 1
}

function canvasSupport(a) {
  return !!a.getContext
}
function canvasApp() {
  function b() {
    f.fillStyle = 'rgba(0,0,0,.07)',
    f.fillRect(0, 0, e, g),
    f.fillStyle = '#' + randomColor,
    f.font = '10px Georgia',
    a.map(function (b, c) {
      text = String.fromCharCode(100 + 33 * Math.random()),
      x = 10 * c,
      f.fillText(text, x, b),
      a[c] = b > 100 + 30000 * Math.random() ? 0 : b + 10
    })
  }
  var c,
  d = document.createElement('canvas');
  if (d.id = 'myCanvas', d.classList = 'background', document.getElementById('canvasContainer').appendChild(d), canvasSupport(d)) {
    var f = d.getContext('2d'),
    e = d.width = window.outerWidth,
    g = d.height = window.outerHeight,
    a = Array(300).join(0).split('');
    'undefined' != typeof c && clearInterval(Game_interval),
    c = setInterval(b, 45)
  }
}
window.onresize = canvasApp;
canvasApp();




