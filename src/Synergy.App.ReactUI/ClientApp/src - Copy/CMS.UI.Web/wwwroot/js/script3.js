/*
 Copyright (C) Federico Zivolo 2017
 Distributed under the MIT License (license terms are at http://opensource.org/licenses/MIT).
 */(function (e, t) { 'object' == typeof exports && 'undefined' != typeof module ? module.exports = t() : 'function' == typeof define && define.amd ? define(t) : e.Popper = t() })(this, function () { 'use strict'; function e(e) { return e && '[object Function]' === {}.toString.call(e) } function t(e, t) { if (1 !== e.nodeType) return []; var o = window.getComputedStyle(e, null); return t ? o[t] : o } function o(e) { return 'HTML' === e.nodeName ? e : e.parentNode || e.host } function n(e) { if (!e || -1 !== ['HTML', 'BODY', '#document'].indexOf(e.nodeName)) return window.document.body; var i = t(e), r = i.overflow, p = i.overflowX, s = i.overflowY; return /(auto|scroll)/.test(r + s + p) ? e : n(o(e)) } function r(e) { var o = e && e.offsetParent, i = o && o.nodeName; return i && 'BODY' !== i && 'HTML' !== i ? -1 !== ['TD', 'TABLE'].indexOf(o.nodeName) && 'static' === t(o, 'position') ? r(o) : o : window.document.documentElement } function p(e) { var t = e.nodeName; return 'BODY' !== t && ('HTML' === t || r(e.firstElementChild) === e) } function s(e) { return null === e.parentNode ? e : s(e.parentNode) } function d(e, t) { if (!e || !e.nodeType || !t || !t.nodeType) return window.document.documentElement; var o = e.compareDocumentPosition(t) & Node.DOCUMENT_POSITION_FOLLOWING, i = o ? e : t, n = o ? t : e, a = document.createRange(); a.setStart(i, 0), a.setEnd(n, 0); var f = a.commonAncestorContainer; if (e !== f && t !== f || i.contains(n)) return p(f) ? f : r(f); var l = s(e); return l.host ? d(l.host, t) : d(e, s(t).host) } function a(e) { var t = 1 < arguments.length && void 0 !== arguments[1] ? arguments[1] : 'top', o = 'top' === t ? 'scrollTop' : 'scrollLeft', i = e.nodeName; if ('BODY' === i || 'HTML' === i) { var n = window.document.documentElement, r = window.document.scrollingElement || n; return r[o] } return e[o] } function f(e, t) { var o = 2 < arguments.length && void 0 !== arguments[2] && arguments[2], i = a(t, 'top'), n = a(t, 'left'), r = o ? -1 : 1; return e.top += i * r, e.bottom += i * r, e.left += n * r, e.right += n * r, e } function l(e, t) { var o = 'x' === t ? 'Left' : 'Top', i = 'Left' == o ? 'Right' : 'Bottom'; return +e['border' + o + 'Width'].split('px')[0] + +e['border' + i + 'Width'].split('px')[0] } function m(e, t, o, i) { return _(t['offset' + e], o['client' + e], o['offset' + e], ie() ? o['offset' + e] + i['margin' + ('Height' === e ? 'Top' : 'Left')] + i['margin' + ('Height' === e ? 'Bottom' : 'Right')] : 0) } function h() { var e = window.document.body, t = window.document.documentElement, o = ie() && window.getComputedStyle(t); return { height: m('Height', e, t, o), width: m('Width', e, t, o) } } function c(e) { return se({}, e, { right: e.left + e.width, bottom: e.top + e.height }) } function g(e) { var o = {}; if (ie()) try { o = e.getBoundingClientRect(); var i = a(e, 'top'), n = a(e, 'left'); o.top += i, o.left += n, o.bottom += i, o.right += n } catch (e) { } else o = e.getBoundingClientRect(); var r = { left: o.left, top: o.top, width: o.right - o.left, height: o.bottom - o.top }, p = 'HTML' === e.nodeName ? h() : {}, s = p.width || e.clientWidth || r.right - r.left, d = p.height || e.clientHeight || r.bottom - r.top, f = e.offsetWidth - s, m = e.offsetHeight - d; if (f || m) { var g = t(e); f -= l(g, 'x'), m -= l(g, 'y'), r.width -= f, r.height -= m } return c(r) } function u(e, o) { var i = ie(), r = 'HTML' === o.nodeName, p = g(e), s = g(o), d = n(e), a = t(o), l = +a.borderTopWidth.split('px')[0], m = +a.borderLeftWidth.split('px')[0], h = c({ top: p.top - s.top - l, left: p.left - s.left - m, width: p.width, height: p.height }); if (h.marginTop = 0, h.marginLeft = 0, !i && r) { var u = +a.marginTop.split('px')[0], b = +a.marginLeft.split('px')[0]; h.top -= l - u, h.bottom -= l - u, h.left -= m - b, h.right -= m - b, h.marginTop = u, h.marginLeft = b } return (i ? o.contains(d) : o === d && 'BODY' !== d.nodeName) && (h = f(h, o)), h } function b(e) { var t = window.document.documentElement, o = u(e, t), i = _(t.clientWidth, window.innerWidth || 0), n = _(t.clientHeight, window.innerHeight || 0), r = a(t), p = a(t, 'left'), s = { top: r - o.top + o.marginTop, left: p - o.left + o.marginLeft, width: i, height: n }; return c(s) } function y(e) { var i = e.nodeName; return 'BODY' === i || 'HTML' === i ? !1 : 'fixed' === t(e, 'position') || y(o(e)) } function w(e, t, i, r) { var p = { top: 0, left: 0 }, s = d(e, t); if ('viewport' === r) p = b(s); else { var a; 'scrollParent' === r ? (a = n(o(e)), 'BODY' === a.nodeName && (a = window.document.documentElement)) : 'window' === r ? a = window.document.documentElement : a = r; var f = u(a, s); if ('HTML' === a.nodeName && !y(s)) { var l = h(), m = l.height, c = l.width; p.top += f.top - f.marginTop, p.bottom = m + f.top, p.left += f.left - f.marginLeft, p.right = c + f.left } else p = f } return p.left += i, p.top += i, p.right -= i, p.bottom -= i, p } function v(e) { var t = e.width, o = e.height; return t * o } function E(e, t, o, i, n) { var r = 5 < arguments.length && void 0 !== arguments[5] ? arguments[5] : 0; if (-1 === e.indexOf('auto')) return e; var p = w(o, i, r, n), s = { top: { width: p.width, height: t.top - p.top }, right: { width: p.right - t.right, height: p.height }, bottom: { width: p.width, height: p.bottom - t.bottom }, left: { width: t.left - p.left, height: p.height } }, d = Object.keys(s).map(function (e) { return se({ key: e }, s[e], { area: v(s[e]) }) }).sort(function (e, t) { return t.area - e.area }), a = d.filter(function (e) { var t = e.width, i = e.height; return t >= o.clientWidth && i >= o.clientHeight }), f = 0 < a.length ? a[0].key : d[0].key, l = e.split('-')[1]; return f + (l ? '-' + l : '') } function x(e, t, o) { var i = d(t, o); return u(o, i) } function O(e) { var t = window.getComputedStyle(e), o = parseFloat(t.marginTop) + parseFloat(t.marginBottom), i = parseFloat(t.marginLeft) + parseFloat(t.marginRight), n = { width: e.offsetWidth + i, height: e.offsetHeight + o }; return n } function L(e) { var t = { left: 'right', right: 'left', bottom: 'top', top: 'bottom' }; return e.replace(/left|right|bottom|top/g, function (e) { return t[e] }) } function S(e, t, o) { o = o.split('-')[0]; var i = O(e), n = { width: i.width, height: i.height }, r = -1 !== ['right', 'left'].indexOf(o), p = r ? 'top' : 'left', s = r ? 'left' : 'top', d = r ? 'height' : 'width', a = r ? 'width' : 'height'; return n[p] = t[p] + t[d] / 2 - i[d] / 2, n[s] = o === s ? t[s] - i[a] : t[L(s)], n } function T(e, t) { return Array.prototype.find ? e.find(t) : e.filter(t)[0] } function C(e, t, o) { if (Array.prototype.findIndex) return e.findIndex(function (e) { return e[t] === o }); var i = T(e, function (e) { return e[t] === o }); return e.indexOf(i) } function N(t, o, i) { var n = void 0 === i ? t : t.slice(0, C(t, 'name', i)); return n.forEach(function (t) { t.function && console.warn('`modifier.function` is deprecated, use `modifier.fn`!'); var i = t.function || t.fn; t.enabled && e(i) && (o.offsets.popper = c(o.offsets.popper), o.offsets.reference = c(o.offsets.reference), o = i(o, t)) }), o } function k() { if (!this.state.isDestroyed) { var e = { instance: this, styles: {}, attributes: {}, flipped: !1, offsets: {} }; e.offsets.reference = x(this.state, this.popper, this.reference), e.placement = E(this.options.placement, e.offsets.reference, this.popper, this.reference, this.options.modifiers.flip.boundariesElement, this.options.modifiers.flip.padding), e.originalPlacement = e.placement, e.offsets.popper = S(this.popper, e.offsets.reference, e.placement), e.offsets.popper.position = 'absolute', e = N(this.modifiers, e), this.state.isCreated ? this.options.onUpdate(e) : (this.state.isCreated = !0, this.options.onCreate(e)) } } function W(e, t) { return e.some(function (e) { var o = e.name, i = e.enabled; return i && o === t }) } function B(e) { for (var t = [!1, 'ms', 'Webkit', 'Moz', 'O'], o = e.charAt(0).toUpperCase() + e.slice(1), n = 0; n < t.length - 1; n++) { var i = t[n], r = i ? '' + i + o : e; if ('undefined' != typeof window.document.body.style[r]) return r } return null } function D() { return this.state.isDestroyed = !0, W(this.modifiers, 'applyStyle') && (this.popper.removeAttribute('x-placement'), this.popper.style.left = '', this.popper.style.position = '', this.popper.style.top = '', this.popper.style[B('transform')] = ''), this.disableEventListeners(), this.options.removeOnDestroy && this.popper.parentNode.removeChild(this.popper), this } function H(e, t, o, i) { var r = 'BODY' === e.nodeName, p = r ? window : e; p.addEventListener(t, o, { passive: !0 }), r || H(n(p.parentNode), t, o, i), i.push(p) } function P(e, t, o, i) { o.updateBound = i, window.addEventListener('resize', o.updateBound, { passive: !0 }); var r = n(e); return H(r, 'scroll', o.updateBound, o.scrollParents), o.scrollElement = r, o.eventsEnabled = !0, o } function A() { this.state.eventsEnabled || (this.state = P(this.reference, this.options, this.state, this.scheduleUpdate)) } function M(e, t) { return window.removeEventListener('resize', t.updateBound), t.scrollParents.forEach(function (e) { e.removeEventListener('scroll', t.updateBound) }), t.updateBound = null, t.scrollParents = [], t.scrollElement = null, t.eventsEnabled = !1, t } function I() { this.state.eventsEnabled && (window.cancelAnimationFrame(this.scheduleUpdate), this.state = M(this.reference, this.state)) } function R(e) { return '' !== e && !isNaN(parseFloat(e)) && isFinite(e) } function U(e, t) { Object.keys(t).forEach(function (o) { var i = ''; -1 !== ['width', 'height', 'top', 'right', 'bottom', 'left'].indexOf(o) && R(t[o]) && (i = 'px'), e.style[o] = t[o] + i }) } function Y(e, t) { Object.keys(t).forEach(function (o) { var i = t[o]; !1 === i ? e.removeAttribute(o) : e.setAttribute(o, t[o]) }) } function F(e, t, o) { var i = T(e, function (e) { var o = e.name; return o === t }), n = !!i && e.some(function (e) { return e.name === o && e.enabled && e.order < i.order }); if (!n) { var r = '`' + t + '`'; console.warn('`' + o + '`' + ' modifier is required by ' + r + ' modifier in order to work, be sure to include it before ' + r + '!') } return n } function j(e) { return 'end' === e ? 'start' : 'start' === e ? 'end' : e } function K(e) { var t = 1 < arguments.length && void 0 !== arguments[1] && arguments[1], o = ae.indexOf(e), i = ae.slice(o + 1).concat(ae.slice(0, o)); return t ? i.reverse() : i } function q(e, t, o, i) { var n = e.match(/((?:\-|\+)?\d*\.?\d*)(.*)/), r = +n[1], p = n[2]; if (!r) return e; if (0 === p.indexOf('%')) { var s; switch (p) { case '%p': s = o; break; case '%': case '%r': default: s = i; }var d = c(s); return d[t] / 100 * r } if ('vh' === p || 'vw' === p) { var a; return a = 'vh' === p ? _(document.documentElement.clientHeight, window.innerHeight || 0) : _(document.documentElement.clientWidth, window.innerWidth || 0), a / 100 * r } return r } function G(e, t, o, i) { var n = [0, 0], r = -1 !== ['right', 'left'].indexOf(i), p = e.split(/(\+|\-)/).map(function (e) { return e.trim() }), s = p.indexOf(T(p, function (e) { return -1 !== e.search(/,|\s/) })); p[s] && -1 === p[s].indexOf(',') && console.warn('Offsets separated by white space(s) are deprecated, use a comma (,) instead.'); var d = /\s*,\s*|\s+/, a = -1 === s ? [p] : [p.slice(0, s).concat([p[s].split(d)[0]]), [p[s].split(d)[1]].concat(p.slice(s + 1))]; return a = a.map(function (e, i) { var n = (1 === i ? !r : r) ? 'height' : 'width', p = !1; return e.reduce(function (e, t) { return '' === e[e.length - 1] && -1 !== ['+', '-'].indexOf(t) ? (e[e.length - 1] = t, p = !0, e) : p ? (e[e.length - 1] += t, p = !1, e) : e.concat(t) }, []).map(function (e) { return q(e, n, t, o) }) }), a.forEach(function (e, t) { e.forEach(function (o, i) { R(o) && (n[t] += o * ('-' === e[i - 1] ? -1 : 1)) }) }), n } for (var z = Math.min, V = Math.floor, _ = Math.max, X = ['native code', '[object MutationObserverConstructor]'], Q = function (e) { return X.some(function (t) { return -1 < (e || '').toString().indexOf(t) }) }, J = 'undefined' != typeof window, Z = ['Edge', 'Trident', 'Firefox'], $ = 0, ee = 0; ee < Z.length; ee += 1)if (J && 0 <= navigator.userAgent.indexOf(Z[ee])) { $ = 1; break } var i, te = J && Q(window.MutationObserver), oe = te ? function (e) { var t = !1, o = 0, i = document.createElement('span'), n = new MutationObserver(function () { e(), t = !1 }); return n.observe(i, { attributes: !0 }), function () { t || (t = !0, i.setAttribute('x-index', o), ++o) } } : function (e) { var t = !1; return function () { t || (t = !0, setTimeout(function () { t = !1, e() }, $)) } }, ie = function () { return void 0 == i && (i = -1 !== navigator.appVersion.indexOf('MSIE 10')), i }, ne = function (e, t) { if (!(e instanceof t)) throw new TypeError('Cannot call a class as a function') }, re = function () { function e(e, t) { for (var o, n = 0; n < t.length; n++)o = t[n], o.enumerable = o.enumerable || !1, o.configurable = !0, 'value' in o && (o.writable = !0), Object.defineProperty(e, o.key, o) } return function (t, o, i) { return o && e(t.prototype, o), i && e(t, i), t } }(), pe = function (e, t, o) { return t in e ? Object.defineProperty(e, t, { value: o, enumerable: !0, configurable: !0, writable: !0 }) : e[t] = o, e }, se = Object.assign || function (e) { for (var t, o = 1; o < arguments.length; o++)for (var i in t = arguments[o], t) Object.prototype.hasOwnProperty.call(t, i) && (e[i] = t[i]); return e }, de = ['auto-start', 'auto', 'auto-end', 'top-start', 'top', 'top-end', 'right-start', 'right', 'right-end', 'bottom-end', 'bottom', 'bottom-start', 'left-end', 'left', 'left-start'], ae = de.slice(3), fe = { FLIP: 'flip', CLOCKWISE: 'clockwise', COUNTERCLOCKWISE: 'counterclockwise' }, le = function () { function t(o, i) { var n = this, r = 2 < arguments.length && void 0 !== arguments[2] ? arguments[2] : {}; ne(this, t), this.scheduleUpdate = function () { return requestAnimationFrame(n.update) }, this.update = oe(this.update.bind(this)), this.options = se({}, t.Defaults, r), this.state = { isDestroyed: !1, isCreated: !1, scrollParents: [] }, this.reference = o.jquery ? o[0] : o, this.popper = i.jquery ? i[0] : i, this.options.modifiers = {}, Object.keys(se({}, t.Defaults.modifiers, r.modifiers)).forEach(function (e) { n.options.modifiers[e] = se({}, t.Defaults.modifiers[e] || {}, r.modifiers ? r.modifiers[e] : {}) }), this.modifiers = Object.keys(this.options.modifiers).map(function (e) { return se({ name: e }, n.options.modifiers[e]) }).sort(function (e, t) { return e.order - t.order }), this.modifiers.forEach(function (t) { t.enabled && e(t.onLoad) && t.onLoad(n.reference, n.popper, n.options, t, n.state) }), this.update(); var p = this.options.eventsEnabled; p && this.enableEventListeners(), this.state.eventsEnabled = p } return re(t, [{ key: 'update', value: function () { return k.call(this) } }, { key: 'destroy', value: function () { return D.call(this) } }, { key: 'enableEventListeners', value: function () { return A.call(this) } }, { key: 'disableEventListeners', value: function () { return I.call(this) } }]), t }(); return le.Utils = ('undefined' == typeof window ? global : window).PopperUtils, le.placements = de, le.Defaults = { placement: 'bottom', eventsEnabled: !0, removeOnDestroy: !1, onCreate: function () { }, onUpdate: function () { }, modifiers: { shift: { order: 100, enabled: !0, fn: function (e) { var t = e.placement, o = t.split('-')[0], i = t.split('-')[1]; if (i) { var n = e.offsets, r = n.reference, p = n.popper, s = -1 !== ['bottom', 'top'].indexOf(o), d = s ? 'left' : 'top', a = s ? 'width' : 'height', f = { start: pe({}, d, r[d]), end: pe({}, d, r[d] + r[a] - p[a]) }; e.offsets.popper = se({}, p, f[i]) } return e } }, offset: { order: 200, enabled: !0, fn: function (e, t) { var o, i = t.offset, n = e.placement, r = e.offsets, p = r.popper, s = r.reference, d = n.split('-')[0]; return o = R(+i) ? [+i, 0] : G(i, p, s, d), 'left' === d ? (p.top += o[0], p.left -= o[1]) : 'right' === d ? (p.top += o[0], p.left += o[1]) : 'top' === d ? (p.left += o[0], p.top -= o[1]) : 'bottom' === d && (p.left += o[0], p.top += o[1]), e.popper = p, e }, offset: 0 }, preventOverflow: { order: 300, enabled: !0, fn: function (e, t) { var o = t.boundariesElement || r(e.instance.popper); e.instance.reference === o && (o = r(o)); var i = w(e.instance.popper, e.instance.reference, t.padding, o); t.boundaries = i; var n = t.priority, p = e.offsets.popper, s = { primary: function (e) { var o = p[e]; return p[e] < i[e] && !t.escapeWithReference && (o = _(p[e], i[e])), pe({}, e, o) }, secondary: function (e) { var o = 'right' === e ? 'left' : 'top', n = p[o]; return p[e] > i[e] && !t.escapeWithReference && (n = z(p[o], i[e] - ('right' === e ? p.width : p.height))), pe({}, o, n) } }; return n.forEach(function (e) { var t = -1 === ['left', 'top'].indexOf(e) ? 'secondary' : 'primary'; p = se({}, p, s[t](e)) }), e.offsets.popper = p, e }, priority: ['left', 'right', 'top', 'bottom'], padding: 5, boundariesElement: 'scrollParent' }, keepTogether: { order: 400, enabled: !0, fn: function (e) { var t = e.offsets, o = t.popper, i = t.reference, n = e.placement.split('-')[0], r = V, p = -1 !== ['top', 'bottom'].indexOf(n), s = p ? 'right' : 'bottom', d = p ? 'left' : 'top', a = p ? 'width' : 'height'; return o[s] < r(i[d]) && (e.offsets.popper[d] = r(i[d]) - o[a]), o[d] > r(i[s]) && (e.offsets.popper[d] = r(i[s])), e } }, arrow: { order: 500, enabled: !0, fn: function (e, t) { if (!F(e.instance.modifiers, 'arrow', 'keepTogether')) return e; var o = t.element; if ('string' == typeof o) { if (o = e.instance.popper.querySelector(o), !o) return e; } else if (!e.instance.popper.contains(o)) return console.warn('WARNING: `arrow.element` must be child of its popper element!'), e; var i = e.placement.split('-')[0], n = e.offsets, r = n.popper, p = n.reference, s = -1 !== ['left', 'right'].indexOf(i), d = s ? 'height' : 'width', a = s ? 'top' : 'left', f = s ? 'left' : 'top', l = s ? 'bottom' : 'right', m = O(o)[d]; p[l] - m < r[a] && (e.offsets.popper[a] -= r[a] - (p[l] - m)), p[a] + m > r[l] && (e.offsets.popper[a] += p[a] + m - r[l]); var h = p[a] + p[d] / 2 - m / 2, g = h - c(e.offsets.popper)[a]; return g = _(z(r[d] - m, g), 0), e.arrowElement = o, e.offsets.arrow = {}, e.offsets.arrow[a] = Math.round(g), e.offsets.arrow[f] = '', e }, element: '[x-arrow]' }, flip: { order: 600, enabled: !0, fn: function (e, t) { if (W(e.instance.modifiers, 'inner')) return e; if (e.flipped && e.placement === e.originalPlacement) return e; var o = w(e.instance.popper, e.instance.reference, t.padding, t.boundariesElement), i = e.placement.split('-')[0], n = L(i), r = e.placement.split('-')[1] || '', p = []; switch (t.behavior) { case fe.FLIP: p = [i, n]; break; case fe.CLOCKWISE: p = K(i); break; case fe.COUNTERCLOCKWISE: p = K(i, !0); break; default: p = t.behavior; }return p.forEach(function (s, d) { if (i !== s || p.length === d + 1) return e; i = e.placement.split('-')[0], n = L(i); var a = e.offsets.popper, f = e.offsets.reference, l = V, m = 'left' === i && l(a.right) > l(f.left) || 'right' === i && l(a.left) < l(f.right) || 'top' === i && l(a.bottom) > l(f.top) || 'bottom' === i && l(a.top) < l(f.bottom), h = l(a.left) < l(o.left), c = l(a.right) > l(o.right), g = l(a.top) < l(o.top), u = l(a.bottom) > l(o.bottom), b = 'left' === i && h || 'right' === i && c || 'top' === i && g || 'bottom' === i && u, y = -1 !== ['top', 'bottom'].indexOf(i), w = !!t.flipVariations && (y && 'start' === r && h || y && 'end' === r && c || !y && 'start' === r && g || !y && 'end' === r && u); (m || b || w) && (e.flipped = !0, (m || b) && (i = p[d + 1]), w && (r = j(r)), e.placement = i + (r ? '-' + r : ''), e.offsets.popper = se({}, e.offsets.popper, S(e.instance.popper, e.offsets.reference, e.placement)), e = N(e.instance.modifiers, e, 'flip')) }), e }, behavior: 'flip', padding: 5, boundariesElement: 'viewport' }, inner: { order: 700, enabled: !1, fn: function (e) { var t = e.placement, o = t.split('-')[0], i = e.offsets, n = i.popper, r = i.reference, p = -1 !== ['left', 'right'].indexOf(o), s = -1 === ['top', 'left'].indexOf(o); return n[p ? 'left' : 'top'] = r[t] - (s ? n[p ? 'width' : 'height'] : 0), e.placement = L(t), e.offsets.popper = c(n), e } }, hide: { order: 800, enabled: !0, fn: function (e) { if (!F(e.instance.modifiers, 'hide', 'preventOverflow')) return e; var t = e.offsets.reference, o = T(e.instance.modifiers, function (e) { return 'preventOverflow' === e.name }).boundaries; if (t.bottom < o.top || t.left > o.right || t.top > o.bottom || t.right < o.left) { if (!0 === e.hide) return e; e.hide = !0, e.attributes['x-out-of-boundaries'] = '' } else { if (!1 === e.hide) return e; e.hide = !1, e.attributes['x-out-of-boundaries'] = !1 } return e } }, computeStyle: { order: 850, enabled: !0, fn: function (e, t) { var o = t.x, i = t.y, n = e.offsets.popper, p = T(e.instance.modifiers, function (e) { return 'applyStyle' === e.name }).gpuAcceleration; void 0 !== p && console.warn('WARNING: `gpuAcceleration` option moved to `computeStyle` modifier and will not be supported in future versions of Popper.js!'); var s, d, a = void 0 === p ? t.gpuAcceleration : p, f = r(e.instance.popper), l = g(f), m = { position: n.position }, h = { left: V(n.left), top: V(n.top), bottom: V(n.bottom), right: V(n.right) }, c = 'bottom' === o ? 'top' : 'bottom', u = 'right' === i ? 'left' : 'right', b = B('transform'); if (d = 'bottom' == c ? -l.height + h.bottom : h.top, s = 'right' == u ? -l.width + h.right : h.left, a && b) m[b] = 'translate3d(' + s + 'px, ' + d + 'px, 0)', m[c] = 0, m[u] = 0, m.willChange = 'transform'; else { var y = 'bottom' == c ? -1 : 1, w = 'right' == u ? -1 : 1; m[c] = d * y, m[u] = s * w, m.willChange = c + ', ' + u } var v = { "x-placement": e.placement }; return e.attributes = se({}, v, e.attributes), e.styles = se({}, m, e.styles), e }, gpuAcceleration: !0, x: 'bottom', y: 'right' }, applyStyle: { order: 900, enabled: !0, fn: function (e) { return U(e.instance.popper, e.styles), Y(e.instance.popper, e.attributes), e.offsets.arrow && U(e.arrowElement, e.offsets.arrow), e }, onLoad: function (e, t, o, i, n) { var r = x(n, t, e), p = E(o.placement, r, t, e, o.modifiers.flip.boundariesElement, o.modifiers.flip.padding); return t.setAttribute('x-placement', p), U(t, { position: 'absolute' }), o }, gpuAcceleration: void 0 } } }, le });
//# sourceMappingURL=popper.min.js.map
/*
 Leaflet.markercluster, Provides Beautiful Animated Marker Clustering functionality for Leaflet, a JS library for interactive maps.
 https://github.com/Leaflet/Leaflet.markercluster
 (c) 2012-2013, Dave Leaver, smartrak
*/
(function (window, document, undefined) {/*
 * L.MarkerClusterGroup extends L.FeatureGroup by clustering the markers contained within
 */

	L.MarkerClusterGroup = L.FeatureGroup.extend({

		options: {
			maxClusterRadius: 80, //A cluster will cover at most this many pixels from its center
			iconCreateFunction: null,

			spiderfyOnMaxZoom: true,
			showCoverageOnHover: true,
			zoomToBoundsOnClick: true,
			singleMarkerMode: false,

			disableClusteringAtZoom: null,

			// Setting this to false prevents the removal of any clusters outside of the viewpoint, which
			// is the default behaviour for performance reasons.
			removeOutsideVisibleBounds: true,

			// Set to false to disable all animations (zoom and spiderfy).
			// If false, option animateAddingMarkers below has no effect.
			// If L.DomUtil.TRANSITION is falsy, this option has no effect.
			animate: true,

			//Whether to animate adding markers after adding the MarkerClusterGroup to the map
			// If you are adding individual markers set to true, if adding bulk markers leave false for massive performance gains.
			animateAddingMarkers: false,

			//Increase to increase the distance away that spiderfied markers appear from the center
			spiderfyDistanceMultiplier: 1,

			// Make it possible to specify a polyline options on a spider leg
			spiderLegPolylineOptions: { weight: 1.5, color: '#222', opacity: 0.5 },

			// When bulk adding layers, adds markers in chunks. Means addLayers may not add all the layers in the call, others will be loaded during setTimeouts
			chunkedLoading: false,
			chunkInterval: 200, // process markers for a maximum of ~ n milliseconds (then trigger the chunkProgress callback)
			chunkDelay: 50, // at the end of each interval, give n milliseconds back to system/browser
			chunkProgress: null, // progress callback: function(processed, total, elapsed) (e.g. for a progress indicator)

			//Options to pass to the L.Polygon constructor
			polygonOptions: {}
		},

		initialize: function (options) {
			L.Util.setOptions(this, options);
			if (!this.options.iconCreateFunction) {
				this.options.iconCreateFunction = this._defaultIconCreateFunction;
			}
			if (!this.options.clusterPane) {
				this.options.clusterPane = L.Marker.prototype.options.pane;
			}

			this._featureGroup = L.featureGroup();
			this._featureGroup.addEventParent(this);

			this._nonPointGroup = L.featureGroup();
			this._nonPointGroup.addEventParent(this);

			this._inZoomAnimation = 0;
			this._needsClustering = [];
			this._needsRemoving = []; //Markers removed while we aren't on the map need to be kept track of
			//The bounds of the currently shown area (from _getExpandedVisibleBounds) Updated on zoom/move
			this._currentShownBounds = null;

			this._queue = [];

			this._childMarkerEventHandlers = {
				'dragstart': this._childMarkerDragStart,
				'move': this._childMarkerMoved,
				'dragend': this._childMarkerDragEnd,
			};

			// Hook the appropriate animation methods.
			var animate = L.DomUtil.TRANSITION && this.options.animate;
			L.extend(this, animate ? this._withAnimation : this._noAnimation);
			// Remember which MarkerCluster class to instantiate (animated or not).
			this._markerCluster = animate ? L.MarkerCluster : L.MarkerClusterNonAnimated;
		},

		addLayer: function (layer) {

			if (layer instanceof L.LayerGroup) {
				return this.addLayers([layer]);
			}

			//Don't cluster non point data
			if (!layer.getLatLng) {
				this._nonPointGroup.addLayer(layer);
				this.fire('layeradd', { layer: layer });
				return this;
			}

			if (!this._map) {
				this._needsClustering.push(layer);
				this.fire('layeradd', { layer: layer });
				return this;
			}

			if (this.hasLayer(layer)) {
				return this;
			}


			//If we have already clustered we'll need to add this one to a cluster

			if (this._unspiderfy) {
				this._unspiderfy();
			}

			this._addLayer(layer, this._maxZoom);
			this.fire('layeradd', { layer: layer });

			// Refresh bounds and weighted positions.
			this._topClusterLevel._recalculateBounds();

			this._refreshClustersIcons();

			//Work out what is visible
			var visibleLayer = layer,
				currentZoom = this._zoom;
			if (layer.__parent) {
				while (visibleLayer.__parent._zoom >= currentZoom) {
					visibleLayer = visibleLayer.__parent;
				}
			}

			if (this._currentShownBounds.contains(visibleLayer.getLatLng())) {
				if (this.options.animateAddingMarkers) {
					this._animationAddLayer(layer, visibleLayer);
				} else {
					this._animationAddLayerNonAnimated(layer, visibleLayer);
				}
			}
			return this;
		},

		removeLayer: function (layer) {

			if (layer instanceof L.LayerGroup) {
				return this.removeLayers([layer]);
			}

			//Non point layers
			if (!layer.getLatLng) {
				this._nonPointGroup.removeLayer(layer);
				this.fire('layerremove', { layer: layer });
				return this;
			}

			if (!this._map) {
				if (!this._arraySplice(this._needsClustering, layer) && this.hasLayer(layer)) {
					this._needsRemoving.push({ layer: layer, latlng: layer._latlng });
				}
				this.fire('layerremove', { layer: layer });
				return this;
			}

			if (!layer.__parent) {
				return this;
			}

			if (this._unspiderfy) {
				this._unspiderfy();
				this._unspiderfyLayer(layer);
			}

			//Remove the marker from clusters
			this._removeLayer(layer, true);
			this.fire('layerremove', { layer: layer });

			// Refresh bounds and weighted positions.
			this._topClusterLevel._recalculateBounds();

			this._refreshClustersIcons();

			layer.off(this._childMarkerEventHandlers, this);

			if (this._featureGroup.hasLayer(layer)) {
				this._featureGroup.removeLayer(layer);
				if (layer.clusterShow) {
					layer.clusterShow();
				}
			}

			return this;
		},

		//Takes an array of markers and adds them in bulk
		addLayers: function (layersArray, skipLayerAddEvent) {
			if (!L.Util.isArray(layersArray)) {
				return this.addLayer(layersArray);
			}

			var fg = this._featureGroup,
				npg = this._nonPointGroup,
				chunked = this.options.chunkedLoading,
				chunkInterval = this.options.chunkInterval,
				chunkProgress = this.options.chunkProgress,
				l = layersArray.length,
				offset = 0,
				originalArray = true,
				m;

			if (this._map) {
				var started = (new Date()).getTime();
				var process = L.bind(function () {
					var start = (new Date()).getTime();
					for (; offset < l; offset++) {
						if (chunked && offset % 200 === 0) {
							// every couple hundred markers, instrument the time elapsed since processing started:
							var elapsed = (new Date()).getTime() - start;
							if (elapsed > chunkInterval) {
								break; // been working too hard, time to take a break :-)
							}
						}

						m = layersArray[offset];

						// Group of layers, append children to layersArray and skip.
						// Side effects:
						// - Total increases, so chunkProgress ratio jumps backward.
						// - Groups are not included in this group, only their non-group child layers (hasLayer).
						// Changing array length while looping does not affect performance in current browsers:
						// http://jsperf.com/for-loop-changing-length/6
						if (m instanceof L.LayerGroup) {
							if (originalArray) {
								layersArray = layersArray.slice();
								originalArray = false;
							}
							this._extractNonGroupLayers(m, layersArray);
							l = layersArray.length;
							continue;
						}

						//Not point data, can't be clustered
						if (!m.getLatLng) {
							npg.addLayer(m);
							if (!skipLayerAddEvent) {
								this.fire('layeradd', { layer: m });
							}
							continue;
						}

						if (this.hasLayer(m)) {
							continue;
						}

						this._addLayer(m, this._maxZoom);
						if (!skipLayerAddEvent) {
							this.fire('layeradd', { layer: m });
						}

						//If we just made a cluster of size 2 then we need to remove the other marker from the map (if it is) or we never will
						if (m.__parent) {
							if (m.__parent.getChildCount() === 2) {
								var markers = m.__parent.getAllChildMarkers(),
									otherMarker = markers[0] === m ? markers[1] : markers[0];
								fg.removeLayer(otherMarker);
							}
						}
					}

					if (chunkProgress) {
						// report progress and time elapsed:
						chunkProgress(offset, l, (new Date()).getTime() - started);
					}

					// Completed processing all markers.
					if (offset === l) {

						// Refresh bounds and weighted positions.
						this._topClusterLevel._recalculateBounds();

						this._refreshClustersIcons();

						this._topClusterLevel._recursivelyAddChildrenToMap(null, this._zoom, this._currentShownBounds);
					} else {
						setTimeout(process, this.options.chunkDelay);
					}
				}, this);

				process();
			} else {
				var needsClustering = this._needsClustering;

				for (; offset < l; offset++) {
					m = layersArray[offset];

					// Group of layers, append children to layersArray and skip.
					if (m instanceof L.LayerGroup) {
						if (originalArray) {
							layersArray = layersArray.slice();
							originalArray = false;
						}
						this._extractNonGroupLayers(m, layersArray);
						l = layersArray.length;
						continue;
					}

					//Not point data, can't be clustered
					if (!m.getLatLng) {
						npg.addLayer(m);
						continue;
					}

					if (this.hasLayer(m)) {
						continue;
					}

					needsClustering.push(m);
				}
			}
			return this;
		},

		//Takes an array of markers and removes them in bulk
		removeLayers: function (layersArray) {
			var i, m,
				l = layersArray.length,
				fg = this._featureGroup,
				npg = this._nonPointGroup,
				originalArray = true;

			if (!this._map) {
				for (i = 0; i < l; i++) {
					m = layersArray[i];

					// Group of layers, append children to layersArray and skip.
					if (m instanceof L.LayerGroup) {
						if (originalArray) {
							layersArray = layersArray.slice();
							originalArray = false;
						}
						this._extractNonGroupLayers(m, layersArray);
						l = layersArray.length;
						continue;
					}

					this._arraySplice(this._needsClustering, m);
					npg.removeLayer(m);
					if (this.hasLayer(m)) {
						this._needsRemoving.push({ layer: m, latlng: m._latlng });
					}
					this.fire('layerremove', { layer: m });
				}
				return this;
			}

			if (this._unspiderfy) {
				this._unspiderfy();

				// Work on a copy of the array, so that next loop is not affected.
				var layersArray2 = layersArray.slice(),
					l2 = l;
				for (i = 0; i < l2; i++) {
					m = layersArray2[i];

					// Group of layers, append children to layersArray and skip.
					if (m instanceof L.LayerGroup) {
						this._extractNonGroupLayers(m, layersArray2);
						l2 = layersArray2.length;
						continue;
					}

					this._unspiderfyLayer(m);
				}
			}

			for (i = 0; i < l; i++) {
				m = layersArray[i];

				// Group of layers, append children to layersArray and skip.
				if (m instanceof L.LayerGroup) {
					if (originalArray) {
						layersArray = layersArray.slice();
						originalArray = false;
					}
					this._extractNonGroupLayers(m, layersArray);
					l = layersArray.length;
					continue;
				}

				if (!m.__parent) {
					npg.removeLayer(m);
					this.fire('layerremove', { layer: m });
					continue;
				}

				this._removeLayer(m, true, true);
				this.fire('layerremove', { layer: m });

				if (fg.hasLayer(m)) {
					fg.removeLayer(m);
					if (m.clusterShow) {
						m.clusterShow();
					}
				}
			}

			// Refresh bounds and weighted positions.
			this._topClusterLevel._recalculateBounds();

			this._refreshClustersIcons();

			//Fix up the clusters and markers on the map
			this._topClusterLevel._recursivelyAddChildrenToMap(null, this._zoom, this._currentShownBounds);

			return this;
		},

		//Removes all layers from the MarkerClusterGroup
		clearLayers: function () {
			//Need our own special implementation as the LayerGroup one doesn't work for us

			//If we aren't on the map (yet), blow away the markers we know of
			if (!this._map) {
				this._needsClustering = [];
				delete this._gridClusters;
				delete this._gridUnclustered;
			}

			if (this._noanimationUnspiderfy) {
				this._noanimationUnspiderfy();
			}

			//Remove all the visible layers
			this._featureGroup.clearLayers();
			this._nonPointGroup.clearLayers();

			this.eachLayer(function (marker) {
				marker.off(this._childMarkerEventHandlers, this);
				delete marker.__parent;
			}, this);

			if (this._map) {
				//Reset _topClusterLevel and the DistanceGrids
				this._generateInitialClusters();
			}

			return this;
		},

		//Override FeatureGroup.getBounds as it doesn't work
		getBounds: function () {
			var bounds = new L.LatLngBounds();

			if (this._topClusterLevel) {
				bounds.extend(this._topClusterLevel._bounds);
			}

			for (var i = this._needsClustering.length - 1; i >= 0; i--) {
				bounds.extend(this._needsClustering[i].getLatLng());
			}

			bounds.extend(this._nonPointGroup.getBounds());

			return bounds;
		},

		//Overrides LayerGroup.eachLayer
		eachLayer: function (method, context) {
			var markers = this._needsClustering.slice(),
				needsRemoving = this._needsRemoving,
				thisNeedsRemoving, i, j;

			if (this._topClusterLevel) {
				this._topClusterLevel.getAllChildMarkers(markers);
			}

			for (i = markers.length - 1; i >= 0; i--) {
				thisNeedsRemoving = true;

				for (j = needsRemoving.length - 1; j >= 0; j--) {
					if (needsRemoving[j].layer === markers[i]) {
						thisNeedsRemoving = false;
						break;
					}
				}

				if (thisNeedsRemoving) {
					method.call(context, markers[i]);
				}
			}

			this._nonPointGroup.eachLayer(method, context);
		},

		//Overrides LayerGroup.getLayers
		getLayers: function () {
			var layers = [];
			this.eachLayer(function (l) {
				layers.push(l);
			});
			return layers;
		},

		//Overrides LayerGroup.getLayer, WARNING: Really bad performance
		getLayer: function (id) {
			var result = null;

			id = parseInt(id, 10);

			this.eachLayer(function (l) {
				if (L.stamp(l) === id) {
					result = l;
				}
			});

			return result;
		},

		//Returns true if the given layer is in this MarkerClusterGroup
		hasLayer: function (layer) {
			if (!layer) {
				return false;
			}

			var i, anArray = this._needsClustering;

			for (i = anArray.length - 1; i >= 0; i--) {
				if (anArray[i] === layer) {
					return true;
				}
			}

			anArray = this._needsRemoving;
			for (i = anArray.length - 1; i >= 0; i--) {
				if (anArray[i].layer === layer) {
					return false;
				}
			}

			return !!(layer.__parent && layer.__parent._group === this) || this._nonPointGroup.hasLayer(layer);
		},

		//Zoom down to show the given layer (spiderfying if necessary) then calls the callback
		zoomToShowLayer: function (layer, callback) {

			if (typeof callback !== 'function') {
				callback = function () { };
			}

			var showMarker = function () {
				if ((layer._icon || layer.__parent._icon) && !this._inZoomAnimation) {
					this._map.off('moveend', showMarker, this);
					this.off('animationend', showMarker, this);

					if (layer._icon) {
						callback();
					} else if (layer.__parent._icon) {
						this.once('spiderfied', callback, this);
						layer.__parent.spiderfy();
					}
				}
			};

			if (layer._icon && this._map.getBounds().contains(layer.getLatLng())) {
				//Layer is visible ond on screen, immediate return
				callback();
			} else if (layer.__parent._zoom < Math.round(this._map._zoom)) {
				//Layer should be visible at this zoom level. It must not be on screen so just pan over to it
				this._map.on('moveend', showMarker, this);
				this._map.panTo(layer.getLatLng());
			} else {
				this._map.on('moveend', showMarker, this);
				this.on('animationend', showMarker, this);
				layer.__parent.zoomToBounds();
			}
		},

		//Overrides FeatureGroup.onAdd
		onAdd: function (map) {
			this._map = map;
			var i, l, layer;

			if (!isFinite(this._map.getMaxZoom())) {
				throw "Map has no maxZoom specified";
			}

			this._featureGroup.addTo(map);
			this._nonPointGroup.addTo(map);

			if (!this._gridClusters) {
				this._generateInitialClusters();
			}

			this._maxLat = map.options.crs.projection.MAX_LATITUDE;

			//Restore all the positions as they are in the MCG before removing them
			for (i = 0, l = this._needsRemoving.length; i < l; i++) {
				layer = this._needsRemoving[i];
				layer.newlatlng = layer.layer._latlng;
				layer.layer._latlng = layer.latlng;
			}
			//Remove them, then restore their new positions
			for (i = 0, l = this._needsRemoving.length; i < l; i++) {
				layer = this._needsRemoving[i];
				this._removeLayer(layer.layer, true);
				layer.layer._latlng = layer.newlatlng;
			}
			this._needsRemoving = [];

			//Remember the current zoom level and bounds
			this._zoom = Math.round(this._map._zoom);
			this._currentShownBounds = this._getExpandedVisibleBounds();

			this._map.on('zoomend', this._zoomEnd, this);
			this._map.on('moveend', this._moveEnd, this);

			if (this._spiderfierOnAdd) { //TODO FIXME: Not sure how to have spiderfier add something on here nicely
				this._spiderfierOnAdd();
			}

			this._bindEvents();

			//Actually add our markers to the map:
			l = this._needsClustering;
			this._needsClustering = [];
			this.addLayers(l, true);
		},

		//Overrides FeatureGroup.onRemove
		onRemove: function (map) {
			map.off('zoomend', this._zoomEnd, this);
			map.off('moveend', this._moveEnd, this);

			this._unbindEvents();

			//In case we are in a cluster animation
			this._map._mapPane.className = this._map._mapPane.className.replace(' leaflet-cluster-anim', '');

			if (this._spiderfierOnRemove) { //TODO FIXME: Not sure how to have spiderfier add something on here nicely
				this._spiderfierOnRemove();
			}

			delete this._maxLat;

			//Clean up all the layers we added to the map
			this._hideCoverage();
			this._featureGroup.remove();
			this._nonPointGroup.remove();

			this._featureGroup.clearLayers();

			this._map = null;
		},

		getVisibleParent: function (marker) {
			var vMarker = marker;
			while (vMarker && !vMarker._icon) {
				vMarker = vMarker.__parent;
			}
			return vMarker || null;
		},

		//Remove the given object from the given array
		_arraySplice: function (anArray, obj) {
			for (var i = anArray.length - 1; i >= 0; i--) {
				if (anArray[i] === obj) {
					anArray.splice(i, 1);
					return true;
				}
			}
		},

		/**
		 * Removes a marker from all _gridUnclustered zoom levels, starting at the supplied zoom.
		 * @param marker to be removed from _gridUnclustered.
		 * @param z integer bottom start zoom level (included)
		 * @private
		 */
		_removeFromGridUnclustered: function (marker, z) {
			var map = this._map,
				gridUnclustered = this._gridUnclustered,
				minZoom = Math.floor(this._map.getMinZoom());

			for (; z >= minZoom; z--) {
				if (!gridUnclustered[z].removeObject(marker, map.project(marker.getLatLng(), z))) {
					break;
				}
			}
		},

		_childMarkerDragStart: function (e) {
			e.target.__dragStart = e.target._latlng;
		},

		_childMarkerMoved: function (e) {
			if (!this._ignoreMove && !e.target.__dragStart) {
				var isPopupOpen = e.target._popup && e.target._popup.isOpen();

				this._moveChild(e.target, e.oldLatLng, e.latlng);

				if (isPopupOpen) {
					e.target.openPopup();
				}
			}
		},

		_moveChild: function (layer, from, to) {
			layer._latlng = from;
			this.removeLayer(layer);

			layer._latlng = to;
			this.addLayer(layer);
		},

		_childMarkerDragEnd: function (e) {
			if (e.target.__dragStart) {
				this._moveChild(e.target, e.target.__dragStart, e.target._latlng);
			}
			delete e.target.__dragStart;
		},


		//Internal function for removing a marker from everything.
		//dontUpdateMap: set to true if you will handle updating the map manually (for bulk functions)
		_removeLayer: function (marker, removeFromDistanceGrid, dontUpdateMap) {
			var gridClusters = this._gridClusters,
				gridUnclustered = this._gridUnclustered,
				fg = this._featureGroup,
				map = this._map,
				minZoom = Math.floor(this._map.getMinZoom());

			//Remove the marker from distance clusters it might be in
			if (removeFromDistanceGrid) {
				this._removeFromGridUnclustered(marker, this._maxZoom);
			}

			//Work our way up the clusters removing them as we go if required
			var cluster = marker.__parent,
				markers = cluster._markers,
				otherMarker;

			//Remove the marker from the immediate parents marker list
			this._arraySplice(markers, marker);

			while (cluster) {
				cluster._childCount--;
				cluster._boundsNeedUpdate = true;

				if (cluster._zoom < minZoom) {
					//Top level, do nothing
					break;
				} else if (removeFromDistanceGrid && cluster._childCount <= 1) { //Cluster no longer required
					//We need to push the other marker up to the parent
					otherMarker = cluster._markers[0] === marker ? cluster._markers[1] : cluster._markers[0];

					//Update distance grid
					gridClusters[cluster._zoom].removeObject(cluster, map.project(cluster._cLatLng, cluster._zoom));
					gridUnclustered[cluster._zoom].addObject(otherMarker, map.project(otherMarker.getLatLng(), cluster._zoom));

					//Move otherMarker up to parent
					this._arraySplice(cluster.__parent._childClusters, cluster);
					cluster.__parent._markers.push(otherMarker);
					otherMarker.__parent = cluster.__parent;

					if (cluster._icon) {
						//Cluster is currently on the map, need to put the marker on the map instead
						fg.removeLayer(cluster);
						if (!dontUpdateMap) {
							fg.addLayer(otherMarker);
						}
					}
				} else {
					cluster._iconNeedsUpdate = true;
				}

				cluster = cluster.__parent;
			}

			delete marker.__parent;
		},

		_isOrIsParent: function (el, oel) {
			while (oel) {
				if (el === oel) {
					return true;
				}
				oel = oel.parentNode;
			}
			return false;
		},

		//Override L.Evented.fire
		fire: function (type, data, propagate) {
			if (data && data.layer instanceof L.MarkerCluster) {
				//Prevent multiple clustermouseover/off events if the icon is made up of stacked divs (Doesn't work in ie <= 8, no relatedTarget)
				if (data.originalEvent && this._isOrIsParent(data.layer._icon, data.originalEvent.relatedTarget)) {
					return;
				}
				type = 'cluster' + type;
			}

			L.FeatureGroup.prototype.fire.call(this, type, data, propagate);
		},

		//Override L.Evented.listens
		listens: function (type, propagate) {
			return L.FeatureGroup.prototype.listens.call(this, type, propagate) || L.FeatureGroup.prototype.listens.call(this, 'cluster' + type, propagate);
		},

		//Default functionality
		_defaultIconCreateFunction: function (cluster) {
			var childCount = cluster.getChildCount();

			var c = ' marker-cluster-';
			if (childCount < 10) {
				c += 'small';
			} else if (childCount < 100) {
				c += 'medium';
			} else {
				c += 'large';
			}

			return new L.DivIcon({ html: '<div><span>' + childCount + '</span></div>', className: 'marker-cluster' + c, iconSize: new L.Point(40, 40) });
		},

		_bindEvents: function () {
			var map = this._map,
				spiderfyOnMaxZoom = this.options.spiderfyOnMaxZoom,
				showCoverageOnHover = this.options.showCoverageOnHover,
				zoomToBoundsOnClick = this.options.zoomToBoundsOnClick;

			//Zoom on cluster click or spiderfy if we are at the lowest level
			if (spiderfyOnMaxZoom || zoomToBoundsOnClick) {
				this.on('clusterclick', this._zoomOrSpiderfy, this);
			}

			//Show convex hull (boundary) polygon on mouse over
			if (showCoverageOnHover) {
				this.on('clustermouseover', this._showCoverage, this);
				this.on('clustermouseout', this._hideCoverage, this);
				map.on('zoomend', this._hideCoverage, this);
			}
		},

		_zoomOrSpiderfy: function (e) {
			var cluster = e.layer,
				bottomCluster = cluster;

			while (bottomCluster._childClusters.length === 1) {
				bottomCluster = bottomCluster._childClusters[0];
			}

			if (bottomCluster._zoom === this._maxZoom &&
				bottomCluster._childCount === cluster._childCount &&
				this.options.spiderfyOnMaxZoom) {

				// All child markers are contained in a single cluster from this._maxZoom to this cluster.
				cluster.spiderfy();
			} else if (this.options.zoomToBoundsOnClick) {
				cluster.zoomToBounds();
			}

			// Focus the map again for keyboard users.
			if (e.originalEvent && e.originalEvent.keyCode === 13) {
				this._map._container.focus();
			}
		},

		_showCoverage: function (e) {
			var map = this._map;
			if (this._inZoomAnimation) {
				return;
			}
			if (this._shownPolygon) {
				map.removeLayer(this._shownPolygon);
			}
			if (e.layer.getChildCount() > 2 && e.layer !== this._spiderfied) {
				this._shownPolygon = new L.Polygon(e.layer.getConvexHull(), this.options.polygonOptions);
				map.addLayer(this._shownPolygon);
			}
		},

		_hideCoverage: function () {
			if (this._shownPolygon) {
				this._map.removeLayer(this._shownPolygon);
				this._shownPolygon = null;
			}
		},

		_unbindEvents: function () {
			var spiderfyOnMaxZoom = this.options.spiderfyOnMaxZoom,
				showCoverageOnHover = this.options.showCoverageOnHover,
				zoomToBoundsOnClick = this.options.zoomToBoundsOnClick,
				map = this._map;

			if (spiderfyOnMaxZoom || zoomToBoundsOnClick) {
				this.off('clusterclick', this._zoomOrSpiderfy, this);
			}
			if (showCoverageOnHover) {
				this.off('clustermouseover', this._showCoverage, this);
				this.off('clustermouseout', this._hideCoverage, this);
				map.off('zoomend', this._hideCoverage, this);
			}
		},

		_zoomEnd: function () {
			if (!this._map) { //May have been removed from the map by a zoomEnd handler
				return;
			}
			this._mergeSplitClusters();

			this._zoom = Math.round(this._map._zoom);
			this._currentShownBounds = this._getExpandedVisibleBounds();
		},

		_moveEnd: function () {
			if (this._inZoomAnimation) {
				return;
			}

			var newBounds = this._getExpandedVisibleBounds();

			this._topClusterLevel._recursivelyRemoveChildrenFromMap(this._currentShownBounds, Math.floor(this._map.getMinZoom()), this._zoom, newBounds);
			this._topClusterLevel._recursivelyAddChildrenToMap(null, Math.round(this._map._zoom), newBounds);

			this._currentShownBounds = newBounds;
			return;
		},

		_generateInitialClusters: function () {
			var maxZoom = Math.ceil(this._map.getMaxZoom()),
				minZoom = Math.floor(this._map.getMinZoom()),
				radius = this.options.maxClusterRadius,
				radiusFn = radius;

			//If we just set maxClusterRadius to a single number, we need to create
			//a simple function to return that number. Otherwise, we just have to
			//use the function we've passed in.
			if (typeof radius !== "function") {
				radiusFn = function () { return radius; };
			}

			if (this.options.disableClusteringAtZoom !== null) {
				maxZoom = this.options.disableClusteringAtZoom - 1;
			}
			this._maxZoom = maxZoom;
			this._gridClusters = {};
			this._gridUnclustered = {};

			//Set up DistanceGrids for each zoom
			for (var zoom = maxZoom; zoom >= minZoom; zoom--) {
				this._gridClusters[zoom] = new L.DistanceGrid(radiusFn(zoom));
				this._gridUnclustered[zoom] = new L.DistanceGrid(radiusFn(zoom));
			}

			// Instantiate the appropriate L.MarkerCluster class (animated or not).
			this._topClusterLevel = new this._markerCluster(this, minZoom - 1);
		},

		//Zoom: Zoom to start adding at (Pass this._maxZoom to start at the bottom)
		_addLayer: function (layer, zoom) {
			var gridClusters = this._gridClusters,
				gridUnclustered = this._gridUnclustered,
				minZoom = Math.floor(this._map.getMinZoom()),
				markerPoint, z;

			if (this.options.singleMarkerMode) {
				this._overrideMarkerIcon(layer);
			}

			layer.on(this._childMarkerEventHandlers, this);

			//Find the lowest zoom level to slot this one in
			for (; zoom >= minZoom; zoom--) {
				markerPoint = this._map.project(layer.getLatLng(), zoom); // calculate pixel position

				//Try find a cluster close by
				var closest = gridClusters[zoom].getNearObject(markerPoint);
				if (closest) {
					closest._addChild(layer);
					layer.__parent = closest;
					return;
				}

				//Try find a marker close by to form a new cluster with
				closest = gridUnclustered[zoom].getNearObject(markerPoint);
				if (closest) {
					var parent = closest.__parent;
					if (parent) {
						this._removeLayer(closest, false);
					}

					//Create new cluster with these 2 in it

					var newCluster = new this._markerCluster(this, zoom, closest, layer);
					gridClusters[zoom].addObject(newCluster, this._map.project(newCluster._cLatLng, zoom));
					closest.__parent = newCluster;
					layer.__parent = newCluster;

					//First create any new intermediate parent clusters that don't exist
					var lastParent = newCluster;
					for (z = zoom - 1; z > parent._zoom; z--) {
						lastParent = new this._markerCluster(this, z, lastParent);
						gridClusters[z].addObject(lastParent, this._map.project(closest.getLatLng(), z));
					}
					parent._addChild(lastParent);

					//Remove closest from this zoom level and any above that it is in, replace with newCluster
					this._removeFromGridUnclustered(closest, zoom);

					return;
				}

				//Didn't manage to cluster in at this zoom, record us as a marker here and continue upwards
				gridUnclustered[zoom].addObject(layer, markerPoint);
			}

			//Didn't get in anything, add us to the top
			this._topClusterLevel._addChild(layer);
			layer.__parent = this._topClusterLevel;
			return;
		},

		/**
		 * Refreshes the icon of all "dirty" visible clusters.
		 * Non-visible "dirty" clusters will be updated when they are added to the map.
		 * @private
		 */
		_refreshClustersIcons: function () {
			this._featureGroup.eachLayer(function (c) {
				if (c instanceof L.MarkerCluster && c._iconNeedsUpdate) {
					c._updateIcon();
				}
			});
		},

		//Enqueue code to fire after the marker expand/contract has happened
		_enqueue: function (fn) {
			this._queue.push(fn);
			if (!this._queueTimeout) {
				this._queueTimeout = setTimeout(L.bind(this._processQueue, this), 300);
			}
		},
		_processQueue: function () {
			for (var i = 0; i < this._queue.length; i++) {
				this._queue[i].call(this);
			}
			this._queue.length = 0;
			clearTimeout(this._queueTimeout);
			this._queueTimeout = null;
		},

		//Merge and split any existing clusters that are too big or small
		_mergeSplitClusters: function () {
			var mapZoom = Math.round(this._map._zoom);

			//In case we are starting to split before the animation finished
			this._processQueue();

			if (this._zoom < mapZoom && this._currentShownBounds.intersects(this._getExpandedVisibleBounds())) { //Zoom in, split
				this._animationStart();
				//Remove clusters now off screen
				this._topClusterLevel._recursivelyRemoveChildrenFromMap(this._currentShownBounds, Math.floor(this._map.getMinZoom()), this._zoom, this._getExpandedVisibleBounds());

				this._animationZoomIn(this._zoom, mapZoom);

			} else if (this._zoom > mapZoom) { //Zoom out, merge
				this._animationStart();

				this._animationZoomOut(this._zoom, mapZoom);
			} else {
				this._moveEnd();
			}
		},

		//Gets the maps visible bounds expanded in each direction by the size of the screen (so the user cannot see an area we do not cover in one pan)
		_getExpandedVisibleBounds: function () {
			if (!this.options.removeOutsideVisibleBounds) {
				return this._mapBoundsInfinite;
			} else if (L.Browser.mobile) {
				return this._checkBoundsMaxLat(this._map.getBounds());
			}

			return this._checkBoundsMaxLat(this._map.getBounds().pad(1)); // Padding expands the bounds by its own dimensions but scaled with the given factor.
		},

		/**
		 * Expands the latitude to Infinity (or -Infinity) if the input bounds reach the map projection maximum defined latitude
		 * (in the case of Web/Spherical Mercator, it is 85.0511287798 / see https://en.wikipedia.org/wiki/Web_Mercator#Formulas).
		 * Otherwise, the removeOutsideVisibleBounds option will remove markers beyond that limit, whereas the same markers without
		 * this option (or outside MCG) will have their position floored (ceiled) by the projection and rendered at that limit,
		 * making the user think that MCG "eats" them and never displays them again.
		 * @param bounds L.LatLngBounds
		 * @returns {L.LatLngBounds}
		 * @private
		 */
		_checkBoundsMaxLat: function (bounds) {
			var maxLat = this._maxLat;

			if (maxLat !== undefined) {
				if (bounds.getNorth() >= maxLat) {
					bounds._northEast.lat = Infinity;
				}
				if (bounds.getSouth() <= -maxLat) {
					bounds._southWest.lat = -Infinity;
				}
			}

			return bounds;
		},

		//Shared animation code
		_animationAddLayerNonAnimated: function (layer, newCluster) {
			if (newCluster === layer) {
				this._featureGroup.addLayer(layer);
			} else if (newCluster._childCount === 2) {
				newCluster._addToMap();

				var markers = newCluster.getAllChildMarkers();
				this._featureGroup.removeLayer(markers[0]);
				this._featureGroup.removeLayer(markers[1]);
			} else {
				newCluster._updateIcon();
			}
		},

		/**
		 * Extracts individual (i.e. non-group) layers from a Layer Group.
		 * @param group to extract layers from.
		 * @param output {Array} in which to store the extracted layers.
		 * @returns {*|Array}
		 * @private
		 */
		_extractNonGroupLayers: function (group, output) {
			var layers = group.getLayers(),
				i = 0,
				layer;

			output = output || [];

			for (; i < layers.length; i++) {
				layer = layers[i];

				if (layer instanceof L.LayerGroup) {
					this._extractNonGroupLayers(layer, output);
					continue;
				}

				output.push(layer);
			}

			return output;
		},

		/**
		 * Implements the singleMarkerMode option.
		 * @param layer Marker to re-style using the Clusters iconCreateFunction.
		 * @returns {L.Icon} The newly created icon.
		 * @private
		 */
		_overrideMarkerIcon: function (layer) {
			var icon = layer.options.icon = this.options.iconCreateFunction({
				getChildCount: function () {
					return 1;
				},
				getAllChildMarkers: function () {
					return [layer];
				}
			});

			return icon;
		}
	});

	// Constant bounds used in case option "removeOutsideVisibleBounds" is set to false.
	L.MarkerClusterGroup.include({
		_mapBoundsInfinite: new L.LatLngBounds(new L.LatLng(-Infinity, -Infinity), new L.LatLng(Infinity, Infinity))
	});

	L.MarkerClusterGroup.include({
		_noAnimation: {
			//Non Animated versions of everything
			_animationStart: function () {
				//Do nothing...
			},
			_animationZoomIn: function (previousZoomLevel, newZoomLevel) {
				this._topClusterLevel._recursivelyRemoveChildrenFromMap(this._currentShownBounds, Math.floor(this._map.getMinZoom()), previousZoomLevel);
				this._topClusterLevel._recursivelyAddChildrenToMap(null, newZoomLevel, this._getExpandedVisibleBounds());

				//We didn't actually animate, but we use this event to mean "clustering animations have finished"
				this.fire('animationend');
			},
			_animationZoomOut: function (previousZoomLevel, newZoomLevel) {
				this._topClusterLevel._recursivelyRemoveChildrenFromMap(this._currentShownBounds, Math.floor(this._map.getMinZoom()), previousZoomLevel);
				this._topClusterLevel._recursivelyAddChildrenToMap(null, newZoomLevel, this._getExpandedVisibleBounds());

				//We didn't actually animate, but we use this event to mean "clustering animations have finished"
				this.fire('animationend');
			},
			_animationAddLayer: function (layer, newCluster) {
				this._animationAddLayerNonAnimated(layer, newCluster);
			}
		},

		_withAnimation: {
			//Animated versions here
			_animationStart: function () {
				this._map._mapPane.className += ' leaflet-cluster-anim';
				this._inZoomAnimation++;
			},

			_animationZoomIn: function (previousZoomLevel, newZoomLevel) {
				var bounds = this._getExpandedVisibleBounds(),
					fg = this._featureGroup,
					minZoom = Math.floor(this._map.getMinZoom()),
					i;

				this._ignoreMove = true;

				//Add all children of current clusters to map and remove those clusters from map
				this._topClusterLevel._recursively(bounds, previousZoomLevel, minZoom, function (c) {
					var startPos = c._latlng,
						markers = c._markers,
						m;

					if (!bounds.contains(startPos)) {
						startPos = null;
					}

					if (c._isSingleParent() && previousZoomLevel + 1 === newZoomLevel) { //Immediately add the new child and remove us
						fg.removeLayer(c);
						c._recursivelyAddChildrenToMap(null, newZoomLevel, bounds);
					} else {
						//Fade out old cluster
						c.clusterHide();
						c._recursivelyAddChildrenToMap(startPos, newZoomLevel, bounds);
					}

					//Remove all markers that aren't visible any more
					//TODO: Do we actually need to do this on the higher levels too?
					for (i = markers.length - 1; i >= 0; i--) {
						m = markers[i];
						if (!bounds.contains(m._latlng)) {
							fg.removeLayer(m);
						}
					}

				});

				this._forceLayout();

				//Update opacities
				this._topClusterLevel._recursivelyBecomeVisible(bounds, newZoomLevel);
				//TODO Maybe? Update markers in _recursivelyBecomeVisible
				fg.eachLayer(function (n) {
					if (!(n instanceof L.MarkerCluster) && n._icon) {
						n.clusterShow();
					}
				});

				//update the positions of the just added clusters/markers
				this._topClusterLevel._recursively(bounds, previousZoomLevel, newZoomLevel, function (c) {
					c._recursivelyRestoreChildPositions(newZoomLevel);
				});

				this._ignoreMove = false;

				//Remove the old clusters and close the zoom animation
				this._enqueue(function () {
					//update the positions of the just added clusters/markers
					this._topClusterLevel._recursively(bounds, previousZoomLevel, minZoom, function (c) {
						fg.removeLayer(c);
						c.clusterShow();
					});

					this._animationEnd();
				});
			},

			_animationZoomOut: function (previousZoomLevel, newZoomLevel) {
				this._animationZoomOutSingle(this._topClusterLevel, previousZoomLevel - 1, newZoomLevel);

				//Need to add markers for those that weren't on the map before but are now
				this._topClusterLevel._recursivelyAddChildrenToMap(null, newZoomLevel, this._getExpandedVisibleBounds());
				//Remove markers that were on the map before but won't be now
				this._topClusterLevel._recursivelyRemoveChildrenFromMap(this._currentShownBounds, Math.floor(this._map.getMinZoom()), previousZoomLevel, this._getExpandedVisibleBounds());
			},

			_animationAddLayer: function (layer, newCluster) {
				var me = this,
					fg = this._featureGroup;

				fg.addLayer(layer);
				if (newCluster !== layer) {
					if (newCluster._childCount > 2) { //Was already a cluster

						newCluster._updateIcon();
						this._forceLayout();
						this._animationStart();

						layer._setPos(this._map.latLngToLayerPoint(newCluster.getLatLng()));
						layer.clusterHide();

						this._enqueue(function () {
							fg.removeLayer(layer);
							layer.clusterShow();

							me._animationEnd();
						});

					} else { //Just became a cluster
						this._forceLayout();

						me._animationStart();
						me._animationZoomOutSingle(newCluster, this._map.getMaxZoom(), this._zoom);
					}
				}
			}
		},

		// Private methods for animated versions.
		_animationZoomOutSingle: function (cluster, previousZoomLevel, newZoomLevel) {
			var bounds = this._getExpandedVisibleBounds(),
				minZoom = Math.floor(this._map.getMinZoom());

			//Animate all of the markers in the clusters to move to their cluster center point
			cluster._recursivelyAnimateChildrenInAndAddSelfToMap(bounds, minZoom, previousZoomLevel + 1, newZoomLevel);

			var me = this;

			//Update the opacity (If we immediately set it they won't animate)
			this._forceLayout();
			cluster._recursivelyBecomeVisible(bounds, newZoomLevel);

			//TODO: Maybe use the transition timing stuff to make this more reliable
			//When the animations are done, tidy up
			this._enqueue(function () {

				//This cluster stopped being a cluster before the timeout fired
				if (cluster._childCount === 1) {
					var m = cluster._markers[0];
					//If we were in a cluster animation at the time then the opacity and position of our child could be wrong now, so fix it
					this._ignoreMove = true;
					m.setLatLng(m.getLatLng());
					this._ignoreMove = false;
					if (m.clusterShow) {
						m.clusterShow();
					}
				} else {
					cluster._recursively(bounds, newZoomLevel, minZoom, function (c) {
						c._recursivelyRemoveChildrenFromMap(bounds, minZoom, previousZoomLevel + 1);
					});
				}
				me._animationEnd();
			});
		},

		_animationEnd: function () {
			if (this._map) {
				this._map._mapPane.className = this._map._mapPane.className.replace(' leaflet-cluster-anim', '');
			}
			this._inZoomAnimation--;
			this.fire('animationend');
		},

		//Force a browser layout of stuff in the map
		// Should apply the current opacity and location to all elements so we can update them again for an animation
		_forceLayout: function () {
			//In my testing this works, infact offsetWidth of any element seems to work.
			//Could loop all this._layers and do this for each _icon if it stops working

			L.Util.falseFn(document.body.offsetWidth);
		}
	});

	L.markerClusterGroup = function (options) {
		return new L.MarkerClusterGroup(options);
	};


	L.MarkerCluster = L.Marker.extend({
		initialize: function (group, zoom, a, b) {

			L.Marker.prototype.initialize.call(this, a ? (a._cLatLng || a.getLatLng()) : new L.LatLng(0, 0),
				{ icon: this, pane: group.options.clusterPane });

			this._group = group;
			this._zoom = zoom;

			this._markers = [];
			this._childClusters = [];
			this._childCount = 0;
			this._iconNeedsUpdate = true;
			this._boundsNeedUpdate = true;

			this._bounds = new L.LatLngBounds();

			if (a) {
				this._addChild(a);
			}
			if (b) {
				this._addChild(b);
			}
		},

		//Recursively retrieve all child markers of this cluster
		getAllChildMarkers: function (storageArray) {
			storageArray = storageArray || [];

			for (var i = this._childClusters.length - 1; i >= 0; i--) {
				this._childClusters[i].getAllChildMarkers(storageArray);
			}

			for (var j = this._markers.length - 1; j >= 0; j--) {
				storageArray.push(this._markers[j]);
			}

			return storageArray;
		},

		//Returns the count of how many child markers we have
		getChildCount: function () {
			return this._childCount;
		},

		//Zoom to the minimum of showing all of the child markers, or the extents of this cluster
		zoomToBounds: function (fitBoundsOptions) {
			var childClusters = this._childClusters.slice(),
				map = this._group._map,
				boundsZoom = map.getBoundsZoom(this._bounds),
				zoom = this._zoom + 1,
				mapZoom = map.getZoom(),
				i;

			//calculate how far we need to zoom down to see all of the markers
			while (childClusters.length > 0 && boundsZoom > zoom) {
				zoom++;
				var newClusters = [];
				for (i = 0; i < childClusters.length; i++) {
					newClusters = newClusters.concat(childClusters[i]._childClusters);
				}
				childClusters = newClusters;
			}

			if (boundsZoom > zoom) {
				this._group._map.setView(this._latlng, zoom);
			} else if (boundsZoom <= mapZoom) { //If fitBounds wouldn't zoom us down, zoom us down instead
				this._group._map.setView(this._latlng, mapZoom + 1);
			} else {
				this._group._map.fitBounds(this._bounds, fitBoundsOptions);
			}
		},

		getBounds: function () {
			var bounds = new L.LatLngBounds();
			bounds.extend(this._bounds);
			return bounds;
		},

		_updateIcon: function () {
			this._iconNeedsUpdate = true;
			if (this._icon) {
				this.setIcon(this);
			}
		},

		//Cludge for Icon, we pretend to be an icon for performance
		createIcon: function () {
			if (this._iconNeedsUpdate) {
				this._iconObj = this._group.options.iconCreateFunction(this);
				this._iconNeedsUpdate = false;
			}
			return this._iconObj.createIcon();
		},
		createShadow: function () {
			return this._iconObj.createShadow();
		},


		_addChild: function (new1, isNotificationFromChild) {

			this._iconNeedsUpdate = true;

			this._boundsNeedUpdate = true;
			this._setClusterCenter(new1);

			if (new1 instanceof L.MarkerCluster) {
				if (!isNotificationFromChild) {
					this._childClusters.push(new1);
					new1.__parent = this;
				}
				this._childCount += new1._childCount;
			} else {
				if (!isNotificationFromChild) {
					this._markers.push(new1);
				}
				this._childCount++;
			}

			if (this.__parent) {
				this.__parent._addChild(new1, true);
			}
		},

		/**
		 * Makes sure the cluster center is set. If not, uses the child center if it is a cluster, or the marker position.
		 * @param child L.MarkerCluster|L.Marker that will be used as cluster center if not defined yet.
		 * @private
		 */
		_setClusterCenter: function (child) {
			if (!this._cLatLng) {
				// when clustering, take position of the first point as the cluster center
				this._cLatLng = child._cLatLng || child._latlng;
			}
		},

		/**
		 * Assigns impossible bounding values so that the next extend entirely determines the new bounds.
		 * This method avoids having to trash the previous L.LatLngBounds object and to create a new one, which is much slower for this class.
		 * As long as the bounds are not extended, most other methods would probably fail, as they would with bounds initialized but not extended.
		 * @private
		 */
		_resetBounds: function () {
			var bounds = this._bounds;

			if (bounds._southWest) {
				bounds._southWest.lat = Infinity;
				bounds._southWest.lng = Infinity;
			}
			if (bounds._northEast) {
				bounds._northEast.lat = -Infinity;
				bounds._northEast.lng = -Infinity;
			}
		},

		_recalculateBounds: function () {
			var markers = this._markers,
				childClusters = this._childClusters,
				latSum = 0,
				lngSum = 0,
				totalCount = this._childCount,
				i, child, childLatLng, childCount;

			// Case where all markers are removed from the map and we are left with just an empty _topClusterLevel.
			if (totalCount === 0) {
				return;
			}

			// Reset rather than creating a new object, for performance.
			this._resetBounds();

			// Child markers.
			for (i = 0; i < markers.length; i++) {
				childLatLng = markers[i]._latlng;

				this._bounds.extend(childLatLng);

				latSum += childLatLng.lat;
				lngSum += childLatLng.lng;
			}

			// Child clusters.
			for (i = 0; i < childClusters.length; i++) {
				child = childClusters[i];

				// Re-compute child bounds and weighted position first if necessary.
				if (child._boundsNeedUpdate) {
					child._recalculateBounds();
				}

				this._bounds.extend(child._bounds);

				childLatLng = child._wLatLng;
				childCount = child._childCount;

				latSum += childLatLng.lat * childCount;
				lngSum += childLatLng.lng * childCount;
			}

			this._latlng = this._wLatLng = new L.LatLng(latSum / totalCount, lngSum / totalCount);

			// Reset dirty flag.
			this._boundsNeedUpdate = false;
		},

		//Set our markers position as given and add it to the map
		_addToMap: function (startPos) {
			if (startPos) {
				this._backupLatlng = this._latlng;
				this.setLatLng(startPos);
			}
			this._group._featureGroup.addLayer(this);
		},

		_recursivelyAnimateChildrenIn: function (bounds, center, maxZoom) {
			this._recursively(bounds, this._group._map.getMinZoom(), maxZoom - 1,
				function (c) {
					var markers = c._markers,
						i, m;
					for (i = markers.length - 1; i >= 0; i--) {
						m = markers[i];

						//Only do it if the icon is still on the map
						if (m._icon) {
							m._setPos(center);
							m.clusterHide();
						}
					}
				},
				function (c) {
					var childClusters = c._childClusters,
						j, cm;
					for (j = childClusters.length - 1; j >= 0; j--) {
						cm = childClusters[j];
						if (cm._icon) {
							cm._setPos(center);
							cm.clusterHide();
						}
					}
				}
			);
		},

		_recursivelyAnimateChildrenInAndAddSelfToMap: function (bounds, mapMinZoom, previousZoomLevel, newZoomLevel) {
			this._recursively(bounds, newZoomLevel, mapMinZoom,
				function (c) {
					c._recursivelyAnimateChildrenIn(bounds, c._group._map.latLngToLayerPoint(c.getLatLng()).round(), previousZoomLevel);

					//TODO: depthToAnimateIn affects _isSingleParent, if there is a multizoom we may/may not be.
					//As a hack we only do a animation free zoom on a single level zoom, if someone does multiple levels then we always animate
					if (c._isSingleParent() && previousZoomLevel - 1 === newZoomLevel) {
						c.clusterShow();
						c._recursivelyRemoveChildrenFromMap(bounds, mapMinZoom, previousZoomLevel); //Immediately remove our children as we are replacing them. TODO previousBounds not bounds
					} else {
						c.clusterHide();
					}

					c._addToMap();
				}
			);
		},

		_recursivelyBecomeVisible: function (bounds, zoomLevel) {
			this._recursively(bounds, this._group._map.getMinZoom(), zoomLevel, null, function (c) {
				c.clusterShow();
			});
		},

		_recursivelyAddChildrenToMap: function (startPos, zoomLevel, bounds) {
			this._recursively(bounds, this._group._map.getMinZoom() - 1, zoomLevel,
				function (c) {
					if (zoomLevel === c._zoom) {
						return;
					}

					//Add our child markers at startPos (so they can be animated out)
					for (var i = c._markers.length - 1; i >= 0; i--) {
						var nm = c._markers[i];

						if (!bounds.contains(nm._latlng)) {
							continue;
						}

						if (startPos) {
							nm._backupLatlng = nm.getLatLng();

							nm.setLatLng(startPos);
							if (nm.clusterHide) {
								nm.clusterHide();
							}
						}

						c._group._featureGroup.addLayer(nm);
					}
				},
				function (c) {
					c._addToMap(startPos);
				}
			);
		},

		_recursivelyRestoreChildPositions: function (zoomLevel) {
			//Fix positions of child markers
			for (var i = this._markers.length - 1; i >= 0; i--) {
				var nm = this._markers[i];
				if (nm._backupLatlng) {
					nm.setLatLng(nm._backupLatlng);
					delete nm._backupLatlng;
				}
			}

			if (zoomLevel - 1 === this._zoom) {
				//Reposition child clusters
				for (var j = this._childClusters.length - 1; j >= 0; j--) {
					this._childClusters[j]._restorePosition();
				}
			} else {
				for (var k = this._childClusters.length - 1; k >= 0; k--) {
					this._childClusters[k]._recursivelyRestoreChildPositions(zoomLevel);
				}
			}
		},

		_restorePosition: function () {
			if (this._backupLatlng) {
				this.setLatLng(this._backupLatlng);
				delete this._backupLatlng;
			}
		},

		//exceptBounds: If set, don't remove any markers/clusters in it
		_recursivelyRemoveChildrenFromMap: function (previousBounds, mapMinZoom, zoomLevel, exceptBounds) {
			var m, i;
			this._recursively(previousBounds, mapMinZoom - 1, zoomLevel - 1,
				function (c) {
					//Remove markers at every level
					for (i = c._markers.length - 1; i >= 0; i--) {
						m = c._markers[i];
						if (!exceptBounds || !exceptBounds.contains(m._latlng)) {
							c._group._featureGroup.removeLayer(m);
							if (m.clusterShow) {
								m.clusterShow();
							}
						}
					}
				},
				function (c) {
					//Remove child clusters at just the bottom level
					for (i = c._childClusters.length - 1; i >= 0; i--) {
						m = c._childClusters[i];
						if (!exceptBounds || !exceptBounds.contains(m._latlng)) {
							c._group._featureGroup.removeLayer(m);
							if (m.clusterShow) {
								m.clusterShow();
							}
						}
					}
				}
			);
		},

		//Run the given functions recursively to this and child clusters
		// boundsToApplyTo: a L.LatLngBounds representing the bounds of what clusters to recurse in to
		// zoomLevelToStart: zoom level to start running functions (inclusive)
		// zoomLevelToStop: zoom level to stop running functions (inclusive)
		// runAtEveryLevel: function that takes an L.MarkerCluster as an argument that should be applied on every level
		// runAtBottomLevel: function that takes an L.MarkerCluster as an argument that should be applied at only the bottom level
		_recursively: function (boundsToApplyTo, zoomLevelToStart, zoomLevelToStop, runAtEveryLevel, runAtBottomLevel) {
			var childClusters = this._childClusters,
				zoom = this._zoom,
				i, c;

			if (zoomLevelToStart <= zoom) {
				if (runAtEveryLevel) {
					runAtEveryLevel(this);
				}
				if (runAtBottomLevel && zoom === zoomLevelToStop) {
					runAtBottomLevel(this);
				}
			}

			if (zoom < zoomLevelToStart || zoom < zoomLevelToStop) {
				for (i = childClusters.length - 1; i >= 0; i--) {
					c = childClusters[i];
					if (boundsToApplyTo.intersects(c._bounds)) {
						c._recursively(boundsToApplyTo, zoomLevelToStart, zoomLevelToStop, runAtEveryLevel, runAtBottomLevel);
					}
				}
			}
		},

		//Returns true if we are the parent of only one cluster and that cluster is the same as us
		_isSingleParent: function () {
			//Don't need to check this._markers as the rest won't work if there are any
			return this._childClusters.length > 0 && this._childClusters[0]._childCount === this._childCount;
		}
	});



	/*
	* Extends L.Marker to include two extra methods: clusterHide and clusterShow.
	* 
	* They work as setOpacity(0) and setOpacity(1) respectively, but
	* they will remember the marker's opacity when hiding and showing it again.
	* 
	*/


	L.Marker.include({

		clusterHide: function () {
			this.options.opacityWhenUnclustered = this.options.opacity || 1;
			return this.setOpacity(0);
		},

		clusterShow: function () {
			var ret = this.setOpacity(this.options.opacity || this.options.opacityWhenUnclustered);
			delete this.options.opacityWhenUnclustered;
			return ret;
		}

	});





	L.DistanceGrid = function (cellSize) {
		this._cellSize = cellSize;
		this._sqCellSize = cellSize * cellSize;
		this._grid = {};
		this._objectPoint = {};
	};

	L.DistanceGrid.prototype = {

		addObject: function (obj, point) {
			var x = this._getCoord(point.x),
				y = this._getCoord(point.y),
				grid = this._grid,
				row = grid[y] = grid[y] || {},
				cell = row[x] = row[x] || [],
				stamp = L.Util.stamp(obj);

			this._objectPoint[stamp] = point;

			cell.push(obj);
		},

		updateObject: function (obj, point) {
			this.removeObject(obj);
			this.addObject(obj, point);
		},

		//Returns true if the object was found
		removeObject: function (obj, point) {
			var x = this._getCoord(point.x),
				y = this._getCoord(point.y),
				grid = this._grid,
				row = grid[y] = grid[y] || {},
				cell = row[x] = row[x] || [],
				i, len;

			delete this._objectPoint[L.Util.stamp(obj)];

			for (i = 0, len = cell.length; i < len; i++) {
				if (cell[i] === obj) {

					cell.splice(i, 1);

					if (len === 1) {
						delete row[x];
					}

					return true;
				}
			}

		},

		eachObject: function (fn, context) {
			var i, j, k, len, row, cell, removed,
				grid = this._grid;

			for (i in grid) {
				row = grid[i];

				for (j in row) {
					cell = row[j];

					for (k = 0, len = cell.length; k < len; k++) {
						removed = fn.call(context, cell[k]);
						if (removed) {
							k--;
							len--;
						}
					}
				}
			}
		},

		getNearObject: function (point) {
			var x = this._getCoord(point.x),
				y = this._getCoord(point.y),
				i, j, k, row, cell, len, obj, dist,
				objectPoint = this._objectPoint,
				closestDistSq = this._sqCellSize,
				closest = null;

			for (i = y - 1; i <= y + 1; i++) {
				row = this._grid[i];
				if (row) {

					for (j = x - 1; j <= x + 1; j++) {
						cell = row[j];
						if (cell) {

							for (k = 0, len = cell.length; k < len; k++) {
								obj = cell[k];
								dist = this._sqDist(objectPoint[L.Util.stamp(obj)], point);
								if (dist < closestDistSq) {
									closestDistSq = dist;
									closest = obj;
								}
							}
						}
					}
				}
			}
			return closest;
		},

		_getCoord: function (x) {
			return Math.floor(x / this._cellSize);
		},

		_sqDist: function (p, p2) {
			var dx = p2.x - p.x,
				dy = p2.y - p.y;
			return dx * dx + dy * dy;
		}
	};


	/* Copyright (c) 2012 the authors listed at the following URL, and/or
	the authors of referenced articles or incorporated external code:
	http://en.literateprograms.org/Quickhull_(Javascript)?action=history&offset=20120410175256
	
	Permission is hereby granted, free of charge, to any person obtaining
	a copy of this software and associated documentation files (the
	"Software"), to deal in the Software without restriction, including
	without limitation the rights to use, copy, modify, merge, publish,
	distribute, sublicense, and/or sell copies of the Software, and to
	permit persons to whom the Software is furnished to do so, subject to
	the following conditions:
	
	The above copyright notice and this permission notice shall be
	included in all copies or substantial portions of the Software.
	
	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
	EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
	MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
	IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
	CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
	TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
	SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
	
	Retrieved from: http://en.literateprograms.org/Quickhull_(Javascript)?oldid=18434
	*/

	(function () {
		L.QuickHull = {

			/*
			 * @param {Object} cpt a point to be measured from the baseline
			 * @param {Array} bl the baseline, as represented by a two-element
			 *   array of latlng objects.
			 * @returns {Number} an approximate distance measure
			 */
			getDistant: function (cpt, bl) {
				var vY = bl[1].lat - bl[0].lat,
					vX = bl[0].lng - bl[1].lng;
				return (vX * (cpt.lat - bl[0].lat) + vY * (cpt.lng - bl[0].lng));
			},

			/*
			 * @param {Array} baseLine a two-element array of latlng objects
			 *   representing the baseline to project from
			 * @param {Array} latLngs an array of latlng objects
			 * @returns {Object} the maximum point and all new points to stay
			 *   in consideration for the hull.
			 */
			findMostDistantPointFromBaseLine: function (baseLine, latLngs) {
				var maxD = 0,
					maxPt = null,
					newPoints = [],
					i, pt, d;

				for (i = latLngs.length - 1; i >= 0; i--) {
					pt = latLngs[i];
					d = this.getDistant(pt, baseLine);

					if (d > 0) {
						newPoints.push(pt);
					} else {
						continue;
					}

					if (d > maxD) {
						maxD = d;
						maxPt = pt;
					}
				}

				return { maxPoint: maxPt, newPoints: newPoints };
			},


			/*
			 * Given a baseline, compute the convex hull of latLngs as an array
			 * of latLngs.
			 *
			 * @param {Array} latLngs
			 * @returns {Array}
			 */
			buildConvexHull: function (baseLine, latLngs) {
				var convexHullBaseLines = [],
					t = this.findMostDistantPointFromBaseLine(baseLine, latLngs);

				if (t.maxPoint) { // if there is still a point "outside" the base line
					convexHullBaseLines =
						convexHullBaseLines.concat(
							this.buildConvexHull([baseLine[0], t.maxPoint], t.newPoints)
						);
					convexHullBaseLines =
						convexHullBaseLines.concat(
							this.buildConvexHull([t.maxPoint, baseLine[1]], t.newPoints)
						);
					return convexHullBaseLines;
				} else {  // if there is no more point "outside" the base line, the current base line is part of the convex hull
					return [baseLine[0]];
				}
			},

			/*
			 * Given an array of latlngs, compute a convex hull as an array
			 * of latlngs
			 *
			 * @param {Array} latLngs
			 * @returns {Array}
			 */
			getConvexHull: function (latLngs) {
				// find first baseline
				var maxLat = false, minLat = false,
					maxLng = false, minLng = false,
					maxLatPt = null, minLatPt = null,
					maxLngPt = null, minLngPt = null,
					maxPt = null, minPt = null,
					i;

				for (i = latLngs.length - 1; i >= 0; i--) {
					var pt = latLngs[i];
					if (maxLat === false || pt.lat > maxLat) {
						maxLatPt = pt;
						maxLat = pt.lat;
					}
					if (minLat === false || pt.lat < minLat) {
						minLatPt = pt;
						minLat = pt.lat;
					}
					if (maxLng === false || pt.lng > maxLng) {
						maxLngPt = pt;
						maxLng = pt.lng;
					}
					if (minLng === false || pt.lng < minLng) {
						minLngPt = pt;
						minLng = pt.lng;
					}
				}

				if (minLat !== maxLat) {
					minPt = minLatPt;
					maxPt = maxLatPt;
				} else {
					minPt = minLngPt;
					maxPt = maxLngPt;
				}

				var ch = [].concat(this.buildConvexHull([minPt, maxPt], latLngs),
					this.buildConvexHull([maxPt, minPt], latLngs));
				return ch;
			}
		};
	}());

	L.MarkerCluster.include({
		getConvexHull: function () {
			var childMarkers = this.getAllChildMarkers(),
				points = [],
				p, i;

			for (i = childMarkers.length - 1; i >= 0; i--) {
				p = childMarkers[i].getLatLng();
				points.push(p);
			}

			return L.QuickHull.getConvexHull(points);
		}
	});


	//This code is 100% based on https://github.com/jawj/OverlappingMarkerSpiderfier-Leaflet
	//Huge thanks to jawj for implementing it first to make my job easy :-)

	L.MarkerCluster.include({

		_2PI: Math.PI * 2,
		_circleFootSeparation: 25, //related to circumference of circle
		_circleStartAngle: Math.PI / 6,

		_spiralFootSeparation: 28, //related to size of spiral (experiment!)
		_spiralLengthStart: 11,
		_spiralLengthFactor: 5,

		_circleSpiralSwitchover: 9, //show spiral instead of circle from this marker count upwards.
		// 0 -> always spiral; Infinity -> always circle

		spiderfy: function () {
			if (this._group._spiderfied === this || this._group._inZoomAnimation) {
				return;
			}

			var childMarkers = this.getAllChildMarkers(),
				group = this._group,
				map = group._map,
				center = map.latLngToLayerPoint(this._latlng),
				positions;

			this._group._unspiderfy();
			this._group._spiderfied = this;

			//TODO Maybe: childMarkers order by distance to center

			if (childMarkers.length >= this._circleSpiralSwitchover) {
				positions = this._generatePointsSpiral(childMarkers.length, center);
			} else {
				center.y += 10; // Otherwise circles look wrong => hack for standard blue icon, renders differently for other icons.
				positions = this._generatePointsCircle(childMarkers.length, center);
			}

			this._animationSpiderfy(childMarkers, positions);
		},

		unspiderfy: function (zoomDetails) {
			/// <param Name="zoomDetails">Argument from zoomanim if being called in a zoom animation or null otherwise</param>
			if (this._group._inZoomAnimation) {
				return;
			}
			this._animationUnspiderfy(zoomDetails);

			this._group._spiderfied = null;
		},

		_generatePointsCircle: function (count, centerPt) {
			var circumference = this._group.options.spiderfyDistanceMultiplier * this._circleFootSeparation * (2 + count),
				legLength = circumference / this._2PI,  //radius from circumference
				angleStep = this._2PI / count,
				res = [],
				i, angle;

			res.length = count;

			for (i = count - 1; i >= 0; i--) {
				angle = this._circleStartAngle + i * angleStep;
				res[i] = new L.Point(centerPt.x + legLength * Math.cos(angle), centerPt.y + legLength * Math.sin(angle))._round();
			}

			return res;
		},

		_generatePointsSpiral: function (count, centerPt) {
			var spiderfyDistanceMultiplier = this._group.options.spiderfyDistanceMultiplier,
				legLength = spiderfyDistanceMultiplier * this._spiralLengthStart,
				separation = spiderfyDistanceMultiplier * this._spiralFootSeparation,
				lengthFactor = spiderfyDistanceMultiplier * this._spiralLengthFactor * this._2PI,
				angle = 0,
				res = [],
				i;

			res.length = count;

			// Higher index, closer position to cluster center.
			for (i = count - 1; i >= 0; i--) {
				angle += separation / legLength + i * 0.0005;
				res[i] = new L.Point(centerPt.x + legLength * Math.cos(angle), centerPt.y + legLength * Math.sin(angle))._round();
				legLength += lengthFactor / angle;
			}
			return res;
		},

		_noanimationUnspiderfy: function () {
			var group = this._group,
				map = group._map,
				fg = group._featureGroup,
				childMarkers = this.getAllChildMarkers(),
				m, i;

			group._ignoreMove = true;

			this.setOpacity(1);
			for (i = childMarkers.length - 1; i >= 0; i--) {
				m = childMarkers[i];

				fg.removeLayer(m);

				if (m._preSpiderfyLatlng) {
					m.setLatLng(m._preSpiderfyLatlng);
					delete m._preSpiderfyLatlng;
				}
				if (m.setZIndexOffset) {
					m.setZIndexOffset(0);
				}

				if (m._spiderLeg) {
					map.removeLayer(m._spiderLeg);
					delete m._spiderLeg;
				}
			}

			group.fire('unspiderfied', {
				cluster: this,
				markers: childMarkers
			});
			group._ignoreMove = false;
			group._spiderfied = null;
		}
	});

	//Non Animated versions of everything
	L.MarkerClusterNonAnimated = L.MarkerCluster.extend({
		_animationSpiderfy: function (childMarkers, positions) {
			var group = this._group,
				map = group._map,
				fg = group._featureGroup,
				legOptions = this._group.options.spiderLegPolylineOptions,
				i, m, leg, newPos;

			group._ignoreMove = true;

			// Traverse in ascending order to make sure that inner circleMarkers are on top of further legs. Normal markers are re-ordered by newPosition.
			// The reverse order trick no longer improves performance on modern browsers.
			for (i = 0; i < childMarkers.length; i++) {
				newPos = map.layerPointToLatLng(positions[i]);
				m = childMarkers[i];

				// Add the leg before the marker, so that in case the latter is a circleMarker, the leg is behind it.
				leg = new L.Polyline([this._latlng, newPos], legOptions);
				map.addLayer(leg);
				m._spiderLeg = leg;

				// Now add the marker.
				m._preSpiderfyLatlng = m._latlng;
				m.setLatLng(newPos);
				if (m.setZIndexOffset) {
					m.setZIndexOffset(1000000); //Make these appear on top of EVERYTHING
				}

				fg.addLayer(m);
			}
			this.setOpacity(0.3);

			group._ignoreMove = false;
			group.fire('spiderfied', {
				cluster: this,
				markers: childMarkers
			});
		},

		_animationUnspiderfy: function () {
			this._noanimationUnspiderfy();
		}
	});

	//Animated versions here
	L.MarkerCluster.include({

		_animationSpiderfy: function (childMarkers, positions) {
			var me = this,
				group = this._group,
				map = group._map,
				fg = group._featureGroup,
				thisLayerLatLng = this._latlng,
				thisLayerPos = map.latLngToLayerPoint(thisLayerLatLng),
				svg = L.Path.SVG,
				legOptions = L.extend({}, this._group.options.spiderLegPolylineOptions), // Copy the options so that we can modify them for animation.
				finalLegOpacity = legOptions.opacity,
				i, m, leg, legPath, legLength, newPos;

			if (finalLegOpacity === undefined) {
				finalLegOpacity = L.MarkerClusterGroup.prototype.options.spiderLegPolylineOptions.opacity;
			}

			if (svg) {
				// If the initial opacity of the spider leg is not 0 then it appears before the animation starts.
				legOptions.opacity = 0;

				// Add the class for CSS transitions.
				legOptions.className = (legOptions.className || '') + ' leaflet-cluster-spider-leg';
			} else {
				// Make sure we have a defined opacity.
				legOptions.opacity = finalLegOpacity;
			}

			group._ignoreMove = true;

			// Add markers and spider legs to map, hidden at our center point.
			// Traverse in ascending order to make sure that inner circleMarkers are on top of further legs. Normal markers are re-ordered by newPosition.
			// The reverse order trick no longer improves performance on modern browsers.
			for (i = 0; i < childMarkers.length; i++) {
				m = childMarkers[i];

				newPos = map.layerPointToLatLng(positions[i]);

				// Add the leg before the marker, so that in case the latter is a circleMarker, the leg is behind it.
				leg = new L.Polyline([thisLayerLatLng, newPos], legOptions);
				map.addLayer(leg);
				m._spiderLeg = leg;

				// Explanations: https://jakearchibald.com/2013/animated-line-drawing-svg/
				// In our case the transition property is declared in the CSS file.
				if (svg) {
					legPath = leg._path;
					legLength = legPath.getTotalLength() + 0.1; // Need a small extra length to avoid remaining dot in Firefox.
					legPath.style.strokeDasharray = legLength; // Just 1 length is enough, it will be duplicated.
					legPath.style.strokeDashoffset = legLength;
				}

				// If it is a marker, add it now and we'll animate it out
				if (m.setZIndexOffset) {
					m.setZIndexOffset(1000000); // Make normal markers appear on top of EVERYTHING
				}
				if (m.clusterHide) {
					m.clusterHide();
				}

				// Vectors just get immediately added
				fg.addLayer(m);

				if (m._setPos) {
					m._setPos(thisLayerPos);
				}
			}

			group._forceLayout();
			group._animationStart();

			// Reveal markers and spider legs.
			for (i = childMarkers.length - 1; i >= 0; i--) {
				newPos = map.layerPointToLatLng(positions[i]);
				m = childMarkers[i];

				//Move marker to new position
				m._preSpiderfyLatlng = m._latlng;
				m.setLatLng(newPos);

				if (m.clusterShow) {
					m.clusterShow();
				}

				// Animate leg (animation is actually delegated to CSS transition).
				if (svg) {
					leg = m._spiderLeg;
					legPath = leg._path;
					legPath.style.strokeDashoffset = 0;
					//legPath.style.strokeOpacity = finalLegOpacity;
					leg.setStyle({ opacity: finalLegOpacity });
				}
			}
			this.setOpacity(0.3);

			group._ignoreMove = false;

			setTimeout(function () {
				group._animationEnd();
				group.fire('spiderfied', {
					cluster: me,
					markers: childMarkers
				});
			}, 200);
		},

		_animationUnspiderfy: function (zoomDetails) {
			var me = this,
				group = this._group,
				map = group._map,
				fg = group._featureGroup,
				thisLayerPos = zoomDetails ? map._latLngToNewLayerPoint(this._latlng, zoomDetails.zoom, zoomDetails.center) : map.latLngToLayerPoint(this._latlng),
				childMarkers = this.getAllChildMarkers(),
				svg = L.Path.SVG,
				m, i, leg, legPath, legLength, nonAnimatable;

			group._ignoreMove = true;
			group._animationStart();

			//Make us visible and bring the child markers back in
			this.setOpacity(1);
			for (i = childMarkers.length - 1; i >= 0; i--) {
				m = childMarkers[i];

				//Marker was added to us after we were spiderfied
				if (!m._preSpiderfyLatlng) {
					continue;
				}

				//Close any popup on the marker first, otherwise setting the location of the marker will make the map scroll
				m.closePopup();

				//Fix up the location to the real one
				m.setLatLng(m._preSpiderfyLatlng);
				delete m._preSpiderfyLatlng;

				//Hack override the location to be our center
				nonAnimatable = true;
				if (m._setPos) {
					m._setPos(thisLayerPos);
					nonAnimatable = false;
				}
				if (m.clusterHide) {
					m.clusterHide();
					nonAnimatable = false;
				}
				if (nonAnimatable) {
					fg.removeLayer(m);
				}

				// Animate the spider leg back in (animation is actually delegated to CSS transition).
				if (svg) {
					leg = m._spiderLeg;
					legPath = leg._path;
					legLength = legPath.getTotalLength() + 0.1;
					legPath.style.strokeDashoffset = legLength;
					leg.setStyle({ opacity: 0 });
				}
			}

			group._ignoreMove = false;

			setTimeout(function () {
				//If we have only <= one child left then that marker will be shown on the map so don't remove it!
				var stillThereChildCount = 0;
				for (i = childMarkers.length - 1; i >= 0; i--) {
					m = childMarkers[i];
					if (m._spiderLeg) {
						stillThereChildCount++;
					}
				}


				for (i = childMarkers.length - 1; i >= 0; i--) {
					m = childMarkers[i];

					if (!m._spiderLeg) { //Has already been unspiderfied
						continue;
					}

					if (m.clusterShow) {
						m.clusterShow();
					}
					if (m.setZIndexOffset) {
						m.setZIndexOffset(0);
					}

					if (stillThereChildCount > 1) {
						fg.removeLayer(m);
					}

					map.removeLayer(m._spiderLeg);
					delete m._spiderLeg;
				}
				group._animationEnd();
				group.fire('unspiderfied', {
					cluster: me,
					markers: childMarkers
				});
			}, 200);
		}
	});


	L.MarkerClusterGroup.include({
		//The MarkerCluster currently spiderfied (if any)
		_spiderfied: null,

		unspiderfy: function () {
			this._unspiderfy.apply(this, arguments);
		},

		_spiderfierOnAdd: function () {
			this._map.on('click', this._unspiderfyWrapper, this);

			if (this._map.options.zoomAnimation) {
				this._map.on('zoomstart', this._unspiderfyZoomStart, this);
			}
			//Browsers without zoomAnimation or a big zoom don't fire zoomstart
			this._map.on('zoomend', this._noanimationUnspiderfy, this);

			if (!L.Browser.touch) {
				this._map.getRenderer(this);
				//Needs to happen in the pageload, not after, or animations don't work in webkit
				//  http://stackoverflow.com/questions/8455200/svg-animate-with-dynamically-added-elements
				//Disable on touch browsers as the animation messes up on a touch zoom and isn't very noticable
			}
		},

		_spiderfierOnRemove: function () {
			this._map.off('click', this._unspiderfyWrapper, this);
			this._map.off('zoomstart', this._unspiderfyZoomStart, this);
			this._map.off('zoomanim', this._unspiderfyZoomAnim, this);
			this._map.off('zoomend', this._noanimationUnspiderfy, this);

			//Ensure that markers are back where they should be
			// Use no animation to avoid a sticky leaflet-cluster-anim class on mapPane
			this._noanimationUnspiderfy();
		},

		//On zoom start we add a zoomanim handler so that we are guaranteed to be last (after markers are animated)
		//This means we can define the animation they do rather than Markers doing an animation to their actual location
		_unspiderfyZoomStart: function () {
			if (!this._map) { //May have been removed from the map by a zoomEnd handler
				return;
			}

			this._map.on('zoomanim', this._unspiderfyZoomAnim, this);
		},

		_unspiderfyZoomAnim: function (zoomDetails) {
			//Wait until the first zoomanim after the user has finished touch-zooming before running the animation
			if (L.DomUtil.hasClass(this._map._mapPane, 'leaflet-touching')) {
				return;
			}

			this._map.off('zoomanim', this._unspiderfyZoomAnim, this);
			this._unspiderfy(zoomDetails);
		},

		_unspiderfyWrapper: function () {
			/// <summary>_unspiderfy but passes no arguments</summary>
			this._unspiderfy();
		},

		_unspiderfy: function (zoomDetails) {
			if (this._spiderfied) {
				this._spiderfied.unspiderfy(zoomDetails);
			}
		},

		_noanimationUnspiderfy: function () {
			if (this._spiderfied) {
				this._spiderfied._noanimationUnspiderfy();
			}
		},

		//If the given layer is currently being spiderfied then we unspiderfy it so it isn't on the map anymore etc
		_unspiderfyLayer: function (layer) {
			if (layer._spiderLeg) {
				this._featureGroup.removeLayer(layer);

				if (layer.clusterShow) {
					layer.clusterShow();
				}
				//Position will be fixed up immediately in _animationUnspiderfy
				if (layer.setZIndexOffset) {
					layer.setZIndexOffset(0);
				}

				this._map.removeLayer(layer._spiderLeg);
				delete layer._spiderLeg;
			}
		}
	});


	/**
	 * Adds 1 public method to MCG and 1 to L.Marker to facilitate changing
	 * markers' icon options and refreshing their icon and their parent clusters
	 * accordingly (case where their iconCreateFunction uses data of childMarkers
	 * to make up the cluster icon).
	 */


	L.MarkerClusterGroup.include({
		/**
		 * Updates the icon of all clusters which are parents of the given marker(s).
		 * In singleMarkerMode, also updates the given marker(s) icon.
		 * @param layers L.MarkerClusterGroup|L.LayerGroup|Array(L.Marker)|Map(L.Marker)|
		 * L.MarkerCluster|L.Marker (optional) list of markers (or single marker) whose parent
		 * clusters need to be updated. If not provided, retrieves all child markers of this.
		 * @returns {L.MarkerClusterGroup}
		 */
		refreshClusters: function (layers) {
			if (!layers) {
				layers = this._topClusterLevel.getAllChildMarkers();
			} else if (layers instanceof L.MarkerClusterGroup) {
				layers = layers._topClusterLevel.getAllChildMarkers();
			} else if (layers instanceof L.LayerGroup) {
				layers = layers._layers;
			} else if (layers instanceof L.MarkerCluster) {
				layers = layers.getAllChildMarkers();
			} else if (layers instanceof L.Marker) {
				layers = [layers];
			} // else: must be an Array(L.Marker)|Map(L.Marker)
			this._flagParentsIconsNeedUpdate(layers);
			this._refreshClustersIcons();

			// In case of singleMarkerMode, also re-draw the markers.
			if (this.options.singleMarkerMode) {
				this._refreshSingleMarkerModeMarkers(layers);
			}

			return this;
		},

		/**
		 * Simply flags all parent clusters of the given markers as having a "dirty" icon.
		 * @param layers Array(L.Marker)|Map(L.Marker) list of markers.
		 * @private
		 */
		_flagParentsIconsNeedUpdate: function (layers) {
			var id, parent;

			// Assumes layers is an Array or an Object whose prototype is non-enumerable.
			for (id in layers) {
				// Flag parent clusters' icon as "dirty", all the way up.
				// Dumb process that flags multiple times upper parents, but still
				// much more efficient than trying to be smart and make short lists,
				// at least in the case of a hierarchy following a power law:
				// http://jsperf.com/flag-nodes-in-power-hierarchy/2
				parent = layers[id].__parent;
				while (parent) {
					parent._iconNeedsUpdate = true;
					parent = parent.__parent;
				}
			}
		},

		/**
		 * Re-draws the icon of the supplied markers.
		 * To be used in singleMarkerMode only.
		 * @param layers Array(L.Marker)|Map(L.Marker) list of markers.
		 * @private
		 */
		_refreshSingleMarkerModeMarkers: function (layers) {
			var id, layer;

			for (id in layers) {
				layer = layers[id];

				// Make sure we do not override markers that do not belong to THIS group.
				if (this.hasLayer(layer)) {
					// Need to re-create the icon first, then re-draw the marker.
					layer.setIcon(this._overrideMarkerIcon(layer));
				}
			}
		}
	});

	L.Marker.include({
		/**
		 * Updates the given options in the marker's icon and refreshes the marker.
		 * @param options map object of icon options.
		 * @param directlyRefreshClusters boolean (optional) true to trigger
		 * MCG.refreshClustersOf() right away with this single marker.
		 * @returns {L.Marker}
		 */
		refreshIconOptions: function (options, directlyRefreshClusters) {
			var icon = this.options.icon;

			L.setOptions(icon, options);

			this.setIcon(icon);

			// Shortcut to refresh the associated MCG clusters right away.
			// To be used when refreshing a single marker.
			// Otherwise, better use MCG.refreshClusters() once at the end with
			// the list of modified markers.
			if (directlyRefreshClusters && this.__parent) {
				this.__parent._group.refreshClusters(this);
			}

			return this;
		}
	});


}(window, document));
/*
 Leaflet.draw 1.0.4, a plugin that adds drawing and editing tools to Leaflet powered maps.
 (c) 2012-2017, Jacob Toye, Jon West, Smartrak, Leaflet

 https://github.com/Leaflet/Leaflet.draw
 http://leafletjs.com
 */
!function (t, e, i) {
    function o(t, e) { for (; (t = t.parentElement) && !t.classList.contains(e);); return t } L.drawVersion = "1.0.4", L.Draw = {}, L.drawLocal = { draw: { toolbar: { actions: { title: "Cancel drawing", text: "Cancel" }, finish: { title: "Finish drawing", text: "Finish" }, undo: { title: "Delete last point drawn", text: "Delete last point" }, buttons: { polyline: "Draw a polyline", polygon: "Draw a polygon", rectangle: "Draw a rectangle", circle: "Draw a circle", marker: "Draw a marker", circlemarker: "Draw a circlemarker" } }, handlers: { circle: { tooltip: { start: "Click and drag to draw circle." }, radius: "Radius" }, circlemarker: { tooltip: { start: "Click map to place circle marker." } }, marker: { tooltip: { start: "Click map to place marker." } }, polygon: { tooltip: { start: "Click to start drawing shape.", cont: "Click to continue drawing shape.", end: "Click first point to close this shape." } }, polyline: { error: "<strong>Error:</strong> shape edges cannot cross!", tooltip: { start: "Click to start drawing line.", cont: "Click to continue drawing line.", end: "Click last point to finish line." } }, rectangle: { tooltip: { start: "Click and drag to draw rectangle." } }, simpleshape: { tooltip: { end: "Release mouse to finish drawing." } } } }, edit: { toolbar: { actions: { save: { title: "Save changes", text: "Save" }, cancel: { title: "Cancel editing, discards all changes", text: "Cancel" }, clearAll: { title: "Clear all layers", text: "Clear All" } }, buttons: { edit: "Edit layers", editDisabled: "No layers to edit", remove: "Delete layers", removeDisabled: "No layers to delete" } }, handlers: { edit: { tooltip: { text: "Drag handles or markers to edit features.", subtext: "Click cancel to undo changes." } }, remove: { tooltip: { text: "Click on a feature to remove." } } } } }, L.Draw.Event = {}, L.Draw.Event.CREATED = "draw:created", L.Draw.Event.EDITED = "draw:edited", L.Draw.Event.DELETED = "draw:deleted", L.Draw.Event.DRAWSTART = "draw:drawstart", L.Draw.Event.DRAWSTOP = "draw:drawstop", L.Draw.Event.DRAWVERTEX = "draw:drawvertex", L.Draw.Event.EDITSTART = "draw:editstart", L.Draw.Event.EDITMOVE = "draw:editmove", L.Draw.Event.EDITRESIZE = "draw:editresize", L.Draw.Event.EDITVERTEX = "draw:editvertex", L.Draw.Event.EDITSTOP = "draw:editstop", L.Draw.Event.DELETESTART = "draw:deletestart", L.Draw.Event.DELETESTOP = "draw:deletestop", L.Draw.Event.TOOLBAROPENED = "draw:toolbaropened", L.Draw.Event.TOOLBARCLOSED = "draw:toolbarclosed", L.Draw.Event.MARKERCONTEXT = "draw:markercontext", L.Draw = L.Draw || {}, L.Draw.Feature = L.Handler.extend({ initialize: function (t, e) { this._map = t, this._container = t._container, this._overlayPane = t._panes.overlayPane, this._popupPane = t._panes.popupPane, e && e.shapeOptions && (e.shapeOptions = L.Util.extend({}, this.options.shapeOptions, e.shapeOptions)), L.setOptions(this, e); var i = L.version.split("."); 1 === parseInt(i[0], 10) && parseInt(i[1], 10) >= 2 ? L.Draw.Feature.include(L.Evented.prototype) : L.Draw.Feature.include(L.Mixin.Events) }, enable: function () { this._enabled || (L.Handler.prototype.enable.call(this), this.fire("enabled", { handler: this.type }), this._map.fire(L.Draw.Event.DRAWSTART, { layerType: this.type })) }, disable: function () { this._enabled && (L.Handler.prototype.disable.call(this), this._map.fire(L.Draw.Event.DRAWSTOP, { layerType: this.type }), this.fire("disabled", { handler: this.type })) }, addHooks: function () { var t = this._map; t && (L.DomUtil.disableTextSelection(), t.getContainer().focus(), this._tooltip = new L.Draw.Tooltip(this._map), L.DomEvent.on(this._container, "keyup", this._cancelDrawing, this)) }, removeHooks: function () { this._map && (L.DomUtil.enableTextSelection(), this._tooltip.dispose(), this._tooltip = null, L.DomEvent.off(this._container, "keyup", this._cancelDrawing, this)) }, setOptions: function (t) { L.setOptions(this, t) }, _fireCreatedEvent: function (t) { this._map.fire(L.Draw.Event.CREATED, { layer: t, layerType: this.type }) }, _cancelDrawing: function (t) { 27 === t.keyCode && (this._map.fire("draw:canceled", { layerType: this.type }), this.disable()) } }), L.Draw.Polyline = L.Draw.Feature.extend({ statics: { TYPE: "polyline" }, Poly: L.Polyline, options: { allowIntersection: !0, repeatMode: !1, drawError: { color: "#b00b00", timeout: 2500 }, icon: new L.DivIcon({ iconSize: new L.Point(8, 8), className: "leaflet-div-icon leaflet-editing-icon" }), touchIcon: new L.DivIcon({ iconSize: new L.Point(20, 20), className: "leaflet-div-icon leaflet-editing-icon leaflet-touch-icon" }), guidelineDistance: 20, maxGuideLineLength: 4e3, shapeOptions: { stroke: !0, color: "#3388ff", weight: 4, opacity: .5, fill: !1, clickable: !0 }, metric: !0, feet: !0, nautic: !1, showLength: !0, zIndexOffset: 2e3, factor: 1, maxPoints: 0 }, initialize: function (t, e) { L.Browser.touch && (this.options.icon = this.options.touchIcon), this.options.drawError.message = L.drawLocal.draw.handlers.polyline.error, e && e.drawError && (e.drawError = L.Util.extend({}, this.options.drawError, e.drawError)), this.type = L.Draw.Polyline.TYPE, L.Draw.Feature.prototype.initialize.call(this, t, e) }, addHooks: function () { L.Draw.Feature.prototype.addHooks.call(this), this._map && (this._markers = [], this._markerGroup = new L.LayerGroup, this._map.addLayer(this._markerGroup), this._poly = new L.Polyline([], this.options.shapeOptions), this._tooltip.updateContent(this._getTooltipText()), this._mouseMarker || (this._mouseMarker = L.marker(this._map.getCenter(), { icon: L.divIcon({ className: "leaflet-mouse-marker", iconAnchor: [20, 20], iconSize: [40, 40] }), opacity: 0, zIndexOffset: this.options.zIndexOffset })), this._mouseMarker.on("mouseout", this._onMouseOut, this).on("mousemove", this._onMouseMove, this).on("mousedown", this._onMouseDown, this).on("mouseup", this._onMouseUp, this).addTo(this._map), this._map.on("mouseup", this._onMouseUp, this).on("mousemove", this._onMouseMove, this).on("zoomlevelschange", this._onZoomEnd, this).on("touchstart", this._onTouch, this).on("zoomend", this._onZoomEnd, this)) }, removeHooks: function () { L.Draw.Feature.prototype.removeHooks.call(this), this._clearHideErrorTimeout(), this._cleanUpShape(), this._map.removeLayer(this._markerGroup), delete this._markerGroup, delete this._markers, this._map.removeLayer(this._poly), delete this._poly, this._mouseMarker.off("mousedown", this._onMouseDown, this).off("mouseout", this._onMouseOut, this).off("mouseup", this._onMouseUp, this).off("mousemove", this._onMouseMove, this), this._map.removeLayer(this._mouseMarker), delete this._mouseMarker, this._clearGuides(), this._map.off("mouseup", this._onMouseUp, this).off("mousemove", this._onMouseMove, this).off("zoomlevelschange", this._onZoomEnd, this).off("zoomend", this._onZoomEnd, this).off("touchstart", this._onTouch, this).off("click", this._onTouch, this) }, deleteLastVertex: function () { if (!(this._markers.length <= 1)) { var t = this._markers.pop(), e = this._poly, i = e.getLatLngs(), o = i.splice(-1, 1)[0]; this._poly.setLatLngs(i), this._markerGroup.removeLayer(t), e.getLatLngs().length < 2 && this._map.removeLayer(e), this._vertexChanged(o, !1) } }, addVertex: function (t) { if (this._markers.length >= 2 && !this.options.allowIntersection && this._poly.newLatLngIntersects(t)) return void this._showErrorTooltip(); this._errorShown && this._hideErrorTooltip(), this._markers.push(this._createMarker(t)), this._poly.addLatLng(t), 2 === this._poly.getLatLngs().length && this._map.addLayer(this._poly), this._vertexChanged(t, !0) }, completeShape: function () { this._markers.length <= 1 || !this._shapeIsValid() || (this._fireCreatedEvent(), this.disable(), this.options.repeatMode && this.enable()) }, _finishShape: function () { var t = this._poly._defaultShape ? this._poly._defaultShape() : this._poly.getLatLngs(), e = this._poly.newLatLngIntersects(t[t.length - 1]); if (!this.options.allowIntersection && e || !this._shapeIsValid()) return void this._showErrorTooltip(); this._fireCreatedEvent(), this.disable(), this.options.repeatMode && this.enable() }, _shapeIsValid: function () { return !0 }, _onZoomEnd: function () { null !== this._markers && this._updateGuide() }, _onMouseMove: function (t) { var e = this._map.mouseEventToLayerPoint(t.originalEvent), i = this._map.layerPointToLatLng(e); this._currentLatLng = i, this._updateTooltip(i), this._updateGuide(e), this._mouseMarker.setLatLng(i), L.DomEvent.preventDefault(t.originalEvent) }, _vertexChanged: function (t, e) { this._map.fire(L.Draw.Event.DRAWVERTEX, { layers: this._markerGroup }), this._updateFinishHandler(), this._updateRunningMeasure(t, e), this._clearGuides(), this._updateTooltip() }, _onMouseDown: function (t) { if (!this._clickHandled && !this._touchHandled && !this._disableMarkers) { this._onMouseMove(t), this._clickHandled = !0, this._disableNewMarkers(); var e = t.originalEvent, i = e.clientX, o = e.clientY; this._startPoint.call(this, i, o) } }, _startPoint: function (t, e) { this._mouseDownOrigin = L.point(t, e) }, _onMouseUp: function (t) { var e = t.originalEvent, i = e.clientX, o = e.clientY; this._endPoint.call(this, i, o, t), this._clickHandled = null }, _endPoint: function (e, i, o) { if (this._mouseDownOrigin) { var a = L.point(e, i).distanceTo(this._mouseDownOrigin), n = this._calculateFinishDistance(o.latlng); this.options.maxPoints > 1 && this.options.maxPoints == this._markers.length + 1 ? (this.addVertex(o.latlng), this._finishShape()) : n < 10 && L.Browser.touch ? this._finishShape() : Math.abs(a) < 9 * (t.devicePixelRatio || 1) && this.addVertex(o.latlng), this._enableNewMarkers() } this._mouseDownOrigin = null }, _onTouch: function (t) { var e, i, o = t.originalEvent; !o.touches || !o.touches[0] || this._clickHandled || this._touchHandled || this._disableMarkers || (e = o.touches[0].clientX, i = o.touches[0].clientY, this._disableNewMarkers(), this._touchHandled = !0, this._startPoint.call(this, e, i), this._endPoint.call(this, e, i, t), this._touchHandled = null), this._clickHandled = null }, _onMouseOut: function () { this._tooltip && this._tooltip._onMouseOut.call(this._tooltip) }, _calculateFinishDistance: function (t) { var e; if (this._markers.length > 0) { var i; if (this.type === L.Draw.Polyline.TYPE) i = this._markers[this._markers.length - 1]; else { if (this.type !== L.Draw.Polygon.TYPE) return 1 / 0; i = this._markers[0] } var o = this._map.latLngToContainerPoint(i.getLatLng()), a = new L.Marker(t, { icon: this.options.icon, zIndexOffset: 2 * this.options.zIndexOffset }), n = this._map.latLngToContainerPoint(a.getLatLng()); e = o.distanceTo(n) } else e = 1 / 0; return e }, _updateFinishHandler: function () { var t = this._markers.length; t > 1 && this._markers[t - 1].on("click", this._finishShape, this), t > 2 && this._markers[t - 2].off("click", this._finishShape, this) }, _createMarker: function (t) { var e = new L.Marker(t, { icon: this.options.icon, zIndexOffset: 2 * this.options.zIndexOffset }); return this._markerGroup.addLayer(e), e }, _updateGuide: function (t) { var e = this._markers ? this._markers.length : 0; e > 0 && (t = t || this._map.latLngToLayerPoint(this._currentLatLng), this._clearGuides(), this._drawGuide(this._map.latLngToLayerPoint(this._markers[e - 1].getLatLng()), t)) }, _updateTooltip: function (t) { var e = this._getTooltipText(); t && this._tooltip.updatePosition(t), this._errorShown || this._tooltip.updateContent(e) }, _drawGuide: function (t, e) { var i, o, a, n = Math.floor(Math.sqrt(Math.pow(e.x - t.x, 2) + Math.pow(e.y - t.y, 2))), s = this.options.guidelineDistance, r = this.options.maxGuideLineLength, l = n > r ? n - r : s; for (this._guidesContainer || (this._guidesContainer = L.DomUtil.create("div", "leaflet-draw-guides", this._overlayPane)); l < n; l += this.options.guidelineDistance)i = l / n, o = { x: Math.floor(t.x * (1 - i) + i * e.x), y: Math.floor(t.y * (1 - i) + i * e.y) }, a = L.DomUtil.create("div", "leaflet-draw-guide-dash", this._guidesContainer), a.style.backgroundColor = this._errorShown ? this.options.drawError.color : this.options.shapeOptions.color, L.DomUtil.setPosition(a, o) }, _updateGuideColor: function (t) { if (this._guidesContainer) for (var e = 0, i = this._guidesContainer.childNodes.length; e < i; e++)this._guidesContainer.childNodes[e].style.backgroundColor = t }, _clearGuides: function () { if (this._guidesContainer) for (; this._guidesContainer.firstChild;)this._guidesContainer.removeChild(this._guidesContainer.firstChild) }, _getTooltipText: function () { var t, e, i = this.options.showLength; return 0 === this._markers.length ? t = { text: L.drawLocal.draw.handlers.polyline.tooltip.start } : (e = i ? this._getMeasurementString() : "", t = 1 === this._markers.length ? { text: L.drawLocal.draw.handlers.polyline.tooltip.cont, subtext: e } : { text: L.drawLocal.draw.handlers.polyline.tooltip.end, subtext: e }), t }, _updateRunningMeasure: function (t, e) { var i, o, a = this._markers.length; 1 === this._markers.length ? this._measurementRunningTotal = 0 : (i = a - (e ? 2 : 1), o = L.GeometryUtil.isVersion07x() ? t.distanceTo(this._markers[i].getLatLng()) * (this.options.factor || 1) : this._map.distance(t, this._markers[i].getLatLng()) * (this.options.factor || 1), this._measurementRunningTotal += o * (e ? 1 : -1)) }, _getMeasurementString: function () { var t, e = this._currentLatLng, i = this._markers[this._markers.length - 1].getLatLng(); return t = L.GeometryUtil.isVersion07x() ? i && e && e.distanceTo ? this._measurementRunningTotal + e.distanceTo(i) * (this.options.factor || 1) : this._measurementRunningTotal || 0 : i && e ? this._measurementRunningTotal + this._map.distance(e, i) * (this.options.factor || 1) : this._measurementRunningTotal || 0, L.GeometryUtil.readableDistance(t, this.options.metric, this.options.feet, this.options.nautic, this.options.precision) }, _showErrorTooltip: function () { this._errorShown = !0, this._tooltip.showAsError().updateContent({ text: this.options.drawError.message }), this._updateGuideColor(this.options.drawError.color), this._poly.setStyle({ color: this.options.drawError.color }), this._clearHideErrorTimeout(), this._hideErrorTimeout = setTimeout(L.Util.bind(this._hideErrorTooltip, this), this.options.drawError.timeout) }, _hideErrorTooltip: function () { this._errorShown = !1, this._clearHideErrorTimeout(), this._tooltip.removeError().updateContent(this._getTooltipText()), this._updateGuideColor(this.options.shapeOptions.color), this._poly.setStyle({ color: this.options.shapeOptions.color }) }, _clearHideErrorTimeout: function () { this._hideErrorTimeout && (clearTimeout(this._hideErrorTimeout), this._hideErrorTimeout = null) }, _disableNewMarkers: function () { this._disableMarkers = !0 }, _enableNewMarkers: function () { setTimeout(function () { this._disableMarkers = !1 }.bind(this), 50) }, _cleanUpShape: function () { this._markers.length > 1 && this._markers[this._markers.length - 1].off("click", this._finishShape, this) }, _fireCreatedEvent: function () { var t = new this.Poly(this._poly.getLatLngs(), this.options.shapeOptions); L.Draw.Feature.prototype._fireCreatedEvent.call(this, t) } }), L.Draw.Polygon = L.Draw.Polyline.extend({ statics: { TYPE: "polygon" }, Poly: L.Polygon, options: { showArea: !1, showLength: !1, shapeOptions: { stroke: !0, color: "#3388ff", weight: 4, opacity: .5, fill: !0, fillColor: null, fillOpacity: .2, clickable: !0 }, metric: !0, feet: !0, nautic: !1, precision: {} }, initialize: function (t, e) { L.Draw.Polyline.prototype.initialize.call(this, t, e), this.type = L.Draw.Polygon.TYPE }, _updateFinishHandler: function () { var t = this._markers.length; 1 === t && this._markers[0].on("click", this._finishShape, this), t > 2 && (this._markers[t - 1].on("dblclick", this._finishShape, this), t > 3 && this._markers[t - 2].off("dblclick", this._finishShape, this)) }, _getTooltipText: function () { var t, e; return 0 === this._markers.length ? t = L.drawLocal.draw.handlers.polygon.tooltip.start : this._markers.length < 3 ? (t = L.drawLocal.draw.handlers.polygon.tooltip.cont, e = this._getMeasurementString()) : (t = L.drawLocal.draw.handlers.polygon.tooltip.end, e = this._getMeasurementString()), { text: t, subtext: e } }, _getMeasurementString: function () { var t = this._area, e = ""; return t || this.options.showLength ? (this.options.showLength && (e = L.Draw.Polyline.prototype._getMeasurementString.call(this)), t && (e += "<br>" + L.GeometryUtil.readableArea(t, this.options.metric, this.options.precision)), e) : null }, _shapeIsValid: function () { return this._markers.length >= 3 }, _vertexChanged: function (t, e) { var i; !this.options.allowIntersection && this.options.showArea && (i = this._poly.getLatLngs(), this._area = L.GeometryUtil.geodesicArea(i)), L.Draw.Polyline.prototype._vertexChanged.call(this, t, e) }, _cleanUpShape: function () { var t = this._markers.length; t > 0 && (this._markers[0].off("click", this._finishShape, this), t > 2 && this._markers[t - 1].off("dblclick", this._finishShape, this)) } }), L.SimpleShape = {}, L.Draw.SimpleShape = L.Draw.Feature.extend({ options: { repeatMode: !1 }, initialize: function (t, e) { this._endLabelText = L.drawLocal.draw.handlers.simpleshape.tooltip.end, L.Draw.Feature.prototype.initialize.call(this, t, e) }, addHooks: function () { L.Draw.Feature.prototype.addHooks.call(this), this._map && (this._mapDraggable = this._map.dragging.enabled(), this._mapDraggable && this._map.dragging.disable(), this._container.style.cursor = "crosshair", this._tooltip.updateContent({ text: this._initialLabelText }), this._map.on("mousedown", this._onMouseDown, this).on("mousemove", this._onMouseMove, this).on("touchstart", this._onMouseDown, this).on("touchmove", this._onMouseMove, this), e.addEventListener("touchstart", L.DomEvent.preventDefault, { passive: !1 })) }, removeHooks: function () { L.Draw.Feature.prototype.removeHooks.call(this), this._map && (this._mapDraggable && this._map.dragging.enable(), this._container.style.cursor = "", this._map.off("mousedown", this._onMouseDown, this).off("mousemove", this._onMouseMove, this).off("touchstart", this._onMouseDown, this).off("touchmove", this._onMouseMove, this), L.DomEvent.off(e, "mouseup", this._onMouseUp, this), L.DomEvent.off(e, "touchend", this._onMouseUp, this), e.removeEventListener("touchstart", L.DomEvent.preventDefault), this._shape && (this._map.removeLayer(this._shape), delete this._shape)), this._isDrawing = !1 }, _getTooltipText: function () { return { text: this._endLabelText } }, _onMouseDown: function (t) { this._isDrawing = !0, this._startLatLng = t.latlng, L.DomEvent.on(e, "mouseup", this._onMouseUp, this).on(e, "touchend", this._onMouseUp, this).preventDefault(t.originalEvent) }, _onMouseMove: function (t) { var e = t.latlng; this._tooltip.updatePosition(e), this._isDrawing && (this._tooltip.updateContent(this._getTooltipText()), this._drawShape(e)) }, _onMouseUp: function () { this._shape && this._fireCreatedEvent(), this.disable(), this.options.repeatMode && this.enable() } }), L.Draw.Rectangle = L.Draw.SimpleShape.extend({ statics: { TYPE: "rectangle" }, options: { shapeOptions: { stroke: !0, color: "#3388ff", weight: 4, opacity: .5, fill: !0, fillColor: null, fillOpacity: .2, clickable: !0 }, showArea: !0, metric: !0 }, initialize: function (t, e) { this.type = L.Draw.Rectangle.TYPE, this._initialLabelText = L.drawLocal.draw.handlers.rectangle.tooltip.start, L.Draw.SimpleShape.prototype.initialize.call(this, t, e) }, disable: function () { this._enabled && (this._isCurrentlyTwoClickDrawing = !1, L.Draw.SimpleShape.prototype.disable.call(this)) }, _onMouseUp: function (t) { if (!this._shape && !this._isCurrentlyTwoClickDrawing) return void (this._isCurrentlyTwoClickDrawing = !0); this._isCurrentlyTwoClickDrawing && !o(t.target, "leaflet-pane") || L.Draw.SimpleShape.prototype._onMouseUp.call(this) }, _drawShape: function (t) { this._shape ? this._shape.setBounds(new L.LatLngBounds(this._startLatLng, t)) : (this._shape = new L.Rectangle(new L.LatLngBounds(this._startLatLng, t), this.options.shapeOptions), this._map.addLayer(this._shape)) }, _fireCreatedEvent: function () { var t = new L.Rectangle(this._shape.getBounds(), this.options.shapeOptions); L.Draw.SimpleShape.prototype._fireCreatedEvent.call(this, t) }, _getTooltipText: function () { var t, e, i, o = L.Draw.SimpleShape.prototype._getTooltipText.call(this), a = this._shape, n = this.options.showArea; return a && (t = this._shape._defaultShape ? this._shape._defaultShape() : this._shape.getLatLngs(), e = L.GeometryUtil.geodesicArea(t), i = n ? L.GeometryUtil.readableArea(e, this.options.metric) : ""), { text: o.text, subtext: i } } }), L.Draw.Marker = L.Draw.Feature.extend({ statics: { TYPE: "marker" }, options: { icon: new L.Icon.Default, repeatMode: !1, zIndexOffset: 2e3 }, initialize: function (t, e) { this.type = L.Draw.Marker.TYPE, this._initialLabelText = L.drawLocal.draw.handlers.marker.tooltip.start, L.Draw.Feature.prototype.initialize.call(this, t, e) }, addHooks: function () { L.Draw.Feature.prototype.addHooks.call(this), this._map && (this._tooltip.updateContent({ text: this._initialLabelText }), this._mouseMarker || (this._mouseMarker = L.marker(this._map.getCenter(), { icon: L.divIcon({ className: "leaflet-mouse-marker", iconAnchor: [20, 20], iconSize: [40, 40] }), opacity: 0, zIndexOffset: this.options.zIndexOffset })), this._mouseMarker.on("click", this._onClick, this).addTo(this._map), this._map.on("mousemove", this._onMouseMove, this), this._map.on("click", this._onTouch, this)) }, removeHooks: function () { L.Draw.Feature.prototype.removeHooks.call(this), this._map && (this._map.off("click", this._onClick, this).off("click", this._onTouch, this), this._marker && (this._marker.off("click", this._onClick, this), this._map.removeLayer(this._marker), delete this._marker), this._mouseMarker.off("click", this._onClick, this), this._map.removeLayer(this._mouseMarker), delete this._mouseMarker, this._map.off("mousemove", this._onMouseMove, this)) }, _onMouseMove: function (t) { var e = t.latlng; this._tooltip.updatePosition(e), this._mouseMarker.setLatLng(e), this._marker ? (e = this._mouseMarker.getLatLng(), this._marker.setLatLng(e)) : (this._marker = this._createMarker(e), this._marker.on("click", this._onClick, this), this._map.on("click", this._onClick, this).addLayer(this._marker)) }, _createMarker: function (t) { return new L.Marker(t, { icon: this.options.icon, zIndexOffset: this.options.zIndexOffset }) }, _onClick: function () { this._fireCreatedEvent(), this.disable(), this.options.repeatMode && this.enable() }, _onTouch: function (t) { this._onMouseMove(t), this._onClick() }, _fireCreatedEvent: function () { var t = new L.Marker.Touch(this._marker.getLatLng(), { icon: this.options.icon }); L.Draw.Feature.prototype._fireCreatedEvent.call(this, t) } }), L.Draw.CircleMarker = L.Draw.Marker.extend({ statics: { TYPE: "circlemarker" }, options: { stroke: !0, color: "#3388ff", weight: 4, opacity: .5, fill: !0, fillColor: null, fillOpacity: .2, clickable: !0, zIndexOffset: 2e3 }, initialize: function (t, e) { this.type = L.Draw.CircleMarker.TYPE, this._initialLabelText = L.drawLocal.draw.handlers.circlemarker.tooltip.start, L.Draw.Feature.prototype.initialize.call(this, t, e) }, _fireCreatedEvent: function () { var t = new L.CircleMarker(this._marker.getLatLng(), this.options); L.Draw.Feature.prototype._fireCreatedEvent.call(this, t) }, _createMarker: function (t) { return new L.CircleMarker(t, this.options) } }), L.Draw.Circle = L.Draw.SimpleShape.extend({ statics: { TYPE: "circle" }, options: { shapeOptions: { stroke: !0, color: "#3388ff", weight: 4, opacity: .5, fill: !0, fillColor: null, fillOpacity: .2, clickable: !0 }, showRadius: !0, metric: !0, feet: !0, nautic: !1 }, initialize: function (t, e) { this.type = L.Draw.Circle.TYPE, this._initialLabelText = L.drawLocal.draw.handlers.circle.tooltip.start, L.Draw.SimpleShape.prototype.initialize.call(this, t, e) }, _drawShape: function (t) { if (L.GeometryUtil.isVersion07x()) var e = this._startLatLng.distanceTo(t); else var e = this._map.distance(this._startLatLng, t); this._shape ? this._shape.setRadius(e) : (this._shape = new L.Circle(this._startLatLng, e, this.options.shapeOptions), this._map.addLayer(this._shape)) }, _fireCreatedEvent: function () { var t = new L.Circle(this._startLatLng, this._shape.getRadius(), this.options.shapeOptions); L.Draw.SimpleShape.prototype._fireCreatedEvent.call(this, t) }, _onMouseMove: function (t) { var e, i = t.latlng, o = this.options.showRadius, a = this.options.metric; if (this._tooltip.updatePosition(i), this._isDrawing) { this._drawShape(i), e = this._shape.getRadius().toFixed(1); var n = ""; o && (n = L.drawLocal.draw.handlers.circle.radius + ": " + L.GeometryUtil.readableDistance(e, a, this.options.feet, this.options.nautic)), this._tooltip.updateContent({ text: this._endLabelText, subtext: n }) } } }), L.Edit = L.Edit || {}, L.Edit.Marker = L.Handler.extend({ initialize: function (t, e) { this._marker = t, L.setOptions(this, e) }, addHooks: function () { var t = this._marker; t.dragging.enable(), t.on("dragend", this._onDragEnd, t), this._toggleMarkerHighlight() }, removeHooks: function () { var t = this._marker; t.dragging.disable(), t.off("dragend", this._onDragEnd, t), this._toggleMarkerHighlight() }, _onDragEnd: function (t) { var e = t.target; e.edited = !0, this._map.fire(L.Draw.Event.EDITMOVE, { layer: e }) }, _toggleMarkerHighlight: function () { var t = this._marker._icon; t && (t.style.display = "none", L.DomUtil.hasClass(t, "leaflet-edit-marker-selected") ? (L.DomUtil.removeClass(t, "leaflet-edit-marker-selected"), this._offsetMarker(t, -4)) : (L.DomUtil.addClass(t, "leaflet-edit-marker-selected"), this._offsetMarker(t, 4)), t.style.display = "") }, _offsetMarker: function (t, e) { var i = parseInt(t.style.marginTop, 10) - e, o = parseInt(t.style.marginLeft, 10) - e; t.style.marginTop = i + "px", t.style.marginLeft = o + "px" } }), L.Marker.addInitHook(function () { L.Edit.Marker && (this.editing = new L.Edit.Marker(this), this.options.editable && this.editing.enable()) }), L.Edit = L.Edit || {}, L.Edit.Poly = L.Handler.extend({ initialize: function (t) { this.latlngs = [t._latlngs], t._holes && (this.latlngs = this.latlngs.concat(t._holes)), this._poly = t, this._poly.on("revert-edited", this._updateLatLngs, this) }, _defaultShape: function () { return L.Polyline._flat ? L.Polyline._flat(this._poly._latlngs) ? this._poly._latlngs : this._poly._latlngs[0] : this._poly._latlngs }, _eachVertexHandler: function (t) { for (var e = 0; e < this._verticesHandlers.length; e++)t(this._verticesHandlers[e]) }, addHooks: function () { this._initHandlers(), this._eachVertexHandler(function (t) { t.addHooks() }) }, removeHooks: function () { this._eachVertexHandler(function (t) { t.removeHooks() }) }, updateMarkers: function () { this._eachVertexHandler(function (t) { t.updateMarkers() }) }, _initHandlers: function () { this._verticesHandlers = []; for (var t = 0; t < this.latlngs.length; t++)this._verticesHandlers.push(new L.Edit.PolyVerticesEdit(this._poly, this.latlngs[t], this._poly.options.poly)) }, _updateLatLngs: function (t) { this.latlngs = [t.layer._latlngs], t.layer._holes && (this.latlngs = this.latlngs.concat(t.layer._holes)) } }), L.Edit.PolyVerticesEdit = L.Handler.extend({ options: { icon: new L.DivIcon({ iconSize: new L.Point(8, 8), className: "leaflet-div-icon leaflet-editing-icon" }), touchIcon: new L.DivIcon({ iconSize: new L.Point(20, 20), className: "leaflet-div-icon leaflet-editing-icon leaflet-touch-icon" }), drawError: { color: "#b00b00", timeout: 1e3 } }, initialize: function (t, e, i) { L.Browser.touch && (this.options.icon = this.options.touchIcon), this._poly = t, i && i.drawError && (i.drawError = L.Util.extend({}, this.options.drawError, i.drawError)), this._latlngs = e, L.setOptions(this, i) }, _defaultShape: function () { return L.Polyline._flat ? L.Polyline._flat(this._latlngs) ? this._latlngs : this._latlngs[0] : this._latlngs }, addHooks: function () { var t = this._poly, e = t._path; t instanceof L.Polygon || (t.options.fill = !1, t.options.editing && (t.options.editing.fill = !1)), e && t.options.editing && t.options.editing.className && (t.options.original.className && t.options.original.className.split(" ").forEach(function (t) { L.DomUtil.removeClass(e, t) }), t.options.editing.className.split(" ").forEach(function (t) { L.DomUtil.addClass(e, t) })), t.setStyle(t.options.editing), this._poly._map && (this._map = this._poly._map, this._markerGroup || this._initMarkers(), this._poly._map.addLayer(this._markerGroup)) }, removeHooks: function () { var t = this._poly, e = t._path; e && t.options.editing && t.options.editing.className && (t.options.editing.className.split(" ").forEach(function (t) { L.DomUtil.removeClass(e, t) }), t.options.original.className && t.options.original.className.split(" ").forEach(function (t) { L.DomUtil.addClass(e, t) })), t.setStyle(t.options.original), t._map && (t._map.removeLayer(this._markerGroup), delete this._markerGroup, delete this._markers) }, updateMarkers: function () { this._markerGroup.clearLayers(), this._initMarkers() }, _initMarkers: function () { this._markerGroup || (this._markerGroup = new L.LayerGroup), this._markers = []; var t, e, i, o, a = this._defaultShape(); for (t = 0, i = a.length; t < i; t++)o = this._createMarker(a[t], t), o.on("click", this._onMarkerClick, this), o.on("contextmenu", this._onContextMenu, this), this._markers.push(o); var n, s; for (t = 0, e = i - 1; t < i; e = t++)(0 !== t || L.Polygon && this._poly instanceof L.Polygon) && (n = this._markers[e], s = this._markers[t], this._createMiddleMarker(n, s), this._updatePrevNext(n, s)) }, _createMarker: function (t, e) { var i = new L.Marker.Touch(t, { draggable: !0, icon: this.options.icon }); return i._origLatLng = t, i._index = e, i.on("dragstart", this._onMarkerDragStart, this).on("drag", this._onMarkerDrag, this).on("dragend", this._fireEdit, this).on("touchmove", this._onTouchMove, this).on("touchend", this._fireEdit, this).on("MSPointerMove", this._onTouchMove, this).on("MSPointerUp", this._fireEdit, this), this._markerGroup.addLayer(i), i }, _onMarkerDragStart: function () { this._poly.fire("editstart") }, _spliceLatLngs: function () { var t = this._defaultShape(), e = [].splice.apply(t, arguments); return this._poly._convertLatLngs(t, !0), this._poly.redraw(), e }, _removeMarker: function (t) { var e = t._index; this._markerGroup.removeLayer(t), this._markers.splice(e, 1), this._spliceLatLngs(e, 1), this._updateIndexes(e, -1), t.off("dragstart", this._onMarkerDragStart, this).off("drag", this._onMarkerDrag, this).off("dragend", this._fireEdit, this).off("touchmove", this._onMarkerDrag, this).off("touchend", this._fireEdit, this).off("click", this._onMarkerClick, this).off("MSPointerMove", this._onTouchMove, this).off("MSPointerUp", this._fireEdit, this) }, _fireEdit: function () { this._poly.edited = !0, this._poly.fire("edit"), this._poly._map.fire(L.Draw.Event.EDITVERTEX, { layers: this._markerGroup, poly: this._poly }) }, _onMarkerDrag: function (t) { var e = t.target, i = this._poly, o = L.LatLngUtil.cloneLatLng(e._origLatLng); if (L.extend(e._origLatLng, e._latlng), i.options.poly) { var a = i._map._editTooltip; if (!i.options.poly.allowIntersection && i.intersects()) { L.extend(e._origLatLng, o), e.setLatLng(o); var n = i.options.color; i.setStyle({ color: this.options.drawError.color }), a && a.updateContent({ text: L.drawLocal.draw.handlers.polyline.error }), setTimeout(function () { i.setStyle({ color: n }), a && a.updateContent({ text: L.drawLocal.edit.handlers.edit.tooltip.text, subtext: L.drawLocal.edit.handlers.edit.tooltip.subtext }) }, 1e3) } } e._middleLeft && e._middleLeft.setLatLng(this._getMiddleLatLng(e._prev, e)), e._middleRight && e._middleRight.setLatLng(this._getMiddleLatLng(e, e._next)), this._poly._bounds._southWest = L.latLng(1 / 0, 1 / 0), this._poly._bounds._northEast = L.latLng(-1 / 0, -1 / 0); var s = this._poly.getLatLngs(); this._poly._convertLatLngs(s, !0), this._poly.redraw(), this._poly.fire("editdrag") }, _onMarkerClick: function (t) { var e = L.Polygon && this._poly instanceof L.Polygon ? 4 : 3, i = t.target; this._defaultShape().length < e || (this._removeMarker(i), this._updatePrevNext(i._prev, i._next), i._middleLeft && this._markerGroup.removeLayer(i._middleLeft), i._middleRight && this._markerGroup.removeLayer(i._middleRight), i._prev && i._next ? this._createMiddleMarker(i._prev, i._next) : i._prev ? i._next || (i._prev._middleRight = null) : i._next._middleLeft = null, this._fireEdit()) }, _onContextMenu: function (t) { var e = t.target; this._poly; this._poly._map.fire(L.Draw.Event.MARKERCONTEXT, { marker: e, layers: this._markerGroup, poly: this._poly }), L.DomEvent.stopPropagation }, _onTouchMove: function (t) { var e = this._map.mouseEventToLayerPoint(t.originalEvent.touches[0]), i = this._map.layerPointToLatLng(e), o = t.target; L.extend(o._origLatLng, i), o._middleLeft && o._middleLeft.setLatLng(this._getMiddleLatLng(o._prev, o)), o._middleRight && o._middleRight.setLatLng(this._getMiddleLatLng(o, o._next)), this._poly.redraw(), this.updateMarkers() }, _updateIndexes: function (t, e) { this._markerGroup.eachLayer(function (i) { i._index > t && (i._index += e) }) }, _createMiddleMarker: function (t, e) { var i, o, a, n = this._getMiddleLatLng(t, e), s = this._createMarker(n); s.setOpacity(.6), t._middleRight = e._middleLeft = s, o = function () { s.off("touchmove", o, this); var a = e._index; s._index = a, s.off("click", i, this).on("click", this._onMarkerClick, this), n.lat = s.getLatLng().lat, n.lng = s.getLatLng().lng, this._spliceLatLngs(a, 0, n), this._markers.splice(a, 0, s), s.setOpacity(1), this._updateIndexes(a, 1), e._index++, this._updatePrevNext(t, s), this._updatePrevNext(s, e), this._poly.fire("editstart") }, a = function () { s.off("dragstart", o, this), s.off("dragend", a, this), s.off("touchmove", o, this), this._createMiddleMarker(t, s), this._createMiddleMarker(s, e) }, i = function () { o.call(this), a.call(this), this._fireEdit() }, s.on("click", i, this).on("dragstart", o, this).on("dragend", a, this).on("touchmove", o, this), this._markerGroup.addLayer(s) }, _updatePrevNext: function (t, e) { t && (t._next = e), e && (e._prev = t) }, _getMiddleLatLng: function (t, e) { var i = this._poly._map, o = i.project(t.getLatLng()), a = i.project(e.getLatLng()); return i.unproject(o._add(a)._divideBy(2)) } }), L.Polyline.addInitHook(function () { this.editing || (L.Edit.Poly && (this.editing = new L.Edit.Poly(this), this.options.editable && this.editing.enable()), this.on("add", function () { this.editing && this.editing.enabled() && this.editing.addHooks() }), this.on("remove", function () { this.editing && this.editing.enabled() && this.editing.removeHooks() })) }), L.Edit = L.Edit || {}, L.Edit.SimpleShape = L.Handler.extend({
        options: {
            moveIcon: new L.DivIcon({ iconSize: new L.Point(8, 8), className: "leaflet-div-icon leaflet-editing-icon leaflet-edit-move" }), resizeIcon: new L.DivIcon({
                iconSize: new L.Point(8, 8),
                className: "leaflet-div-icon leaflet-editing-icon leaflet-edit-resize"
            }), touchMoveIcon: new L.DivIcon({ iconSize: new L.Point(20, 20), className: "leaflet-div-icon leaflet-editing-icon leaflet-edit-move leaflet-touch-icon" }), touchResizeIcon: new L.DivIcon({ iconSize: new L.Point(20, 20), className: "leaflet-div-icon leaflet-editing-icon leaflet-edit-resize leaflet-touch-icon" })
        }, initialize: function (t, e) { L.Browser.touch && (this.options.moveIcon = this.options.touchMoveIcon, this.options.resizeIcon = this.options.touchResizeIcon), this._shape = t, L.Util.setOptions(this, e) }, addHooks: function () { var t = this._shape; this._shape._map && (this._map = this._shape._map, t.setStyle(t.options.editing), t._map && (this._map = t._map, this._markerGroup || this._initMarkers(), this._map.addLayer(this._markerGroup))) }, removeHooks: function () { var t = this._shape; if (t.setStyle(t.options.original), t._map) { this._unbindMarker(this._moveMarker); for (var e = 0, i = this._resizeMarkers.length; e < i; e++)this._unbindMarker(this._resizeMarkers[e]); this._resizeMarkers = null, this._map.removeLayer(this._markerGroup), delete this._markerGroup } this._map = null }, updateMarkers: function () { this._markerGroup.clearLayers(), this._initMarkers() }, _initMarkers: function () { this._markerGroup || (this._markerGroup = new L.LayerGroup), this._createMoveMarker(), this._createResizeMarker() }, _createMoveMarker: function () { }, _createResizeMarker: function () { }, _createMarker: function (t, e) { var i = new L.Marker.Touch(t, { draggable: !0, icon: e, zIndexOffset: 10 }); return this._bindMarker(i), this._markerGroup.addLayer(i), i }, _bindMarker: function (t) { t.on("dragstart", this._onMarkerDragStart, this).on("drag", this._onMarkerDrag, this).on("dragend", this._onMarkerDragEnd, this).on("touchstart", this._onTouchStart, this).on("touchmove", this._onTouchMove, this).on("MSPointerMove", this._onTouchMove, this).on("touchend", this._onTouchEnd, this).on("MSPointerUp", this._onTouchEnd, this) }, _unbindMarker: function (t) { t.off("dragstart", this._onMarkerDragStart, this).off("drag", this._onMarkerDrag, this).off("dragend", this._onMarkerDragEnd, this).off("touchstart", this._onTouchStart, this).off("touchmove", this._onTouchMove, this).off("MSPointerMove", this._onTouchMove, this).off("touchend", this._onTouchEnd, this).off("MSPointerUp", this._onTouchEnd, this) }, _onMarkerDragStart: function (t) { t.target.setOpacity(0), this._shape.fire("editstart") }, _fireEdit: function () { this._shape.edited = !0, this._shape.fire("edit") }, _onMarkerDrag: function (t) { var e = t.target, i = e.getLatLng(); e === this._moveMarker ? this._move(i) : this._resize(i), this._shape.redraw(), this._shape.fire("editdrag") }, _onMarkerDragEnd: function (t) { t.target.setOpacity(1), this._fireEdit() }, _onTouchStart: function (t) { if (L.Edit.SimpleShape.prototype._onMarkerDragStart.call(this, t), "function" == typeof this._getCorners) { var e = this._getCorners(), i = t.target, o = i._cornerIndex; i.setOpacity(0), this._oppositeCorner = e[(o + 2) % 4], this._toggleCornerMarkers(0, o) } this._shape.fire("editstart") }, _onTouchMove: function (t) { var e = this._map.mouseEventToLayerPoint(t.originalEvent.touches[0]), i = this._map.layerPointToLatLng(e); return t.target === this._moveMarker ? this._move(i) : this._resize(i), this._shape.redraw(), !1 }, _onTouchEnd: function (t) { t.target.setOpacity(1), this.updateMarkers(), this._fireEdit() }, _move: function () { }, _resize: function () { }
    }), L.Edit = L.Edit || {}, L.Edit.Rectangle = L.Edit.SimpleShape.extend({ _createMoveMarker: function () { var t = this._shape.getBounds(), e = t.getCenter(); this._moveMarker = this._createMarker(e, this.options.moveIcon) }, _createResizeMarker: function () { var t = this._getCorners(); this._resizeMarkers = []; for (var e = 0, i = t.length; e < i; e++)this._resizeMarkers.push(this._createMarker(t[e], this.options.resizeIcon)), this._resizeMarkers[e]._cornerIndex = e }, _onMarkerDragStart: function (t) { L.Edit.SimpleShape.prototype._onMarkerDragStart.call(this, t); var e = this._getCorners(), i = t.target, o = i._cornerIndex; this._oppositeCorner = e[(o + 2) % 4], this._toggleCornerMarkers(0, o) }, _onMarkerDragEnd: function (t) { var e, i, o = t.target; o === this._moveMarker && (e = this._shape.getBounds(), i = e.getCenter(), o.setLatLng(i)), this._toggleCornerMarkers(1), this._repositionCornerMarkers(), L.Edit.SimpleShape.prototype._onMarkerDragEnd.call(this, t) }, _move: function (t) { for (var e, i = this._shape._defaultShape ? this._shape._defaultShape() : this._shape.getLatLngs(), o = this._shape.getBounds(), a = o.getCenter(), n = [], s = 0, r = i.length; s < r; s++)e = [i[s].lat - a.lat, i[s].lng - a.lng], n.push([t.lat + e[0], t.lng + e[1]]); this._shape.setLatLngs(n), this._repositionCornerMarkers(), this._map.fire(L.Draw.Event.EDITMOVE, { layer: this._shape }) }, _resize: function (t) { var e; this._shape.setBounds(L.latLngBounds(t, this._oppositeCorner)), e = this._shape.getBounds(), this._moveMarker.setLatLng(e.getCenter()), this._map.fire(L.Draw.Event.EDITRESIZE, { layer: this._shape }) }, _getCorners: function () { var t = this._shape.getBounds(); return [t.getNorthWest(), t.getNorthEast(), t.getSouthEast(), t.getSouthWest()] }, _toggleCornerMarkers: function (t) { for (var e = 0, i = this._resizeMarkers.length; e < i; e++)this._resizeMarkers[e].setOpacity(t) }, _repositionCornerMarkers: function () { for (var t = this._getCorners(), e = 0, i = this._resizeMarkers.length; e < i; e++)this._resizeMarkers[e].setLatLng(t[e]) } }), L.Rectangle.addInitHook(function () { L.Edit.Rectangle && (this.editing = new L.Edit.Rectangle(this), this.options.editable && this.editing.enable()) }), L.Edit = L.Edit || {}, L.Edit.CircleMarker = L.Edit.SimpleShape.extend({ _createMoveMarker: function () { var t = this._shape.getLatLng(); this._moveMarker = this._createMarker(t, this.options.moveIcon) }, _createResizeMarker: function () { this._resizeMarkers = [] }, _move: function (t) { if (this._resizeMarkers.length) { var e = this._getResizeMarkerPoint(t); this._resizeMarkers[0].setLatLng(e) } this._shape.setLatLng(t), this._map.fire(L.Draw.Event.EDITMOVE, { layer: this._shape }) } }), L.CircleMarker.addInitHook(function () { L.Edit.CircleMarker && (this.editing = new L.Edit.CircleMarker(this), this.options.editable && this.editing.enable()), this.on("add", function () { this.editing && this.editing.enabled() && this.editing.addHooks() }), this.on("remove", function () { this.editing && this.editing.enabled() && this.editing.removeHooks() }) }), L.Edit = L.Edit || {}, L.Edit.Circle = L.Edit.CircleMarker.extend({ _createResizeMarker: function () { var t = this._shape.getLatLng(), e = this._getResizeMarkerPoint(t); this._resizeMarkers = [], this._resizeMarkers.push(this._createMarker(e, this.options.resizeIcon)) }, _getResizeMarkerPoint: function (t) { var e = this._shape._radius * Math.cos(Math.PI / 4), i = this._map.project(t); return this._map.unproject([i.x + e, i.y - e]) }, _resize: function (t) { var e = this._moveMarker.getLatLng(); L.GeometryUtil.isVersion07x() ? radius = e.distanceTo(t) : radius = this._map.distance(e, t), this._shape.setRadius(radius), this._map.editTooltip && this._map._editTooltip.updateContent({ text: L.drawLocal.edit.handlers.edit.tooltip.subtext + "<br />" + L.drawLocal.edit.handlers.edit.tooltip.text, subtext: L.drawLocal.draw.handlers.circle.radius + ": " + L.GeometryUtil.readableDistance(radius, !0, this.options.feet, this.options.nautic) }), this._shape.setRadius(radius), this._map.fire(L.Draw.Event.EDITRESIZE, { layer: this._shape }) } }), L.Circle.addInitHook(function () { L.Edit.Circle && (this.editing = new L.Edit.Circle(this), this.options.editable && this.editing.enable()) }), L.Map.mergeOptions({ touchExtend: !0 }), L.Map.TouchExtend = L.Handler.extend({ initialize: function (t) { this._map = t, this._container = t._container, this._pane = t._panes.overlayPane }, addHooks: function () { L.DomEvent.on(this._container, "touchstart", this._onTouchStart, this), L.DomEvent.on(this._container, "touchend", this._onTouchEnd, this), L.DomEvent.on(this._container, "touchmove", this._onTouchMove, this), this._detectIE() ? (L.DomEvent.on(this._container, "MSPointerDown", this._onTouchStart, this), L.DomEvent.on(this._container, "MSPointerUp", this._onTouchEnd, this), L.DomEvent.on(this._container, "MSPointerMove", this._onTouchMove, this), L.DomEvent.on(this._container, "MSPointerCancel", this._onTouchCancel, this)) : (L.DomEvent.on(this._container, "touchcancel", this._onTouchCancel, this), L.DomEvent.on(this._container, "touchleave", this._onTouchLeave, this)) }, removeHooks: function () { L.DomEvent.off(this._container, "touchstart", this._onTouchStart, this), L.DomEvent.off(this._container, "touchend", this._onTouchEnd, this), L.DomEvent.off(this._container, "touchmove", this._onTouchMove, this), this._detectIE() ? (L.DomEvent.off(this._container, "MSPointerDown", this._onTouchStart, this), L.DomEvent.off(this._container, "MSPointerUp", this._onTouchEnd, this), L.DomEvent.off(this._container, "MSPointerMove", this._onTouchMove, this), L.DomEvent.off(this._container, "MSPointerCancel", this._onTouchCancel, this)) : (L.DomEvent.off(this._container, "touchcancel", this._onTouchCancel, this), L.DomEvent.off(this._container, "touchleave", this._onTouchLeave, this)) }, _touchEvent: function (t, e) { var i = {}; if (void 0 !== t.touches) { if (!t.touches.length) return; i = t.touches[0] } else { if ("touch" !== t.pointerType) return; if (i = t, !this._filterClick(t)) return } var o = this._map.mouseEventToContainerPoint(i), a = this._map.mouseEventToLayerPoint(i), n = this._map.layerPointToLatLng(a); this._map.fire(e, { latlng: n, layerPoint: a, containerPoint: o, pageX: i.pageX, pageY: i.pageY, originalEvent: t }) }, _filterClick: function (t) { var e = t.timeStamp || t.originalEvent.timeStamp, i = L.DomEvent._lastClick && e - L.DomEvent._lastClick; return i && i > 100 && i < 500 || t.target._simulatedClick && !t._simulated ? (L.DomEvent.stop(t), !1) : (L.DomEvent._lastClick = e, !0) }, _onTouchStart: function (t) { if (this._map._loaded) { this._touchEvent(t, "touchstart") } }, _onTouchEnd: function (t) { if (this._map._loaded) { this._touchEvent(t, "touchend") } }, _onTouchCancel: function (t) { if (this._map._loaded) { var e = "touchcancel"; this._detectIE() && (e = "pointercancel"), this._touchEvent(t, e) } }, _onTouchLeave: function (t) { if (this._map._loaded) { this._touchEvent(t, "touchleave") } }, _onTouchMove: function (t) { if (this._map._loaded) { this._touchEvent(t, "touchmove") } }, _detectIE: function () { var e = t.navigator.userAgent, i = e.indexOf("MSIE "); if (i > 0) return parseInt(e.substring(i + 5, e.indexOf(".", i)), 10); if (e.indexOf("Trident/") > 0) { var o = e.indexOf("rv:"); return parseInt(e.substring(o + 3, e.indexOf(".", o)), 10) } var a = e.indexOf("Edge/"); return a > 0 && parseInt(e.substring(a + 5, e.indexOf(".", a)), 10) } }), L.Map.addInitHook("addHandler", "touchExtend", L.Map.TouchExtend), L.Marker.Touch = L.Marker.extend({ _initInteraction: function () { return this.addInteractiveTarget ? L.Marker.prototype._initInteraction.apply(this) : this._initInteractionLegacy() }, _initInteractionLegacy: function () { if (this.options.clickable) { var t = this._icon, e = ["dblclick", "mousedown", "mouseover", "mouseout", "contextmenu", "touchstart", "touchend", "touchmove"]; this._detectIE ? e.concat(["MSPointerDown", "MSPointerUp", "MSPointerMove", "MSPointerCancel"]) : e.concat(["touchcancel"]), L.DomUtil.addClass(t, "leaflet-clickable"), L.DomEvent.on(t, "click", this._onMouseClick, this), L.DomEvent.on(t, "keypress", this._onKeyPress, this); for (var i = 0; i < e.length; i++)L.DomEvent.on(t, e[i], this._fireMouseEvent, this); L.Handler.MarkerDrag && (this.dragging = new L.Handler.MarkerDrag(this), this.options.draggable && this.dragging.enable()) } }, _detectIE: function () { var e = t.navigator.userAgent, i = e.indexOf("MSIE "); if (i > 0) return parseInt(e.substring(i + 5, e.indexOf(".", i)), 10); if (e.indexOf("Trident/") > 0) { var o = e.indexOf("rv:"); return parseInt(e.substring(o + 3, e.indexOf(".", o)), 10) } var a = e.indexOf("Edge/"); return a > 0 && parseInt(e.substring(a + 5, e.indexOf(".", a)), 10) } }), L.LatLngUtil = { cloneLatLngs: function (t) { for (var e = [], i = 0, o = t.length; i < o; i++)Array.isArray(t[i]) ? e.push(L.LatLngUtil.cloneLatLngs(t[i])) : e.push(this.cloneLatLng(t[i])); return e }, cloneLatLng: function (t) { return L.latLng(t.lat, t.lng) } }, function () { var t = { km: 2, ha: 2, m: 0, mi: 2, ac: 2, yd: 0, ft: 0, nm: 2 }; L.GeometryUtil = L.extend(L.GeometryUtil || {}, { geodesicArea: function (t) { var e, i, o = t.length, a = 0, n = Math.PI / 180; if (o > 2) { for (var s = 0; s < o; s++)e = t[s], i = t[(s + 1) % o], a += (i.lng - e.lng) * n * (2 + Math.sin(e.lat * n) + Math.sin(i.lat * n)); a = 6378137 * a * 6378137 / 2 } return Math.abs(a) }, formattedNumber: function (t, e) { var i = parseFloat(t).toFixed(e), o = L.drawLocal.format && L.drawLocal.format.numeric, a = o && o.delimiters, n = a && a.thousands, s = a && a.decimal; if (n || s) { var r = i.split("."); i = n ? r[0].replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1" + n) : r[0], s = s || ".", r.length > 1 && (i = i + s + r[1]) } return i }, readableArea: function (e, i, o) { var a, n, o = L.Util.extend({}, t, o); return i ? (n = ["ha", "m"], type = typeof i, "string" === type ? n = [i] : "boolean" !== type && (n = i), a = e >= 1e6 && -1 !== n.indexOf("km") ? L.GeometryUtil.formattedNumber(1e-6 * e, o.km) + " km²" : e >= 1e4 && -1 !== n.indexOf("ha") ? L.GeometryUtil.formattedNumber(1e-4 * e, o.ha) + " ha" : L.GeometryUtil.formattedNumber(e, o.m) + " m²") : (e /= .836127, a = e >= 3097600 ? L.GeometryUtil.formattedNumber(e / 3097600, o.mi) + " mi²" : e >= 4840 ? L.GeometryUtil.formattedNumber(e / 4840, o.ac) + " acres" : L.GeometryUtil.formattedNumber(e, o.yd) + " yd²"), a }, readableDistance: function (e, i, o, a, n) { var s, n = L.Util.extend({}, t, n); switch (i ? "string" == typeof i ? i : "metric" : o ? "feet" : a ? "nauticalMile" : "yards") { case "metric": s = e > 1e3 ? L.GeometryUtil.formattedNumber(e / 1e3, n.km) + " km" : L.GeometryUtil.formattedNumber(e, n.m) + " m"; break; case "feet": e *= 3.28083, s = L.GeometryUtil.formattedNumber(e, n.ft) + " ft"; break; case "nauticalMile": e *= .53996, s = L.GeometryUtil.formattedNumber(e / 1e3, n.nm) + " nm"; break; case "yards": default: e *= 1.09361, s = e > 1760 ? L.GeometryUtil.formattedNumber(e / 1760, n.mi) + " miles" : L.GeometryUtil.formattedNumber(e, n.yd) + " yd" }return s }, isVersion07x: function () { var t = L.version.split("."); return 0 === parseInt(t[0], 10) && 7 === parseInt(t[1], 10) } }) }(), L.Util.extend(L.LineUtil, { segmentsIntersect: function (t, e, i, o) { return this._checkCounterclockwise(t, i, o) !== this._checkCounterclockwise(e, i, o) && this._checkCounterclockwise(t, e, i) !== this._checkCounterclockwise(t, e, o) }, _checkCounterclockwise: function (t, e, i) { return (i.y - t.y) * (e.x - t.x) > (e.y - t.y) * (i.x - t.x) } }), L.Polyline.include({ intersects: function () { var t, e, i, o = this._getProjectedPoints(), a = o ? o.length : 0; if (this._tooFewPointsForIntersection()) return !1; for (t = a - 1; t >= 3; t--)if (e = o[t - 1], i = o[t], this._lineSegmentsIntersectsRange(e, i, t - 2)) return !0; return !1 }, newLatLngIntersects: function (t, e) { return !!this._map && this.newPointIntersects(this._map.latLngToLayerPoint(t), e) }, newPointIntersects: function (t, e) { var i = this._getProjectedPoints(), o = i ? i.length : 0, a = i ? i[o - 1] : null, n = o - 2; return !this._tooFewPointsForIntersection(1) && this._lineSegmentsIntersectsRange(a, t, n, e ? 1 : 0) }, _tooFewPointsForIntersection: function (t) { var e = this._getProjectedPoints(), i = e ? e.length : 0; return i += t || 0, !e || i <= 3 }, _lineSegmentsIntersectsRange: function (t, e, i, o) { var a, n, s = this._getProjectedPoints(); o = o || 0; for (var r = i; r > o; r--)if (a = s[r - 1], n = s[r], L.LineUtil.segmentsIntersect(t, e, a, n)) return !0; return !1 }, _getProjectedPoints: function () { if (!this._defaultShape) return this._originalPoints; for (var t = [], e = this._defaultShape(), i = 0; i < e.length; i++)t.push(this._map.latLngToLayerPoint(e[i])); return t } }), L.Polygon.include({ intersects: function () { var t, e, i, o, a = this._getProjectedPoints(); return !this._tooFewPointsForIntersection() && (!!L.Polyline.prototype.intersects.call(this) || (t = a.length, e = a[0], i = a[t - 1], o = t - 2, this._lineSegmentsIntersectsRange(i, e, o, 1))) } }), L.Control.Draw = L.Control.extend({ options: { position: "topleft", draw: {}, edit: !1 }, initialize: function (t) { if (L.version < "0.7") throw new Error("Leaflet.draw 0.2.3+ requires Leaflet 0.7.0+. Download latest from https://github.com/Leaflet/Leaflet/"); L.Control.prototype.initialize.call(this, t); var e; this._toolbars = {}, L.DrawToolbar && this.options.draw && (e = new L.DrawToolbar(this.options.draw), this._toolbars[L.DrawToolbar.TYPE] = e, this._toolbars[L.DrawToolbar.TYPE].on("enable", this._toolbarEnabled, this)), L.EditToolbar && this.options.edit && (e = new L.EditToolbar(this.options.edit), this._toolbars[L.EditToolbar.TYPE] = e, this._toolbars[L.EditToolbar.TYPE].on("enable", this._toolbarEnabled, this)), L.toolbar = this }, onAdd: function (t) { var e, i = L.DomUtil.create("div", "leaflet-draw"), o = !1; for (var a in this._toolbars) this._toolbars.hasOwnProperty(a) && (e = this._toolbars[a].addToolbar(t)) && (o || (L.DomUtil.hasClass(e, "leaflet-draw-toolbar-top") || L.DomUtil.addClass(e.childNodes[0], "leaflet-draw-toolbar-top"), o = !0), i.appendChild(e)); return i }, onRemove: function () { for (var t in this._toolbars) this._toolbars.hasOwnProperty(t) && this._toolbars[t].removeToolbar() }, setDrawingOptions: function (t) { for (var e in this._toolbars) this._toolbars[e] instanceof L.DrawToolbar && this._toolbars[e].setOptions(t) }, _toolbarEnabled: function (t) { var e = t.target; for (var i in this._toolbars) this._toolbars[i] !== e && this._toolbars[i].disable() } }), L.Map.mergeOptions({ drawControlTooltips: !0, drawControl: !1 }), L.Map.addInitHook(function () { this.options.drawControl && (this.drawControl = new L.Control.Draw, this.addControl(this.drawControl)) }), L.Toolbar = L.Class.extend({ initialize: function (t) { L.setOptions(this, t), this._modes = {}, this._actionButtons = [], this._activeMode = null; var e = L.version.split("."); 1 === parseInt(e[0], 10) && parseInt(e[1], 10) >= 2 ? L.Toolbar.include(L.Evented.prototype) : L.Toolbar.include(L.Mixin.Events) }, enabled: function () { return null !== this._activeMode }, disable: function () { this.enabled() && this._activeMode.handler.disable() }, addToolbar: function (t) { var e, i = L.DomUtil.create("div", "leaflet-draw-section"), o = 0, a = this._toolbarClass || "", n = this.getModeHandlers(t); for (this._toolbarContainer = L.DomUtil.create("div", "leaflet-draw-toolbar leaflet-bar"), this._map = t, e = 0; e < n.length; e++)n[e].enabled && this._initModeHandler(n[e].handler, this._toolbarContainer, o++, a, n[e].title); if (o) return this._lastButtonIndex = --o, this._actionsContainer = L.DomUtil.create("ul", "leaflet-draw-actions"), i.appendChild(this._toolbarContainer), i.appendChild(this._actionsContainer), i }, removeToolbar: function () { for (var t in this._modes) this._modes.hasOwnProperty(t) && (this._disposeButton(this._modes[t].button, this._modes[t].handler.enable, this._modes[t].handler), this._modes[t].handler.disable(), this._modes[t].handler.off("enabled", this._handlerActivated, this).off("disabled", this._handlerDeactivated, this)); this._modes = {}; for (var e = 0, i = this._actionButtons.length; e < i; e++)this._disposeButton(this._actionButtons[e].button, this._actionButtons[e].callback, this); this._actionButtons = [], this._actionsContainer = null }, _initModeHandler: function (t, e, i, o, a) { var n = t.type; this._modes[n] = {}, this._modes[n].handler = t, this._modes[n].button = this._createButton({ type: n, title: a, className: o + "-" + n, container: e, callback: this._modes[n].handler.enable, context: this._modes[n].handler }), this._modes[n].buttonIndex = i, this._modes[n].handler.on("enabled", this._handlerActivated, this).on("disabled", this._handlerDeactivated, this) }, _detectIOS: function () { return /iPad|iPhone|iPod/.test(navigator.userAgent) && !t.MSStream }, _createButton: function (t) { var e = L.DomUtil.create("a", t.className || "", t.container), i = L.DomUtil.create("span", "sr-only", t.container); e.href = "#", e.appendChild(i), t.title && (e.title = t.title, i.innerHTML = t.title), t.text && (e.innerHTML = t.text, i.innerHTML = t.text); var o = this._detectIOS() ? "touchstart" : "click"; return L.DomEvent.on(e, "click", L.DomEvent.stopPropagation).on(e, "mousedown", L.DomEvent.stopPropagation).on(e, "dblclick", L.DomEvent.stopPropagation).on(e, "touchstart", L.DomEvent.stopPropagation).on(e, "click", L.DomEvent.preventDefault).on(e, o, t.callback, t.context), e }, _disposeButton: function (t, e) { var i = this._detectIOS() ? "touchstart" : "click"; L.DomEvent.off(t, "click", L.DomEvent.stopPropagation).off(t, "mousedown", L.DomEvent.stopPropagation).off(t, "dblclick", L.DomEvent.stopPropagation).off(t, "touchstart", L.DomEvent.stopPropagation).off(t, "click", L.DomEvent.preventDefault).off(t, i, e) }, _handlerActivated: function (t) { this.disable(), this._activeMode = this._modes[t.handler], L.DomUtil.addClass(this._activeMode.button, "leaflet-draw-toolbar-button-enabled"), this._showActionsToolbar(), this.fire("enable") }, _handlerDeactivated: function () { this._hideActionsToolbar(), L.DomUtil.removeClass(this._activeMode.button, "leaflet-draw-toolbar-button-enabled"), this._activeMode = null, this.fire("disable") }, _createActions: function (t) { var e, i, o, a, n = this._actionsContainer, s = this.getActions(t), r = s.length; for (i = 0, o = this._actionButtons.length; i < o; i++)this._disposeButton(this._actionButtons[i].button, this._actionButtons[i].callback); for (this._actionButtons = []; n.firstChild;)n.removeChild(n.firstChild); for (var l = 0; l < r; l++)"enabled" in s[l] && !s[l].enabled || (e = L.DomUtil.create("li", "", n), a = this._createButton({ title: s[l].title, text: s[l].text, container: e, callback: s[l].callback, context: s[l].context }), this._actionButtons.push({ button: a, callback: s[l].callback })) }, _showActionsToolbar: function () { var t = this._activeMode.buttonIndex, e = this._lastButtonIndex, i = this._activeMode.button.offsetTop - 1; this._createActions(this._activeMode.handler), this._actionsContainer.style.top = i + "px", 0 === t && (L.DomUtil.addClass(this._toolbarContainer, "leaflet-draw-toolbar-notop"), L.DomUtil.addClass(this._actionsContainer, "leaflet-draw-actions-top")), t === e && (L.DomUtil.addClass(this._toolbarContainer, "leaflet-draw-toolbar-nobottom"), L.DomUtil.addClass(this._actionsContainer, "leaflet-draw-actions-bottom")), this._actionsContainer.style.display = "block", this._map.fire(L.Draw.Event.TOOLBAROPENED) }, _hideActionsToolbar: function () { this._actionsContainer.style.display = "none", L.DomUtil.removeClass(this._toolbarContainer, "leaflet-draw-toolbar-notop"), L.DomUtil.removeClass(this._toolbarContainer, "leaflet-draw-toolbar-nobottom"), L.DomUtil.removeClass(this._actionsContainer, "leaflet-draw-actions-top"), L.DomUtil.removeClass(this._actionsContainer, "leaflet-draw-actions-bottom"), this._map.fire(L.Draw.Event.TOOLBARCLOSED) } }), L.Draw = L.Draw || {}, L.Draw.Tooltip = L.Class.extend({ initialize: function (t) { this._map = t, this._popupPane = t._panes.popupPane, this._visible = !1, this._container = t.options.drawControlTooltips ? L.DomUtil.create("div", "leaflet-draw-tooltip", this._popupPane) : null, this._singleLineLabel = !1, this._map.on("mouseout", this._onMouseOut, this) }, dispose: function () { this._map.off("mouseout", this._onMouseOut, this), this._container && (this._popupPane.removeChild(this._container), this._container = null) }, updateContent: function (t) { return this._container ? (t.subtext = t.subtext || "", 0 !== t.subtext.length || this._singleLineLabel ? t.subtext.length > 0 && this._singleLineLabel && (L.DomUtil.removeClass(this._container, "leaflet-draw-tooltip-single"), this._singleLineLabel = !1) : (L.DomUtil.addClass(this._container, "leaflet-draw-tooltip-single"), this._singleLineLabel = !0), this._container.innerHTML = (t.subtext.length > 0 ? '<span class="leaflet-draw-tooltip-subtext">' + t.subtext + "</span><br />" : "") + "<span>" + t.text + "</span>", t.text || t.subtext ? (this._visible = !0, this._container.style.visibility = "inherit") : (this._visible = !1, this._container.style.visibility = "hidden"), this) : this }, updatePosition: function (t) { var e = this._map.latLngToLayerPoint(t), i = this._container; return this._container && (this._visible && (i.style.visibility = "inherit"), L.DomUtil.setPosition(i, e)), this }, showAsError: function () { return this._container && L.DomUtil.addClass(this._container, "leaflet-error-draw-tooltip"), this }, removeError: function () { return this._container && L.DomUtil.removeClass(this._container, "leaflet-error-draw-tooltip"), this }, _onMouseOut: function () { this._container && (this._container.style.visibility = "hidden") } }), L.DrawToolbar = L.Toolbar.extend({ statics: { TYPE: "draw" }, options: { polyline: {}, polygon: {}, rectangle: {}, circle: {}, marker: {}, circlemarker: {} }, initialize: function (t) { for (var e in this.options) this.options.hasOwnProperty(e) && t[e] && (t[e] = L.extend({}, this.options[e], t[e])); this._toolbarClass = "leaflet-draw-draw", L.Toolbar.prototype.initialize.call(this, t) }, getModeHandlers: function (t) { return [{ enabled: this.options.polyline, handler: new L.Draw.Polyline(t, this.options.polyline), title: L.drawLocal.draw.toolbar.buttons.polyline }, { enabled: this.options.polygon, handler: new L.Draw.Polygon(t, this.options.polygon), title: L.drawLocal.draw.toolbar.buttons.polygon }, { enabled: this.options.rectangle, handler: new L.Draw.Rectangle(t, this.options.rectangle), title: L.drawLocal.draw.toolbar.buttons.rectangle }, { enabled: this.options.circle, handler: new L.Draw.Circle(t, this.options.circle), title: L.drawLocal.draw.toolbar.buttons.circle }, { enabled: this.options.marker, handler: new L.Draw.Marker(t, this.options.marker), title: L.drawLocal.draw.toolbar.buttons.marker }, { enabled: this.options.circlemarker, handler: new L.Draw.CircleMarker(t, this.options.circlemarker), title: L.drawLocal.draw.toolbar.buttons.circlemarker }] }, getActions: function (t) { return [{ enabled: t.completeShape, title: L.drawLocal.draw.toolbar.finish.title, text: L.drawLocal.draw.toolbar.finish.text, callback: t.completeShape, context: t }, { enabled: t.deleteLastVertex, title: L.drawLocal.draw.toolbar.undo.title, text: L.drawLocal.draw.toolbar.undo.text, callback: t.deleteLastVertex, context: t }, { title: L.drawLocal.draw.toolbar.actions.title, text: L.drawLocal.draw.toolbar.actions.text, callback: this.disable, context: this }] }, setOptions: function (t) { L.setOptions(this, t); for (var e in this._modes) this._modes.hasOwnProperty(e) && t.hasOwnProperty(e) && this._modes[e].handler.setOptions(t[e]) } }), L.EditToolbar = L.Toolbar.extend({ statics: { TYPE: "edit" }, options: { edit: { selectedPathOptions: { dashArray: "10, 10", fill: !0, fillColor: "#fe57a1", fillOpacity: .1, maintainColor: !1 } }, remove: {}, poly: null, featureGroup: null }, initialize: function (t) { t.edit && (void 0 === t.edit.selectedPathOptions && (t.edit.selectedPathOptions = this.options.edit.selectedPathOptions), t.edit.selectedPathOptions = L.extend({}, this.options.edit.selectedPathOptions, t.edit.selectedPathOptions)), t.remove && (t.remove = L.extend({}, this.options.remove, t.remove)), t.poly && (t.poly = L.extend({}, this.options.poly, t.poly)), this._toolbarClass = "leaflet-draw-edit", L.Toolbar.prototype.initialize.call(this, t), this._selectedFeatureCount = 0 }, getModeHandlers: function (t) { var e = this.options.featureGroup; return [{ enabled: this.options.edit, handler: new L.EditToolbar.Edit(t, { featureGroup: e, selectedPathOptions: this.options.edit.selectedPathOptions, poly: this.options.poly }), title: L.drawLocal.edit.toolbar.buttons.edit }, { enabled: this.options.remove, handler: new L.EditToolbar.Delete(t, { featureGroup: e }), title: L.drawLocal.edit.toolbar.buttons.remove }] }, getActions: function (t) { var e = [{ title: L.drawLocal.edit.toolbar.actions.save.title, text: L.drawLocal.edit.toolbar.actions.save.text, callback: this._save, context: this }, { title: L.drawLocal.edit.toolbar.actions.cancel.title, text: L.drawLocal.edit.toolbar.actions.cancel.text, callback: this.disable, context: this }]; return t.removeAllLayers && e.push({ title: L.drawLocal.edit.toolbar.actions.clearAll.title, text: L.drawLocal.edit.toolbar.actions.clearAll.text, callback: this._clearAllLayers, context: this }), e }, addToolbar: function (t) { var e = L.Toolbar.prototype.addToolbar.call(this, t); return this._checkDisabled(), this.options.featureGroup.on("layeradd layerremove", this._checkDisabled, this), e }, removeToolbar: function () { this.options.featureGroup.off("layeradd layerremove", this._checkDisabled, this), L.Toolbar.prototype.removeToolbar.call(this) }, disable: function () { this.enabled() && (this._activeMode.handler.revertLayers(), L.Toolbar.prototype.disable.call(this)) }, _save: function () { this._activeMode.handler.save(), this._activeMode && this._activeMode.handler.disable() }, _clearAllLayers: function () { this._activeMode.handler.removeAllLayers(), this._activeMode && this._activeMode.handler.disable() }, _checkDisabled: function () { var t, e = this.options.featureGroup, i = 0 !== e.getLayers().length; this.options.edit && (t = this._modes[L.EditToolbar.Edit.TYPE].button, i ? L.DomUtil.removeClass(t, "leaflet-disabled") : L.DomUtil.addClass(t, "leaflet-disabled"), t.setAttribute("title", i ? L.drawLocal.edit.toolbar.buttons.edit : L.drawLocal.edit.toolbar.buttons.editDisabled)), this.options.remove && (t = this._modes[L.EditToolbar.Delete.TYPE].button, i ? L.DomUtil.removeClass(t, "leaflet-disabled") : L.DomUtil.addClass(t, "leaflet-disabled"), t.setAttribute("title", i ? L.drawLocal.edit.toolbar.buttons.remove : L.drawLocal.edit.toolbar.buttons.removeDisabled)) } }), L.EditToolbar.Edit = L.Handler.extend({
        statics: { TYPE: "edit" }, initialize: function (t, e) { if (L.Handler.prototype.initialize.call(this, t), L.setOptions(this, e), this._featureGroup = e.featureGroup, !(this._featureGroup instanceof L.FeatureGroup)) throw new Error("options.featureGroup must be a L.FeatureGroup"); this._uneditedLayerProps = {}, this.type = L.EditToolbar.Edit.TYPE; var i = L.version.split("."); 1 === parseInt(i[0], 10) && parseInt(i[1], 10) >= 2 ? L.EditToolbar.Edit.include(L.Evented.prototype) : L.EditToolbar.Edit.include(L.Mixin.Events) }, enable: function () { !this._enabled && this._hasAvailableLayers() && (this.fire("enabled", { handler: this.type }), this._map.fire(L.Draw.Event.EDITSTART, { handler: this.type }), L.Handler.prototype.enable.call(this), this._featureGroup.on("layeradd", this._enableLayerEdit, this).on("layerremove", this._disableLayerEdit, this)) }, disable: function () { this._enabled && (this._featureGroup.off("layeradd", this._enableLayerEdit, this).off("layerremove", this._disableLayerEdit, this), L.Handler.prototype.disable.call(this), this._map.fire(L.Draw.Event.EDITSTOP, { handler: this.type }), this.fire("disabled", { handler: this.type })) }, addHooks: function () { var t = this._map; t && (t.getContainer().focus(), this._featureGroup.eachLayer(this._enableLayerEdit, this), this._tooltip = new L.Draw.Tooltip(this._map), this._tooltip.updateContent({ text: L.drawLocal.edit.handlers.edit.tooltip.text, subtext: L.drawLocal.edit.handlers.edit.tooltip.subtext }), t._editTooltip = this._tooltip, this._updateTooltip(), this._map.on("mousemove", this._onMouseMove, this).on("touchmove", this._onMouseMove, this).on("MSPointerMove", this._onMouseMove, this).on(L.Draw.Event.EDITVERTEX, this._updateTooltip, this)) }, removeHooks: function () { this._map && (this._featureGroup.eachLayer(this._disableLayerEdit, this), this._uneditedLayerProps = {}, this._tooltip.dispose(), this._tooltip = null, this._map.off("mousemove", this._onMouseMove, this).off("touchmove", this._onMouseMove, this).off("MSPointerMove", this._onMouseMove, this).off(L.Draw.Event.EDITVERTEX, this._updateTooltip, this)) }, revertLayers: function () { this._featureGroup.eachLayer(function (t) { this._revertLayer(t) }, this) }, save: function () { var t = new L.LayerGroup; this._featureGroup.eachLayer(function (e) { e.edited && (t.addLayer(e), e.edited = !1) }), this._map.fire(L.Draw.Event.EDITED, { layers: t }) }, _backupLayer: function (t) { var e = L.Util.stamp(t); this._uneditedLayerProps[e] || (t instanceof L.Polyline || t instanceof L.Polygon || t instanceof L.Rectangle ? this._uneditedLayerProps[e] = { latlngs: L.LatLngUtil.cloneLatLngs(t.getLatLngs()) } : t instanceof L.Circle ? this._uneditedLayerProps[e] = { latlng: L.LatLngUtil.cloneLatLng(t.getLatLng()), radius: t.getRadius() } : (t instanceof L.Marker || t instanceof L.CircleMarker) && (this._uneditedLayerProps[e] = { latlng: L.LatLngUtil.cloneLatLng(t.getLatLng()) })) }, _getTooltipText: function () { return { text: L.drawLocal.edit.handlers.edit.tooltip.text, subtext: L.drawLocal.edit.handlers.edit.tooltip.subtext } }, _updateTooltip: function () { this._tooltip.updateContent(this._getTooltipText()) }, _revertLayer: function (t) { var e = L.Util.stamp(t); t.edited = !1, this._uneditedLayerProps.hasOwnProperty(e) && (t instanceof L.Polyline || t instanceof L.Polygon || t instanceof L.Rectangle ? t.setLatLngs(this._uneditedLayerProps[e].latlngs) : t instanceof L.Circle ? (t.setLatLng(this._uneditedLayerProps[e].latlng), t.setRadius(this._uneditedLayerProps[e].radius)) : (t instanceof L.Marker || t instanceof L.CircleMarker) && t.setLatLng(this._uneditedLayerProps[e].latlng), t.fire("revert-edited", { layer: t })) }, _enableLayerEdit: function (t) { var e, i, o = t.layer || t.target || t; this._backupLayer(o), this.options.poly && (i = L.Util.extend({}, this.options.poly), o.options.poly = i), this.options.selectedPathOptions && (e = L.Util.extend({}, this.options.selectedPathOptions), e.maintainColor && (e.color = o.options.color, e.fillColor = o.options.fillColor), o.options.original = L.extend({}, o.options), o.options.editing = e), o instanceof L.Marker ? (o.editing && o.editing.enable(), o.dragging.enable(), o.on("dragend", this._onMarkerDragEnd).on("touchmove", this._onTouchMove, this).on("MSPointerMove", this._onTouchMove, this).on("touchend", this._onMarkerDragEnd, this).on("MSPointerUp", this._onMarkerDragEnd, this)) : o.editing.enable() }, _disableLayerEdit: function (t) {
            var e = t.layer || t.target || t; e.edited = !1, e.editing && e.editing.disable(), delete e.options.editing, delete e.options.original,
                this._selectedPathOptions && (e instanceof L.Marker ? this._toggleMarkerHighlight(e) : (e.setStyle(e.options.previousOptions), delete e.options.previousOptions)), e instanceof L.Marker ? (e.dragging.disable(), e.off("dragend", this._onMarkerDragEnd, this).off("touchmove", this._onTouchMove, this).off("MSPointerMove", this._onTouchMove, this).off("touchend", this._onMarkerDragEnd, this).off("MSPointerUp", this._onMarkerDragEnd, this)) : e.editing.disable()
        }, _onMouseMove: function (t) { this._tooltip.updatePosition(t.latlng) }, _onMarkerDragEnd: function (t) { var e = t.target; e.edited = !0, this._map.fire(L.Draw.Event.EDITMOVE, { layer: e }) }, _onTouchMove: function (t) { var e = t.originalEvent.changedTouches[0], i = this._map.mouseEventToLayerPoint(e), o = this._map.layerPointToLatLng(i); t.target.setLatLng(o) }, _hasAvailableLayers: function () { return 0 !== this._featureGroup.getLayers().length }
    }), L.EditToolbar.Delete = L.Handler.extend({ statics: { TYPE: "remove" }, initialize: function (t, e) { if (L.Handler.prototype.initialize.call(this, t), L.Util.setOptions(this, e), this._deletableLayers = this.options.featureGroup, !(this._deletableLayers instanceof L.FeatureGroup)) throw new Error("options.featureGroup must be a L.FeatureGroup"); this.type = L.EditToolbar.Delete.TYPE; var i = L.version.split("."); 1 === parseInt(i[0], 10) && parseInt(i[1], 10) >= 2 ? L.EditToolbar.Delete.include(L.Evented.prototype) : L.EditToolbar.Delete.include(L.Mixin.Events) }, enable: function () { !this._enabled && this._hasAvailableLayers() && (this.fire("enabled", { handler: this.type }), this._map.fire(L.Draw.Event.DELETESTART, { handler: this.type }), L.Handler.prototype.enable.call(this), this._deletableLayers.on("layeradd", this._enableLayerDelete, this).on("layerremove", this._disableLayerDelete, this)) }, disable: function () { this._enabled && (this._deletableLayers.off("layeradd", this._enableLayerDelete, this).off("layerremove", this._disableLayerDelete, this), L.Handler.prototype.disable.call(this), this._map.fire(L.Draw.Event.DELETESTOP, { handler: this.type }), this.fire("disabled", { handler: this.type })) }, addHooks: function () { var t = this._map; t && (t.getContainer().focus(), this._deletableLayers.eachLayer(this._enableLayerDelete, this), this._deletedLayers = new L.LayerGroup, this._tooltip = new L.Draw.Tooltip(this._map), this._tooltip.updateContent({ text: L.drawLocal.edit.handlers.remove.tooltip.text }), this._map.on("mousemove", this._onMouseMove, this)) }, removeHooks: function () { this._map && (this._deletableLayers.eachLayer(this._disableLayerDelete, this), this._deletedLayers = null, this._tooltip.dispose(), this._tooltip = null, this._map.off("mousemove", this._onMouseMove, this)) }, revertLayers: function () { this._deletedLayers.eachLayer(function (t) { this._deletableLayers.addLayer(t), t.fire("revert-deleted", { layer: t }) }, this) }, save: function () { this._map.fire(L.Draw.Event.DELETED, { layers: this._deletedLayers }) }, removeAllLayers: function () { this._deletableLayers.eachLayer(function (t) { this._removeLayer({ layer: t }) }, this), this.save() }, _enableLayerDelete: function (t) { (t.layer || t.target || t).on("click", this._removeLayer, this) }, _disableLayerDelete: function (t) { var e = t.layer || t.target || t; e.off("click", this._removeLayer, this), this._deletedLayers.removeLayer(e) }, _removeLayer: function (t) { var e = t.layer || t.target || t; this._deletableLayers.removeLayer(e), this._deletedLayers.addLayer(e), e.fire("deleted") }, _onMouseMove: function (t) { this._tooltip.updatePosition(t.latlng) }, _hasAvailableLayers: function () { return 0 !== this._deletableLayers.getLayers().length } })
}(window, document);
/*! Leaflet-WMS 1.0.0 2017-08-02 */
!function (window, document, undefined) { "use strict"; String.prototype.trim || (String.prototype.trim = function () { return this.replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g, "") }), Array.prototype.indexOf || (Array.prototype.indexOf = function (a, b) { var c; if (null == this) throw new TypeError("'this' is null or not defined"); var d = Object(this), e = d.length >>> 0; if (0 === e) return -1; var f = 0 | b; if (f >= e) return -1; for (c = Math.max(f >= 0 ? f : e - Math.abs(f), 0); c < e;) { if (c in d && d[c] === a) return c; c++ } return -1 }), Array.prototype.map || (Array.prototype.map = function (a, b) { var c, d, e; if (null == this) throw new TypeError("'this' is null or not defined"); var f = Object(this), g = f.length >>> 0; if ("function" != typeof a) throw new TypeError(a + " is not a function"); for (arguments.length > 1 && (c = b), d = new Array(g), e = 0; e < g;) { var h, i; e in f && (h = f[e], i = a.call(c, h, e, f), d[e] = i), e++ } return d }), L.TileLayer.WMS.Util = {}, L.TileLayer.WMS.Util.JSON = { parse: window.JSON ? window.JSON.parse : function (jsonString) { return eval("(" + jsonString + ")") } }, L.TileLayer.WMS.Util.XML = { parse: function (a) { var b; try { if (window.DOMParser) { var c = new window.DOMParser; b = c.parseFromString(a, "text/xml") } else window.ActiveXObject && (b = new window.ActiveXObject("Microsoft.XMLDOM"), b.async = "false", b.loadXML(a)) } catch (d) { b = null } if (!b || !b.documentElement) throw new Error("Unable to parse specified 'xmlString' it isn't valid"); var e = b.getElementsByTagName("parsererror"); if (e.length > 0) { for (var f = "", g = 0, h = e.length; g < h; g++) { var i = e[g]; f += L.TileLayer.WMS.Util.XML.getElementText(i).trim(), g < h - 1 && (f += " ") } throw new Error("Unable to parse specified 'xmlString' it isn't valid: " + f) } return L.TileLayer.WMS.Util.XML.normalizeElement(b.documentElement) }, getElementText: function (a) { return a ? a.innerText || a.textContent || a.text : "" }, normalizeElement: function (a) { 3 === a.nodeType && "" === L.TileLayer.WMS.Util.XML.getElementText(a).trim() && a.parentNode && a.parentNode.removeChild(a); var b, c; for (b = a.firstChild; b;)c = b.nextSibling, L.TileLayer.WMS.Util.XML.normalizeElement(b), b = c; return a }, extractInfoFormats: function (a) { var b = []; if (a) for (var c = a.getElementsByTagName("Capability")[0], d = c.getElementsByTagName("GetFeatureInfo")[0], e = d.getElementsByTagName("Format"), f = 0, g = e.length; f < g; f++) { var h = e[f]; b.push(L.TileLayer.WMS.Util.XML.getElementText(h).trim().toLowerCase()) } return b } }, L.TileLayer.WMS.Util.XML.ExceptionReport = { getExisting: function () { var a = [], b = L.TileLayer.WMS.Util.XML.ExceptionReport; for (var c in b) { var d = b.hasOwnProperty(c) ? b[c] : null; d && "object" == typeof d && "function" == typeof d.parse && a.push(d) } return a }, parse: function (a) { for (var b = L.TileLayer.WMS.Util.XML.ExceptionReport.getExisting(), c = null, d = 0, e = b.length; d < e; d++) { var f = b[d]; if (c = f.parse(a)) break } return c } }, L.TileLayer.WMS.Util.XML.ExceptionReport.OWS = { parse: function (a) { if ("string" != typeof a || a.indexOf("<ows:ExceptionReport") < 0) return null; var b = L.TileLayer.WMS.Util.XML.parse(a); if (!b || "ows:ExceptionReport" !== b.tagName) return null; for (var c = { exceptions: [], message: "" }, d = b.getElementsByTagName("Exception"), e = 0, f = d.length; e < f; e++) { for (var g = d[e], h = g.getAttribute("exceptionCode"), i = g.getElementsByTagName("ExceptionText"), j = { code: h, text: "" }, k = 0, l = i.length; k < l; k++) { var m = i[k], n = L.TileLayer.WMS.Util.XML.getElementText(m).trim(); j.text += n, k < l - 1 && (j.text += " ") } c.message += (j.code ? j.code + " - " : "") + j.text + ". ", c.exceptions.push(j) } return c } }, L.TileLayer.WMS.Util.XML.ExceptionReport.Service = { parse: function (a) { if ("string" != typeof a || a.indexOf("<ServiceExceptionReport") < 0) return null; var b = L.TileLayer.WMS.Util.XML.parse(a); if (!b || "ServiceExceptionReport" !== b.tagName) return null; for (var c = { exceptions: [], message: "" }, d = b.getElementsByTagName("ServiceException"), e = 0, f = d.length; e < f; e++) { var g = d[e], h = g.getAttribute("code"), i = { code: h, text: (h ? h + " - " : "") + L.TileLayer.WMS.Util.XML.getElementText(g).trim() + ". " }; c.message += i.text, c.exceptions.push(i) } return c } }, L.TileLayer.WMS.Util.XML.ExceptionReport.Exception = { parse: function (a) { if ("string" != typeof a || a.indexOf("<ExceptionReport") < 0) return null; var b = L.TileLayer.WMS.Util.XML.parse(a); if (!b || "ExceptionReport" !== b.tagName) return null; for (var c = { exceptions: [], message: "" }, d = b.getElementsByTagName("Exception"), e = 0, f = d.length; e < f; e++) { for (var g = d[e], h = g.getAttribute("exceptionCode"), i = g.getElementsByTagName("ExceptionText"), j = { code: h, text: "" }, k = 0, l = i.length; k < l; k++) { var m = i[k], n = L.TileLayer.WMS.Util.XML.getElementText(m).trim(); j.text += n, k < l - 1 && (j.text += " ") } c.message += (j.code ? j.code + " - " : "") + j.text + ". ", c.exceptions.push(j) } return c } }, function () { var a = function () { var a, b, c, d, e; for (a = 0, b = arguments.length; a < b; a++) { d = arguments[a], e = e || {}; for (c in d) e[c] = d[c] } return e }, b = function (a) { return encodeURIComponent(a).replace(/[!'()*]/g, function (a) { return "%" + a.charCodeAt(0).toString(16) }) }, c = function (a, c) { var d, e; c = c || {}, d = []; for (e in c) d.push(b(e) + "=" + b(c[e])); return a = a || "", a + (a.indexOf("?") === -1 ? "?" : "&") + d.join("&") }, d = function (a) { if ("string" != typeof a) return !1; var b = window.location, c = document.createElement("a"); return c.href = a, "" !== c.protocol && b.protocol !== c.protocol || ("" !== c.hostname && b.hostname !== c.hostname || "" !== c.port && b.port !== c.port) }, e = function (a) { var b, c, e, f = !document.addEventListener || !document.querySelector; if (!window.ActiveXObject || window.XMLHttpRequest && !f) window.XMLHttpRequest && (b = new window.XMLHttpRequest); else for (var g = ["Msxml2.XMLHTTP.6.0", "Msxml2.XMLHTTP.3.0", "Msxml2.XMLHTTP", "Microsoft.XMLHTTP"], h = 0, i = g.length; h < i; h++)try { b = new window.ActiveXObject(g[h]) } catch (j) { } b || (e = new Error("Your browser doesn't support neither 'XMLHttpRequest' nor 'ActiveXObject'")); var k = "undefined" != typeof b.withCredentials; d(a) && !k && (window.XDomainRequest ? this.xdr = new window.XDomainRequest : e = new Error("Your browser doesn't support cross-domain requests")), this.url = a, this.xhr = b, this.xdr = c, this.error = e }; e.prototype.send = function (b) { b = a({ method: "GET", headers: null, content: null, done: function (a, b) { }, fail: function (a, b) { throw a }, always: function () { } }, b || {}); var d = this.xdr || this.xhr, e = function () { "function" == typeof b.done && b.done(d.responseText, d), "function" == typeof b.always && b.always() }, f = function (a) { "function" == typeof b.fail && b.fail(a, d), "function" == typeof b.always && b.always() }; if (this.error) return void f(this.error); if ("string" == typeof b.method && (b.method = b.method.trim().toUpperCase()), "GET" !== b.method && "DELETE" !== b.method || (this.url = c(this.url, b.content), b.content = null, b.headers && b.headers["Content-type"] && delete b.headers["Content-type"]), d === this.xdr) d.open(b.method, this.url), d.onload = e, d.onerror = f, d.send(b.content); else { if (d.onreadystatechange = function () { var a, b; try { if (4 !== d.readyState) return; a = 1223 === d.status ? 204 : d.status } catch (c) { a = 0, b = c } a >= 200 && a < 300 ? e() : f(b || new Error(a + " - " + d.responseText)) }, d.open(b.method, this.url, !0), b.headers) for (var g in b.headers) b.headers.hasOwnProperty(g) && d.setRequestHeader(g, b.headers[g]); d.send(b.content) } }, L.TileLayer.WMS.Util.AJAX = function (a) { var b = new e(a.url); b.send(a) } }(), L.TileLayer.WMS.Format = { getExisting: function () { var a = [], b = L.TileLayer.WMS.Format; for (var c in b) { var d = b.hasOwnProperty(c) ? b[c] : null; d && "object" == typeof d && "function" == typeof d.toGeoJSON && a.push(c) } return a.sort(function (a, b) { var c = L.TileLayer.WMS.Format[a], d = L.TileLayer.WMS.Format[b]; return c.priority > d.priority ? 1 : c.priority < d.priority ? -1 : 0 }), a }, getAvailable: function (a) { a = L.Util.extend({ fail: function (a) { throw a } }, a || {}); var b = function (b, c) { "function" == typeof a.done && a.done.call(window, b, c), "function" == typeof a.always && a.always.call(window) }, c = function (b, c) { "function" == typeof a.fail && a.fail.call(window, b, c), "function" == typeof a.always && a.always.call(window) }; L.TileLayer.WMS.Util.AJAX({ url: a.url, method: "GET", content: { request: "GetCapabilities", service: "WMS", version: a.wmsParams ? a.wmsParams.version || "1.1.0" : "1.1.0" }, done: function (a, d) { try { var e = L.TileLayer.WMS.Util.XML.ExceptionReport.parse(a); if (e) throw new Error(e.message); for (var f = L.TileLayer.WMS.Util.XML.parse(a), g = L.TileLayer.WMS.Util.XML.extractInfoFormats(f), h = L.TileLayer.WMS.Format.getExisting(), i = [], j = 0; j < h.length; j++) { var k = h[j]; g.indexOf(k) >= 0 && i.push(k) } b(i, d) } catch (l) { c(l, d) } }, fail: c }) } }, L.TileLayer.WMS.Format["application/geojson"] = { priority: 1, toGeoJSON: function (a) { return L.TileLayer.WMS.Util.JSON.parse(a) } }, L.TileLayer.WMS.Format["application/json"] = { priority: 2, toGeoJSON: function (a) { return L.TileLayer.WMS.Format["application/geojson"].toGeoJSON(a) } }, L.TileLayer.WMS.Format["application/vnd.ogc.gml/3.1.1"] = { priority: 3, toGeoJSON: function (a) { return L.TileLayer.WMS.Format["application/vnd.ogc.gml"].toGeoJSON(a) } }, function () { var a = { gml: "http://www.opengis.net/gml" }, b = { decimal: ".", component: ",", tuple: " " }, c = ["gml:Point", "gml:MultiPoint", "gml:LineString", "gml:MultiLineString", "gml:Polygon", "gml:MultiPolygon"], d = { toGeoJSON: function (a) { return d[a.tagName].toGeoJSON(a) }, "wfs:FeatureCollection": { toGeoJSON: function (a) { return d["gml:featureCollection"].toGeoJSON(a) } }, "gml:featureCollection": { toGeoJSON: function (b) { for (var c = { type: "FeatureCollection", features: [] }, e = b.getElementsByTagNameNS(a.gml, "featureMember"), f = 0, g = e.length; f < g; f++) { var h = e[f], i = d.toGeoJSON(h); c.features.push(i) } return c } }, "gml:featureMember": { toGeoJSON: function (a) { for (var b = { type: "Feature", geometry: null, properties: {} }, e = a.firstChild, f = e.childNodes, g = 0, h = f.length; g < h; g++) { var i = f[g], j = i.tagName.split(":")[1], k = i.firstChild; k && c.indexOf(k.tagName) >= 0 ? b.geometry = d.toGeoJSON(k) : b.properties[j] = L.TileLayer.WMS.Util.XML.getElementText(i) || null } return b } }, "gml:coord": { toGeoJSON: function (b) { var c = [], d = function (a) { var b = window.parseFloat(L.TileLayer.WMS.Util.XML.getElementText(a).trim()); c.push(b) }, e = b.getElementsByTagNameNS(a.gml, "X")[0]; d(e); var f = b.getElementsByTagNameNS(a.gml, "Y")[0]; d(f); var g = b.getElementsByTagNameNS(a.gml, "Z")[0]; return g && d(g), c } }, "gml:coordinates": { toGeoJSON: function (a) { for (var c = a.attributes, d = c.decimal && c.decimal.value || b.decimal, e = c.cs && c.cs.value || b.component, f = c.ts && c.ts.value || b.tuple, g = [], h = L.TileLayer.WMS.Util.XML.getElementText(a).trim().replace(new RegExp("\\s*" + e + "\\s*", "gi"), e).split(f), i = function (a) { return "." !== d && (a = a.replace(d, ".")), window.parseFloat(a) }, j = 0, k = h.length; j < k; j++) { var l = h[j].split(e).map(i); g.push(l) } return g.length > 1 ? g : g[0] } }, "gml:pos": { toGeoJSON: function (a) { var c = a.attributes, d = c.decimal && c.decimal.value || b.decimal, e = c.ts && c.ts.value || b.tuple; return L.TileLayer.WMS.Util.XML.getElementText(a).trim().split(e).map(function (a) { return "." !== d && (a = a.replace(d, ".")), window.parseFloat(a) }) } }, "gml:posList": { toGeoJSON: function (a) { for (var c = a.attributes, d = c.decimal && c.decimal.value || b.decimal, e = c.ts && c.ts.value || b.tuple, f = window.parseInt((c.srsDimension || c.dimension || {}).value, 10) || 2, g = [], h = L.TileLayer.WMS.Util.XML.getElementText(a).trim().split(e), i = 0, j = h.length; i < j; i += f) { for (var k = [], l = i, m = i + f; l < m; l++) { var n = h[l]; "." !== d && (n = n.replace(d, ".")), k.push(window.parseFloat(n)) } g.push(k) } return g } }, "gml:Point": { toGeoJSON: function (a) { var b = { type: "Point", coordinates: [] }, c = a.firstChild; return b.coordinates = d.toGeoJSON(c), b } }, "gml:MultiPoint": { toGeoJSON: function (b) { for (var c = { type: "MultiPoint", coordinates: [] }, e = b.getElementsByTagNameNS(a.gml, "Point"), f = 0, g = e.length; f < g; f++) { var h = e[f], i = d.toGeoJSON(h).coordinates; c.coordinates.push(i) } return c } }, "gml:LineString": { toGeoJSON: function (a) { var b, c = { type: "LineString", coordinates: [] }, e = a.childNodes; if (1 === e.length) b = e[0], c.coordinates = d.toGeoJSON(b); else for (var f = 0, g = e.length; f < g; f++)b = e[f], c.coordinates.push(d.toGeoJSON(b)); return c } }, "gml:MultiLineString": { toGeoJSON: function (b) { for (var c = { type: "MultiLineString", coordinates: [] }, e = b.getElementsByTagNameNS(a.gml, "LineString"), f = 0, g = e.length; f < g; f++) { var h = e[f], i = d.toGeoJSON(h).coordinates; c.coordinates.push(i) } return c } }, "gml:LinearRing": { toGeoJSON: function (a) { return d["gml:LineString"].toGeoJSON(a).coordinates } }, "gml:Polygon": { toGeoJSON: function (b) { var c = { type: "Polygon", coordinates: [] }, e = b.getElementsByTagNameNS(a.gml, "exterior"); 0 === e.length && (e = b.getElementsByTagNameNS(a.gml, "outerBoundaryIs")); var f = e[0].getElementsByTagNameNS(a.gml, "LinearRing")[0], g = d.toGeoJSON(f); c.coordinates.push(g); var h = b.getElementsByTagNameNS(a.gml, "interior"); 0 === h.length && (h = b.getElementsByTagNameNS(a.gml, "innerBoundaryIs")); for (var i = 0, j = h.length; i < j; i++) { var k = h[i].getElementsByTagNameNS(a.gml, "LinearRing")[0], l = d.toGeoJSON(k); c.coordinates.push(l) } return c } }, "gml:MultiPolygon": { toGeoJSON: function (b) { for (var c = { type: "MultiPolygon", coordinates: [] }, e = b.getElementsByTagNameNS(a.gml, "Polygon"), f = 0, g = e.length; f < g; f++) { var h = e[f], i = d.toGeoJSON(h).coordinates; c.coordinates.push(i) } return c } } }; L.TileLayer.WMS.Format["application/vnd.ogc.gml"] = { priority: 4, toGeoJSON: function (a) { var b = L.TileLayer.WMS.Util.XML.parse(a); return d.toGeoJSON(b) } } }(), L.TileLayer.WMS.Format["text/xml"] = { priority: 5, toGeoJSON: function (a) { return L.TileLayer.WMS.Format["application/vnd.ogc.wms_xml"].toGeoJSON(a) } }, L.TileLayer.WMS.Format["application/vnd.ogc.wms_xml"] = { priority: 6, toGeoJSON: function (a) { for (var b = { type: "FeatureCollection", features: [] }, c = L.TileLayer.WMS.Util.XML.parse(a), d = c.getElementsByTagName("FIELDS"), e = 0, f = d.length; e < f; e++) { var g = { type: "Feature", geometry: null, properties: {} }, h = d[e], i = h.attributes; for (var j in i) if (i.hasOwnProperty(j)) { var k = i[j]; k && k.name && (g.properties[k.name] = k.value || null) } b.features.push(g) } return b } }, L.TileLayer.WMS.Format["text/html"] = { priority: 7, toGeoJSON: function (a) { var b = { type: "FeatureCollection", features: [] }, c = L.TileLayer.WMS.Util.XML.parse(a), d = c.getElementsByTagName("table")[0]; if (!d) return b; for (var e = [], f = d.getElementsByTagName("th"), g = 0, h = f.length; g < h; g++) { var i = f[g]; e.push(L.TileLayer.WMS.Util.XML.getElementText(i)) } for (var j = d.getElementsByTagName("tr"), k = 0, l = j.length; k < l; k++) { var m = j[k], n = m.getElementsByTagName("td"); if (n.length === e.length) { for (var o = { type: "Feature", geometry: null, properties: {} }, p = 0, q = n.length; p < q; p++) { var r = n[p]; o.properties[e[p]] = L.TileLayer.WMS.Util.XML.getElementText(r) || null } b.features.push(o) } } return b } }, L.TileLayer.WMS.include({ _boundingBox: null, getBoundingBox: function (a) { a = L.Util.extend({ fail: function (a) { throw a } }, a || {}); var b = this, c = function (c, d) { "function" == typeof a.done && a.done.call(b, c, d), "function" == typeof a.always && a.always.call(b) }, d = function (c, d) { "function" == typeof a.fail && a.fail.call(b, c, d), "function" == typeof a.always && a.always.call(b) }, e = b._boundingBox; return e ? void c(e) : void b.getCapabilities({ done: function (f, g) { try { var h = a.layers || b.wmsParams.layers, i = f.getElementsByTagName("Capability")[0], j = i.getElementsByTagName("Layer")[0], k = j.getElementsByTagName("Layer"), l = []; if (k && k.length > 0 && h && h.length > 0) { for (var m = h.split(","), n = [], o = 0, p = m.length; o < p; o++) { var q = m[o], r = q.split(":"); r = r[1] || r[0], n.push(r) } for (var s = 0, t = k.length; s < t; s++) { var u = k[s], v = L.TileLayer.WMS.Util.XML.getElementText(u.getElementsByTagName("Name")[0]); n.indexOf(v) >= 0 && l.push(u) } } else l.push(j); for (var w = 0, x = l.length; w < x; w++) { var y, z, A, B, C = l[w]; if (!b.wmsParams.version || b.wmsParams.version >= 1.3 || "1.3.0" === b.wmsParams.version) { var D = C.getElementsByTagName("EX_GeographicBoundingBox")[0]; y = L.TileLayer.WMS.Util.XML.getElementText(D.getElementsByTagName("westBoundLongitude")[0]), z = L.TileLayer.WMS.Util.XML.getElementText(D.getElementsByTagName("eastBoundLongitude")[0]), A = L.TileLayer.WMS.Util.XML.getElementText(D.getElementsByTagName("southBoundLatitude")[0]), B = L.TileLayer.WMS.Util.XML.getElementText(D.getElementsByTagName("northBoundLatitude")[0]) } else { var E = C.getElementsByTagName("LatLonBoundingBox")[0]; y = E.getAttribute("minx"), z = E.getAttribute("maxx"), A = E.getAttribute("miny"), B = E.getAttribute("maxy") } var F = L.latLngBounds(L.latLng(A, y), L.latLng(B, z)); e = e ? e.extend(F) : F } b._boundingBox = e, c(e, g) } catch (G) { d(G, g) } }, fail: d }) } }), L.TileLayer.WMS.include({ _capabilities: null, getCapabilities: function (a) { a = L.Util.extend({ fail: function (a) { throw a } }, a || {}); var b = this, c = function (c, d) { "function" == typeof a.done && a.done.call(b, c, d), "function" == typeof a.always && a.always.call(b) }, d = function (c, d) { "function" == typeof a.fail && a.fail.call(b, c, d), "function" == typeof a.always && a.always.call(b) }, e = b._capabilities; return e ? void c(e) : void L.TileLayer.WMS.Util.AJAX({ url: b._url, method: "GET", content: { request: "GetCapabilities", service: "WMS", version: b.wmsParams.version }, done: function (a, e) { try { var f = L.TileLayer.WMS.Util.XML.ExceptionReport.parse(a); if (f) throw new Error(f.message); var g = L.TileLayer.WMS.Util.XML.parse(a); b._capabilities = g, c(g, e) } catch (h) { d(h, e) } }, fail: d }) } }), L.TileLayer.WMS.include({ _infoFormat: null, getInfoFormat: function (a) { a = L.Util.extend({ fail: function (a) { throw a } }, a || {}); var b = this, c = function (c, d) { "function" == typeof a.done && a.done.call(b, c, d), "function" == typeof a.always && a.always.call(b) }, d = function (c, d) { "function" == typeof a.fail && a.fail.call(b, c, d), "function" == typeof a.always && a.always.call(b) }, e = b._infoFormat; return e ? void c(e) : void b.getCapabilities({ done: function (a, f) { try { for (var g = L.TileLayer.WMS.Format.getExisting(), h = L.TileLayer.WMS.Util.XML.extractInfoFormats(a), i = 0, j = g.length; i < j; i++) { var k = g[i]; if (h.indexOf(k) >= 0) { e = k; break } } b._infoFormat = e, c(e, f) } catch (l) { d(l, f) } }, fail: d }) } }), L.TileLayer.WMS.include({ getFeatureInfo: function (a) { a = L.Util.extend({ featureCount: 1, fail: function (a) { throw a } }, a || {}); var b = this, c = function (c, d) { "function" == typeof a.done && a.done.call(b, c, d), "function" == typeof a.always && a.always.call(b) }, d = function (c, d) { "function" == typeof a.fail && a.fail.call(b, c, d), "function" == typeof a.always && a.always.call(b) }, e = a.infoFormat ? function (b) { b.done(a.infoFormat) } : b.getInfoFormat; e.call(b, { done: function (e, f) { var g; try { var h; if (!e) throw h = "None of available formats for '" + b._url + "' 'GetFeatureInfo' requests are not implemented in 'L.TileLayer.WMS'. 'GetFeatureInfo' request can't be performed.", new Error(h); if (!L.TileLayer.WMS.Format[e]) throw h = "Format '" + e + "' ' is not implemented in 'L.TileLayer.WMS'. 'GetFeatureInfo' request can't be performed.", new Error(h); var i = b._map || a.map, j = a.latlng, k = i.latLngToContainerPoint(j, i.getZoom()), l = i.getSize(), m = b.options.crs || i.options.crs, n = i.getBounds(), o = m.project(n.getNorthWest()), p = m.project(n.getSouthEast()), q = a.featureCount || 1; g = { request: "GetFeatureInfo", service: "WMS", version: b.wmsParams.version, layers: b.wmsParams.layers, query_layers: b.wmsParams.layers, info_format: e, feature_count: q, height: l.y, width: l.x }; var r = window.parseFloat(b.wmsParams.version); g[r >= 1.3 ? "crs" : "srs"] = m.code, g.bbox = r >= 1.3 && m === L.CRS.EPSG4326 ? p.y + "," + o.x + "," + o.y + "," + p.x : o.x + "," + p.y + "," + p.x + "," + o.y, g[r >= 1.3 ? "i" : "x"] = k.x, g[r >= 1.3 ? "j" : "y"] = k.y } catch (s) { return void d(s) } L.TileLayer.WMS.Util.AJAX({ url: b._url, method: "GET", content: g, done: function (a, b) { try { var f = L.TileLayer.WMS.Util.XML.ExceptionReport.parse(a); if (f) throw new Error(f.message); var g = L.TileLayer.WMS.Format[e].toGeoJSON(a); g.crs = m, c(g, b) } catch (h) { d(h, b) } }, fail: d }) }, fail: d }) } }) }(window, document);
/*
 (c) 2014, Vladimir Agafonkin
 simpleheat, a tiny JavaScript library for drawing heatmaps with Canvas
 https://github.com/mourner/simpleheat
*/
!function () { "use strict"; function t(i) { return this instanceof t ? (this._canvas = i = "string" == typeof i ? document.getElementById(i) : i, this._ctx = i.getContext("2d"), this._width = i.width, this._height = i.height, this._max = 1, void this.clear()) : new t(i) } t.prototype = { defaultRadius: 25, defaultGradient: { .4: "blue", .6: "cyan", .7: "lime", .8: "yellow", 1: "red" }, data: function (t, i) { return this._data = t, this }, max: function (t) { return this._max = t, this }, add: function (t) { return this._data.push(t), this }, clear: function () { return this._data = [], this }, radius: function (t, i) { i = i || 15; var a = this._circle = document.createElement("canvas"), s = a.getContext("2d"), e = this._r = t + i; return a.width = a.height = 2 * e, s.shadowOffsetX = s.shadowOffsetY = 200, s.shadowBlur = i, s.shadowColor = "black", s.beginPath(), s.arc(e - 200, e - 200, t, 0, 2 * Math.PI, !0), s.closePath(), s.fill(), this }, gradient: function (t) { var i = document.createElement("canvas"), a = i.getContext("2d"), s = a.createLinearGradient(0, 0, 0, 256); i.width = 1, i.height = 256; for (var e in t) s.addColorStop(e, t[e]); return a.fillStyle = s, a.fillRect(0, 0, 1, 256), this._grad = a.getImageData(0, 0, 1, 256).data, this }, draw: function (t) { this._circle || this.radius(this.defaultRadius), this._grad || this.gradient(this.defaultGradient); var i = this._ctx; i.clearRect(0, 0, this._width, this._height); for (var a, s = 0, e = this._data.length; e > s; s++)a = this._data[s], i.globalAlpha = Math.max(a[2] / this._max, t || .05), i.drawImage(this._circle, a[0] - this._r, a[1] - this._r); var n = i.getImageData(0, 0, this._width, this._height); return this._colorize(n.data, this._grad), i.putImageData(n, 0, 0), this }, _colorize: function (t, i) { for (var a, s = 3, e = t.length; e > s; s += 4)a = 4 * t[s], a && (t[s - 3] = i[a], t[s - 2] = i[a + 1], t[s - 1] = i[a + 2]) } }, window.simpleheat = t }(),/*
 (c) 2014, Vladimir Agafonkin
 Leaflet.heat, a tiny and fast heatmap plugin for Leaflet.
 https://github.com/Leaflet/Leaflet.heat
*/
    L.HeatLayer = (L.Layer ? L.Layer : L.Class).extend({ initialize: function (t, i) { this._latlngs = t, L.setOptions(this, i) }, setLatLngs: function (t) { return this._latlngs = t, this.redraw() }, addLatLng: function (t) { return this._latlngs.push(t), this.redraw() }, setOptions: function (t) { return L.setOptions(this, t), this._heat && this._updateOptions(), this.redraw() }, redraw: function () { return !this._heat || this._frame || this._map._animating || (this._frame = L.Util.requestAnimFrame(this._redraw, this)), this }, onAdd: function (t) { this._map = t, this._canvas || this._initCanvas(), t._panes.overlayPane.appendChild(this._canvas), t.on("moveend", this._reset, this), t.options.zoomAnimation && L.Browser.any3d && t.on("zoomanim", this._animateZoom, this), this._reset() }, onRemove: function (t) { t.getPanes().overlayPane.removeChild(this._canvas), t.off("moveend", this._reset, this), t.options.zoomAnimation && t.off("zoomanim", this._animateZoom, this) }, addTo: function (t) { return t.addLayer(this), this }, _initCanvas: function () { var t = this._canvas = L.DomUtil.create("canvas", "leaflet-heatmap-layer leaflet-layer"), i = L.DomUtil.testProp(["transformOrigin", "WebkitTransformOrigin", "msTransformOrigin"]); t.style[i] = "50% 50%"; var a = this._map.getSize(); t.width = a.x, t.height = a.y; var s = this._map.options.zoomAnimation && L.Browser.any3d; L.DomUtil.addClass(t, "leaflet-zoom-" + (s ? "animated" : "hide")), this._heat = simpleheat(t), this._updateOptions() }, _updateOptions: function () { this._heat.radius(this.options.radius || this._heat.defaultRadius, this.options.blur), this.options.gradient && this._heat.gradient(this.options.gradient), this.options.max && this._heat.max(this.options.max) }, _reset: function () { var t = this._map.containerPointToLayerPoint([0, 0]); L.DomUtil.setPosition(this._canvas, t); var i = this._map.getSize(); this._heat._width !== i.x && (this._canvas.width = this._heat._width = i.x), this._heat._height !== i.y && (this._canvas.height = this._heat._height = i.y), this._redraw() }, _redraw: function () { var t, i, a, s, e, n, h, o, r, d = [], _ = this._heat._r, l = this._map.getSize(), m = new L.Bounds(L.point([-_, -_]), l.add([_, _])), c = void 0 === this.options.max ? 1 : this.options.max, u = void 0 === this.options.maxZoom ? this._map.getMaxZoom() : this.options.maxZoom, f = 1 / Math.pow(2, Math.max(0, Math.min(u - this._map.getZoom(), 12))), g = _ / 2, p = [], v = this._map._getMapPanePos(), w = v.x % g, y = v.y % g; for (t = 0, i = this._latlngs.length; i > t; t++)if (a = this._map.latLngToContainerPoint(this._latlngs[t]), m.contains(a)) { e = Math.floor((a.x - w) / g) + 2, n = Math.floor((a.y - y) / g) + 2; var x = void 0 !== this._latlngs[t].alt ? this._latlngs[t].alt : void 0 !== this._latlngs[t][2] ? +this._latlngs[t][2] : 1; r = x * f, p[n] = p[n] || [], s = p[n][e], s ? (s[0] = (s[0] * s[2] + a.x * r) / (s[2] + r), s[1] = (s[1] * s[2] + a.y * r) / (s[2] + r), s[2] += r) : p[n][e] = [a.x, a.y, r] } for (t = 0, i = p.length; i > t; t++)if (p[t]) for (h = 0, o = p[t].length; o > h; h++)s = p[t][h], s && d.push([Math.round(s[0]), Math.round(s[1]), Math.min(s[2], c)]); this._heat.data(d).draw(this.options.minOpacity), this._frame = null }, _animateZoom: function (t) { var i = this._map.getZoomScale(t.zoom), a = this._map._getCenterOffset(t.center)._multiplyBy(-i).subtract(this._map._getMapPanePos()); L.DomUtil.setTransform ? L.DomUtil.setTransform(this._canvas, a, i) : this._canvas.style[L.DomUtil.TRANSFORM] = L.DomUtil.getTranslateString(a) + " scale(" + i + ")" } }), L.heatLayer = function (t, i) { return new L.HeatLayer(t, i) };
/**
 * @ag-grid-community/all-modules - Advanced Data Grid / Data Table supporting Javascript / React / AngularJS / Web Components
 * @version v26.0.0
 * @link http://www.ag-grid.com/
 * @license MIT
 */
// @ag-grid-community/all-modules v26.0.0
/**
 * @ag-grid-community/all-modules - Advanced Data Grid / Data Table supporting Javascript / React / AngularJS / Web Components
 * @version v26.0.0
 * @link http://www.ag-grid.com/
 * @license MIT
 */
// @ag-grid-community/all-modules v26.0.0

function gridConfig(selector,
    url,
    columns,
    editableC,
    sortableC,
    resizableC,
    filterC,
    flexC,
    paginationC,
    pageSizeC,
) {
    ShowLoader($('#' + selector));
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json",
        dataType: "JSON",
        success: function (response) {
            HideLoader($('#' + selector));
            if (response.length >= 0) {
                debugger;
                var tabledata = response;

                // specify the columns
                const columnDefs = columns;

                // specify the data
                const rowData = tabledata

                // let the grid know which columns and what data to use
                const gridOptions = {
                    defaultColDef: {
                        editable: editableC,
                        sortable: sortableC,
                        resizable: resizableC,
                        filter: filterC,
                        flex: flexC,
                    },
                    columnDefs: columnDefs,
                    rowData: rowData,
                    pagination: paginationC,
                    paginationPageSize: pageSizeC,

                };

                // lookup the container we want the Grid to use
                const eGridDiv = document.querySelector('#' + selector);

                // create the grid passing in the div to use together with the columns & data we want to use
                new agGrid.Grid(eGridDiv, gridOptions);4
            } else {
               // document.getElementById("global-overlay").style.display = 'none';
               // alert("No Data Found");
            }
            //document.getElementById("global-overlay").style.display = 'none';
        },
        error: function (ert) {
            HideLoader($('#' + selector));
                }
    });
}



function gridConfigByData(selector,
    data,
    columns,
    editableC,
    sortableC,
    resizableC,
    filterC,
    flexC,
    paginationC,
    pageSizeC,
) {

    // specify the columns
    const columnDefs = columns;

    // specify the data
    const rowData = data

    // let the grid know which columns and what data to use
    const gridOptions = {
        defaultColDef: {
            editable: editableC,
            sortable: sortableC,
            resizable: resizableC,
            filter: filterC,
            flex: flexC,
        },
        columnDefs: columnDefs,
        rowData: rowData,
        pagination: paginationC,
        paginationPageSize: pageSizeC,

    };

    // lookup the container we want the Grid to use
    const eGridDiv = document.querySelector('#' + selector);

    // create the grid passing in the div to use together with the columns & data we want to use
    new agGrid.Grid(eGridDiv, gridOptions);
}

/*
 * jsGrid v1.5.3 (http://js-grid.com)
 * (c) 2016 Artem Tabalin
 * Licensed under MIT (https://github.com/tabalinas/jsgrid/blob/master/LICENSE)
 */

!function(a,b,c){function d(a,c){var d=b(a);d.data(f,this),this._container=d,this.data=[],this.fields=[],this._editingRow=null,this._sortField=null,this._sortOrder=i,this._firstDisplayingPage=1,this._init(c),this.render()}var e="JSGrid",f=e,g="JSGridItem",h="JSGridEditRow",i="asc",j="desc",k="{first}",l="{pages}",m="{prev}",n="{next}",o="{last}",p="{pageIndex}",q="{pageCount}",r="{itemCount}",s="javascript:void(0);",t=function(a,c){return b.isFunction(a)?a.apply(c,b.makeArray(arguments).slice(2)):a},u=function(a){var c=b.Deferred();return a&&a.then?a.then(function(){c.resolve.apply(c,arguments)},function(){c.reject.apply(c,arguments)}):c.resolve(a),c.promise()},v={loadData:b.noop,insertItem:b.noop,updateItem:b.noop,deleteItem:b.noop};d.prototype={width:"auto",height:"auto",updateOnResize:!0,rowClass:b.noop,rowRenderer:null,rowClick:function(a){this.editing&&this.editItem(b(a.event.target).closest("tr"))},rowDoubleClick:b.noop,noDataContent:"Not found",noDataRowClass:"jsgrid-nodata-row",heading:!0,headerRowRenderer:null,headerRowClass:"jsgrid-header-row",headerCellClass:"jsgrid-header-cell",filtering:!1,filterRowRenderer:null,filterRowClass:"jsgrid-filter-row",inserting:!1,insertRowRenderer:null,insertRowClass:"jsgrid-insert-row",editing:!1,editRowRenderer:null,editRowClass:"jsgrid-edit-row",confirmDeleting:!0,deleteConfirm:"Are you sure?",selecting:!0,selectedRowClass:"jsgrid-selected-row",oddRowClass:"jsgrid-row",evenRowClass:"jsgrid-alt-row",cellClass:"jsgrid-cell",sorting:!1,sortableClass:"jsgrid-header-sortable",sortAscClass:"jsgrid-header-sort jsgrid-header-sort-asc",sortDescClass:"jsgrid-header-sort jsgrid-header-sort-desc",paging:!1,pagerContainer:null,pageIndex:1,pageSize:20,pageButtonCount:15,pagerFormat:"Pages: {first} {prev} {pages} {next} {last} &nbsp;&nbsp; {pageIndex} of {pageCount}",pagePrevText:"Prev",pageNextText:"Next",pageFirstText:"First",pageLastText:"Last",pageNavigatorNextText:"...",pageNavigatorPrevText:"...",pagerContainerClass:"jsgrid-pager-container",pagerClass:"jsgrid-pager",pagerNavButtonClass:"jsgrid-pager-nav-button",pagerNavButtonInactiveClass:"jsgrid-pager-nav-inactive-button",pageClass:"jsgrid-pager-page",currentPageClass:"jsgrid-pager-current-page",customLoading:!1,pageLoading:!1,autoload:!1,controller:v,loadIndication:!0,loadIndicationDelay:500,loadMessage:"Please, wait...",loadShading:!0,invalidMessage:"Invalid data entered!",invalidNotify:function(c){var d=b.map(c.errors,function(a){return a.message||null});a.alert([this.invalidMessage].concat(d).join("\n"))},onInit:b.noop,onRefreshing:b.noop,onRefreshed:b.noop,onPageChanged:b.noop,onItemDeleting:b.noop,onItemDeleted:b.noop,onItemInserting:b.noop,onItemInserted:b.noop,onItemEditing:b.noop,onItemUpdating:b.noop,onItemUpdated:b.noop,onItemInvalid:b.noop,onDataLoading:b.noop,onDataLoaded:b.noop,onOptionChanging:b.noop,onOptionChanged:b.noop,onError:b.noop,invalidClass:"jsgrid-invalid",containerClass:"jsgrid",tableClass:"jsgrid-table",gridHeaderClass:"jsgrid-grid-header",gridBodyClass:"jsgrid-grid-body",_init:function(a){b.extend(this,a),this._initLoadStrategy(),this._initController(),this._initFields(),this._attachWindowLoadResize(),this._attachWindowResizeCallback(),this._callEventHandler(this.onInit)},loadStrategy:function(){return this.pageLoading?new jsGrid.loadStrategies.PageLoadingStrategy(this):new jsGrid.loadStrategies.DirectLoadingStrategy(this)},_initLoadStrategy:function(){this._loadStrategy=t(this.loadStrategy,this)},_initController:function(){this._controller=b.extend({},v,t(this.controller,this))},renderTemplate:function(a,b,d){args=[];for(var e in d)args.push(d[e]);return args.unshift(a,b),a=t.apply(null,args),a===c||null===a?"":a},loadIndicator:function(a){return new jsGrid.LoadIndicator(a)},validation:function(a){return jsGrid.Validation&&new jsGrid.Validation(a)},_initFields:function(){var a=this;a.fields=b.map(a.fields,function(c){if(b.isPlainObject(c)){var d=c.type&&jsGrid.fields[c.type]||jsGrid.Field;c=new d(c)}return c._grid=a,c})},_attachWindowLoadResize:function(){b(a).on("load",b.proxy(this._refreshSize,this))},_attachWindowResizeCallback:function(){this.updateOnResize&&b(a).on("resize",b.proxy(this._refreshSize,this))},_detachWindowResizeCallback:function(){b(a).off("resize",this._refreshSize)},option:function(a,b){var c,d;return 1===arguments.length?this[a]:(c={option:a,oldValue:this[a],newValue:b},this._callEventHandler(this.onOptionChanging,c),this._handleOptionChange(c.option,c.newValue),d={option:c.option,value:c.newValue},void this._callEventHandler(this.onOptionChanged,d))},fieldOption:function(a,b,c){return a=this._normalizeField(a),2===arguments.length?a[b]:(a[b]=c,void this._renderGrid())},_handleOptionChange:function(a,b){switch(this[a]=b,a){case"width":case"height":this._refreshSize();break;case"rowClass":case"rowRenderer":case"rowClick":case"rowDoubleClick":case"noDataRowClass":case"noDataContent":case"selecting":case"selectedRowClass":case"oddRowClass":case"evenRowClass":this._refreshContent();break;case"pageButtonCount":case"pagerFormat":case"pagePrevText":case"pageNextText":case"pageFirstText":case"pageLastText":case"pageNavigatorNextText":case"pageNavigatorPrevText":case"pagerClass":case"pagerNavButtonClass":case"pageClass":case"currentPageClass":case"pagerRenderer":this._refreshPager();break;case"fields":this._initFields(),this.render();break;case"data":case"editing":case"heading":case"filtering":case"inserting":case"paging":this.refresh();break;case"loadStrategy":case"pageLoading":this._initLoadStrategy(),this.search();break;case"pageIndex":this.openPage(b);break;case"pageSize":this.refresh(),this.search();break;case"editRowRenderer":case"editRowClass":this.cancelEdit();break;case"updateOnResize":this._detachWindowResizeCallback(),this._attachWindowResizeCallback();break;case"invalidNotify":case"invalidMessage":break;default:this.render()}},destroy:function(){this._detachWindowResizeCallback(),this._clear(),this._container.removeData(f)},render:function(){return this._renderGrid(),this.autoload?this.loadData():b.Deferred().resolve().promise()},_renderGrid:function(){this._clear(),this._container.addClass(this.containerClass).css("position","relative").append(this._createHeader()).append(this._createBody()),this._pagerContainer=this._createPagerContainer(),this._loadIndicator=this._createLoadIndicator(),this._validation=this._createValidation(),this.refresh()},_createLoadIndicator:function(){return t(this.loadIndicator,this,{message:this.loadMessage,shading:this.loadShading,container:this._container})},_createValidation:function(){return t(this.validation,this)},_clear:function(){this.cancelEdit(),clearTimeout(this._loadingTimer),this._pagerContainer&&this._pagerContainer.empty(),this._container.empty().css({position:"",width:"",height:""})},_createHeader:function(){var a=this._headerRow=this._createHeaderRow(),c=this._filterRow=this._createFilterRow(),d=this._insertRow=this._createInsertRow(),e=this._headerGrid=b("<table>").addClass(this.tableClass).append(a).append(c).append(d),f=this._header=b("<div>").addClass(this.gridHeaderClass).addClass(this._scrollBarWidth()?"jsgrid-header-scrollbar":"").append(e);return f},_createBody:function(){var a=this._content=b("<tbody>"),c=this._bodyGrid=b("<table>").addClass(this.tableClass).append(a),d=this._body=b("<div>").addClass(this.gridBodyClass).append(c).on("scroll",b.proxy(function(a){this._header.scrollLeft(a.target.scrollLeft)},this));return d},_createPagerContainer:function(){var a=this.pagerContainer||b("<div>").appendTo(this._container);return b(a).addClass(this.pagerContainerClass)},_eachField:function(a){var c=this;b.each(this.fields,function(b,d){d.visible&&a.call(c,d,b)})},_createHeaderRow:function(){if(b.isFunction(this.headerRowRenderer))return b(this.renderTemplate(this.headerRowRenderer,this));var a=b("<tr>").addClass(this.headerRowClass);return this._eachField(function(c,d){var e=this._prepareCell("<th>",c,"headercss",this.headerCellClass).append(this.renderTemplate(c.headerTemplate,c)).appendTo(a);this.sorting&&c.sorting&&e.addClass(this.sortableClass).on("click",b.proxy(function(){this.sort(d)},this))}),a},_prepareCell:function(a,c,d,e){return b(a).css("width",c.width).addClass(e||this.cellClass).addClass(d&&c[d]||c.css).addClass(c.align?"jsgrid-align-"+c.align:"")},_createFilterRow:function(){if(b.isFunction(this.filterRowRenderer))return b(this.renderTemplate(this.filterRowRenderer,this));var a=b("<tr>").addClass(this.filterRowClass);return this._eachField(function(b){this._prepareCell("<td>",b,"filtercss").append(this.renderTemplate(b.filterTemplate,b)).appendTo(a)}),a},_createInsertRow:function(){if(b.isFunction(this.insertRowRenderer))return b(this.renderTemplate(this.insertRowRenderer,this));var a=b("<tr>").addClass(this.insertRowClass);return this._eachField(function(b){this._prepareCell("<td>",b,"insertcss").append(this.renderTemplate(b.insertTemplate,b)).appendTo(a)}),a},_callEventHandler:function(a,c){return a.call(this,b.extend(c,{grid:this})),c},reset:function(){return this._resetSorting(),this._resetPager(),this._loadStrategy.reset()},_resetPager:function(){this._firstDisplayingPage=1,this._setPage(1)},_resetSorting:function(){this._sortField=null,this._sortOrder=i,this._clearSortingCss()},refresh:function(){this._callEventHandler(this.onRefreshing),this.cancelEdit(),this._refreshHeading(),this._refreshFiltering(),this._refreshInserting(),this._refreshContent(),this._refreshPager(),this._refreshSize(),this._callEventHandler(this.onRefreshed)},_refreshHeading:function(){this._headerRow.toggle(this.heading)},_refreshFiltering:function(){this._filterRow.toggle(this.filtering)},_refreshInserting:function(){this._insertRow.toggle(this.inserting)},_refreshContent:function(){var a=this._content;if(a.empty(),!this.data.length)return a.append(this._createNoDataRow()),this;for(var b=this._loadStrategy.firstDisplayIndex(),c=this._loadStrategy.lastDisplayIndex(),d=b;c>d;d++){var e=this.data[d];a.append(this._createRow(e,d))}},_createNoDataRow:function(){var a=0;return this._eachField(function(){a++}),b("<tr>").addClass(this.noDataRowClass).append(b("<td>").addClass(this.cellClass).attr("colspan",a).append(this.renderTemplate(this.noDataContent,this)))},_createRow:function(a,c){var d;return b.isFunction(this.rowRenderer)?d=this.renderTemplate(this.rowRenderer,this,{item:a,itemIndex:c}):(d=b("<tr>"),this._renderCells(d,a)),d.addClass(this._getRowClasses(a,c)).data(g,a).on("click",b.proxy(function(b){this.rowClick({item:a,itemIndex:c,event:b})},this)).on("dblclick",b.proxy(function(b){this.rowDoubleClick({item:a,itemIndex:c,event:b})},this)),this.selecting&&this._attachRowHover(d),d},_getRowClasses:function(a,b){var c=[];return c.push((b+1)%2?this.oddRowClass:this.evenRowClass),c.push(t(this.rowClass,this,a,b)),c.join(" ")},_attachRowHover:function(a){var c=this.selectedRowClass;a.hover(function(){b(this).addClass(c)},function(){b(this).removeClass(c)})},_renderCells:function(a,b){return this._eachField(function(c){a.append(this._createCell(b,c))}),this},_createCell:function(a,c){var d,e=this._getItemFieldValue(a,c),f={value:e,item:a};return d=b.isFunction(c.cellRenderer)?this.renderTemplate(c.cellRenderer,c,f):b("<td>").append(this.renderTemplate(c.itemTemplate||e,c,f)),this._prepareCell(d,c)},_getItemFieldValue:function(a,b){for(var c=b.name.split("."),d=a[c.shift()];d&&c.length;)d=d[c.shift()];return d},_setItemFieldValue:function(a,b,c){for(var d=b.name.split("."),e=a,f=d[0];e&&d.length;)a=e,f=d.shift(),e=a[f];if(!e)for(;d.length;)a=a[f]={},f=d.shift();a[f]=c},sort:function(a,c){return b.isPlainObject(a)&&(c=a.order,a=a.field),this._clearSortingCss(),this._setSortingParams(a,c),this._setSortingCss(),this._loadStrategy.sort()},_clearSortingCss:function(){this._headerRow.find("th").removeClass(this.sortAscClass).removeClass(this.sortDescClass)},_setSortingParams:function(a,b){a=this._normalizeField(a),b=b||(this._sortField===a?this._reversedSortOrder(this._sortOrder):i),this._sortField=a,this._sortOrder=b},_normalizeField:function(a){return b.isNumeric(a)?this.fields[a]:"string"==typeof a?b.grep(this.fields,function(b){return b.name===a})[0]:a},_reversedSortOrder:function(a){return a===i?j:i},_setSortingCss:function(){var a=this._visibleFieldIndex(this._sortField);this._headerRow.find("th").eq(a).addClass(this._sortOrder===i?this.sortAscClass:this.sortDescClass)},_visibleFieldIndex:function(a){return b.inArray(a,b.grep(this.fields,function(a){return a.visible}))},_sortData:function(){var a=this._sortFactor(),b=this._sortField;b&&this.data.sort(function(c,d){return a*b.sortingFunc(c[b.name],d[b.name])})},_sortFactor:function(){return this._sortOrder===i?1:-1},_itemsCount:function(){return this._loadStrategy.itemsCount()},_pagesCount:function(){var a=this._itemsCount(),b=this.pageSize;return Math.floor(a/b)+(a%b?1:0)},_refreshPager:function(){var a=this._pagerContainer;a.empty(),this.paging&&a.append(this._createPager());var b=this.paging&&this._pagesCount()>1;a.toggle(b)},_createPager:function(){var a;return a=b.isFunction(this.pagerRenderer)?b(this.pagerRenderer({pageIndex:this.pageIndex,pageCount:this._pagesCount()})):b("<div>").append(this._createPagerByFormat()),a.addClass(this.pagerClass),a},_createPagerByFormat:function(){var a=this.pageIndex,c=this._pagesCount(),d=this._itemsCount(),e=this.pagerFormat.split(" ");return b.map(e,b.proxy(function(e){var f=e;return e===l?f=this._createPages():e===k?f=this._createPagerNavButton(this.pageFirstText,1,a>1):e===m?f=this._createPagerNavButton(this.pagePrevText,a-1,a>1):e===n?f=this._createPagerNavButton(this.pageNextText,a+1,c>a):e===o?f=this._createPagerNavButton(this.pageLastText,c,c>a):e===p?f=a:e===q?f=c:e===r&&(f=d),b.isArray(f)?f.concat([" "]):[f," "]},this))},_createPages:function(){var a=this._pagesCount(),b=this.pageButtonCount,c=this._firstDisplayingPage,d=[];c>1&&d.push(this._createPagerPageNavButton(this.pageNavigatorPrevText,this.showPrevPages));for(var e=0,f=c;b>e&&a>=f;e++,f++)d.push(f===this.pageIndex?this._createPagerCurrentPage():this._createPagerPage(f));return a>c+b-1&&d.push(this._createPagerPageNavButton(this.pageNavigatorNextText,this.showNextPages)),d},_createPagerNavButton:function(a,c,d){return this._createPagerButton(a,this.pagerNavButtonClass+(d?"":" "+this.pagerNavButtonInactiveClass),d?function(){this.openPage(c)}:b.noop)},_createPagerPageNavButton:function(a,b){return this._createPagerButton(a,this.pagerNavButtonClass,b)},_createPagerPage:function(a){return this._createPagerButton(a,this.pageClass,function(){this.openPage(a)})},_createPagerButton:function(a,c,d){var e=b("<a>").attr("href",s).html(a).on("click",b.proxy(d,this));return b("<span>").addClass(c).append(e)},_createPagerCurrentPage:function(){return b("<span>").addClass(this.pageClass).addClass(this.currentPageClass).text(this.pageIndex)},_refreshSize:function(){this._refreshHeight(),this._refreshWidth()},_refreshWidth:function(){var a="auto"===this.width?this._getAutoWidth():this.width;this._container.width(a)},_getAutoWidth:function(){var a=this._headerGrid,b=this._header;a.width("auto");var c=a.outerWidth(),d=b.outerWidth()-b.innerWidth();return a.width(""),c+d},_scrollBarWidth:function(){var a;return function(){if(a===c){var d=b("<div style='width:50px;height:50px;overflow:hidden;position:absolute;top:-10000px;left:-10000px;'></div>"),e=b("<div style='height:100px;'></div>");d.append(e).appendTo("body");var f=e.innerWidth();d.css("overflow-y","auto");var g=e.innerWidth();d.remove(),a=f-g}return a}}(),_refreshHeight:function(){var a,b=this._container,c=this._pagerContainer,d=this.height;b.height(d),"auto"!==d&&(d=b.height(),a=this._header.outerHeight(!0),c.parents(b).length&&(a+=c.outerHeight(!0)),this._body.outerHeight(d-a))},showPrevPages:function(){var a=this._firstDisplayingPage,b=this.pageButtonCount;this._firstDisplayingPage=a>b?a-b:1,this._refreshPager()},showNextPages:function(){var a=this._firstDisplayingPage,b=this.pageButtonCount,c=this._pagesCount();this._firstDisplayingPage=a+2*b>c?c-b+1:a+b,this._refreshPager()},openPage:function(a){1>a||a>this._pagesCount()||(this._setPage(a),this._loadStrategy.openPage(a))},_setPage:function(a){var b=this._firstDisplayingPage,c=this.pageButtonCount;this.pageIndex=a,b>a&&(this._firstDisplayingPage=a),a>b+c-1&&(this._firstDisplayingPage=a-c+1),this._callEventHandler(this.onPageChanged,{pageIndex:a})},_controllerCall:function(a,c,d,e){if(d)return b.Deferred().reject().promise();this._showLoading();var f=this._controller;if(!f||!f[a])throw Error("controller has no method '"+a+"'");return u(f[a](c)).done(b.proxy(e,this)).fail(b.proxy(this._errorHandler,this)).always(b.proxy(this._hideLoading,this))},_errorHandler:function(){this._callEventHandler(this.onError,{args:b.makeArray(arguments)})},_showLoading:function(){this.loadIndication&&(clearTimeout(this._loadingTimer),this._loadingTimer=setTimeout(b.proxy(function(){this._loadIndicator.show()},this),this.loadIndicationDelay))},_hideLoading:function(){this.loadIndication&&(clearTimeout(this._loadingTimer),this._loadIndicator.hide())},search:function(a){return this._resetSorting(),this._resetPager(),this.loadData(a)},loadData:function(a){a=a||(this.filtering?this.getFilter():{}),b.extend(a,this._loadStrategy.loadParams(),this._sortingParams());var c=this._callEventHandler(this.onDataLoading,{filter:a});return this._controllerCall("loadData",a,c.cancel,function(a){a&&(this._loadStrategy.finishLoad(a),this._callEventHandler(this.onDataLoaded,{data:a}))})},getFilter:function(){var a={};return this._eachField(function(b){b.filtering&&this._setItemFieldValue(a,b,b.filterValue())}),a},_sortingParams:function(){return this.sorting&&this._sortField?{sortField:this._sortField.name,sortOrder:this._sortOrder}:{}},getSorting:function(){var a=this._sortingParams();return{field:a.sortField,order:a.sortOrder}},clearFilter:function(){var a=this._createFilterRow();return this._filterRow.replaceWith(a),this._filterRow=a,this.search()},insertItem:function(a){var c=a||this._getValidatedInsertItem();if(!c)return b.Deferred().reject().promise();var d=this._callEventHandler(this.onItemInserting,{item:c});return this._controllerCall("insertItem",c,d.cancel,function(a){a=a||c,this._loadStrategy.finishInsert(a),this._callEventHandler(this.onItemInserted,{item:a})})},_getValidatedInsertItem:function(){var a=this._getInsertItem();return this._validateItem(a,this._insertRow)?a:null},_getInsertItem:function(){var a={};return this._eachField(function(b){b.inserting&&this._setItemFieldValue(a,b,b.insertValue())}),a},_validateItem:function(a,c){var d=[],e={item:a,itemIndex:this._rowIndex(c),row:c};if(this._eachField(function(f){if(f.validate&&(c!==this._insertRow||f.inserting)&&(c!==this._getEditRow()||f.editing)){var g=this._getItemFieldValue(a,f),h=this._validation.validate(b.extend({value:g,rules:f.validate},e));this._setCellValidity(c.children().eq(this._visibleFieldIndex(f)),h),h.length&&d.push.apply(d,b.map(h,function(a){return{field:f,message:a}}))}}),!d.length)return!0;var f=b.extend({errors:d},e);return this._callEventHandler(this.onItemInvalid,f),this.invalidNotify(f),!1},_setCellValidity:function(a,b){a.toggleClass(this.invalidClass,!!b.length).attr("title",b.join("\n"))},clearInsert:function(){var a=this._createInsertRow();this._insertRow.replaceWith(a),this._insertRow=a,this.refresh()},editItem:function(a){var b=this.rowByItem(a);b.length&&this._editRow(b)},rowByItem:function(a){return a.jquery||a.nodeType?b(a):this._content.find("tr").filter(function(){return b.data(this,g)===a})},_editRow:function(a){if(this.editing){var b=a.data(g),c=this._callEventHandler(this.onItemEditing,{row:a,item:b,itemIndex:this._itemIndex(b)});if(!c.cancel){this._editingRow&&this.cancelEdit();var d=this._createEditRow(b);this._editingRow=a,a.hide(),d.insertBefore(a),a.data(h,d)}}},_createEditRow:function(a){if(b.isFunction(this.editRowRenderer))return b(this.renderTemplate(this.editRowRenderer,this,{item:a,itemIndex:this._itemIndex(a)}));var c=b("<tr>").addClass(this.editRowClass);return this._eachField(function(b){var d=this._getItemFieldValue(a,b);this._prepareCell("<td>",b,"editcss").append(this.renderTemplate(b.editTemplate||"",b,{value:d,item:a})).appendTo(c)}),c},updateItem:function(a,b){1===arguments.length&&(b=a);var c=a?this.rowByItem(a):this._editingRow;return(b=b||this._getValidatedEditedItem())?this._updateRow(c,b):void 0},_getValidatedEditedItem:function(){var a=this._getEditedItem();return this._validateItem(a,this._getEditRow())?a:null},_updateRow:function(a,c){var d=a.data(g),e=this._itemIndex(d),f=b.extend(!0,{},d,c),h=this._callEventHandler(this.onItemUpdating,{row:a,item:f,itemIndex:e,previousItem:d});return this._controllerCall("updateItem",f,h.cancel,function(g){var h=b.extend(!0,{},d);f=g||b.extend(!0,d,c);var i=this._finishUpdate(a,f,e);this._callEventHandler(this.onItemUpdated,{row:i,item:f,itemIndex:e,previousItem:h})})},_rowIndex:function(a){return this._content.children().index(b(a))},_itemIndex:function(a){return b.inArray(a,this.data)},_finishUpdate:function(a,b,c){this.cancelEdit(),this.data[c]=b;var d=this._createRow(b,c);return a.replaceWith(d),d},_getEditedItem:function(){var a={};return this._eachField(function(b){b.editing&&this._setItemFieldValue(a,b,b.editValue())}),a},cancelEdit:function(){this._editingRow&&(this._getEditRow().remove(),this._editingRow.show(),this._editingRow=null)},_getEditRow:function(){return this._editingRow&&this._editingRow.data(h)},deleteItem:function(b){var c=this.rowByItem(b);if(c.length&&(!this.confirmDeleting||a.confirm(t(this.deleteConfirm,this,c.data(g)))))return this._deleteRow(c)},_deleteRow:function(a){var b=a.data(g),c=this._itemIndex(b),d=this._callEventHandler(this.onItemDeleting,{row:a,item:b,itemIndex:c});return this._controllerCall("deleteItem",b,d.cancel,function(){this._loadStrategy.finishDelete(b,c),this._callEventHandler(this.onItemDeleted,{row:a,item:b,itemIndex:c})})}},b.fn.jsGrid=function(a){var e=b.makeArray(arguments),g=e.slice(1),h=this;return this.each(function(){var e,i=b(this),j=i.data(f);if(j)if("string"==typeof a){if(e=j[a].apply(j,g),e!==c&&e!==j)return h=e,!1}else j._detachWindowResizeCallback(),j._init(a),j.render();else new d(i,a)}),h};var w={},x=function(a){var c;b.isPlainObject(a)?c=d.prototype:(c=w[a].prototype,a=arguments[1]||{}),b.extend(c,a)},y={},z=function(a){var c=b.isPlainObject(a)?a:y[a];if(!c)throw Error("unknown locale "+a);A(jsGrid,c)},A=function(a,c){b.each(c,function(c,d){return b.isPlainObject(d)?void A(a[c]||a[c[0].toUpperCase()+c.slice(1)],d):void(a.hasOwnProperty(c)?a[c]=d:a.prototype[c]=d)})};a.jsGrid={Grid:d,fields:w,setDefaults:x,locales:y,locale:z,version:"1.5.3"}}(window,jQuery),function(a,b){function c(a){this._init(a)}c.prototype={container:"body",message:"Loading...",shading:!0,zIndex:1e3,shaderClass:"jsgrid-load-shader",loadPanelClass:"jsgrid-load-panel",_init:function(a){b.extend(!0,this,a),this._initContainer(),this._initShader(),this._initLoadPanel()},_initContainer:function(){this._container=b(this.container)},_initShader:function(){this.shading&&(this._shader=b("<div>").addClass(this.shaderClass).hide().css({position:"absolute",top:0,right:0,bottom:0,left:0,zIndex:this.zIndex}).appendTo(this._container))},_initLoadPanel:function(){this._loadPanel=b("<div>").addClass(this.loadPanelClass).text(this.message).hide().css({position:"absolute",top:"50%",left:"50%",zIndex:this.zIndex}).appendTo(this._container)},show:function(){var a=this._loadPanel.show(),b=a.outerWidth(),c=a.outerHeight();a.css({marginTop:-c/2,marginLeft:-b/2}),this._shader.show()},hide:function(){this._loadPanel.hide(),this._shader.hide()}},a.LoadIndicator=c}(jsGrid,jQuery),function(a,b){function c(a){this._grid=a}function d(a){this._grid=a,this._itemsCount=0}c.prototype={firstDisplayIndex:function(){var a=this._grid;return a.option("paging")?(a.option("pageIndex")-1)*a.option("pageSize"):0},lastDisplayIndex:function(){var a=this._grid,b=a.option("data").length;return a.option("paging")?Math.min(a.option("pageIndex")*a.option("pageSize"),b):b},itemsCount:function(){return this._grid.option("data").length},openPage:function(){this._grid.refresh()},loadParams:function(){return{}},sort:function(){return this._grid._sortData(),this._grid.refresh(),b.Deferred().resolve().promise()},reset:function(){return this._grid.refresh(),b.Deferred().resolve().promise()},finishLoad:function(a){this._grid.option("data",a)},finishInsert:function(a){var b=this._grid;b.option("data").push(a),b.refresh()},finishDelete:function(a,b){var c=this._grid;c.option("data").splice(b,1),c.reset()}},d.prototype={firstDisplayIndex:function(){return 0},lastDisplayIndex:function(){return this._grid.option("data").length},itemsCount:function(){return this._itemsCount},openPage:function(){this._grid.loadData()},loadParams:function(){var a=this._grid;return{pageIndex:a.option("pageIndex"),pageSize:a.option("pageSize")}},reset:function(){return this._grid.loadData()},sort:function(){return this._grid.loadData()},finishLoad:function(a){this._itemsCount=a.itemsCount,this._grid.option("data",a.data)},finishInsert:function(){this._grid.search()},finishDelete:function(){this._grid.search()}},a.loadStrategies={DirectLoadingStrategy:c,PageLoadingStrategy:d}}(jsGrid,jQuery),function(a){var b=function(a){return"undefined"!=typeof a&&null!==a},c={string:function(a,c){return b(a)||b(c)?b(a)?b(c)?(""+a).localeCompare(""+c):1:-1:0},number:function(a,b){return a-b},date:function(a,b){return a-b},numberAsString:function(a,b){return parseFloat(a)-parseFloat(b)}};a.sortStrategies=c}(jsGrid,jQuery),function(a,b,c){function d(a){this._init(a)}d.prototype={_init:function(a){b.extend(!0,this,a)},validate:function(a){var c=[];return b.each(this._normalizeRules(a.rules),function(d,e){if(!e.validator(a.value,a.item,e.param)){var f=b.isFunction(e.message)?e.message(a.value,a.item):e.message;c.push(f)}}),c},_normalizeRules:function(a){return b.isArray(a)||(a=[a]),b.map(a,b.proxy(function(a){return this._normalizeRule(a)},this))},_normalizeRule:function(a){if("string"==typeof a&&(a={validator:a}),b.isFunction(a)&&(a={validator:a}),!b.isPlainObject(a))throw Error("wrong validation config specified");return a=b.extend({},a),b.isFunction(a.validator)?a:this._applyNamedValidator(a,a.validator)},_applyNamedValidator:function(a,c){delete a.validator;var d=e[c];if(!d)throw Error('unknown validator "'+c+'"');return b.isFunction(d)&&(d={validator:d}),b.extend({},d,a)}},a.Validation=d;var e={required:{message:"Field is required",validator:function(a){return a!==c&&null!==a&&""!==a}},rangeLength:{message:"Field value length is out of the defined range",validator:function(a,b,c){return a.length>=c[0]&&a.length<=c[1]}},minLength:{message:"Field value is too short",validator:function(a,b,c){return a.length>=c}},maxLength:{message:"Field value is too long",validator:function(a,b,c){return a.length<=c}},pattern:{message:"Field value is not matching the defined pattern",validator:function(a,b,c){return"string"==typeof c&&(c=new RegExp("^(?:"+c+")$")),c.test(a)}},range:{message:"Field value is out of the defined range",validator:function(a,b,c){return a>=c[0]&&a<=c[1]}},min:{message:"Field value is too small",validator:function(a,b,c){return a>=c}},max:{message:"Field value is too large",validator:function(a,b,c){return c>=a}}};a.validators=e}(jsGrid,jQuery),function(a,b,c){function d(a){b.extend(!0,this,a),this.sortingFunc=this._getSortingFunc()}d.prototype={name:"",title:null,css:"",align:"",width:100,visible:!0,filtering:!0,inserting:!0,editing:!0,sorting:!0,sorter:"string",headerTemplate:function(){return this.title===c||null===this.title?this.name:this.title},itemTemplate:function(a){return a},filterTemplate:function(){return""},insertTemplate:function(){return""},editTemplate:function(a,b){return this._value=a,this.itemTemplate(a,b)},filterValue:function(){return""},insertValue:function(){return""},editValue:function(){return this._value},_getSortingFunc:function(){var c=this.sorter;if(b.isFunction(c))return c;if("string"==typeof c)return a.sortStrategies[c];throw Error('wrong sorter for the field "'+this.name+'"!')}},a.Field=d}(jsGrid,jQuery),function(a,b){function c(a){d.call(this,a)}var d=a.Field;c.prototype=new d({autosearch:!0,readOnly:!1,filterTemplate:function(){if(!this.filtering)return"";var a=this._grid,b=this.filterControl=this._createTextBox();return this.autosearch&&b.on("keypress",function(b){13===b.which&&(a.search(),b.preventDefault())}),b},insertTemplate:function(){return this.inserting?this.insertControl=this._createTextBox():""},editTemplate:function(a){if(!this.editing)return this.itemTemplate.apply(this,arguments);var b=this.editControl=this._createTextBox();return b.val(a),b},filterValue:function(){return this.filterControl.val()},insertValue:function(){return this.insertControl.val()},editValue:function(){return this.editControl.val()},_createTextBox:function(){return b("<input>").attr("type","text").prop("readonly",!!this.readOnly)}}),a.fields.text=a.TextField=c}(jsGrid,jQuery),function(a,b,c){function d(a){e.call(this,a)}var e=a.TextField;d.prototype=new e({sorter:"number",align:"right",readOnly:!1,filterValue:function(){return this.filterControl.val()?parseInt(this.filterControl.val()||0,10):c},insertValue:function(){return this.insertControl.val()?parseInt(this.insertControl.val()||0,10):c},editValue:function(){return this.editControl.val()?parseInt(this.editControl.val()||0,10):c},_createTextBox:function(){return b("<input>").attr("type","number").prop("readonly",!!this.readOnly)}}),a.fields.number=a.NumberField=d}(jsGrid,jQuery),function(a,b){function c(a){d.call(this,a)}var d=a.TextField;c.prototype=new d({insertTemplate:function(){return this.inserting?this.insertControl=this._createTextArea():""},editTemplate:function(a){if(!this.editing)return this.itemTemplate.apply(this,arguments);var b=this.editControl=this._createTextArea();return b.val(a),b},_createTextArea:function(){return b("<textarea>").prop("readonly",!!this.readOnly)}}),a.fields.textarea=a.TextAreaField=c}(jsGrid,jQuery),function(a,b,c){function d(a){if(this.items=[],this.selectedIndex=-1,this.valueField="",this.textField="",a.valueField&&a.items.length){var b=a.items[0][a.valueField];this.valueType=typeof b===f?f:g}this.sorter=this.valueType,e.call(this,a)}var e=a.NumberField,f="number",g="string";d.prototype=new e({align:"center",valueType:f,itemTemplate:function(a){var d,e=this.items,f=this.valueField,g=this.textField;d=f?b.grep(e,function(b){return b[f]===a})[0]||{}:e[a];var h=g?d[g]:d;return h===c||null===h?"":h},filterTemplate:function(){if(!this.filtering)return"";var a=this._grid,b=this.filterControl=this._createSelect();return this.autosearch&&b.on("change",function(){a.search()}),b},insertTemplate:function(){return this.inserting?this.insertControl=this._createSelect():""},editTemplate:function(a){if(!this.editing)return this.itemTemplate.apply(this,arguments);var b=this.editControl=this._createSelect();return a!==c&&b.val(a),b},filterValue:function(){var a=this.filterControl.val();return this.valueType===f?parseInt(a||0,10):a},insertValue:function(){var a=this.insertControl.val();return this.valueType===f?parseInt(a||0,10):a},editValue:function(){var a=this.editControl.val();return this.valueType===f?parseInt(a||0,10):a},_createSelect:function(){var a=b("<select>"),c=this.valueField,d=this.textField,e=this.selectedIndex;return b.each(this.items,function(f,g){var h=c?g[c]:f,i=d?g[d]:g,j=b("<option>").attr("value",h).text(i).appendTo(a);j.prop("selected",e===f)}),a.prop("disabled",!!this.readOnly),a}}),a.fields.select=a.SelectField=d}(jsGrid,jQuery),function(a,b,c){function d(a){e.call(this,a)}var e=a.Field;d.prototype=new e({sorter:"number",align:"center",autosearch:!0,itemTemplate:function(a){return this._createCheckbox().prop({checked:a,disabled:!0})},filterTemplate:function(){if(!this.filtering)return"";var a=this._grid,c=this.filterControl=this._createCheckbox();return c.prop({readOnly:!0,indeterminate:!0}),c.on("click",function(){var a=b(this);
a.prop("readOnly")?a.prop({checked:!1,readOnly:!1}):a.prop("checked")||a.prop({readOnly:!0,indeterminate:!0})}),this.autosearch&&c.on("click",function(){a.search()}),c},insertTemplate:function(){return this.inserting?this.insertControl=this._createCheckbox():""},editTemplate:function(a){if(!this.editing)return this.itemTemplate.apply(this,arguments);var b=this.editControl=this._createCheckbox();return b.prop("checked",a),b},filterValue:function(){return this.filterControl.get(0).indeterminate?c:this.filterControl.is(":checked")},insertValue:function(){return this.insertControl.is(":checked")},editValue:function(){return this.editControl.is(":checked")},_createCheckbox:function(){return b("<input>").attr("type","checkbox")}}),a.fields.checkbox=a.CheckboxField=d}(jsGrid,jQuery),function(a,b){function c(a){d.call(this,a),this._configInitialized=!1}var d=a.Field;c.prototype=new d({css:"jsgrid-control-field",align:"center",width:50,filtering:!1,inserting:!1,editing:!1,sorting:!1,buttonClass:"jsgrid-button",modeButtonClass:"jsgrid-mode-button",modeOnButtonClass:"jsgrid-mode-on-button",searchModeButtonClass:"jsgrid-search-mode-button",insertModeButtonClass:"jsgrid-insert-mode-button",editButtonClass:"jsgrid-edit-button",deleteButtonClass:"jsgrid-delete-button",searchButtonClass:"jsgrid-search-button",clearFilterButtonClass:"jsgrid-clear-filter-button",insertButtonClass:"jsgrid-insert-button",updateButtonClass:"jsgrid-update-button",cancelEditButtonClass:"jsgrid-cancel-edit-button",searchModeButtonTooltip:"Switch to searching",insertModeButtonTooltip:"Switch to inserting",editButtonTooltip:"Edit",deleteButtonTooltip:"Delete",searchButtonTooltip:"Search",clearFilterButtonTooltip:"Clear filter",insertButtonTooltip:"Insert",updateButtonTooltip:"Update",cancelEditButtonTooltip:"Cancel edit",editButton:!0,deleteButton:!0,clearFilterButton:!0,modeSwitchButton:!0,_initConfig:function(){this._hasFiltering=this._grid.filtering,this._hasInserting=this._grid.inserting,this._hasInserting&&this.modeSwitchButton&&(this._grid.inserting=!1),this._configInitialized=!0},headerTemplate:function(){this._configInitialized||this._initConfig();var a=this._hasFiltering,b=this._hasInserting;return this.modeSwitchButton&&(a||b)?a&&!b?this._createFilterSwitchButton():b&&!a?this._createInsertSwitchButton():this._createModeSwitchButton():""},itemTemplate:function(a,c){var d=b([]);return this.editButton&&(d=d.add(this._createEditButton(c))),this.deleteButton&&(d=d.add(this._createDeleteButton(c))),d},filterTemplate:function(){var a=this._createSearchButton();return this.clearFilterButton?a.add(this._createClearFilterButton()):a},insertTemplate:function(){return this._createInsertButton()},editTemplate:function(){return this._createUpdateButton().add(this._createCancelEditButton())},_createFilterSwitchButton:function(){return this._createOnOffSwitchButton("filtering",this.searchModeButtonClass,!0)},_createInsertSwitchButton:function(){return this._createOnOffSwitchButton("inserting",this.insertModeButtonClass,!1)},_createOnOffSwitchButton:function(a,c,d){var e=d,f=b.proxy(function(){g.toggleClass(this.modeOnButtonClass,e)},this),g=this._createGridButton(this.modeButtonClass+" "+c,"",function(b){e=!e,b.option(a,e),f()});return f(),g},_createModeSwitchButton:function(){var a=!1,c=b.proxy(function(){d.attr("title",a?this.searchModeButtonTooltip:this.insertModeButtonTooltip).toggleClass(this.insertModeButtonClass,!a).toggleClass(this.searchModeButtonClass,a)},this),d=this._createGridButton(this.modeButtonClass,"",function(b){a=!a,b.option("inserting",a),b.option("filtering",!a),c()});return c(),d},_createEditButton:function(a){return this._createGridButton(this.editButtonClass,this.editButtonTooltip,function(b,c){b.editItem(a),c.stopPropagation()})},_createDeleteButton:function(a){return this._createGridButton(this.deleteButtonClass,this.deleteButtonTooltip,function(b,c){b.deleteItem(a),c.stopPropagation()})},_createSearchButton:function(){return this._createGridButton(this.searchButtonClass,this.searchButtonTooltip,function(a){a.search()})},_createClearFilterButton:function(){return this._createGridButton(this.clearFilterButtonClass,this.clearFilterButtonTooltip,function(a){a.clearFilter()})},_createInsertButton:function(){return this._createGridButton(this.insertButtonClass,this.insertButtonTooltip,function(a){a.insertItem().done(function(){a.clearInsert()})})},_createUpdateButton:function(){return this._createGridButton(this.updateButtonClass,this.updateButtonTooltip,function(a,b){a.updateItem(),b.stopPropagation()})},_createCancelEditButton:function(){return this._createGridButton(this.cancelEditButtonClass,this.cancelEditButtonTooltip,function(a,b){a.cancelEdit(),b.stopPropagation()})},_createGridButton:function(a,c,d){var e=this._grid;return b("<input>").addClass(this.buttonClass).addClass(a).attr({type:"button",title:c}).on("click",function(a){d(e,a)})},editValue:function(){return""}}),a.fields.control=a.ControlField=c}(jsGrid,jQuery);