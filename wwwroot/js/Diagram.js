// function to create a node
class Node {
    constructor(
        x = 0, y = 0,
        width = 0, height = 0,
        image = '', description = ''
    ) {
        this.x = Number(x)
        this.y = Number(y)
        this.width = Number(width)
        this.height = Number(height)
        this.image = image
        this.description = description
    }

    get area() {
        return this.width * this.height
    }

    get left() {
        return this.x
    }

    get right() {
        return this.x + this.width
    }

    get top() {
        return this.y
    }

    get bottom() {
        return this.y + this.height
    }

    get get_width() {
        return this.width;
    }

    get get_height() {
        return this.height;
    }

    draw() {
        var canvas = document.getElementById('myCanvas');
        var ctx = canvas.getContext('2d');

        var imageToDraw = new Image(this.width, this.height);
        imageToDraw.src = this.image;

        ctx.drawImage(imageToDraw, this.x, this.y, this.width, this.height);

        // Include the description draw slightly below the node
        ctx.font = "20px Arial";
        ctx.textAlign = "center";
        ctx.fillText(this.description, this.x + (this.width / 2), this.y + this.height + 20);
    }
}

function canvas_arrow(context, fromx, fromy, tox, toy, r) {
    var x_center = tox;
    var y_center = toy;

    var angle;
    var x;
    var y;

    context.beginPath();

    angle = Math.atan2(toy - fromy, tox - fromx)
    x_center -= r * Math.cos(angle);
    y_center -= r * Math.sin(angle);
    x = r * Math.cos(angle) + x_center;
    y = r * Math.sin(angle) + y_center;

    context.moveTo(x, y);

    angle += (1 / 3) * (2 * Math.PI)
    x = r * Math.cos(angle) + x_center;
    y = r * Math.sin(angle) + y_center;

    context.lineTo(x, y);

    angle += (1 / 3) * (2 * Math.PI)
    x = r * Math.cos(angle) + x_center;
    y = r * Math.sin(angle) + y_center;

    context.lineTo(x, y);

    context.closePath();

    context.fill();
}

function arrow_right(ctx, node1, node2, offset, arrow_weight, line_weight, color, step) {

    ctx.lineWidth = line_weight;
    ctx.strokeStyle = color;
    ctx.fillStyle = color;
    ctx.lineJoin = 'butt';

    ctx.beginPath();
    ctx.moveTo(node1.left + node1.get_width, node1.top + (node1.get_height / 2));
    ctx.lineTo(node2.left - offset, node2.top + (node2.get_height / 2));
    ctx.stroke();

    canvas_arrow(ctx, node1.left + node1.get_width, node1.top + (node1.get_height / 2), node2.left, node2.top + (node2.get_height / 2), arrow_weight);

    ctx.font = "20px Arial";
    ctx.textAlign = "center";
    ctx.fillText(step, (node2.left + node1.right) / 2, (node1.top + node1.bottom) / 2 + 20);

}

function arrow_left(ctx, node1, node2, offset, arrow_weight, line_weight, color, step) {

    ctx.lineWidth = line_weight;
    ctx.strokeStyle = color;
    ctx.fillStyle = color;
    ctx.lineJoin = 'butt';

    ctx.beginPath();
    ctx.moveTo(node1.left, node1.top + (node1.get_height / 2));
    ctx.lineTo(node2.left + node2.get_width + offset, node2.top + (node2.get_height / 2));
    ctx.stroke();

    canvas_arrow(ctx, node1.left, node1.top + (node1.get_height / 2), node2.left + node2.get_width, node2.top + (node2.get_height / 2), arrow_weight);

    ctx.font = "20px Arial";
    ctx.textAlign = "center";
    ctx.fillText(step, (node2.left + node1.right) / 2, (node1.top + node1.bottom) / 2 + 20);

}

function arrow_down(ctx, node1, node2, offset, arrow_weight, line_weight, color, step) {

    var text_offset = 30;

    ctx.lineWidth = line_weight;
    ctx.strokeStyle = color;
    ctx.fillStyle = color;
    ctx.lineJoin = 'butt';

    ctx.beginPath();
    ctx.moveTo(node1.left + (node1.get_width / 2), node1.bottom + text_offset);
    ctx.lineTo(node2.left + (node2.get_width / 2), node2.top - offset);
    ctx.stroke();

    canvas_arrow(ctx, node1.left + (node1.get_width / 2), node1.top + (node1.get_height / 2), node2.left + (node2.get_width / 2), node2.top, arrow_weight);

    ctx.font = "20px Arial";
    ctx.textAlign = "center";
    ctx.fillText(step, (node1.left + node1.right) / 2 + 20, (node2.top + node1.bottom) / 2 );

}

function arrow_up(ctx, node1, node2, offset, arrow_weight, line_weight, color, step) {

    var text_offset = 30;

    ctx.lineWidth = line_weight;
    ctx.strokeStyle = color;
    ctx.fillStyle = color;
    ctx.lineJoin = 'butt';

    ctx.beginPath();
    ctx.moveTo(node1.left + (node1.get_width / 2), node1.top);
    ctx.lineTo(node2.left + (node2.get_width / 2), node2.top + node2.get_height + offset + text_offset);
    ctx.stroke();

    canvas_arrow(ctx, node1.left + (node1.get_width / 2), node1.top + node1.get_height, node2.left + (node2.get_width / 2), node2.top + node2.get_height + text_offset, arrow_weight);

    ctx.font = "20px Arial";
    ctx.textAlign = "center";
    ctx.fillText(step, (node1.left + node1.right) / 2 + 20, (node1.top + node2.bottom) / 2);

}

function arrow_down_left(ctx, node1, node2, offset, arrow_weight, line_weight, color, step) {

    var text_offset = 30;

    ctx.lineWidth = line_weight;
    ctx.strokeStyle = color;
    ctx.fillStyle = color;
    ctx.lineJoin = 'butt';

    ctx.beginPath();
    ctx.moveTo(node1.left, node1.top + node1.get_height + text_offset);
    ctx.lineTo(node2.left + node2.get_width + (offset*2), node2.top - (offset*2));
    ctx.stroke();

    canvas_arrow(ctx, node1.left, node1.top + node1.get_height + text_offset, node2.left + node2.get_width, node2.top, arrow_weight);

    ctx.font = "20px Arial";
    ctx.textAlign = "center";
    ctx.fillText(step, (node1.left + node2.right) / 2, (node2.top + node1.bottom) / 2);

}

function arrow_down_right(ctx, node1, node2, offset, arrow_weight, line_weight, color, step) {

    var text_offset = 30;

    ctx.lineWidth = line_weight;
    ctx.strokeStyle = color;
    ctx.fillStyle = color;
    ctx.lineJoin = 'butt';

    ctx.beginPath();
    ctx.moveTo(node1.left + node1.get_width, node1.top + node1.get_height + text_offset);
    ctx.lineTo(node2.left - (offset * 2), node2.top - (offset * 2));
    ctx.stroke();

    canvas_arrow(ctx, node1.left + node1.get_width, node1.top + node1.get_height + text_offset, node2.left, node2.top, arrow_weight);

    ctx.font = "20px Arial";
    ctx.textAlign = "center";
    ctx.fillText(step, (node2.left + node1.right) / 2, (node2.top + node1.bottom) / 2);

}

function arrow_up_right(ctx, node1, node2, offset, arrow_weight, line_weight, color, step) {

    var text_offset = 30;

    ctx.lineWidth = line_weight;
    ctx.strokeStyle = color;
    ctx.fillStyle = color;
    ctx.lineJoin = 'butt';

    ctx.beginPath();
    ctx.moveTo(node1.left + node1.get_width, node1.top);
    ctx.lineTo(node2.left - (offset * 2), node2.top + node2.get_height + (offset * 2) + text_offset);
    ctx.stroke();

    canvas_arrow(ctx, node1.left + node1.get_width, node1.top, node2.left, node2.top + node2.get_height + text_offset, arrow_weight);

    ctx.font = "20px Arial";
    ctx.textAlign = "center";
    ctx.fillText(step, (node2.left + node1.right) / 2, (node1.top + node2.bottom) / 2);

}

function arrow_up_left(ctx, node1, node2, offset, arrow_weight, line_weight, color, step) {

    var text_offset = 30;

    ctx.lineWidth = line_weight;
    ctx.strokeStyle = color;
    ctx.fillStyle = color;
    ctx.lineJoin = 'butt';

    ctx.beginPath();
    ctx.moveTo(node1.left, node1.top);
    ctx.lineTo(node2.left + node2.get_width + (offset * 2), node2.top + node2.get_height + (offset * 2) + text_offset);
    ctx.stroke();

    canvas_arrow(ctx, node1.left, node1.top, node2.left + node2.get_width, node2.top + node2.get_height + text_offset, arrow_weight);

    ctx.font = "20px Arial";
    ctx.textAlign = "center";
    ctx.fillText(step, (node1.left + node2.right) / 2, (node1.top + node2.bottom) / 2);

}