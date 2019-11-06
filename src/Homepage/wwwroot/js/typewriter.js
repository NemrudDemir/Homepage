var TxtType = function (el, toRotate, period) {
    this.toRotate = toRotate;
    this.el = el;
    this.loopNum = 0;
    this.period = parseInt(period, 10) || 2000;
    this.txt = '';
    this.tick();
    this.isDeleting = false;
};

TxtType.prototype.tick = function () {
    var fullTxt = this.toRotate[this.loopNum];

    if (this.txt === '') {
        setTimeout(function () { }, 1000); //wait 1000 ms if no letter written
    }

    var txtIntersectsWithNext = false; //if isDeleting and the next element starts with current txt - write next text
    if (this.isDeleting) {
        this.txt = fullTxt.substring(0, this.txt.length - 1);
        var nextElement = this.toRotate[nextElementNo(this)];
        txtIntersectsWithNext = nextElement.startsWith(this.txt);
    } else {
        this.txt = fullTxt.substring(0, this.txt.length + 1);
    }

    this.el.innerHTML = '<span class="wrap">' + this.txt + '</span>';

    var that = this;
    var random = Math.random();
    var delta = 200 - random * 100;

    if (this.isDeleting) { delta /= 2; }

    if (!this.isDeleting && this.txt === fullTxt) {
        delta = this.period;
        this.isDeleting = true;
    } else if (this.isDeleting && //stop deleting if next text intersects with current + randomness
        ((txtIntersectsWithNext && random >= 0.5) || this.txt === '')) {
        this.isDeleting = false;
        this.loopNum = nextElementNo(this);
        delta = 500;
    }

    setTimeout(function () {
        that.tick();
    }, delta);
};

function nextElementNo(txtType) {
    return (txtType.loopNum + 1) % txtType.toRotate.length;
}

window.onload = function () {
    var elements = document.getElementsByClassName('typewrite');
    for (var i = 0; i < elements.length; i++) {
        var toRotate = elements[i].getAttribute('data-type');
        var period = elements[i].getAttribute('data-period');
        if (toRotate) {
            new TxtType(elements[i], JSON.parse(toRotate), period);
        }
    }
};