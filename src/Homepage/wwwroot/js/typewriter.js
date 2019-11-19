var TxtType = function (el, toRotate, period) {
    this.toRotate = toRotate;
    this.el = el;
    this.loopNum = 0;
    this.period = parseInt(period, 10) || 2000;
    this.txt = "";
    this.tick();
    this.isDeleting = false;
};

function nextElementNo(txtType) {
    return (txtType.loopNum + 1) % txtType.toRotate.length;
}

function getDelta(isDeleting, random) {
    var delta = 200 - random * 100;
    if (isDeleting) { delta /= 2; }
    return delta;
}

function updateText(txtType) {
    var fullTxt = txtType.toRotate[txtType.loopNum];
    if (txtType.isDeleting) {
        txtType.txt = fullTxt.substring(0, txtType.txt.length - 1);
    } else {
        txtType.txt = fullTxt.substring(0, txtType.txt.length + 1);
    }
}

TxtType.prototype.tick = function () {
    var fullTxt = this.toRotate[this.loopNum];

    if (this.txt === "") {
        setTimeout(function () { }, 1000); //wait 1000 ms if no letter written
    }

    updateText(this);
    this.el.innerHTML = "<span class=\"wrap\">" + this.txt + "</span>";

    var random = Math.random();
    var delta = getDelta(this.isDeleting, random);

    var nextElement = this.toRotate[nextElementNo(this)];
    var txtIntersectsWithNext = nextElement.startsWith(this.txt);

    if (!this.isDeleting && this.txt === fullTxt) {
        delta = this.period;
        this.isDeleting = true;
    } else if (this.isDeleting && //stop deleting if next text intersects with current + randomness
        ((txtIntersectsWithNext && random >= 0.5) || this.txt === "")) {
        this.isDeleting = false;
        this.loopNum = nextElementNo(this);
        delta = 500;
    }

    var that = this;
    setTimeout(function () {
        that.tick();
    }, delta);
};

window.onload = function () {
    var element = document.getElementById("typewriteSpan");// $("#typewriteSpan");
    var toRotate = element.getAttribute("data-type");
    var period = element.getAttribute("data-period");
    if (toRotate) {
        var _ = new TxtType(element, JSON.parse(toRotate), period);
    }
};