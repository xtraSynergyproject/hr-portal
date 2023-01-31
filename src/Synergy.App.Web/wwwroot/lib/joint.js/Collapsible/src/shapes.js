/*! Rappid v3.3.0 - HTML5 Diagramming Framework - TRIAL VERSION

Copyright (c) 2021 client IO

 2021-04-08 


This Source Code Form is subject to the terms of the Rappid Trial License
, v. 2.0. If a copy of the Rappid License was not distributed with this
file, You can obtain one at http://jointjs.com/license/rappid_v2.txt
 or from the Rappid archive as was distributed by client IO. See the LICENSE file.*/


joint.dia.Element.define('collapsible.Model', {
  
    size: {
        width: 100,
        height: 27
    },
    z: 2,
    hidden: false,
    attrs: {
        root: {
            pointerEvents: 'none'
        },
        body: {
            refWidth: '100%',
            refHeight: '100%',
            strokeWidth: 1,
            fill: '#FFFFFF',
            stroke: '#A0A0A0'
        },
        label: {
            textWrap: {
                ellipsis: true,
                width: -10
            },
            textVerticalAnchor: 'middle',
            textAnchor: 'middle',
            refX: '50%',
            refY: '50%',
            fontFamily: 'source_sans_prosemibold',
            fontSize: 14
        },
        buttonGroup: {
            refX: '100%',
            refY: '50%'
        },
        button: {
            fill: '#4C65DD',
            stroke: 'none',
            x: -10,
            y: -10,
            height: 20,
            width: 30,
            rx: 10,
            ry: 10,
            cursor: 'pointer',
            event: 'element:collapse'
        },
        buttonSign: {
            refX: 5,
            refY: -5,
            stroke: '#FFFFFF',
            strokeWidth: 1.6
        }
    }
}, {

    PLUS_SIGN: 'M 1 5 9 5 M 5 1 5 9',
    MINUS_SIGN: 'M 2 5 8 5',

    markup: [{
        tagName: 'g',
        selector: 'buttonGroup',
        children: [{
            tagName: 'rect',
            selector: 'button',
            attributes: {
                'pointer-events': 'visiblePainted'
            }
        }, {
            tagName: 'path',
            selector: 'buttonSign',
            attributes: {
                'fill': 'none',
                'pointer-events': 'none'
            }
        }]
    },{
        tagName: 'g',
        selector: 'buttonGroup',
        children: [{
            tagName: 'rect',
            selector: 'button',
            attributes: {
                'pointer-events': 'visiblePainted'
            }
        }, {
            tagName: 'path',
            selector: 'buttonSign',
            attributes: {
                'fill': 'none',
                'pointer-events': 'none'
            }
        }]
    }, {
        tagName: 'rect',
        selector: 'body',
    }, {
        tagName: 'text',
        selector: 'label'
    }],

    isHidden: function() {
        return !!this.get('hidden');
    },

    isCollapsed: function() {
        return !!this.get('collapsed');
    },

    toggleButtonVisibility: function(visible) {
        this.attr('buttonGroup', { display: visible ? 'block' : 'none' });
    },

    toggleButtonSign: function(plus) {
        if (plus) {
            this.attr('buttonSign', { d: this.PLUS_SIGN, strokeWidth: 1.6 });
        } else {
            this.attr('buttonSign', { d: this.MINUS_SIGN, strokeWidth: 1.8 });
        }
    }

});

joint.shapes.standard.Link.define('collapsible.Link', {
    attrs: {
        root: {
            pointerEvents: 'none'
        },
        line: {
            stroke: '#A0A0A0',
            strokeWidth: 1,
            targetMarker: null
        }
    },
    z: 1
}, {

    isHidden: function() {
        // If the target element is collapsed, we don't want to
        // show the link either
        var targetElement = this.getTargetElement();
        return !targetElement || targetElement.isHidden();
    }
});
