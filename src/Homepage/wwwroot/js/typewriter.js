var TxtType = function (el, toRotate, period) {
    this.toRotate = toRotate;
    this.el = el;
    this.loopNum = 0;
    this.period = parseInt(period, 10) || 2000;
    this.txt = "";
    this.isDeleting = false;
};

function nextElementNo(txtType) {
    return (txtType.loopNum + 1) % txtType.toRotate.length;
}

function nextElement(txtType) {
    return txtType.toRotate[nextElementNo(txtType)];
}

function getDelta(isDeleting) {
    var random = Math.random();
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
    txtType.el.innerHTML = txtType.txt; //"<span class=\"wrap\">" + this.txt + "</span>";
}

TxtType.prototype.tick = function () {
    var fullTxt = this.toRotate[this.loopNum];

    if (this.txt === "") {
        setTimeout(function () { }, 1000); //wait 1000 ms if no letter written
    }

    updateText(this);

    var delta = getDelta(this.isDeleting);

    var txtIntersectsWithNext = nextElement(this).startsWith(this.txt);

    if (!this.isDeleting && this.txt === fullTxt) {
        delta = this.period;
        this.isDeleting = true;
    } else if (this.isDeleting && txtIntersectsWithNext) { //stop deleting if next text intersects with current
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
        var writer = new TxtType(element, JSON.parse(toRotate), period);
        writer.tick();
    }
};