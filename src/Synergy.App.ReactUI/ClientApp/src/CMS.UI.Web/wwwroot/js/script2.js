/*!
 * jquery.fancytree.js
 * Tree view control with support for lazy loading and much more.
 * https://github.com/mar10/fancytree/
 *
 * Copyright (c) 2008-2017, Martin Wendt (http://wwWendt.de)
 * Released under the MIT license
 * https://github.com/mar10/fancytree/wiki/LicenseInfo
 *
 * @version 2.23.0
 * @date 2017-05-27T20:09:38Z
 */

/** Core Fancytree module.
 */


// Start of local namespace
;(function($, window, document, undefined) {
"use strict";

// prevent duplicate loading
if ( $.ui && $.ui.fancytree ) {
	$.ui.fancytree.warn("Fancytree: ignored duplicate include");
	return;
}


/* *****************************************************************************
 * Private functions and variables
 */

var i, attr,
	FT = null, // initialized below
	TEST_IMG = new RegExp(/\.|\//),  // strings are considered image urls if they contain '.' or '/'
	REX_HTML = /[&<>"'\/]/g,
	REX_TOOLTIP = /[<>"'\/]/g,
	RECURSIVE_REQUEST_ERROR = "$recursive_request",
	ENTITY_MAP = {"&": "&amp;", "<": "&lt;", ">": "&gt;", "\"": "&quot;", "'": "&#39;", "/": "&#x2F;"},
	IGNORE_KEYCODES = { 16: true, 17: true, 18: true },
	SPECIAL_KEYCODES = {
		8: "backspace", 9: "tab", 10: "return", 13: "return",
		// 16: null, 17: null, 18: null, // ignore shift, ctrl, alt
		19: "pause", 20: "capslock", 27: "esc", 32: "space", 33: "pageup",
		34: "pagedown", 35: "end", 36: "home", 37: "left", 38: "up",
		39: "right", 40: "down", 45: "insert", 46: "del", 59: ";", 61: "=",
		96: "0", 97: "1", 98: "2", 99: "3", 100: "4", 101: "5", 102: "6",
		103: "7", 104: "8", 105: "9", 106: "*", 107: "+", 109: "-", 110: ".",
		111: "/", 112: "f1", 113: "f2", 114: "f3", 115: "f4", 116: "f5",
		117: "f6", 118: "f7", 119: "f8", 120: "f9", 121: "f10", 122: "f11",
		123: "f12", 144: "numlock", 145: "scroll", 173: "-", 186: ";", 187: "=",
		188: ",", 189: "-", 190: ".", 191: "/", 192: "`", 219: "[", 220: "\\",
		221: "]", 222: "'"},
	MOUSE_BUTTONS = { 0: "", 1: "left", 2: "middle", 3: "right" },
	// Boolean attributes that can be set with equivalent class names in the LI tags
	// Note: v2.23: checkbox and hideCheckbox are *not* in this list
	CLASS_ATTRS = "active expanded focus folder lazy radiogroup selected unselectable unselectableIgnore".split(" "),
	CLASS_ATTR_MAP = {},
	// Top-level Fancytree node attributes, that can be set by dict
	NODE_ATTRS = "checkbox expanded extraClasses folder icon key lazy radiogroup refKey selected statusNodeType title tooltip unselectable unselectableIgnore unselectableStatus".split(" "),
	NODE_ATTR_MAP = {},
	// Mapping of lowercase -> real name (because HTML5 data-... attribute only supports lowercase)
	NODE_ATTR_LOWERCASE_MAP = {},
	// Attribute names that should NOT be added to node.data
	NONE_NODE_DATA_MAP = {"active": true, "children": true, "data": true, "focus": true};

for(i=0; i<CLASS_ATTRS.length; i++){ CLASS_ATTR_MAP[CLASS_ATTRS[i]] = true; }
for(i=0; i<NODE_ATTRS.length; i++) {
	attr = NODE_ATTRS[i];
	NODE_ATTR_MAP[attr] = true;
	if( attr !== attr.toLowerCase() ) {
		NODE_ATTR_LOWERCASE_MAP[attr.toLowerCase()] = attr;
	}
}


function _assert(cond, msg){
	// TODO: see qunit.js extractStacktrace()
	if(!cond){
		msg = msg ? ": " + msg : "";
		// consoleApply("assert", [!!cond, msg]);
		$.error("Fancytree assertion failed" + msg);
	}
}

_assert($.ui, "Fancytree requires jQuery UI (http://jqueryui.com)");

function consoleApply(method, args){
	var i, s,
		fn = window.console ? window.console[method] : null;

	if(fn){
		try{
			fn.apply(window.console, args);
		} catch(e) {
			// IE 8?
			s = "";
			for( i=0; i<args.length; i++ ) {
				s += args[i];
			}
			fn(s);
		}
	}
	}

	//function minString(value) {
	//		if(value != null) {
	//	return value.length > 26 ?
	//		value.substring(0, 25) + "..." :
	//		value;
	//} else {
	//	return "";
	//}
}

/*Return true if x is a FancytreeNode.*/
function _isNode(x){
	return !!(x.tree && x.statusNodeType !== undefined);
}

/** Return true if dotted version string is equal or higher than requested version.
 *
 * See http://jsfiddle.net/mar10/FjSAN/
 */
function isVersionAtLeast(dottedVersion, major, minor, patch){
	var i, v, t,
		verParts = $.map($.trim(dottedVersion).split("."), function(e){ return parseInt(e, 10); }),
		testParts = $.map(Array.prototype.slice.call(arguments, 1), function(e){ return parseInt(e, 10); });

	for( i = 0; i < testParts.length; i++ ){
		v = verParts[i] || 0;
		t = testParts[i] || 0;
		if( v !== t ){
			return ( v > t );
		}
	}
	return true;
}

/** Return a wrapper that calls sub.methodName() and exposes
 *  this             : tree
 *  this._local      : tree.ext.EXTNAME
 *  this._super      : base.methodName.call()
 *  this._superApply : base.methodName.apply()
 */
function _makeVirtualFunction(methodName, tree, base, extension, extName){
	// $.ui.fancytree.debug("_makeVirtualFunction", methodName, tree, base, extension, extName);
	// if(rexTestSuper && !rexTestSuper.test(func)){
	//     // extension.methodName() doesn't call _super(), so no wrapper required
	//     return func;
	// }
	// Use an immediate function as closure
	var proxy = (function(){
		var prevFunc = tree[methodName],      // org. tree method or prev. proxy
			baseFunc = extension[methodName], //
			_local = tree.ext[extName],
			_super = function(){
				return prevFunc.apply(tree, arguments);
			},
			_superApply = function(args){
				return prevFunc.apply(tree, args);
			};

		// Return the wrapper function
		return function(){
			var prevLocal = tree._local,
				prevSuper = tree._super,
				prevSuperApply = tree._superApply;

			try{
				tree._local = _local;
				tree._super = _super;
				tree._superApply = _superApply;
				return  baseFunc.apply(tree, arguments);
			}finally{
				tree._local = prevLocal;
				tree._super = prevSuper;
				tree._superApply = prevSuperApply;
			}
		};
	})(); // end of Immediate Function
	return proxy;
}

/**
 * Subclass `base` by creating proxy functions
 */
function _subclassObject(tree, base, extension, extName){
	// $.ui.fancytree.debug("_subclassObject", tree, base, extension, extName);
	for(var attrName in extension){
		if(typeof extension[attrName] === "function"){
			if(typeof tree[attrName] === "function"){
				// override existing method
				tree[attrName] = _makeVirtualFunction(attrName, tree, base, extension, extName);
			}else if(attrName.charAt(0) === "_"){
				// Create private methods in tree.ext.EXTENSION namespace
				tree.ext[extName][attrName] = _makeVirtualFunction(attrName, tree, base, extension, extName);
			}else{
				$.error("Could not override tree." + attrName + ". Use prefix '_' to create tree." + extName + "._" + attrName);
			}
		}else{
			// Create member variables in tree.ext.EXTENSION namespace
			if(attrName !== "options"){
				tree.ext[extName][attrName] = extension[attrName];
			}
		}
	}
}


function _getResolvedPromise(context, argArray){
	if(context === undefined){
		return $.Deferred(function(){this.resolve();}).promise();
	}else{
		return $.Deferred(function(){this.resolveWith(context, argArray);}).promise();
	}
}


function _getRejectedPromise(context, argArray){
	if(context === undefined){
		return $.Deferred(function(){this.reject();}).promise();
	}else{
		return $.Deferred(function(){this.rejectWith(context, argArray);}).promise();
	}
}


function _makeResolveFunc(deferred, context){
	return function(){
		deferred.resolveWith(context);
	};
}


function _getElementDataAsDict($el){
	// Evaluate 'data-NAME' attributes with special treatment for 'data-json'.
	var d = $.extend({}, $el.data()),
		json = d.json;

	delete d.fancytree; // added to container by widget factory (old jQuery UI)
	delete d.uiFancytree; // added to container by widget factory

	if( json ) {
		delete d.json;
		// <li data-json='...'> is already returned as object (http://api.jquery.com/data/#data-html5)
		d = $.extend(d, json);
	}
	return d;
}


function _escapeHtml(s){
	return ("" + s).replace(REX_HTML, function(s) {
		return ENTITY_MAP[s];
	});
}


function _escapeTooltip(s){
	return ("" + s).replace(REX_TOOLTIP, function(s) {
		return ENTITY_MAP[s];
	});
}


// TODO: use currying
function _makeNodeTitleMatcher(s){
	s = s.toLowerCase();
	return function(node){
		return node.title.toLowerCase().indexOf(s) >= 0;
	};
}


function _makeNodeTitleStartMatcher(s){
	var reMatch = new RegExp("^" + s, "i");
	return function(node){
		return reMatch.test(node.title);
	};
}


/* *****************************************************************************
 * FancytreeNode
 */


/**
 * Creates a new FancytreeNode
 *
 * @class FancytreeNode
 * @classdesc A FancytreeNode represents the hierarchical data model and operations.
 *
 * @param {FancytreeNode} parent
 * @param {NodeData} obj
 *
 * @property {Fancytree} tree The tree instance
 * @property {FancytreeNode} parent The parent node
 * @property {string} key Node id (must be unique inside the tree)
 * @property {string} title Display name (may contain HTML)
 * @property {object} data Contains all extra data that was passed on node creation
 * @property {FancytreeNode[] | null | undefined} children Array of child nodes.<br>
 *     For lazy nodes, null or undefined means 'not yet loaded'. Use an empty array
 *     to define a node that has no children.
 * @property {boolean} expanded Use isExpanded(), setExpanded() to access this property.
 * @property {string} extraClasses Additional CSS classes, added to the node's `&lt;span>`.<br>
 *     Note: use `node.add/remove/toggleClass()` to modify.
 * @property {boolean} folder Folder nodes have different default icons and click behavior.<br>
 *     Note: Also non-folders may have children.
 * @property {string} statusNodeType null for standard nodes. Otherwise type of special system node: 'error', 'loading', 'nodata', or 'paging'.
 * @property {boolean} lazy True if this node is loaded on demand, i.e. on first expansion.
 * @property {boolean} selected Use isSelected(), setSelected() to access this property.
 * @property {string} tooltip Alternative description used as hover popup
 */
function FancytreeNode(parent, obj){
	var i, l, name, cl;

	this.parent = parent;
	this.tree = parent.tree;
	this.ul = null;
	this.li = null;  // <li id='key' ftnode=this> tag
	this.statusNodeType = null; // if this is a temp. node to display the status of its parent
	this._isLoading = false;    // if this node itself is loading
	this._error = null;         // {message: '...'} if a load error occurred
	this.data = {};

	// TODO: merge this code with node.toDict()
	// copy attributes from obj object
	for(i=0, l=NODE_ATTRS.length; i<l; i++){
		name = NODE_ATTRS[i];
		this[name] = obj[name];
	}
	// unselectableIgnore and unselectableStatus imply unselectable
	if( this.unselectableIgnore != null || this.unselectableStatus != null ) {
		this.unselectable = true;
	}
	if( obj.hideCheckbox ) {
		$.error("'hideCheckbox' node option was removed in v2.23.0: use 'checkbox: false'");
	}
	// node.data += obj.data
	if(obj.data){
		$.extend(this.data, obj.data);
	}
	// copy all other attributes to this.data.NAME
	for(name in obj){
		if(!NODE_ATTR_MAP[name] && !$.isFunction(obj[name]) && !NONE_NODE_DATA_MAP[name]){
			// node.data.NAME = obj.NAME
			this.data[name] = obj[name];
		}
	}

	// Fix missing key
	if( this.key == null ){ // test for null OR undefined
		if( this.tree.options.defaultKey ) {
			this.key = this.tree.options.defaultKey(this);
			_assert(this.key, "defaultKey() must return a unique key");
		} else {
			this.key = "_" + (FT._nextNodeKey++);
		}
	} else {
		this.key = "" + this.key; // Convert to string (#217)
	}

	// Fix tree.activeNode
	// TODO: not elegant: we use obj.active as marker to set tree.activeNode
	// when loading from a dictionary.
	if(obj.active){
		_assert(this.tree.activeNode === null, "only one active node allowed");
		this.tree.activeNode = this;
	}
	if( obj.selected ){ // #186
		this.tree.lastSelectedNode = this;
	}
	// TODO: handle obj.focus = true

	// Create child nodes
	cl = obj.children;
	if( cl ){
		if( cl.length ){
			this._setChildren(cl);
		} else {
			// if an empty array was passed for a lazy node, keep it, in order to mark it 'loaded'
			this.children = this.lazy ? [] : null;
		}
	} else {
		this.children = null;
	}
	// Add to key/ref map (except for root node)
//	if( parent ) {
	this.tree._callHook("treeRegisterNode", this.tree, true, this);
//	}
}


FancytreeNode.prototype = /** @lends FancytreeNode# */{
	/* Return the direct child FancytreeNode with a given key, index. */
	_findDirectChild: function(ptr){
		var i, l,
			cl = this.children;

		if(cl){
			if(typeof ptr === "string"){
				for(i=0, l=cl.length; i<l; i++){
					if(cl[i].key === ptr){
						return cl[i];
					}
				}
			}else if(typeof ptr === "number"){
				return this.children[ptr];
			}else if(ptr.parent === this){
				return ptr;
			}
		}
		return null;
	},
	// TODO: activate()
	// TODO: activateSilently()
	/* Internal helper called in recursive addChildren sequence.*/
	_setChildren: function(children){
		_assert(children && (!this.children || this.children.length === 0), "only init supported");
		this.children = [];
		for(var i=0, l=children.length; i<l; i++){
			this.children.push(new FancytreeNode(this, children[i]));
		}
	},
	/**
	 * Append (or insert) a list of child nodes.
	 *
	 * @param {NodeData[]} children array of child node definitions (also single child accepted)
	 * @param {FancytreeNode | string | Integer} [insertBefore] child node (or key or index of such).
	 *     If omitted, the new children are appended.
	 * @returns {FancytreeNode} first child added
	 *
	 * @see FancytreeNode#applyPatch
	 */
	addChildren: function(children, insertBefore){
		var i, l, pos,
			origFirstChild = this.getFirstChild(),
			origLastChild = this.getLastChild(),
			firstNode = null,
			nodeList = [];

		if($.isPlainObject(children) ){
			children = [children];
		}
		if(!this.children){
			this.children = [];
		}
		for(i=0, l=children.length; i<l; i++){
			nodeList.push(new FancytreeNode(this, children[i]));
		}
		firstNode = nodeList[0];
		if(insertBefore == null){
			this.children = this.children.concat(nodeList);
		}else{
			insertBefore = this._findDirectChild(insertBefore);
			pos = $.inArray(insertBefore, this.children);
			_assert(pos >= 0, "insertBefore must be an existing child");
			// insert nodeList after children[pos]
			this.children.splice.apply(this.children, [pos, 0].concat(nodeList));
		}
		if ( origFirstChild && !insertBefore ) {
			// #708: Fast path -- don't render every child of root, just the new ones!
			// #723, #729: but only if it's appended to an existing child list
			for(i=0, l=nodeList.length; i<l; i++) {
				nodeList[i].render();   // New nodes were never rendered before
			}
			// Adjust classes where status may have changed
			// Has a first child
			if (origFirstChild !== this.getFirstChild()) {
				// Different first child -- recompute classes
				origFirstChild.renderStatus();
			}
			if (origLastChild !== this.getLastChild()) {
				// Different last child -- recompute classes
				origLastChild.renderStatus();
			}
		} else if( !this.parent || this.parent.ul || this.tr ){
			// render if the parent was rendered (or this is a root node)
			this.render();
		}
		if( this.tree.options.selectMode === 3 ){
			this.fixSelection3FromEndNodes();
		}
		this.triggerModifyChild("add", nodeList.length === 1 ? nodeList[0] : null);
		return firstNode;
	},
	/**
	 * Add class to node's span tag and to .extraClasses.
	 *
	 * @param {string} className class name
	 *
	 * @since 2.17
	 */
	addClass: function(className){
		return this.toggleClass(className, true);
	},
	/**
	 * Append or prepend a node, or append a child node.
	 *
	 * This a convenience function that calls addChildren()
	 *
	 * @param {NodeData} node node definition
	 * @param {string} [mode=child] 'before', 'after', 'firstChild', or 'child' ('over' is a synonym for 'child')
	 * @returns {FancytreeNode} new node
	 */
	addNode: function(node, mode){
		if(mode === undefined || mode === "over"){
			mode = "child";
		}
		switch(mode){
		case "after":
			return this.getParent().addChildren(node, this.getNextSibling());
		case "before":
			return this.getParent().addChildren(node, this);
		case "firstChild":
			// Insert before the first child if any
			var insertBefore = (this.children ? this.children[0] : null);
			return this.addChildren(node, insertBefore);
		case "child":
		case "over":
			return this.addChildren(node);
		}
		_assert(false, "Invalid mode: " + mode);
	},
	/**Add child status nodes that indicate 'More...', etc.
	 *
	 * This also maintains the node's `partload` property.
	 * @param {boolean|object} node optional node definition. Pass `false` to remove all paging nodes.
	 * @param {string} [mode='child'] 'child'|firstChild'
	 * @since 2.15
	 */
	addPagingNode: function(node, mode){
		var i, n;

		mode = mode || "child";
		if( node === false ) {
			for(i=this.children.length-1; i >= 0; i--) {
				n = this.children[i];
				if( n.statusNodeType === "paging" ) {
					this.removeChild(n);
				}
			}
			this.partload = false;
			return;
		}
		node = $.extend({
			title: this.tree.options.strings.moreData,
			statusNodeType: "paging",
			icon: false
		}, node);
		this.partload = true;
		return this.addNode(node, mode);
	},
	/**
	 * Append new node after this.
	 *
	 * This a convenience function that calls addNode(node, 'after')
	 *
	 * @param {NodeData} node node definition
	 * @returns {FancytreeNode} new node
	 */
	appendSibling: function(node){
		return this.addNode(node, "after");
	},
	/**
	 * Modify existing child nodes.
	 *
	 * @param {NodePatch} patch
	 * @returns {$.Promise}
	 * @see FancytreeNode#addChildren
	 */
	applyPatch: function(patch) {
		// patch [key, null] means 'remove'
		if(patch === null){
			this.remove();
			return _getResolvedPromise(this);
		}
		// TODO: make sure that root node is not collapsed or modified
		// copy (most) attributes to node.ATTR or node.data.ATTR
		var name, promise, v,
			IGNORE_MAP = { children: true, expanded: true, parent: true }; // TODO: should be global

		for(name in patch){
			v = patch[name];
			if( !IGNORE_MAP[name] && !$.isFunction(v)){
				if(NODE_ATTR_MAP[name]){
					this[name] = v;
				}else{
					this.data[name] = v;
				}
			}
		}
		// Remove and/or create children
		if(patch.hasOwnProperty("children")){
			this.removeChildren();
			if(patch.children){ // only if not null and not empty list
				// TODO: addChildren instead?
				this._setChildren(patch.children);
			}
			// TODO: how can we APPEND or INSERT child nodes?
		}
		if(this.isVisible()){
			this.renderTitle();
			this.renderStatus();
		}
		// Expand collapse (final step, since this may be async)
		if(patch.hasOwnProperty("expanded")){
			promise = this.setExpanded(patch.expanded);
		}else{
			promise = _getResolvedPromise(this);
		}
		return promise;
	},
	/** Collapse all sibling nodes.
	 * @returns {$.Promise}
	 */
	collapseSiblings: function() {
		return this.tree._callHook("nodeCollapseSiblings", this);
	},
	/** Copy this node as sibling or child of `node`.
	 *
	 * @param {FancytreeNode} node source node
	 * @param {string} [mode=child] 'before' | 'after' | 'child'
	 * @param {Function} [map] callback function(NodeData) that could modify the new node
	 * @returns {FancytreeNode} new
	 */
	copyTo: function(node, mode, map) {
		return node.addNode(this.toDict(true, map), mode);
	},
	/** Count direct and indirect children.
	 *
	 * @param {boolean} [deep=true] pass 'false' to only count direct children
	 * @returns {int} number of child nodes
	 */
	countChildren: function(deep) {
		var cl = this.children, i, l, n;
		if( !cl ){
			return 0;
		}
		n = cl.length;
		if(deep !== false){
			for(i=0, l=n; i<l; i++){
				n += cl[i].countChildren();
			}
		}
		return n;
	},
	// TODO: deactivate()
	/** Write to browser console if debugLevel >= 2 (prepending node info)
	 *
	 * @param {*} msg string or object or array of such
	 */
	debug: function(msg){
		if( this.tree.options.debugLevel >= 2 ) {
			Array.prototype.unshift.call(arguments, this.toString());
			consoleApply("log", arguments);
		}
	},
	/** Deprecated.
	 * @deprecated since 2014-02-16. Use resetLazy() instead.
	 */
	discard: function(){
		this.warn("FancytreeNode.discard() is deprecated since 2014-02-16. Use .resetLazy() instead.");
		return this.resetLazy();
	},
	/** Remove DOM elements for all descendents. May be called on .collapse event
	 * to keep the DOM small.
	 * @param {boolean} [includeSelf=false]
	 */
	discardMarkup: function(includeSelf){
		var fn = includeSelf ? "nodeRemoveMarkup" : "nodeRemoveChildMarkup";
		this.tree._callHook(fn, this);
	},
	/**Find all nodes that match condition (excluding self).
	 *
	 * @param {string | function(node)} match title string to search for, or a
	 *     callback function that returns `true` if a node is matched.
	 * @returns {FancytreeNode[]} array of nodes (may be empty)
	 */
	findAll: function(match) {
		match = $.isFunction(match) ? match : _makeNodeTitleMatcher(match);
		var res = [];
		this.visit(function(n){
			if(match(n)){
				res.push(n);
			}
		});
		return res;
	},
	/**Find first node that matches condition (excluding self).
	 *
	 * @param {string | function(node)} match title string to search for, or a
	 *     callback function that returns `true` if a node is matched.
	 * @returns {FancytreeNode} matching node or null
	 * @see FancytreeNode#findAll
	 */
	findFirst: function(match) {
		match = $.isFunction(match) ? match : _makeNodeTitleMatcher(match);
		var res = null;
		this.visit(function(n){
			if(match(n)){
				res = n;
				return false;
			}
		});
		return res;
	},
	/* Apply selection state (internal use only) */
	_changeSelectStatusAttrs: function(state) {
		var changed = false,
			opts = this.tree.options,
			unselectable = FT.evalOption("unselectable", this, this, opts, false),
			unselectableStatus = FT.evalOption("unselectableStatus", this, this, opts, undefined);

		if( unselectable && unselectableStatus != null ) {
			state = unselectableStatus;
		}
		switch(state){
		case false:
			changed = ( this.selected || this.partsel );
			this.selected = false;
			this.partsel = false;
			break;
		case true:
			changed = ( !this.selected || !this.partsel );
			this.selected = true;
			this.partsel = true;
			break;
		case undefined:
			changed = ( this.selected || !this.partsel );
			this.selected = false;
			this.partsel = true;
			break;
		default:
			_assert(false, "invalid state: " + state);
		}
		// this.debug("fixSelection3AfterLoad() _changeSelectStatusAttrs()", state, changed);
		if( changed ){
			this.renderStatus();
		}
		return changed;
	},
	/**
	 * Fix selection status, after this node was (de)selected in multi-hier mode.
	 * This includes (de)selecting all children.
	 */
	fixSelection3AfterClick: function(callOpts) {
		var flag = this.isSelected();

//		this.debug("fixSelection3AfterClick()");

		this.visit(function(node){
			node._changeSelectStatusAttrs(flag);
		});
		this.fixSelection3FromEndNodes(callOpts);
	},
	/**
	 * Fix selection status for multi-hier mode.
	 * Only end-nodes are considered to update the descendants branch and parents.
	 * Should be called after this node has loaded new children or after
	 * children have been modified using the API.
	 */
	fixSelection3FromEndNodes: function(callOpts) {
		var opts = this.tree.options;

//		this.debug("fixSelection3FromEndNodes()");
		_assert(opts.selectMode === 3, "expected selectMode 3");

		// Visit all end nodes and adjust their parent's `selected` and `partsel`
		// attributes. Return selection state true, false, or undefined.
		function _walk(node){
			var i, l, child, s, state, allSelected, someSelected, unselIgnore, unselState,
				children = node.children;

			if( children && children.length ){
				// check all children recursively
				allSelected = true;
				someSelected = false;

				for( i=0, l=children.length; i<l; i++ ){
					child = children[i];
					// the selection state of a node is not relevant; we need the end-nodes
					s = _walk(child);
					// if( !child.unselectableIgnore ) {
					unselIgnore = FT.evalOption("unselectableIgnore", child, child, opts, false);
					if( !unselIgnore ) {
						if( s !== false ) {
							someSelected = true;
						}
						if( s !== true ) {
							allSelected = false;
						}
					}
				}
				state = allSelected ? true : (someSelected ? undefined : false);
			}else{
				// This is an end-node: simply report the status
				unselState = FT.evalOption("unselectableStatus", node, node, opts, undefined);
				state = ( unselState == null ) ? !!node.selected : !!unselState;
			}
			node._changeSelectStatusAttrs(state);
			return state;
		}
		_walk(this);

		// Update parent's state
		this.visitParents(function(node){
			var i, l, child, state, unselIgnore, unselState,
				children = node.children,
				allSelected = true,
				someSelected = false;

			for( i=0, l=children.length; i<l; i++ ){
				child = children[i];
				unselIgnore = FT.evalOption("unselectableIgnore", child, child, opts, false);
				if( !unselIgnore ) {
					unselState = FT.evalOption("unselectableStatus", child,  child, opts, undefined);
					state = ( unselState == null ) ? !!child.selected : !!unselState;
					// When fixing the parents, we trust the sibling status (i.e.
					// we don't recurse)
					if( state || child.partsel ) {
						someSelected = true;
					}
					if( !state ) {
						allSelected = false;
					}
				}
			}
			state = allSelected ? true : (someSelected ? undefined : false);
			node._changeSelectStatusAttrs(state);
		});
	},
	// TODO: focus()
	/**
	 * Update node data. If dict contains 'children', then also replace
	 * the hole sub tree.
	 * @param {NodeData} dict
	 *
	 * @see FancytreeNode#addChildren
	 * @see FancytreeNode#applyPatch
	 */
	fromDict: function(dict) {
		// copy all other attributes to this.data.xxx
		for(var name in dict){
			if(NODE_ATTR_MAP[name]){
				// node.NAME = dict.NAME
				this[name] = dict[name];
			}else if(name === "data"){
				// node.data += dict.data
				$.extend(this.data, dict.data);
			}else if(!$.isFunction(dict[name]) && !NONE_NODE_DATA_MAP[name]){
				// node.data.NAME = dict.NAME
				this.data[name] = dict[name];
			}
		}
		if(dict.children){
			// recursively set children and render
			this.removeChildren();
			this.addChildren(dict.children);
		}
		this.renderTitle();
/*
		var children = dict.children;
		if(children === undefined){
			this.data = $.extend(this.data, dict);
			this.render();
			return;
		}
		dict = $.extend({}, dict);
		dict.children = undefined;
		this.data = $.extend(this.data, dict);
		this.removeChildren();
		this.addChild(children);
*/
	},
	/** Return the list of child nodes (undefined for unexpanded lazy nodes).
	 * @returns {FancytreeNode[] | undefined}
	 */
	getChildren: function() {
		if(this.hasChildren() === undefined){ // TODO: only required for lazy nodes?
			return undefined; // Lazy node: unloaded, currently loading, or load error
		}
		return this.children;
	},
	/** Return the first child node or null.
	 * @returns {FancytreeNode | null}
	 */
	getFirstChild: function() {
		return this.children ? this.children[0] : null;
	},
	/** Return the 0-based child index.
	 * @returns {int}
	 */
	getIndex: function() {
//		return this.parent.children.indexOf(this);
		return $.inArray(this, this.parent.children); // indexOf doesn't work in IE7
	},
	/** Return the hierarchical child index (1-based, e.g. '3.2.4').
	 * @param {string} [separator="."]
	 * @param {int} [digits=1]
	 * @returns {string}
	 */
	getIndexHier: function(separator, digits) {
		separator = separator || ".";
		var s,
			res = [];
		$.each(this.getParentList(false, true), function(i, o){
			s = "" + (o.getIndex() + 1);
			if( digits ){
				// prepend leading zeroes
				s = ("0000000" + s).substr(-digits);
			}
			res.push(s);
		});
		return res.join(separator);
	},
	/** Return the parent keys separated by options.keyPathSeparator, e.g. "id_1/id_17/id_32".
	 * @param {boolean} [excludeSelf=false]
	 * @returns {string}
	 */
	getKeyPath: function(excludeSelf) {
		var path = [],
			sep = this.tree.options.keyPathSeparator;
		this.visitParents(function(n){
			if(n.parent){
				path.unshift(n.key);
			}
		}, !excludeSelf);
		return sep + path.join(sep);
	},
	/** Return the last child of this node or null.
	 * @returns {FancytreeNode | null}
	 */
	getLastChild: function() {
		return this.children ? this.children[this.children.length - 1] : null;
	},
	/** Return node depth. 0: System root node, 1: visible top-level node, 2: first sub-level, ... .
	 * @returns {int}
	 */
	getLevel: function() {
		var level = 0,
			dtn = this.parent;
		while( dtn ) {
			level++;
			dtn = dtn.parent;
		}
		return level;
	},
	/** Return the successor node (under the same parent) or null.
	 * @returns {FancytreeNode | null}
	 */
	getNextSibling: function() {
		// TODO: use indexOf, if available: (not in IE6)
		if( this.parent ){
			var i, l,
				ac = this.parent.children;

			for(i=0, l=ac.length-1; i<l; i++){ // up to length-2, so next(last) = null
				if( ac[i] === this ){
					return ac[i+1];
				}
			}
		}
		return null;
	},
	/** Return the parent node (null for the system root node).
	 * @returns {FancytreeNode | null}
	 */
	getParent: function() {
		// TODO: return null for top-level nodes?
		return this.parent;
	},
	/** Return an array of all parent nodes (top-down).
	 * @param {boolean} [includeRoot=false] Include the invisible system root node.
	 * @param {boolean} [includeSelf=false] Include the node itself.
	 * @returns {FancytreeNode[]}
	 */
	getParentList: function(includeRoot, includeSelf) {
		var l = [],
			dtn = includeSelf ? this : this.parent;
		while( dtn ) {
			if( includeRoot || dtn.parent ){
				l.unshift(dtn);
			}
			dtn = dtn.parent;
		}
		return l;
	},
	/** Return the predecessor node (under the same parent) or null.
	 * @returns {FancytreeNode | null}
	 */
	getPrevSibling: function() {
		if( this.parent ){
			var i, l,
				ac = this.parent.children;

			for(i=1, l=ac.length; i<l; i++){ // start with 1, so prev(first) = null
				if( ac[i] === this ){
					return ac[i-1];
				}
			}
		}
		return null;
	},
	/**
	 * Return an array of selected descendant nodes.
	 * @param {boolean} [stopOnParents=false] only return the topmost selected
	 *     node (useful with selectMode 3)
	 * @returns {FancytreeNode[]}
	 */
	getSelectedNodes: function(stopOnParents) {
		var nodeList = [];
		this.visit(function(node){
			if( node.selected ) {
				nodeList.push(node);
				if( stopOnParents === true ){
					return "skip"; // stop processing this branch
				}
			}
		});
		return nodeList;
	},
	/** Return true if node has children. Return undefined if not sure, i.e. the node is lazy and not yet loaded).
	 * @returns {boolean | undefined}
	 */
	hasChildren: function() {
		if(this.lazy){
			if(this.children == null ){
				// null or undefined: Not yet loaded
				return undefined;
			}else if(this.children.length === 0){
				// Loaded, but response was empty
				return false;
			}else if(this.children.length === 1 && this.children[0].isStatusNode() ){
				// Currently loading or load error
				return undefined;
			}
			return true;
		}
		return !!( this.children && this.children.length );
	},
	/** Return true if node has keyboard focus.
	 * @returns {boolean}
	 */
	hasFocus: function() {
		return (this.tree.hasFocus() && this.tree.focusNode === this);
	},
	/** Write to browser console if debugLevel >= 1 (prepending node info)
	 *
	 * @param {*} msg string or object or array of such
	 */
	info: function(msg){
		if( this.tree.options.debugLevel >= 1 ) {
			Array.prototype.unshift.call(arguments, this.toString());
			consoleApply("info", arguments);
		}
	},
	/** Return true if node is active (see also FancytreeNode#isSelected).
	 * @returns {boolean}
	 */
	isActive: function() {
		return (this.tree.activeNode === this);
	},
	/** Return true if node is a direct child of otherNode.
	 * @param {FancytreeNode} otherNode
	 * @returns {boolean}
	 */
	isChildOf: function(otherNode) {
		return (this.parent && this.parent === otherNode);
	},
	/** Return true, if node is a direct or indirect sub node of otherNode.
	 * @param {FancytreeNode} otherNode
	 * @returns {boolean}
	 */
	isDescendantOf: function(otherNode) {
		if(!otherNode || otherNode.tree !== this.tree){
			return false;
		}
		var p = this.parent;
		while( p ) {
			if( p === otherNode ){
				return true;
			}
			p = p.parent;
		}
		return false;
	},
	/** Return true if node is expanded.
	 * @returns {boolean}
	 */
	isExpanded: function() {
		return !!this.expanded;
	},
	/** Return true if node is the first node of its parent's children.
	 * @returns {boolean}
	 */
	isFirstSibling: function() {
		var p = this.parent;
		return !p || p.children[0] === this;
	},
	/** Return true if node is a folder, i.e. has the node.folder attribute set.
	 * @returns {boolean}
	 */
	isFolder: function() {
		return !!this.folder;
	},
	/** Return true if node is the last node of its parent's children.
	 * @returns {boolean}
	 */
	isLastSibling: function() {
		var p = this.parent;
		return !p || p.children[p.children.length-1] === this;
	},
	/** Return true if node is lazy (even if data was already loaded)
	 * @returns {boolean}
	 */
	isLazy: function() {
		return !!this.lazy;
	},
	/** Return true if node is lazy and loaded. For non-lazy nodes always return true.
	 * @returns {boolean}
	 */
	isLoaded: function() {
		return !this.lazy || this.hasChildren() !== undefined; // Also checks if the only child is a status node
	},
	/** Return true if children are currently beeing loaded, i.e. a Ajax request is pending.
	 * @returns {boolean}
	 */
	isLoading: function() {
		return !!this._isLoading;
	},
	/*
	 * @deprecated since v2.4.0:  Use isRootNode() instead
	 */
	isRoot: function() {
		return this.isRootNode();
	},
	/** Return true if node is partially selected (tri-state).
	 * @returns {boolean}
	 * @since 2.23
	 */
	isPartsel: function() {
		return !this.selected && !!this.partsel;
	},
	/** (experimental) Return true if this is partially loaded.
	 * @returns {boolean}
	 * @since 2.15
	 */
	isPartload: function() {
		return !!this.partload;
	},
	/** Return true if this is the (invisible) system root node.
	 * @returns {boolean}
	 * @since 2.4
	 */
	isRootNode: function() {
		return (this.tree.rootNode === this);
	},
	/** Return true if node is selected, i.e. has a checkmark set (see also FancytreeNode#isActive).
	 * @returns {boolean}
	 */
	isSelected: function() {
		return !!this.selected;
	},
	/** Return true if this node is a temporarily generated system node like
	 * 'loading', 'paging', or 'error' (node.statusNodeType contains the type).
	 * @returns {boolean}
	 */
	isStatusNode: function() {
		return !!this.statusNodeType;
	},
	/** Return true if this node is a status node of type 'paging'.
	 * @returns {boolean}
	 * @since 2.15
	 */
	isPagingNode: function() {
		return this.statusNodeType === "paging";
	},
	/** Return true if this a top level node, i.e. a direct child of the (invisible) system root node.
	 * @returns {boolean}
	 * @since 2.4
	 */
	isTopLevel: function() {
		return (this.tree.rootNode === this.parent);
	},
	/** Return true if node is lazy and not yet loaded. For non-lazy nodes always return false.
	 * @returns {boolean}
	 */
	isUndefined: function() {
		return this.hasChildren() === undefined; // also checks if the only child is a status node
	},
	/** Return true if all parent nodes are expanded. Note: this does not check
	 * whether the node is scrolled into the visible part of the screen.
	 * @returns {boolean}
	 */
	isVisible: function() {
		var i, l,
			parents = this.getParentList(false, false);

		for(i=0, l=parents.length; i<l; i++){
			if( ! parents[i].expanded ){ return false; }
		}
		return true;
	},
	/** Deprecated.
	 * @deprecated since 2014-02-16: use load() instead.
	 */
	lazyLoad: function(discard) {
		this.warn("FancytreeNode.lazyLoad() is deprecated since 2014-02-16. Use .load() instead.");
		return this.load(discard);
	},
	/**
	 * Load all children of a lazy node if neccessary. The <i>expanded</i> state is maintained.
	 * @param {boolean} [forceReload=false] Pass true to discard any existing nodes before. Otherwise this method does nothing if the node was already loaded.
	 * @returns {$.Promise}
	 */
	load: function(forceReload) {
		var res, source,
			that = this,
			wasExpanded = this.isExpanded();

		_assert( this.isLazy(), "load() requires a lazy node" );
		// _assert( forceReload || this.isUndefined(), "Pass forceReload=true to re-load a lazy node" );
		if( !forceReload && !this.isUndefined() ) {
			return _getResolvedPromise(this);
		}
		if( this.isLoaded() ){
			this.resetLazy(); // also collapses
		}
		// This method is also called by setExpanded() and loadKeyPath(), so we
		// have to avoid recursion.
		source = this.tree._triggerNodeEvent("lazyLoad", this);
		if( source === false ) { // #69
			return _getResolvedPromise(this);
		}
		_assert(typeof source !== "boolean", "lazyLoad event must return source in data.result");
		res = this.tree._callHook("nodeLoadChildren", this, source);
		if( wasExpanded ) {
			this.expanded = true;
			res.always(function(){
				that.render();
			});
		} else {
			res.always(function(){
				that.renderStatus();  // fix expander icon to 'loaded'
			});
		}
		return res;
	},
	/** Expand all parents and optionally scroll into visible area as neccessary.
	 * Promise is resolved, when lazy loading and animations are done.
	 * @param {object} [opts] passed to `setExpanded()`.
	 *     Defaults to {noAnimation: false, noEvents: false, scrollIntoView: true}
	 * @returns {$.Promise}
	 */
	makeVisible: function(opts) {
		var i,
			that = this,
			deferreds = [],
			dfd = new $.Deferred(),
			parents = this.getParentList(false, false),
			len = parents.length,
			effects = !(opts && opts.noAnimation === true),
			scroll = !(opts && opts.scrollIntoView === false);

		// Expand bottom-up, so only the top node is animated
		for(i = len - 1; i >= 0; i--){
			// that.debug("pushexpand" + parents[i]);
			deferreds.push(parents[i].setExpanded(true, opts));
		}
		$.when.apply($, deferreds).done(function(){
			// All expands have finished
			// that.debug("expand DONE", scroll);
			if( scroll ){
				that.scrollIntoView(effects).done(function(){
					// that.debug("scroll DONE");
					dfd.resolve();
				});
			} else {
				dfd.resolve();
			}
		});
		return dfd.promise();
	},
	/** Move this node to targetNode.
	 *  @param {FancytreeNode} targetNode
	 *  @param {string} mode <pre>
	 *      'child': append this node as last child of targetNode.
	 *               This is the default. To be compatble with the D'n'd
	 *               hitMode, we also accept 'over'.
	 *      'firstChild': add this node as first child of targetNode.
	 *      'before': add this node as sibling before targetNode.
	 *      'after': add this node as sibling after targetNode.</pre>
	 *  @param {function} [map] optional callback(FancytreeNode) to allow modifcations
	 */
	moveTo: function(targetNode, mode, map) {
		if(mode === undefined || mode === "over"){
			mode = "child";
		} else if ( mode === "firstChild" ) {
			if( targetNode.children && targetNode.children.length ) {
				mode = "before";
				targetNode = targetNode.children[0];
			} else {
				mode = "child";
			}
		}
		var pos,
			prevParent = this.parent,
			targetParent = (mode === "child") ? targetNode : targetNode.parent;

		if(this === targetNode){
			return;
		}else if( !this.parent  ){
			$.error("Cannot move system root");
		}else if( targetParent.isDescendantOf(this) ){
			$.error("Cannot move a node to its own descendant");
		}
		if( targetParent !== prevParent ) {
			prevParent.triggerModifyChild("remove", this);
		}
		// Unlink this node from current parent
		if( this.parent.children.length === 1 ) {
			if( this.parent === targetParent ){
				return; // #258
			}
			this.parent.children = this.parent.lazy ? [] : null;
			this.parent.expanded = false;
		} else {
			pos = $.inArray(this, this.parent.children);
			_assert(pos >= 0, "invalid source parent");
			this.parent.children.splice(pos, 1);
		}
		// Remove from source DOM parent
//		if(this.parent.ul){
//			this.parent.ul.removeChild(this.li);
//		}

		// Insert this node to target parent's child list
		this.parent = targetParent;
		if( targetParent.hasChildren() ) {
			switch(mode) {
			case "child":
				// Append to existing target children
				targetParent.children.push(this);
				break;
			case "before":
				// Insert this node before target node
				pos = $.inArray(targetNode, targetParent.children);
				_assert(pos >= 0, "invalid target parent");
				targetParent.children.splice(pos, 0, this);
				break;
			case "after":
				// Insert this node after target node
				pos = $.inArray(targetNode, targetParent.children);
				_assert(pos >= 0, "invalid target parent");
				targetParent.children.splice(pos+1, 0, this);
				break;
			default:
				$.error("Invalid mode " + mode);
			}
		} else {
			targetParent.children = [ this ];
		}
		// Parent has no <ul> tag yet:
//		if( !targetParent.ul ) {
//			// This is the parent's first child: create UL tag
//			// (Hidden, because it will be
//			targetParent.ul = document.createElement("ul");
//			targetParent.ul.style.display = "none";
//			targetParent.li.appendChild(targetParent.ul);
//		}
//		// Issue 319: Add to target DOM parent (only if node was already rendered(expanded))
//		if(this.li){
//			targetParent.ul.appendChild(this.li);
//		}^

		// Let caller modify the nodes
		if( map ){
			targetNode.visit(map, true);
		}
		if( targetParent === prevParent ) {
			targetParent.triggerModifyChild("move", this);
		} else {
			// prevParent.triggerModifyChild("remove", this);
			targetParent.triggerModifyChild("add", this);
		}
		// Handle cross-tree moves
		if( this.tree !== targetNode.tree ) {
			// Fix node.tree for all source nodes
//			_assert(false, "Cross-tree move is not yet implemented.");
			this.warn("Cross-tree moveTo is experimantal!");
			this.visit(function(n){
				// TODO: fix selection state and activation, ...
				n.tree = targetNode.tree;
			}, true);
		}

		// A collaposed node won't re-render children, so we have to remove it manually
		// if( !targetParent.expanded ){
		//   prevParent.ul.removeChild(this.li);
		// }

		// Update HTML markup
		if( !prevParent.isDescendantOf(targetParent)) {
			prevParent.render();
		}
		if( !targetParent.isDescendantOf(prevParent) && targetParent !== prevParent) {
			targetParent.render();
		}
		// TODO: fix selection state
		// TODO: fix active state

/*
		var tree = this.tree;
		var opts = tree.options;
		var pers = tree.persistence;


		// Always expand, if it's below minExpandLevel
//		tree.logDebug ("%s._addChildNode(%o), l=%o", this, ftnode, ftnode.getLevel());
		if ( opts.minExpandLevel >= ftnode.getLevel() ) {
//			tree.logDebug ("Force expand for %o", ftnode);
			this.bExpanded = true;
		}

		// In multi-hier mode, update the parents selection state
		// DT issue #82: only if not initializing, because the children may not exist yet
//		if( !ftnode.data.isStatusNode() && opts.selectMode==3 && !isInitializing )
//			ftnode._fixSelectionState();

		// In multi-hier mode, update the parents selection state
		if( ftnode.bSelected && opts.selectMode==3 ) {
			var p = this;
			while( p ) {
				if( !p.hasSubSel )
					p._setSubSel(true);
				p = p.parent;
			}
		}
		// render this node and the new child
		if ( tree.bEnableUpdate )
			this.render();

		return ftnode;

*/
	},
	/** Set focus relative to this node and optionally activate.
	 *
	 * @param {number} where The keyCode that would normally trigger this move,
	 *		e.g. `$.ui.keyCode.LEFT` would collapse the node if it
	 *      is expanded or move to the parent oterwise.
	 * @param {boolean} [activate=true]
	 * @returns {$.Promise}
	 */
	navigate: function(where, activate) {
		var i, parents, res,
			handled = true,
			KC = $.ui.keyCode,
			sib = null;

		// Navigate to node
		function _goto(n){
			if( n ){
				// setFocus/setActive will scroll later (if autoScroll is specified)
				try { n.makeVisible({scrollIntoView: false}); } catch(e) {} // #272
				// Node may still be hidden by a filter
				if( ! $(n.span).is(":visible") ) {
					n.debug("Navigate: skipping hidden node");
					n.navigate(where, activate);
					return;
				}
				return activate === false ? n.setFocus() : n.setActive();
			}
		}

		switch( where ) {
			case KC.BACKSPACE:
				if( this.parent && this.parent.parent ) {
					res = _goto(this.parent);
				}
				break;
			case KC.HOME:
				this.tree.visit(function(n){  // goto first visible node
					if( $(n.span).is(":visible") ) {
						res = _goto(n);
						return false;
					}
				});
				break;
			case KC.END:
				this.tree.visit(function(n){  // goto last visible node
					if( $(n.span).is(":visible") ) {
						res = n;
					}
				});
				if( res ) {
					res = _goto(res);
				}
				break;
			case KC.LEFT:
				if( this.expanded ) {
					this.setExpanded(false);
					res = _goto(this);
				} else if( this.parent && this.parent.parent ) {
					res = _goto(this.parent);
				}
				break;
			case KC.RIGHT:
				if( !this.expanded && (this.children || this.lazy) ) {
					this.setExpanded();
					res = _goto(this);
				} else if( this.children && this.children.length ) {
					res = _goto(this.children[0]);
				}
				break;
			case KC.UP:
				sib = this.getPrevSibling();
				// #359: skip hidden sibling nodes, preventing a _goto() recursion
				while( sib && !$(sib.span).is(":visible") ) {
					sib = sib.getPrevSibling();
				}
				while( sib && sib.expanded && sib.children && sib.children.length ) {
					sib = sib.children[sib.children.length - 1];
				}
				if( !sib && this.parent && this.parent.parent ){
					sib = this.parent;
				}
				res = _goto(sib);
				break;
			case KC.DOWN:
				if( this.expanded && this.children && this.children.length ) {
					sib = this.children[0];
				} else {
					parents = this.getParentList(false, true);
					for(i=parents.length-1; i>=0; i--) {
						sib = parents[i].getNextSibling();
						// #359: skip hidden sibling nodes, preventing a _goto() recursion
						while( sib && !$(sib.span).is(":visible") ) {
							sib = sib.getNextSibling();
						}
						if( sib ){ break; }
					}
				}
				res = _goto(sib);
				break;
			default:
				handled = false;
		}
		return res || _getResolvedPromise();
	},
	/**
	 * Remove this node (not allowed for system root).
	 */
	remove: function() {
		return this.parent.removeChild(this);
	},
	/**
	 * Remove childNode from list of direct children.
	 * @param {FancytreeNode} childNode
	 */
	removeChild: function(childNode) {
		return this.tree._callHook("nodeRemoveChild", this, childNode);
	},
	/**
	 * Remove all child nodes and descendents. This converts the node into a leaf.<br>
	 * If this was a lazy node, it is still considered 'loaded'; call node.resetLazy()
	 * in order to trigger lazyLoad on next expand.
	 */
	removeChildren: function() {
		return this.tree._callHook("nodeRemoveChildren", this);
	},
	/**
	 * Remove class from node's span tag and .extraClasses.
	 *
	 * @param {string} className class name
	 *
	 * @since 2.17
	 */
	removeClass: function(className){
		return this.toggleClass(className, false);
	},
	/**
	 * This method renders and updates all HTML markup that is required
	 * to display this node in its current state.<br>
	 * Note:
	 * <ul>
	 * <li>It should only be neccessary to call this method after the node object
	 *     was modified by direct access to its properties, because the common
	 *     API methods (node.setTitle(), moveTo(), addChildren(), remove(), ...)
	 *     already handle this.
	 * <li> {@link FancytreeNode#renderTitle} and {@link FancytreeNode#renderStatus}
	 *     are implied. If changes are more local, calling only renderTitle() or
	 *     renderStatus() may be sufficient and faster.
	 * </ul>
	 *
	 * @param {boolean} [force=false] re-render, even if html markup was already created
	 * @param {boolean} [deep=false] also render all descendants, even if parent is collapsed
	 */
	render: function(force, deep) {
		return this.tree._callHook("nodeRender", this, force, deep);
	},
	/** Create HTML markup for the node's outer &lt;span> (expander, checkbox, icon, and title).
	 * Implies {@link FancytreeNode#renderStatus}.
	 * @see Fancytree_Hooks#nodeRenderTitle
	 */
	renderTitle: function() {
		return this.tree._callHook("nodeRenderTitle", this);
	},
	/** Update element's CSS classes according to node state.
	 * @see Fancytree_Hooks#nodeRenderStatus
	 */
	renderStatus: function() {
		return this.tree._callHook("nodeRenderStatus", this);
	},
	/**
	 * (experimental) Replace this node with `source`.
	 * (Currently only available for paging nodes.)
	 * @param {NodeData[]} source List of child node definitions
	 * @since 2.15
	 */
	replaceWith: function(source) {
		var res,
			parent = this.parent,
			pos = $.inArray(this, parent.children),
			that = this;

		_assert( this.isPagingNode(), "replaceWith() currently requires a paging status node" );

		res = this.tree._callHook("nodeLoadChildren", this, source);
		res.done(function(data){
			// New nodes are currently children of `this`.
			var children = that.children;
			// Prepend newly loaded child nodes to `this`
			// Move new children after self
			for( i=0; i<children.length; i++ ) {
				children[i].parent = parent;
			}
			parent.children.splice.apply(parent.children, [pos + 1, 0].concat(children));

			// Remove self
			that.children = null;
			that.remove();
			// Redraw new nodes
			parent.render();
			// TODO: set node.partload = false if this was tha last paging node?
			// parent.addPagingNode(false);
		}).fail(function(){
			that.setExpanded();
		});
		return res;
		// $.error("Not implemented: replaceWith()");
	},
	/**
	 * Remove all children, collapse, and set the lazy-flag, so that the lazyLoad
	 * event is triggered on next expand.
	 */
	resetLazy: function() {
		this.removeChildren();
		this.expanded = false;
		this.lazy = true;
		this.children = undefined;
		this.renderStatus();
	},
	/** Schedule activity for delayed execution (cancel any pending request).
	 *  scheduleAction('cancel') will only cancel a pending request (if any).
	 * @param {string} mode
	 * @param {number} ms
	 */
	scheduleAction: function(mode, ms) {
		if( this.tree.timer ) {
			clearTimeout(this.tree.timer);
//            this.tree.debug("clearTimeout(%o)", this.tree.timer);
		}
		this.tree.timer = null;
		var self = this; // required for closures
		switch (mode) {
		case "cancel":
			// Simply made sure that timer was cleared
			break;
		case "expand":
			this.tree.timer = setTimeout(function(){
				self.tree.debug("setTimeout: trigger expand");
				self.setExpanded(true);
			}, ms);
			break;
		case "activate":
			this.tree.timer = setTimeout(function(){
				self.tree.debug("setTimeout: trigger activate");
				self.setActive(true);
			}, ms);
			break;
		default:
			$.error("Invalid mode " + mode);
		}
//        this.tree.debug("setTimeout(%s, %s): %s", mode, ms, this.tree.timer);
	},
	/**
	 *
	 * @param {boolean | PlainObject} [effects=false] animation options.
	 * @param {object} [options=null] {topNode: null, effects: ..., parent: ...} this node will remain visible in
	 *     any case, even if `this` is outside the scroll pane.
	 * @returns {$.Promise}
	 */
	scrollIntoView: function(effects, options) {
		if( options !== undefined && _isNode(options) ) {
			this.warn("scrollIntoView() with 'topNode' option is deprecated since 2014-05-08. Use 'options.topNode' instead.");
			options = {topNode: options};
		}
		// this.$scrollParent = (this.options.scrollParent === "auto") ? $ul.scrollParent() : $(this.options.scrollParent);
		// this.$scrollParent = this.$scrollParent.length ? this.$scrollParent || this.$container;

		var topNodeY, nodeY, horzScrollbarHeight, containerOffsetTop,
			opts = $.extend({
				effects: (effects === true) ? {duration: 200, queue: false} : effects,
				scrollOfs: this.tree.options.scrollOfs,
				scrollParent: this.tree.options.scrollParent || this.tree.$container,
				topNode: null
			}, options),
			dfd = new $.Deferred(),
			that = this,
			nodeHeight = $(this.span).height(),
			$container = $(opts.scrollParent),
			topOfs = opts.scrollOfs.top || 0,
			bottomOfs = opts.scrollOfs.bottom || 0,
			containerHeight = $container.height(),// - topOfs - bottomOfs,
			scrollTop = $container.scrollTop(),
			$animateTarget = $container,
			isParentWindow = $container[0] === window,
			topNode = opts.topNode || null,
			newScrollTop = null;

		// this.debug("scrollIntoView(), scrollTop=" + scrollTop, opts.scrollOfs);
//		_assert($(this.span).is(":visible"), "scrollIntoView node is invisible"); // otherwise we cannot calc offsets
		if( !$(this.span).is(":visible") ) {
			// We cannot calc offsets for hidden elements
			this.warn("scrollIntoView(): node is invisible.");
			return _getResolvedPromise();
		}
		if( isParentWindow ) {
			nodeY = $(this.span).offset().top;
			topNodeY = (topNode && topNode.span) ? $(topNode.span).offset().top : 0;
			$animateTarget = $("html,body");

		} else {
			_assert($container[0] !== document && $container[0] !== document.body,
				"scrollParent should be a simple element or `window`, not document or body.");

			containerOffsetTop = $container.offset().top,
			nodeY = $(this.span).offset().top - containerOffsetTop + scrollTop; // relative to scroll parent
			topNodeY = topNode ? $(topNode.span).offset().top - containerOffsetTop + scrollTop : 0;
			horzScrollbarHeight = Math.max(0, ($container.innerHeight() - $container[0].clientHeight));
			containerHeight -= horzScrollbarHeight;
		}

		// this.debug("    scrollIntoView(), nodeY=" + nodeY + ", containerHeight=" + containerHeight);
		if( nodeY < (scrollTop + topOfs) ){
			// Node is above visible container area
			newScrollTop = nodeY - topOfs;
			// this.debug("    scrollIntoView(), UPPER newScrollTop=" + newScrollTop);

		}else if((nodeY + nodeHeight) > (scrollTop + containerHeight - bottomOfs)){
			newScrollTop = nodeY + nodeHeight - containerHeight + bottomOfs;
			// this.debug("    scrollIntoView(), LOWER newScrollTop=" + newScrollTop);
			// If a topNode was passed, make sure that it is never scrolled
			// outside the upper border
			if(topNode){
				_assert(topNode.isRootNode() || $(topNode.span).is(":visible"), "topNode must be visible");
				if( topNodeY < newScrollTop ){
					newScrollTop = topNodeY - topOfs;
					// this.debug("    scrollIntoView(), TOP newScrollTop=" + newScrollTop);
				}
			}
		}

		if(newScrollTop !== null){
			// this.debug("    scrollIntoView(), SET newScrollTop=" + newScrollTop);
			if(opts.effects){
				opts.effects.complete = function(){
					dfd.resolveWith(that);
				};
				$animateTarget.stop(true).animate({
					scrollTop: newScrollTop
				}, opts.effects);
			}else{
				$animateTarget[0].scrollTop = newScrollTop;
				dfd.resolveWith(this);
			}
		}else{
			dfd.resolveWith(this);
		}
		return dfd.promise();
	},

	/**Activate this node.
	 * @param {boolean} [flag=true] pass false to deactivate
	 * @param {object} [opts] additional options. Defaults to {noEvents: false, noFocus: false}
	 * @returns {$.Promise}
	 */
	setActive: function(flag, opts){
		return this.tree._callHook("nodeSetActive", this, flag, opts);
	},
	/**Expand or collapse this node. Promise is resolved, when lazy loading and animations are done.
	 * @param {boolean} [flag=true] pass false to collapse
	 * @param {object} [opts] additional options. Defaults to {noAnimation: false, noEvents: false}
	 * @returns {$.Promise}
	 */
	setExpanded: function(flag, opts){
		return this.tree._callHook("nodeSetExpanded", this, flag, opts);
	},
	/**Set keyboard focus to this node.
	 * @param {boolean} [flag=true] pass false to blur
	 * @see Fancytree#setFocus
	 */
	setFocus: function(flag){
		return this.tree._callHook("nodeSetFocus", this, flag);
	},
	/**Select this node, i.e. check the checkbox.
	 * @param {boolean} [flag=true] pass false to deselect
	 * @param {object} [opts] additional options. Defaults to {noEvents: false, p
	 *     propagateDown: null, propagateUp: null, callback: null }
	 */
	setSelected: function(flag, opts){
		return this.tree._callHook("nodeSetSelected", this, flag, opts);
	},
	/**Mark a lazy node as 'error', 'loading', 'nodata', or 'ok'.
	 * @param {string} status 'error'|'empty'|'ok'
	 * @param {string} [message]
	 * @param {string} [details]
	 */
	setStatus: function(status, message, details){
		return this.tree._callHook("nodeSetStatus", this, status, message, details);
	},
	/**Rename this node.
	 * @param {string} title
	 */
	setTitle: function(title){
		this.title = title;
		this.renderTitle();
		this.triggerModify("rename");
	},
	/**Sort child list by title.
	 * @param {function} [cmp] custom compare function(a, b) that returns -1, 0, or 1 (defaults to sort by title).
	 * @param {boolean} [deep=false] pass true to sort all descendant nodes
	 */
	sortChildren: function(cmp, deep) {
		var i,l,
			cl = this.children;

		if( !cl ){
			return;
		}
		cmp = cmp || function(a, b) {
			var x = a.title.toLowerCase(),
				y = b.title.toLowerCase();
			return x === y ? 0 : x > y ? 1 : -1;
			};
		cl.sort(cmp);
		if( deep ){
			for(i=0, l=cl.length; i<l; i++){
				if( cl[i].children ){
					cl[i].sortChildren(cmp, "$norender$");
				}
			}
		}
		if( deep !== "$norender$" ){
			this.render();
		}
		this.triggerModifyChild("sort");
	},
	/** Convert node (or whole branch) into a plain object.
	 *
	 * The result is compatible with node.addChildren().
	 *
	 * @param {boolean} [recursive=false] include child nodes
	 * @param {function} [callback] callback(dict, node) is called for every node, in order to allow modifications
	 * @returns {NodeData}
	 */
	toDict: function(recursive, callback) {
		var i, l, node,
			dict = {},
			self = this;

		$.each(NODE_ATTRS, function(i, a){
			if(self[a] || self[a] === false){
				dict[a] = self[a];
			}
		});
		if(!$.isEmptyObject(this.data)){
			dict.data = $.extend({}, this.data);
			if($.isEmptyObject(dict.data)){
				delete dict.data;
			}
		}
		if( callback ){
			callback(dict, self);
		}
		if( recursive ) {
			if(this.hasChildren()){
				dict.children = [];
				for(i=0, l=this.children.length; i<l; i++ ){
					node = this.children[i];
					if( !node.isStatusNode() ){
						dict.children.push(node.toDict(true, callback));
					}
				}
			}else{
//                dict.children = null;
			}
		}
		return dict;
	},
	/**
	 * Set, clear, or toggle class of node's span tag and .extraClasses.
	 *
	 * @param {string} className class name (separate multiple classes by space)
	 * @param {boolean} [flag] true/false to add/remove class. If omitted, class is toggled.
	 * @returns {boolean} true if a class was added
	 *
	 * @since 2.17
	 */
	toggleClass: function(value, flag){
		var className, hasClass,
			rnotwhite = ( /\S+/g ),
			classNames = value.match( rnotwhite ) || [],
			i = 0,
			wasAdded = false,
			statusElem = this[this.tree.statusClassPropName],
			curClasses = (" " + (this.extraClasses || "") + " ");

		// this.info("toggleClass('" + value + "', " + flag + ")", curClasses);
		// Modify DOM element directly if it already exists
		if( statusElem ) {
			$(statusElem).toggleClass(value, flag);
		}
		// Modify node.extraClasses to make this change persistent
		// Toggle if flag was not passed
		while ( className = classNames[ i++ ] ) {
			hasClass = curClasses.indexOf(" " + className + " ") >= 0;
			flag = (flag === undefined) ? (!hasClass) : !!flag;
			if ( flag ) {
				if( !hasClass ) {
					curClasses += className + " ";
					wasAdded = true;
				}
			} else {
				while ( curClasses.indexOf( " " + className + " " ) > -1 ) {
					curClasses = curClasses.replace( " " + className + " ", " " );
				}
			}
		}
		this.extraClasses = $.trim(curClasses);
		// this.info("-> toggleClass('" + value + "', " + flag + "): '" + this.extraClasses + "'");
		return wasAdded;
	},
	/** Flip expanded status. */
	toggleExpanded: function(){
		return this.tree._callHook("nodeToggleExpanded", this);
	},
	/** Flip selection status. */
	toggleSelected: function(){
		return this.tree._callHook("nodeToggleSelected", this);
	},
	toString: function() {
		return "<FancytreeNode(#" + this.key + ", '" + this.title + "')>";
	},
	/**
	 * Trigger `modifyChild` event on a parent to signal that a child was modified.
	 * @param {string} operation Type of change: 'add', 'remove', 'rename', 'move', 'data', ...
	 * @param {FancytreeNode} [childNode]
	 * @param {object} [extra]
	 */
	triggerModifyChild: function(operation, childNode, extra){
		var data,
			modifyChild = this.tree.options.modifyChild;

		if ( modifyChild ){
			if( childNode && childNode.parent !== this ) {
				$.error("childNode " + childNode + " is not a child of " + this);
			}
			data = {
				node: this,
				tree: this.tree,
				operation: operation,
				childNode: childNode || null
			};
			if( extra ) {
				$.extend(data, extra);
			}
			modifyChild({type: "modifyChild"}, data);
		}
	},
	/**
	 * Trigger `modifyChild` event on node.parent(!).
	 * @param {string} operation Type of change: 'add', 'remove', 'rename', 'move', 'data', ...
	 * @param {object} [extra]
	 */
	triggerModify: function(operation, extra){
		this.parent.triggerModifyChild(operation, this, extra);
	},
	/** Call fn(node) for all child nodes.<br>
	 * Stop iteration, if fn() returns false. Skip current branch, if fn() returns "skip".<br>
	 * Return false if iteration was stopped.
	 *
	 * @param {function} fn the callback function.
	 *     Return false to stop iteration, return "skip" to skip this node and
	 *     its children only.
	 * @param {boolean} [includeSelf=false]
	 * @returns {boolean}
	 */
	visit: function(fn, includeSelf) {
		var i, l,
			res = true,
			children = this.children;

		if( includeSelf === true ) {
			res = fn(this);
			if( res === false || res === "skip" ){
				return res;
			}
		}
		if(children){
			for(i=0, l=children.length; i<l; i++){
				res = children[i].visit(fn, true);
				if( res === false ){
					break;
				}
			}
		}
		return res;
	},
	/** Call fn(node) for all child nodes and recursively load lazy children.<br>
	 * <b>Note:</b> If you need this method, you probably should consider to review
	 * your architecture! Recursivley loading nodes is a perfect way for lazy
	 * programmers to flood the server with requests ;-)
	 *
	 * @param {function} [fn] optional callback function.
	 *     Return false to stop iteration, return "skip" to skip this node and
	 *     its children only.
	 * @param {boolean} [includeSelf=false]
	 * @returns {$.Promise}
	 * @since 2.4
	 */
	visitAndLoad: function(fn, includeSelf, _recursion) {
		var dfd, res, loaders,
			node = this;

		// node.debug("visitAndLoad");
		if( fn && includeSelf === true ) {
			res = fn(node);
			if( res === false || res === "skip" ) {
				return _recursion ? res : _getResolvedPromise();
			}
		}
		if( !node.children && !node.lazy ) {
			return _getResolvedPromise();
		}
		dfd = new $.Deferred();
		loaders = [];
		// node.debug("load()...");
		node.load().done(function(){
			// node.debug("load()... done.");
			for(var i=0, l=node.children.length; i<l; i++){
				res = node.children[i].visitAndLoad(fn, true, true);
				if( res === false ) {
					dfd.reject();
					break;
				} else if ( res !== "skip" ) {
					loaders.push(res); // Add promise to the list
				}
			}
			$.when.apply(this, loaders).then(function(){
				dfd.resolve();
			});
		});
		return dfd.promise();
	},
	/** Call fn(node) for all parent nodes, bottom-up, including invisible system root.<br>
	 * Stop iteration, if fn() returns false.<br>
	 * Return false if iteration was stopped.
	 *
	 * @param {function} fn the callback function.
	 *     Return false to stop iteration, return "skip" to skip this node and children only.
	 * @param {boolean} [includeSelf=false]
	 * @returns {boolean}
	 */
	visitParents: function(fn, includeSelf) {
		// Visit parent nodes (bottom up)
		if(includeSelf && fn(this) === false){
			return false;
		}
		var p = this.parent;
		while( p ) {
			if(fn(p) === false){
				return false;
			}
			p = p.parent;
		}
		return true;
	},
	/** Call fn(node) for all sibling nodes.<br>
	 * Stop iteration, if fn() returns false.<br>
	 * Return false if iteration was stopped.
	 *
	 * @param {function} fn the callback function.
	 *     Return false to stop iteration.
	 * @param {boolean} [includeSelf=false]
	 * @returns {boolean}
	 */
	visitSiblings: function(fn, includeSelf) {
		var i, l, n,
			ac = this.parent.children;

		for (i=0, l=ac.length; i<l; i++) {
			n = ac[i];
			if ( includeSelf || n !== this ){
				if( fn(n) === false ) {
					return false;
				}
			}
		}
		return true;
	},
	/** Write warning to browser console (prepending node info)
	 *
	 * @param {*} msg string or object or array of such
	 */
	warn: function(msg){
		Array.prototype.unshift.call(arguments, this.toString());
		consoleApply("warn", arguments);
	}
};


/* *****************************************************************************
 * Fancytree
 */
/**
 * Construct a new tree object.
 *
 * @class Fancytree
 * @classdesc The controller behind a fancytree.
 * This class also contains 'hook methods': see {@link Fancytree_Hooks}.
 *
 * @param {Widget} widget
 *
 * @property {string} _id Automatically generated unique tree instance ID, e.g. "1".
 * @property {string} _ns Automatically generated unique tree namespace, e.g. ".fancytree-1".
 * @property {FancytreeNode} activeNode Currently active node or null.
 * @property {string} ariaPropName Property name of FancytreeNode that contains the element which will receive the aria attributes.
 *     Typically "li", but "tr" for table extension.
 * @property {jQueryObject} $container Outer &lt;ul> element (or &lt;table> element for ext-table).
 * @property {jQueryObject} $div A jQuery object containing the element used to instantiate the tree widget (`widget.element`)
 * @property {object} data Metadata, i.e. properties that may be passed to `source` in addition to a children array.
 * @property {object} ext Hash of all active plugin instances.
 * @property {FancytreeNode} focusNode Currently focused node or null.
 * @property {FancytreeNode} lastSelectedNode Used to implement selectMode 1 (single select)
 * @property {string} nodeContainerAttrName Property name of FancytreeNode that contains the outer element of single nodes.
 *     Typically "li", but "tr" for table extension.
 * @property {FancytreeOptions} options Current options, i.e. default options + options passed to constructor.
 * @property {FancytreeNode} rootNode Invisible system root node.
 * @property {string} statusClassPropName Property name of FancytreeNode that contains the element which will receive the status classes.
 *     Typically "span", but "tr" for table extension.
 * @property {object} widget Base widget instance.
 */
function Fancytree(widget) {
	this.widget = widget;
	this.$div = widget.element;
	this.options = widget.options;
	if( this.options ) {
		if(  $.isFunction(this.options.lazyload ) && !$.isFunction(this.options.lazyLoad) ) {
			this.options.lazyLoad = function() {
				FT.warn("The 'lazyload' event is deprecated since 2014-02-25. Use 'lazyLoad' (with uppercase L) instead.");
				return widget.options.lazyload.apply(this, arguments);
			};
		}
		if( $.isFunction(this.options.loaderror) ) {
			$.error("The 'loaderror' event was renamed since 2014-07-03. Use 'loadError' (with uppercase E) instead.");
		}
		if( this.options.fx !== undefined ) {
			FT.warn("The 'fx' option was replaced by 'toggleEffect' since 2014-11-30.");
		}
		if( this.options.removeNode !== undefined ) {
			$.error("The 'removeNode' event was replaced by 'modifyChild' since 2.20 (2016-09-10).");
		}
	}
	this.ext = {}; // Active extension instances
	// allow to init tree.data.foo from <div data-foo=''>
	this.data = _getElementDataAsDict(this.$div);
	// TODO: use widget.uuid instead?
	this._id = $.ui.fancytree._nextId++;
	// TODO: use widget.eventNamespace instead?
	this._ns = ".fancytree-" + this._id; // append for namespaced events
	this.activeNode = null;
	this.focusNode = null;
	this._hasFocus = null;
	this._enableUpdate = true;
	// this._dirtyRoots = null;
	this.lastSelectedNode = null;
	this.systemFocusElement = null;
	this.lastQuicksearchTerm = "";
	this.lastQuicksearchTime = 0;

	this.statusClassPropName = "span";
	this.ariaPropName = "li";
	this.nodeContainerAttrName = "li";

	// Remove previous markup if any
	this.$div.find(">ul.fancytree-container").remove();

	// Create a node without parent.
	var fakeParent = { tree: this },
		$ul;
	this.rootNode = new FancytreeNode(fakeParent, {
		title: "root",
		key: "root_" + this._id,
		children: null,
		expanded: true
	});
	this.rootNode.parent = null;

	// Create root markup
	$ul = $("<ul>", {
		"class": "ui-fancytree fancytree-container fancytree-plain"
	}).appendTo(this.$div);
	this.$container = $ul;
	this.rootNode.ul = $ul[0];

	if(this.options.debugLevel == null){
		this.options.debugLevel = FT.debugLevel;
	}
	// // Add container to the TAB chain
	// // See http://www.w3.org/TR/wai-aria-practices/#focus_activedescendant
	// // #577: Allow to set tabindex to "0", "-1" and ""
	// this.$container.attr("tabindex", this.options.tabindex);

	// if( this.options.rtl ) {
	// 	this.$container.attr("DIR", "RTL").addClass("fancytree-rtl");
	// // }else{
	// //	this.$container.attr("DIR", null).removeClass("fancytree-rtl");
	// }
	// if(this.options.aria){
	// 	this.$container.attr("role", "tree");
	// 	if( this.options.selectMode !== 1 ) {
	// 		this.$container.attr("aria-multiselectable", true);
	// 	}
	// }
}


Fancytree.prototype = /** @lends Fancytree# */{
	/* Return a context object that can be re-used for _callHook().
	 * @param {Fancytree | FancytreeNode | EventData} obj
	 * @param {Event} originalEvent
	 * @param {Object} extra
	 * @returns {EventData}
	 */
	_makeHookContext: function(obj, originalEvent, extra) {
		var ctx, tree;
		if(obj.node !== undefined){
			// obj is already a context object
			if(originalEvent && obj.originalEvent !== originalEvent){
				$.error("invalid args");
			}
			ctx = obj;
		}else if(obj.tree){
			// obj is a FancytreeNode
			tree = obj.tree;
			ctx = { node: obj, tree: tree, widget: tree.widget, options: tree.widget.options, originalEvent: originalEvent };
		}else if(obj.widget){
			// obj is a Fancytree
			ctx = { node: null, tree: obj, widget: obj.widget, options: obj.widget.options, originalEvent: originalEvent };
		}else{
			$.error("invalid args");
		}
		if(extra){
			$.extend(ctx, extra);
		}
		return ctx;
	},
	/* Trigger a hook function: funcName(ctx, [...]).
	 *
	 * @param {string} funcName
	 * @param {Fancytree|FancytreeNode|EventData} contextObject
	 * @param {any}  [_extraArgs] optional additional arguments
	 * @returns {any}
	 */
	_callHook: function(funcName, contextObject, _extraArgs) {
		var ctx = this._makeHookContext(contextObject),
			fn = this[funcName],
			args = Array.prototype.slice.call(arguments, 2);
		if(!$.isFunction(fn)){
			$.error("_callHook('" + funcName + "') is not a function");
		}
		args.unshift(ctx);
//		this.debug("_hook", funcName, ctx.node && ctx.node.toString() || ctx.tree.toString(), args);
		return fn.apply(this, args);
	},
	/* Check if current extensions dependencies are met and throw an error if not.
	 *
	 * This method may be called inside the `treeInit` hook for custom extensions.
	 *
	 * @param {string} extension name of the required extension
	 * @param {boolean} [required=true] pass `false` if the extension is optional, but we want to check for order if it is present
	 * @param {boolean} [before] `true` if `name` must be included before this, `false` otherwise (use `null` if order doesn't matter)
	 * @param {string} [message] optional error message (defaults to a descriptve error message)
	 */
	_requireExtension: function(name, required, before, message) {
		before = !!before;
		var thisName = this._local.name,
			extList = this.options.extensions,
			isBefore = $.inArray(name, extList) < $.inArray(thisName, extList),
			isMissing = required && this.ext[name] == null,
			badOrder = !isMissing && before != null && (before !== isBefore);

		_assert(thisName && thisName !== name, "invalid or same name");

		if( isMissing || badOrder ){
			if( !message ){
				if( isMissing || required ){
					message = "'" + thisName + "' extension requires '" + name + "'";
					if( badOrder ){
						message += " to be registered " + (before ? "before" : "after") + " itself";
					}
				}else{
					message = "If used together, `" + name + "` must be registered " + (before ? "before" : "after") + " `" + thisName + "`";
				}
			}
			$.error(message);
			return false;
		}
		return true;
	},
	/** Activate node with a given key and fire focus and activate events.
	 *
	 * A prevously activated node will be deactivated.
	 * If activeVisible option is set, all parents will be expanded as necessary.
	 * Pass key = false, to deactivate the current node only.
	 * @param {string} key
	 * @returns {FancytreeNode} activated node (null, if not found)
	 */
	activateKey: function(key) {
		var node = this.getNodeByKey(key);
		if(node){
			node.setActive();
		}else if(this.activeNode){
			this.activeNode.setActive(false);
		}
		return node;
	},
	/** (experimental) Add child status nodes that indicate 'More...', ....
	 * @param {boolean|object} node optional node definition. Pass `false` to remove all paging nodes.
	 * @param {string} [mode='append'] 'child'|firstChild'
	 * @since 2.15
	 */
	addPagingNode: function(node, mode){
		return this.rootNode.addPagingNode(node, mode);
	},
	/** (experimental) Modify existing data model.
	 *
	 * @param {Array} patchList array of [key, NodePatch] arrays
	 * @returns {$.Promise} resolved, when all patches have been applied
	 * @see TreePatch
	 */
	applyPatch: function(patchList) {
		var dfd, i, p2, key, patch, node,
			patchCount = patchList.length,
			deferredList = [];

		for(i=0; i<patchCount; i++){
			p2 = patchList[i];
			_assert(p2.length === 2, "patchList must be an array of length-2-arrays");
			key = p2[0];
			patch = p2[1];
			node = (key === null) ? this.rootNode : this.getNodeByKey(key);
			if(node){
				dfd = new $.Deferred();
				deferredList.push(dfd);
				node.applyPatch(patch).always(_makeResolveFunc(dfd, node));
			}else{
				this.warn("could not find node with key '" + key + "'");
			}
		}
		// Return a promise that is resolved, when ALL patches were applied
		return $.when.apply($, deferredList).promise();
	},
	/* TODO: implement in dnd extension
	cancelDrag: function() {
		var dd = $.ui.ddmanager.current;
		if(dd){
			dd.cancel();
		}
	},
   */
	/** Remove all nodes.
	 * @since 2.14
	 */
	clear: function(source) {
		this._callHook("treeClear", this);
	},
   /** Return the number of nodes.
	* @returns {integer}
	*/
	count: function() {
		return this.rootNode.countChildren();
	},
	/** Write to browser console if debugLevel >= 2 (prepending tree name)
	 *
	 * @param {*} msg string or object or array of such
	 */
	debug: function(msg){
		if( this.options.debugLevel >= 2 ) {
			Array.prototype.unshift.call(arguments, this.toString());
			consoleApply("log", arguments);
		}
	},
	// TODO: disable()
	// TODO: enable()
	/** Temporarily suppress rendering to improve performance on bulk-updates.
	 *
	 * @param {boolean} flag
	 * @returns {boolean} previous status
	 * @since 2.19
	 */
	enableUpdate: function(flag) {
		flag = ( flag !== false );
		/*jshint -W018 */  // Confusing use of '!'
		if ( !!this._enableUpdate === !!flag ) {
			return flag;
		}
		/*jshint +W018 */
		this._enableUpdate = flag;
		if ( flag ) {
			this.debug("enableUpdate(true): redraw ");  //, this._dirtyRoots);
			this.render();
		} else {
		// 	this._dirtyRoots = null;
			this.debug("enableUpdate(false)...");
		}
		return !flag; // return previous value
	},
	/**Find all nodes that matches condition.
	 *
	 * @param {string | function(node)} match title string to search for, or a
	 *     callback function that returns `true` if a node is matched.
	 * @returns {FancytreeNode[]} array of nodes (may be empty)
	 * @see FancytreeNode#findAll
	 * @since 2.12
	 */
	findAll: function(match) {
		return this.rootNode.findAll(match);
	},
	/**Find first node that matches condition.
	 *
	 * @param {string | function(node)} match title string to search for, or a
	 *     callback function that returns `true` if a node is matched.
	 * @returns {FancytreeNode} matching node or null
	 * @see FancytreeNode#findFirst
	 * @since 2.12
	 */
	findFirst: function(match) {
		return this.rootNode.findFirst(match);
	},
	/** Find the next visible node that starts with `match`, starting at `startNode`
	 * and wrap-around at the end.
	 *
	 * @param {string|function} match
	 * @param {FancytreeNode} [startNode] defaults to first node
	 * @returns {FancytreeNode} matching node or null
	 */
	findNextNode: function(match, startNode, visibleOnly) {
		var stopNode = null,
			parentChildren = startNode.parent.children,
			matchingNode = null,
			walkVisible = function(parent, idx, fn) {
				var i, grandParent,
					parentChildren = parent.children,
					siblingCount = parentChildren.length,
					node = parentChildren[idx];
				// visit node itself
				if( node && fn(node) === false ) {
					return false;
				}
				// visit descendants
				if( node && node.children && node.expanded ) {
					if( walkVisible(node, 0, fn) === false ) {
						return false;
					}
				}
				// visit subsequent siblings
				for( i = idx + 1; i < siblingCount; i++ ) {
					if( walkVisible(parent, i, fn) === false ) {
						return false;
					}
				}
				// visit parent's subsequent siblings
				grandParent = parent.parent;
				if( grandParent ) {
					return walkVisible(grandParent, grandParent.children.indexOf(parent) + 1, fn);
				} else {
					// wrap-around: restart with first node
					return walkVisible(parent, 0, fn);
				}
			};

		match = (typeof match === "string") ? _makeNodeTitleStartMatcher(match) : match;
		startNode = startNode || this.getFirstChild();

		walkVisible(startNode.parent, parentChildren.indexOf(startNode), function(node){
			// Stop iteration if we see the start node a second time
			if( node === stopNode ) {
				return false;
			}
			stopNode = stopNode || node;
			// Ignore nodes hidden by a filter
			if( ! $(node.span).is(":visible") ) {
				node.debug("quicksearch: skipping hidden node");
				return;
			}
			// Test if we found a match, but search for a second match if this
			// was the currently active node
			if( match(node) ) {
				// node.debug("quicksearch match " + node.title, startNode);
				matchingNode = node;
				if( matchingNode !== startNode ) {
					return false;
				}
			}
		});
		return matchingNode;
	},
	// TODO: fromDict
	/**
	 * Generate INPUT elements that can be submitted with html forms.
	 *
	 * In selectMode 3 only the topmost selected nodes are considered, unless
	 * `opts.stopOnParents: false` is passed.
	 *
	 * @example
	 * // Generate input elements for active and selected nodes
	 * tree.generateFormElements();
	 * // Generate input elements selected nodes, using a custom `name` attribute
	 * tree.generateFormElements("cust_sel", false);
	 * // Generate input elements using a custom filter
	 * tree.generateFormElements(true, true, { filter: function(node) {
	 *     return node.isSelected() && node.data.yes;
	 * }});
	 *
	 * @param {boolean | string} [selected=true] Pass false to disable, pass a string to override the field name (default: 'ft_ID[]')
	 * @param {boolean | string} [active=true] Pass false to disable, pass a string to override the field name (default: 'ft_ID_active')
	 * @param {object} [opts] default { filter: null, stopOnParents: true }
	 */
	generateFormElements: function(selected, active, opts) {
		opts = opts || {};

		var nodeList,
			selectedName = (typeof selected === "string") ? selected : "ft_" + this._id + "[]",
			activeName = (typeof active === "string") ? active : "ft_" + this._id + "_active",
			id = "fancytree_result_" + this._id,
			$result = $("#" + id),
			stopOnParents = this.options.selectMode === 3 && opts.stopOnParents !== false;

		if($result.length){
			$result.empty();
		}else{
			$result = $("<div>", {
				id: id
			}).hide().insertAfter(this.$container);
		}
		if(active !== false && this.activeNode){
			$result.append($("<input>", {
				type: "radio",
				name: activeName,
				value: this.activeNode.key,
				checked: true
			}));
		}
		function _appender( node ) {
			$result.append($("<input>", {
				type: "checkbox",
				name: selectedName,
				value: node.key,
				checked: true
			}));
		}
		if ( opts.filter ) {
			this.visit(function(node) {
				var res = opts.filter(node);
				if( res === "skip" ) { return res; }
				if ( res !== false ) {
					_appender(node);
				}
			});
		} else if ( selected !== false ) {
			nodeList = this.getSelectedNodes(stopOnParents);
			$.each(nodeList, function(idx, node) {
				_appender(node);
			});
		}
	},
	/**
	 * Return the currently active node or null.
	 * @returns {FancytreeNode}
	 */
	getActiveNode: function() {
		return this.activeNode;
	},
	/** Return the first top level node if any (not the invisible root node).
	 * @returns {FancytreeNode | null}
	 */
	getFirstChild: function() {
		return this.rootNode.getFirstChild();
	},
	/**
	 * Return node that has keyboard focus or null.
	 * @returns {FancytreeNode}
	 */
	getFocusNode: function() {
		return this.focusNode;
	},
	/**
	 * Return node with a given key or null if not found.
	 * @param {string} key
	 * @param {FancytreeNode} [searchRoot] only search below this node
	 * @returns {FancytreeNode | null}
	 */
	getNodeByKey: function(key, searchRoot) {
		// Search the DOM by element ID (assuming this is faster than traversing all nodes).
		// $("#...") has problems, if the key contains '.', so we use getElementById()
		var el, match;
		if(!searchRoot){
			el = document.getElementById(this.options.idPrefix + key);
			if( el ){
				return el.ftnode ? el.ftnode : null;
			}
		}
		// Not found in the DOM, but still may be in an unrendered part of tree
		// TODO: optimize with specialized loop
		// TODO: consider keyMap?
		searchRoot = searchRoot || this.rootNode;
		match = null;
		searchRoot.visit(function(node){
//            window.console.log("getNodeByKey(" + key + "): ", node.key);
			if(node.key === key) {
				match = node;
				return false;
			}
		}, true);
		return match;
	},
	/** Return the invisible system root node.
	 * @returns {FancytreeNode}
	 */
	getRootNode: function() {
		return this.rootNode;
	},
	/**
	 * Return an array of selected nodes.
	 * @param {boolean} [stopOnParents=false] only return the topmost selected
	 *     node (useful with selectMode 3)
	 * @returns {FancytreeNode[]}
	 */
	getSelectedNodes: function(stopOnParents) {
		return this.rootNode.getSelectedNodes(stopOnParents);
	},
	/** Return true if the tree control has keyboard focus
	 * @returns {boolean}
	 */
	hasFocus: function(){
		return !!this._hasFocus;
	},
	/** Write to browser console if debugLevel >= 1 (prepending tree name)
	 * @param {*} msg string or object or array of such
	 */
	info: function(msg){
		if( this.options.debugLevel >= 1 ) {
			Array.prototype.unshift.call(arguments, this.toString());
			consoleApply("info", arguments);
		}
	},
/*
	TODO: isInitializing: function() {
		return ( this.phase=="init" || this.phase=="postInit" );
	},
	TODO: isReloading: function() {
		return ( this.phase=="init" || this.phase=="postInit" ) && this.options.persist && this.persistence.cookiesFound;
	},
	TODO: isUserEvent: function() {
		return ( this.phase=="userEvent" );
	},
*/

	/**
	 * Make sure that a node with a given ID is loaded, by traversing - and
	 * loading - its parents. This method is ment for lazy hierarchies.
	 * A callback is executed for every node as we go.
	 * @example
	 * tree.loadKeyPath("/_3/_23/_26/_27", function(node, status){
	 *   if(status === "loaded") {
	 *     console.log("loaded intermiediate node " + node);
	 *   }else if(status === "ok") {
	 *     node.activate();
	 *   }
	 * });
	 *
	 * @param {string | string[]} keyPathList one or more key paths (e.g. '/3/2_1/7')
	 * @param {function} callback callback(node, status) is called for every visited node ('loading', 'loaded', 'ok', 'error')
	 * @returns {$.Promise}
	 */
	loadKeyPath: function(keyPathList, callback, _rootNode) {
		var deferredList, dfd, i, path, key, loadMap, node, root, segList,
			sep = this.options.keyPathSeparator,
			self = this;

		callback = callback || $.noop;
		if(!$.isArray(keyPathList)){
			keyPathList = [keyPathList];
		}
		// Pass 1: handle all path segments for nodes that are already loaded
		// Collect distinct top-most lazy nodes in a map
		loadMap = {};

		for(i=0; i<keyPathList.length; i++){
			root = _rootNode || this.rootNode;
			path = keyPathList[i];
			// strip leading slash
			if(path.charAt(0) === sep){
				path = path.substr(1);
			}
			// traverse and strip keys, until we hit a lazy, unloaded node
			segList = path.split(sep);
			while(segList.length){
				key = segList.shift();
//                node = _findDirectChild(root, key);
				node = root._findDirectChild(key);
				if(!node){
					this.warn("loadKeyPath: key not found: " + key + " (parent: " + root + ")");
					callback.call(this, key, "error");
					break;
				}else if(segList.length === 0){
					callback.call(this, node, "ok");
					break;
				}else if(!node.lazy || (node.hasChildren() !== undefined )){
					callback.call(this, node, "loaded");
					root = node;
				}else{
					callback.call(this, node, "loaded");
//                    segList.unshift(key);
					if(loadMap[key]){
						loadMap[key].push(segList.join(sep));
					}else{
						loadMap[key] = [segList.join(sep)];
					}
					break;
				}
			}
		}
//        alert("loadKeyPath: loadMap=" + JSON.stringify(loadMap));
		// Now load all lazy nodes and continue itearation for remaining paths
		deferredList = [];
		// Avoid jshint warning 'Don't make functions within a loop.':
		function __lazyload(key, node, dfd){
			callback.call(self, node, "loading");
			node.load().done(function(){
				self.loadKeyPath.call(self, loadMap[key], callback, node).always(_makeResolveFunc(dfd, self));
			}).fail(function(errMsg){
				self.warn("loadKeyPath: error loading: " + key + " (parent: " + root + ")");
				callback.call(self, node, "error");
				dfd.reject();
			});
		}
		for(key in loadMap){
			node = root._findDirectChild(key);
			if (node == null) {  // #576
				node = self.getNodeByKey(key);
			}
			dfd = new $.Deferred();
			deferredList.push(dfd);
			__lazyload(key, node, dfd);
		}
		// Return a promise that is resolved, when ALL paths were loaded
		return $.when.apply($, deferredList).promise();
	},
	/** Re-fire beforeActivate, activate, and (optional) focus events.
	 * Calling this method in the `init` event, will activate the node that
	 * was marked 'active' in the source data, and optionally set the keyboard
	 * focus.
	 * @param [setFocus=false]
	 */
	reactivate: function(setFocus) {
		var res,
			node = this.activeNode;

		if( !node ) {
			return _getResolvedPromise();
		}
		this.activeNode = null; // Force re-activating
		res = node.setActive(true, {noFocus: true});
		if( setFocus ){
			node.setFocus();
		}
		return res;
	},
	/** Reload tree from source and return a promise.
	 * @param [source] optional new source (defaults to initial source data)
	 * @returns {$.Promise}
	 */
	reload: function(source) {
		this._callHook("treeClear", this);
		return this._callHook("treeLoad", this, source);
	},
	/**Render tree (i.e. create DOM elements for all top-level nodes).
	 * @param {boolean} [force=false] create DOM elemnts, even if parent is collapsed
	 * @param {boolean} [deep=false]
	 */
	render: function(force, deep) {
		return this.rootNode.render(force, deep);
	},
	// TODO: selectKey: function(key, select)
	// TODO: serializeArray: function(stopOnParents)
	/**
	 * @param {boolean} [flag=true]
	 */
	setFocus: function(flag) {
		return this._callHook("treeSetFocus", this, flag);
	},
	/**
	 * Return all nodes as nested list of {@link NodeData}.
	 *
	 * @param {boolean} [includeRoot=false] Returns the hidden system root node (and its children)
	 * @param {function} [callback] callback(dict, node) is called for every node, in order to allow modifications
	 * @returns {Array | object}
	 * @see FancytreeNode#toDict
	 */
	toDict: function(includeRoot, callback){
		var res = this.rootNode.toDict(true, callback);
		return includeRoot ? res : res.children;
	},
	/* Implicitly called for string conversions.
	 * @returns {string}
	 */
	toString: function(){
		return "<Fancytree(#" + this._id + ")>";
	},
	/* _trigger a widget event with additional node ctx.
	 * @see EventData
	 */
	_triggerNodeEvent: function(type, node, originalEvent, extra) {
//		this.debug("_trigger(" + type + "): '" + ctx.node.title + "'", ctx);
		var ctx = this._makeHookContext(node, originalEvent, extra),
			res = this.widget._trigger(type, originalEvent, ctx);
		if(res !== false && ctx.result !== undefined){
			return ctx.result;
		}
		return res;
	},
	/* _trigger a widget event with additional tree data. */
	_triggerTreeEvent: function(type, originalEvent, extra) {
//		this.debug("_trigger(" + type + ")", ctx);
		var ctx = this._makeHookContext(this, originalEvent, extra),
			res = this.widget._trigger(type, originalEvent, ctx);

		if(res !== false && ctx.result !== undefined){
			return ctx.result;
		}
		return res;
	},
	/** Call fn(node) for all nodes.
	 *
	 * @param {function} fn the callback function.
	 *     Return false to stop iteration, return "skip" to skip this node and children only.
	 * @returns {boolean} false, if the iterator was stopped.
	 */
	visit: function(fn) {
		return this.rootNode.visit(fn, false);
	},
	/** Write warning to browser console (prepending tree info)
	 *
	 * @param {*} msg string or object or array of such
	 */
	warn: function(msg){
		Array.prototype.unshift.call(arguments, this.toString());
		consoleApply("warn", arguments);
	}
};

/**
 * These additional methods of the {@link Fancytree} class are 'hook functions'
 * that can be used and overloaded by extensions.
 * (See <a href="https://github.com/mar10/fancytree/wiki/TutorialExtensions">writing extensions</a>.)
 * @mixin Fancytree_Hooks
 */
$.extend(Fancytree.prototype,
	/** @lends Fancytree_Hooks# */
	{
	/** Default handling for mouse click events.
	 *
	 * @param {EventData} ctx
	 */
	nodeClick: function(ctx) {
		var activate, expand,
			// event = ctx.originalEvent,
			targetType = ctx.targetType,
			node = ctx.node;

//	    this.debug("ftnode.onClick(" + event.type + "): ftnode:" + this + ", button:" + event.button + ", which: " + event.which, ctx);
		// TODO: use switch
		// TODO: make sure clicks on embedded <input> doesn't steal focus (see table sample)
		if( targetType === "expander" ) {
			if( node.isLoading() ) {
				// #495: we probably got a click event while a lazy load is pending.
				// The 'expanded' state is not yet set, so 'toggle' would expand
				// and trigger lazyLoad again.
				// It would be better to allow to collapse/expand the status node
				// while loading (instead of ignoring), but that would require some
				// more work.
				node.debug("Got 2nd click while loading: ignored");
				return;
			}
			// Clicking the expander icon always expands/collapses
			this._callHook("nodeToggleExpanded", ctx);

		} else if( targetType === "checkbox" ) {
			// Clicking the checkbox always (de)selects
			this._callHook("nodeToggleSelected", ctx);
			if( ctx.options.focusOnSelect ) { // #358
				this._callHook("nodeSetFocus", ctx, true);
			}

		} else {
			// Honor `clickFolderMode` for
			expand = false;
			activate = true;
			if( node.folder ) {
				switch( ctx.options.clickFolderMode ) {
				case 2: // expand only
					expand = true;
					activate = false;
					break;
				case 3: // expand and activate
					activate = true;
					expand = true; //!node.isExpanded();
					break;
				// else 1 or 4: just activate
				}
			}
			if( activate ) {
				this.nodeSetFocus(ctx);
				this._callHook("nodeSetActive", ctx, true);
			}
			if( expand ) {
				if(!activate){
//                    this._callHook("nodeSetFocus", ctx);
				}
//				this._callHook("nodeSetExpanded", ctx, true);
				this._callHook("nodeToggleExpanded", ctx);
			}
		}
		// Make sure that clicks stop, otherwise <a href='#'> jumps to the top
		// if(event.target.localName === "a" && event.target.className === "fancytree-title"){
		// 	event.preventDefault();
		// }
		// TODO: return promise?
	},
	/** Collapse all other  children of same parent.
	 *
	 * @param {EventData} ctx
	 * @param {object} callOpts
	 */
	nodeCollapseSiblings: function(ctx, callOpts) {
		// TODO: return promise?
		var ac, i, l,
			node = ctx.node;

		if( node.parent ){
			ac = node.parent.children;
			for (i=0, l=ac.length; i<l; i++) {
				if ( ac[i] !== node && ac[i].expanded ){
					this._callHook("nodeSetExpanded", ac[i], false, callOpts);
				}
			}
		}
	},
	/** Default handling for mouse douleclick events.
	 * @param {EventData} ctx
	 */
	nodeDblclick: function(ctx) {
		// TODO: return promise?
		if( ctx.targetType === "title" && ctx.options.clickFolderMode === 4) {
//			this.nodeSetFocus(ctx);
//			this._callHook("nodeSetActive", ctx, true);
			this._callHook("nodeToggleExpanded", ctx);
		}
		// TODO: prevent text selection on dblclicks
		if( ctx.targetType === "title" ) {
			ctx.originalEvent.preventDefault();
		}
	},
	/** Default handling for mouse keydown events.
	 *
	 * NOTE: this may be called with node == null if tree (but no node) has focus.
	 * @param {EventData} ctx
	 */
	nodeKeydown: function(ctx) {
		// TODO: return promise?
		var matchNode, stamp, res, focusNode,
			event = ctx.originalEvent,
			node = ctx.node,
			tree = ctx.tree,
			opts = ctx.options,
			which = event.which,
			whichChar = String.fromCharCode(which),
			clean = !(event.altKey || event.ctrlKey || event.metaKey || event.shiftKey),
			$target = $(event.target),
			handled = true,
			activate = !(event.ctrlKey || !opts.autoActivate );

		// (node || FT).debug("ftnode.nodeKeydown(" + event.type + "): ftnode:" + this + ", charCode:" + event.charCode + ", keyCode: " + event.keyCode + ", which: " + event.which);
		// FT.debug("eventToString", which, '"' + String.fromCharCode(which) + '"', '"' + FT.eventToString(event) + '"');

		// Set focus to active (or first node) if no other node has the focus yet
		if( !node ){
			focusNode = (this.getActiveNode() || this.getFirstChild());
			if (focusNode){
				focusNode.setFocus();
				node = ctx.node = this.focusNode;
				node.debug("Keydown force focus on active node");
			}
		}

		if( opts.quicksearch && clean && /\w/.test(whichChar) &&
				!SPECIAL_KEYCODES[which] &&  // #659
				!$target.is(":input:enabled") ) {
			// Allow to search for longer streaks if typed in quickly
			stamp = new Date().getTime();
			if( stamp - tree.lastQuicksearchTime > 500 ) {
				tree.lastQuicksearchTerm = "";
			}
			tree.lastQuicksearchTime = stamp;
			tree.lastQuicksearchTerm += whichChar;
			// tree.debug("quicksearch find", tree.lastQuicksearchTerm);
			matchNode = tree.findNextNode(tree.lastQuicksearchTerm, tree.getActiveNode());
			if( matchNode ) {
				matchNode.setActive();
			}
			event.preventDefault();
			return;
		}
		switch( FT.eventToString(event) ) {
			case "+":
			case "=": // 187: '+' @ Chrome, Safari
				tree.nodeSetExpanded(ctx, true);
				break;
			case "-":
				tree.nodeSetExpanded(ctx, false);
				break;
			case "space":
				if( node.isPagingNode() ) {
					tree._triggerNodeEvent("clickPaging", ctx, event);
				} else if(opts.checkbox){
					tree.nodeToggleSelected(ctx);
				}else{
					tree.nodeSetActive(ctx, true);
				}
				break;
			case "return":
				tree.nodeSetActive(ctx, true);
				break;
			case "home":
			case "end":
			case "backspace":
			case "left":
			case "right":
			case "up":
			case "down":
				res = node.navigate(event.which, activate, true);
				break;
			default:
				handled = false;
		}
		if(handled){
			event.preventDefault();
		}
	},


	// /** Default handling for mouse keypress events. */
	// nodeKeypress: function(ctx) {
	//     var event = ctx.originalEvent;
	// },

	// /** Trigger lazyLoad event (async). */
	// nodeLazyLoad: function(ctx) {
	//     var node = ctx.node;
	//     if(this._triggerNodeEvent())
	// },
	/** Load child nodes (async).
	 *
	 * @param {EventData} ctx
	 * @param {object[]|object|string|$.Promise|function} source
	 * @returns {$.Promise} The deferred will be resolved as soon as the (ajax)
	 *     data was rendered.
	 */
	nodeLoadChildren: function(ctx, source) {
		var ajax, delay, dfd,
			tree = ctx.tree,
			node = ctx.node,
			requestId = new Date().getTime();

		if($.isFunction(source)){
			source = source.call(tree, {type: "source"}, ctx);
			_assert(!$.isFunction(source), "source callback must not return another function");
		}
		if(source.url){
			if( node._requestId ) {
				node.warn("Recursive load request #" + requestId + " while #" + node._requestId + " is pending.");
			// } else {
			// 	node.debug("Send load request #" + requestId);
			}
			// `source` is an Ajax options object
			ajax = $.extend({}, ctx.options.ajax, source);
			node._requestId = requestId;
			if(ajax.debugDelay){
				// simulate a slow server
				delay = ajax.debugDelay;
				if($.isArray(delay)){ // random delay range [min..max]
					delay = delay[0] + Math.random() * (delay[1] - delay[0]);
				}
				node.warn("nodeLoadChildren waiting debugDelay " + Math.round(delay) + " ms ...");
				ajax.debugDelay = false;
				dfd = $.Deferred(function (dfd) {
					setTimeout(function () {
						$.ajax(ajax)
							.done(function () {	dfd.resolveWith(this, arguments); })
							.fail(function () {	dfd.rejectWith(this, arguments); });
					}, delay);
				});
			}else{
				dfd = $.ajax(ajax);
			}

			// Defer the deferred: we want to be able to reject, even if ajax
			// resolved ok.
			source = new $.Deferred();
			dfd.done(function (data, textStatus, jqXHR) {
				var errorObj, res;

				if((this.dataType === "json" || this.dataType === "jsonp") && typeof data === "string"){
					$.error("Ajax request returned a string (did you get the JSON dataType wrong?).");
				}
				if( node._requestId && node._requestId > requestId ) {
					// The expected request time stamp is later than `requestId`
					// (which was kept as as closure variable to this handler function)
					// node.warn("Ignored load response for obsolete request #" + requestId + " (expected #" + node._requestId + ")");
					source.rejectWith(this, [RECURSIVE_REQUEST_ERROR]);
					return;
				// } else {
				// 	node.debug("Response returned for load request #" + requestId);
				}
				// postProcess is similar to the standard ajax dataFilter hook,
				// but it is also called for JSONP
				if( ctx.options.postProcess ){
					try {
						res = tree._triggerNodeEvent("postProcess", ctx, ctx.originalEvent, {
							response: data, error: null, dataType: this.dataType
						});
					} catch(e) {
						res = { error: e, message: "" + e, details: "postProcess failed"};
					}
					if( res.error ) {
						errorObj = $.isPlainObject(res.error) ? res.error : {message: res.error};
						errorObj = tree._makeHookContext(node, null, errorObj);
						source.rejectWith(this, [errorObj]);
						return;
					}
					data = $.isArray(res) ? res : data;

				} else if (data && data.hasOwnProperty("d") && ctx.options.enableAspx ) {
					// Process ASPX WebMethod JSON object inside "d" property
					data = (typeof data.d === "string") ? $.parseJSON(data.d) : data.d;
				}
				source.resolveWith(this, [data]);
			}).fail(function (jqXHR, textStatus, errorThrown) {
				var errorObj = tree._makeHookContext(node, null, {
					error: jqXHR,
					args: Array.prototype.slice.call(arguments),
					message: errorThrown,
					details: jqXHR.status + ": " + errorThrown
				});
				source.rejectWith(this, [errorObj]);
			});
		}
		// #383: accept and convert ECMAScript 6 Promise
		if( $.isFunction(source.then) && $.isFunction(source["catch"]) ) {
			dfd = source;
			source = new $.Deferred();
			dfd.then(function(value){
				source.resolve(value);
			}, function(reason){
				source.reject(reason);
			});
		}
		if($.isFunction(source.promise)){
			// `source` is a deferred, i.e. ajax request
			// _assert(!node.isLoading(), "recursive load");
			tree.nodeSetStatus(ctx, "loading");

			source.done(function (children) {
				tree.nodeSetStatus(ctx, "ok");
				node._requestId = null;
			}).fail(function(error){
				var ctxErr;

				if ( error === RECURSIVE_REQUEST_ERROR ) {
					node.warn("Ignored response for obsolete load request #" + requestId + " (expected #" + node._requestId + ")");
					return;
				} else if (error.node && error.error && error.message) {
					// error is already a context object
					ctxErr = error;
				} else {
					ctxErr = tree._makeHookContext(node, null, {
						error: error, // it can be jqXHR or any custom error
						args: Array.prototype.slice.call(arguments),
						message: error ? (error.message || error.toString()) : ""
					});
					if( ctxErr.message === "[object Object]" ) {
						ctxErr.message = "";
					}
				}
				node.warn("Load children failed (" + ctxErr.message + ")", ctxErr);
				if( tree._triggerNodeEvent("loadError", ctxErr, null) !== false ) {
					tree.nodeSetStatus(ctx, "error", ctxErr.message, ctxErr.details);
				}
			});
		}
		// $.when(source) resolves also for non-deferreds
		return $.when(source).done(function(children){
			var metaData;

			if( $.isPlainObject(children) ){
				// We got {foo: 'abc', children: [...]}
				// Copy extra properties to tree.data.foo
				_assert(node.isRootNode(), "source may only be an object for root nodes (expecting an array of child objects otherwise)");
				_assert($.isArray(children.children), "if an object is passed as source, it must contain a 'children' array (all other properties are added to 'tree.data')");
				metaData = children;
				children = children.children;
				delete metaData.children;
				$.extend(tree.data, metaData);
			}
			_assert($.isArray(children), "expected array of children");
			node._setChildren(children);
			// trigger fancytreeloadchildren
			tree._triggerNodeEvent("loadChildren", node);
		});
	},
	/** [Not Implemented]  */
	nodeLoadKeyPath: function(ctx, keyPathList) {
		// TODO: implement and improve
		// http://code.google.com/p/dynatree/issues/detail?id=222
	},
	/**
	 * Remove a single direct child of ctx.node.
	 * @param {EventData} ctx
	 * @param {FancytreeNode} childNode dircect child of ctx.node
	 */
	nodeRemoveChild: function(ctx, childNode) {
		var idx,
			node = ctx.node,
			// opts = ctx.options,
			subCtx = $.extend({}, ctx, {node: childNode}),
			children = node.children;

		// FT.debug("nodeRemoveChild()", node.toString(), childNode.toString());

		if( children.length === 1 ) {
			_assert(childNode === children[0], "invalid single child");
			return this.nodeRemoveChildren(ctx);
		}
		if( this.activeNode && (childNode === this.activeNode || this.activeNode.isDescendantOf(childNode))){
			this.activeNode.setActive(false); // TODO: don't fire events
		}
		if( this.focusNode && (childNode === this.focusNode || this.focusNode.isDescendantOf(childNode))){
			this.focusNode = null;
		}
		// TODO: persist must take care to clear select and expand cookies
		this.nodeRemoveMarkup(subCtx);
		this.nodeRemoveChildren(subCtx);
		idx = $.inArray(childNode, children);
		_assert(idx >= 0, "invalid child");
		// Notify listeners
		node.triggerModifyChild("remove", childNode);
		// Unlink to support GC
		childNode.visit(function(n){
			n.parent = null;
		}, true);
		this._callHook("treeRegisterNode", this, false, childNode);
		// remove from child list
		children.splice(idx, 1);
	},
	/**Remove HTML markup for all descendents of ctx.node.
	 * @param {EventData} ctx
	 */
	nodeRemoveChildMarkup: function(ctx) {
		var node = ctx.node;

		// FT.debug("nodeRemoveChildMarkup()", node.toString());
		// TODO: Unlink attr.ftnode to support GC
		if(node.ul){
			if( node.isRootNode() ) {
				$(node.ul).empty();
			} else {
				$(node.ul).remove();
				node.ul = null;
			}
			node.visit(function(n){
				n.li = n.ul = null;
			});
		}
	},
	/**Remove all descendants of ctx.node.
	* @param {EventData} ctx
	*/
	nodeRemoveChildren: function(ctx) {
		var subCtx,
			tree = ctx.tree,
			node = ctx.node,
			children = node.children;
			// opts = ctx.options;

		// FT.debug("nodeRemoveChildren()", node.toString());
		if(!children){
			return;
		}
		if( this.activeNode && this.activeNode.isDescendantOf(node)){
			this.activeNode.setActive(false); // TODO: don't fire events
		}
		if( this.focusNode && this.focusNode.isDescendantOf(node)){
			this.focusNode = null;
		}
		// TODO: persist must take care to clear select and expand cookies
		this.nodeRemoveChildMarkup(ctx);
		// Unlink children to support GC
		// TODO: also delete this.children (not possible using visit())
		subCtx = $.extend({}, ctx);
		node.triggerModifyChild("remove", null);
		node.visit(function(n){
			n.parent = null;
			tree._callHook("treeRegisterNode", tree, false, n);
		});
		if( node.lazy ){
			// 'undefined' would be interpreted as 'not yet loaded' for lazy nodes
			node.children = [];
		} else{
			node.children = null;
		}
		if( !node.isRootNode() ) {
			node.expanded = false;  // #449, #459
		}
		this.nodeRenderStatus(ctx);
	},
	/**Remove HTML markup for ctx.node and all its descendents.
	 * @param {EventData} ctx
	 */
	nodeRemoveMarkup: function(ctx) {
		var node = ctx.node;
		// FT.debug("nodeRemoveMarkup()", node.toString());
		// TODO: Unlink attr.ftnode to support GC
		if(node.li){
			$(node.li).remove();
			node.li = null;
		}
		this.nodeRemoveChildMarkup(ctx);
	},
	/**
	 * Create `&lt;li>&lt;span>..&lt;/span> .. &lt;/li>` tags for this node.
	 *
	 * This method takes care that all HTML markup is created that is required
	 * to display this node in its current state.
	 *
	 * Call this method to create new nodes, or after the strucuture
	 * was changed (e.g. after moving this node or adding/removing children)
	 * nodeRenderTitle() and nodeRenderStatus() are implied.
	 *
	 * &lt;code>
	 * &lt;li id='KEY' ftnode=NODE>
	 *     &lt;span class='fancytree-node fancytree-expanded fancytree-has-children fancytree-lastsib fancytree-exp-el fancytree-ico-e'>
	 *         &lt;span class="fancytree-expander">&lt;/span>
	 *         &lt;span class="fancytree-checkbox">&lt;/span> // only present in checkbox mode
	 *         &lt;span class="fancytree-icon">&lt;/span>
	 *         &lt;a href="#" class="fancytree-title"> Node 1 &lt;/a>
	 *     &lt;/span>
	 *     &lt;ul> // only present if node has children
	 *         &lt;li id='KEY' ftnode=NODE> child1 ... &lt;/li>
	 *         &lt;li id='KEY' ftnode=NODE> child2 ... &lt;/li>
	 *     &lt;/ul>
	 * &lt;/li>
	 * &lt;/code>
	 *
	 * @param {EventData} ctx
	 * @param {boolean} [force=false] re-render, even if html markup was already created
	 * @param {boolean} [deep=false] also render all descendants, even if parent is collapsed
	 * @param {boolean} [collapsed=false] force root node to be collapsed, so we can apply animated expand later
	 */


	nodeRender: function(ctx, force, deep, collapsed, _recursive) {
		/* This method must take care of all cases where the current data mode
		 * (i.e. node hierarchy) does not match the current markup.
		 *
		 * - node was not yet rendered:
		 *   create markup
		 * - node was rendered: exit fast
		 * - children have been added
		 * - children have been removed
		 */
		var childLI, childNode1, childNode2, i, l, next, subCtx,
			node = ctx.node,
			tree = ctx.tree,
			opts = ctx.options,
			aria = opts.aria,
			firstTime = false,
			parent = node.parent,
			isRootNode = !parent,
			children = node.children,
			successorLi = null;
		// FT.debug("nodeRender(" + !!force + ", " + !!deep + ")", node.toString());

		if( tree._enableUpdate === false ) {
			// tree.debug("no render", tree._enableUpdate);
			return;
		}
		if( ! isRootNode && ! parent.ul ) {
			// Calling node.collapse on a deep, unrendered node
			return;
		}
		_assert(isRootNode || parent.ul, "parent UL must exist");

		// Render the node
		if( !isRootNode ){
			// Discard markup on force-mode, or if it is not linked to parent <ul>
			if(node.li && (force || (node.li.parentNode !== node.parent.ul) ) ){
				if( node.li.parentNode === node.parent.ul ){
					// #486: store following node, so we can insert the new markup there later
					successorLi = node.li.nextSibling;
				}else{
					// May happen, when a top-level node was dropped over another
					this.debug("Unlinking " + node + " (must be child of " + node.parent + ")");
				}
//	            this.debug("nodeRemoveMarkup...");
				this.nodeRemoveMarkup(ctx);
			}
			// Create <li><span /> </li>
//			node.debug("render...");
			if( !node.li ) {
//	            node.debug("render... really");
				firstTime = true;
				node.li = document.createElement("li");
				node.li.ftnode = node;

				if( node.key && opts.generateIds ){
					node.li.id = opts.idPrefix + node.key;
				}
				node.span = document.createElement("span");
				node.span.className = "fancytree-node";
				if( aria && !node.tr ) {
					$(node.li).attr("role", "treeitem");
				}
				node.li.appendChild(node.span);

				// Create inner HTML for the <span> (expander, checkbox, icon, and title)
				this.nodeRenderTitle(ctx);

				// Allow tweaking and binding, after node was created for the first time
				if ( opts.createNode ){
					opts.createNode.call(tree, {type: "createNode"}, ctx);
				}
			}else{
//				this.nodeRenderTitle(ctx);
				this.nodeRenderStatus(ctx);
			}
			// Allow tweaking after node state was rendered
			if ( opts.renderNode ){
				opts.renderNode.call(tree, {type: "renderNode"}, ctx);
			}
		}

		// Visit child nodes
		if( children ){
			if( isRootNode || node.expanded || deep === true ) {
				// Create a UL to hold the children
				if( !node.ul ){
					node.ul = document.createElement("ul");
					if((collapsed === true && !_recursive) || !node.expanded){
						// hide top UL, so we can use an animation to show it later
						node.ul.style.display = "none";
					}
					if(aria){
						$(node.ul).attr("role", "group");
					}
					if ( node.li ) { // issue #67
						node.li.appendChild(node.ul);
					} else {
						node.tree.$div.append(node.ul);
					}
				}
				// Add child markup
				for(i=0, l=children.length; i<l; i++) {
					subCtx = $.extend({}, ctx, {node: children[i]});
					this.nodeRender(subCtx, force, deep, false, true);
				}
				// Remove <li> if nodes have moved to another parent
				childLI = node.ul.firstChild;
				while( childLI ){
					childNode2 = childLI.ftnode;
					if( childNode2 && childNode2.parent !== node ) {
						node.debug("_fixParent: remove missing " + childNode2, childLI);
						next = childLI.nextSibling;
						childLI.parentNode.removeChild(childLI);
						childLI = next;
					}else{
						childLI = childLI.nextSibling;
					}
				}
				// Make sure, that <li> order matches node.children order.
				childLI = node.ul.firstChild;
				for(i=0, l=children.length-1; i<l; i++) {
					childNode1 = children[i];
					childNode2 = childLI.ftnode;
					if( childNode1 !== childNode2 ) {
						// node.debug("_fixOrder: mismatch at index " + i + ": " + childNode1 + " != " + childNode2);
						node.ul.insertBefore(childNode1.li, childNode2.li);
					} else {
						childLI = childLI.nextSibling;
					}
				}
			}
		}else{
			// No children: remove markup if any
			if( node.ul ){
//				alert("remove child markup for " + node);
				this.warn("remove child markup for " + node);
				this.nodeRemoveChildMarkup(ctx);
			}
		}
		if( !isRootNode ){
			// Update element classes according to node state
			// this.nodeRenderStatus(ctx);
			// Finally add the whole structure to the DOM, so the browser can render
			if( firstTime ){
				// #486: successorLi is set, if we re-rendered (i.e. discarded)
				// existing markup, which  we want to insert at the same position.
				// (null is equivalent to append)
//				parent.ul.appendChild(node.li);
				parent.ul.insertBefore(node.li, successorLi);
			}
		}
		},

		
	/** Create HTML inside the node's outer &lt;span> (i.e. expander, checkbox,
	 * icon, and title).
	 *
	 * nodeRenderStatus() is implied.
	 * @param {EventData} ctx
	 * @param {string} [title] optinal new title
	 */
	nodeRenderTitle: function(ctx, title) {
		// set node connector images, links and text
		var checkbox, className, icon, nodeTitle, role, tabindex, tooltip,
			node = ctx.node,
			tree = ctx.tree,
			opts = ctx.options,
			aria = opts.aria,
			level = node.getLevel(),
			ares = [];

		if(title !== undefined){
			node.title = title;
		}
		if ( !node.span || tree._enableUpdate === false ) {
			// Silently bail out if node was not rendered yet, assuming
			// node.render() will be called as the node becomes visible
			return;
		}
		// Connector (expanded, expandable or simple)
		role = (aria && node.hasChildren() !== false) ? " role='button'" : "";
		if( level < opts.minExpandLevel ) {
			if( !node.lazy ) {
				node.expanded = true;
			}
			if(level > 1){
				ares.push("<span " + role + " class='fancytree-expander fancytree-expander-fixed'></span>");
			}
			// .. else (i.e. for root level) skip expander/connector alltogether
		} else {
			ares.push("<span " + role + " class='fancytree-expander'></span>");
		}
		// Checkbox mode
		checkbox = FT.evalOption("checkbox", node, node, opts, false);

		if( checkbox && !node.isStatusNode() ) {
			role = aria ? " role='checkbox'" : "";
			className = "fancytree-checkbox";
			if( checkbox === "radio" || (node.parent && node.parent.radiogroup) ) {
				className += " fancytree-radio";
			}
			ares.push("<span " + role + " class='" + className + "'></span>");
		}
		// Folder or doctype icon
		if( node.data.iconClass !== undefined ) {  // 2015-11-16
			// Handle / warn about backward compatibility
			if( node.icon ) {
				$.error("'iconClass' node option is deprecated since v2.14.0: use 'icon' only instead");
			} else {
				node.warn("'iconClass' node option is deprecated since v2.14.0: use 'icon' instead");
				node.icon = node.data.iconClass;
			}
		}
		// If opts.icon is a callback and returns something other than undefined, use that
		// else if node.icon is a boolean or string, use that
		// else if opts.icon is a boolean or string, use that
		// else show standard icon (which may be different for folders or documents)
		icon = FT.evalOption("icon", node, node, opts, true);
		if( typeof icon !== "boolean" ) {
			// icon is defined, but not true/false: must be a string
			icon = "" + icon;
		}
		if( icon !== false ) {
			role = aria ? " role='presentation'" : "";
			if ( typeof icon === "string" ) {
				if( TEST_IMG.test(icon) ) {
					// node.icon is an image url. Prepend imagePath
					icon = (icon.charAt(0) === "/") ? icon : ((opts.imagePath || "") + icon);
					ares.push("<img src='" + icon + "' class='fancytree-icon' alt='' />");
				} else {
					ares.push("<span " + role + " class='fancytree-custom-icon " + icon +  "'></span>");
				}
			} else {
				// standard icon: theme css will take care of this
				ares.push("<span " + role + " class='fancytree-icon'></span>");
			}
		}
		// Node title
		nodeTitle = "";
		if ( opts.renderTitle ){
			nodeTitle = opts.renderTitle.call(tree, {type: "renderTitle"}, ctx) || "";
		}
		if ( !nodeTitle ) {
			tooltip = FT.evalOption("tooltip", node, node, opts, null);
			if( tooltip === true ) {
				tooltip = node.title;
			}
			// if( node.tooltip ) {
			// 	tooltip = node.tooltip;
			// } else if ( opts.tooltip ) {
			// 	tooltip = opts.tooltip === true ? node.title : opts.tooltip.call(tree, node);
			// }
			tooltip = tooltip ? " title='" + _escapeTooltip(tooltip) + "'" : "";
			tabindex = opts.titlesTabbable ? " tabindex='0'" : "";

			nodeTitle = "<span class='fancytree-title'" +
				tooltip + tabindex + ">" +
				(opts.escapeTitles ? _escapeHtml(node.title) : node.title) +
				"</span>";
			//nodeTitle = "<span class='fancytree-title'" +
			//	tooltip + tabindex + ">" +
			//	(opts.escapeTitles ? _escapeHtml(minString(node.title)) : minString(node.title)) +
			//	"</span>";
		}
		ares.push(nodeTitle);
		// Note: this will trigger focusout, if node had the focus
		//$(node.span).html(ares.join("")); // it will cleanup the jQuery data currently associated with SPAN (if any), but it executes more slowly
		node.span.innerHTML = ares.join("");
		// Update CSS classes
		this.nodeRenderStatus(ctx);
		if ( opts.enhanceTitle ){
			ctx.$title = $(">span.fancytree-title", node.span);
			nodeTitle = opts.enhanceTitle.call(tree, {type: "enhanceTitle"}, ctx) || "";
		}
		},

		

	/** Update element classes according to node state.
	 * @param {EventData} ctx
	 */
	nodeRenderStatus: function(ctx) {
		// Set classes for current status
		var $ariaElem,
			node = ctx.node,
			tree = ctx.tree,
			opts = ctx.options,
//			nodeContainer = node[tree.nodeContainerAttrName],
			hasChildren = node.hasChildren(),
			isLastSib = node.isLastSibling(),
			aria = opts.aria,
			cn = opts._classNames,
			cnList = [],
			statusElem = node[tree.statusClassPropName];

		if( !statusElem || tree._enableUpdate === false ){
			// if this function is called for an unrendered node, ignore it (will be updated on nect render anyway)
			return;
		}
		if( aria ) {
			$ariaElem = $(node.tr || node.li);
		}
		// Build a list of class names that we will add to the node <span>
		cnList.push(cn.node);
		if( tree.activeNode === node ){
			cnList.push(cn.active);
//			$(">span.fancytree-title", statusElem).attr("tabindex", "0");
//			tree.$container.removeAttr("tabindex");
		// }else{
//			$(">span.fancytree-title", statusElem).removeAttr("tabindex");
//			tree.$container.attr("tabindex", "0");
		}
		if( tree.focusNode === node ){
			cnList.push(cn.focused);
		}
		if( node.expanded ){
			cnList.push(cn.expanded);
		}
		if( aria ){
			if (hasChildren !== false) {
				$ariaElem.attr("aria-expanded", Boolean(node.expanded));
			}
			else {
				$ariaElem.removeAttr("aria-expanded");
			}
		}
		if( node.folder ){
			cnList.push(cn.folder);
		}
		if( hasChildren !== false ){
			cnList.push(cn.hasChildren);
		}
		// TODO: required?
		if( isLastSib ){
			cnList.push(cn.lastsib);
		}
		if( node.lazy && node.children == null ){
			cnList.push(cn.lazy);
		}
		if( node.partload ){
			cnList.push(cn.partload);
		}
		if( node.partsel ){
			cnList.push(cn.partsel);
		}
		if( FT.evalOption("unselectable", node, node, opts, false) ){
			cnList.push(cn.unselectable);
		}
		if( node._isLoading ){
			cnList.push(cn.loading);
		}
		if( node._error ){
			cnList.push(cn.error);
		}
		if( node.statusNodeType ) {
			cnList.push(cn.statusNodePrefix + node.statusNodeType);
		}
		if( node.selected ){
			cnList.push(cn.selected);
			if(aria){
				$ariaElem.attr("aria-selected", true);
			}
		}else if(aria){
			$ariaElem.attr("aria-selected", false);
		}
		if( node.extraClasses ){
			cnList.push(node.extraClasses);
		}
		// IE6 doesn't correctly evaluate multiple class names,
		// so we create combined class names that can be used in the CSS
		if( hasChildren === false ){
			cnList.push(cn.combinedExpanderPrefix + "n" +
					(isLastSib ? "l" : "")
					);
		}else{
			cnList.push(cn.combinedExpanderPrefix +
					(node.expanded ? "e" : "c") +
					(node.lazy && node.children == null ? "d" : "") +
					(isLastSib ? "l" : "")
					);
		}
		cnList.push(cn.combinedIconPrefix +
				(node.expanded ? "e" : "c") +
				(node.folder ? "f" : "")
				);
//        node.span.className = cnList.join(" ");
		statusElem.className = cnList.join(" ");

		// TODO: we should not set this in the <span> tag also, if we set it here:
		// Maybe most (all) of the classes should be set in LI instead of SPAN?
		if(node.li){
			// #719: we have to consider that there may be already other classes:
			$(node.li).toggleClass(cn.lastsib, isLastSib);
		}
	},
	/** Activate node.
	 * flag defaults to true.
	 * If flag is true, the node is activated (must be a synchronous operation)
	 * If flag is false, the node is deactivated (must be a synchronous operation)
	 * @param {EventData} ctx
	 * @param {boolean} [flag=true]
	 * @param {object} [opts] additional options. Defaults to {noEvents: false, noFocus: false}
	 * @returns {$.Promise}
	 */
	nodeSetActive: function(ctx, flag, callOpts) {
		// Handle user click / [space] / [enter], according to clickFolderMode.
		callOpts = callOpts || {};
		var subCtx,
			node = ctx.node,
			tree = ctx.tree,
			opts = ctx.options,
			noEvents = (callOpts.noEvents === true),
			noFocus = (callOpts.noFocus === true),
			isActive = (node === tree.activeNode);

		// flag defaults to true
		flag = (flag !== false);
		// node.debug("nodeSetActive", flag);

		if(isActive === flag){
			// Nothing to do
			return _getResolvedPromise(node);
		}else if(flag && !noEvents && this._triggerNodeEvent("beforeActivate", node, ctx.originalEvent) === false ){
			// Callback returned false
			return _getRejectedPromise(node, ["rejected"]);
		}
		if(flag){
			if(tree.activeNode){
				_assert(tree.activeNode !== node, "node was active (inconsistency)");
				subCtx = $.extend({}, ctx, {node: tree.activeNode});
				tree.nodeSetActive(subCtx, false);
				_assert(tree.activeNode === null, "deactivate was out of sync?");
			}
			if(opts.activeVisible){
				// If no focus is set (noFocus: true) and there is no focused node, this node is made visible.
				node.makeVisible({scrollIntoView: noFocus && tree.focusNode == null});
			}
			tree.activeNode = node;
			tree.nodeRenderStatus(ctx);
			if( !noFocus ) {
				tree.nodeSetFocus(ctx);
			}
			if( !noEvents ) {
				tree._triggerNodeEvent("activate", node, ctx.originalEvent);
			}
		}else{
			_assert(tree.activeNode === node, "node was not active (inconsistency)");
			tree.activeNode = null;
			this.nodeRenderStatus(ctx);
			if( !noEvents ) {
				ctx.tree._triggerNodeEvent("deactivate", node, ctx.originalEvent);
			}
		}
		return _getResolvedPromise(node);
	},
	/** Expand or collapse node, return Deferred.promise.
	 *
	 * @param {EventData} ctx
	 * @param {boolean} [flag=true]
	 * @param {object} [opts] additional options. Defaults to {noAnimation: false, noEvents: false}
	 * @returns {$.Promise} The deferred will be resolved as soon as the (lazy)
	 *     data was retrieved, rendered, and the expand animation finshed.
	 */
	nodeSetExpanded: function(ctx, flag, callOpts) {
		callOpts = callOpts || {};
		var _afterLoad, dfd, i, l, parents, prevAC,
			node = ctx.node,
			tree = ctx.tree,
			opts = ctx.options,
			noAnimation = (callOpts.noAnimation === true),
			noEvents = (callOpts.noEvents === true);

		// flag defaults to true
		flag = (flag !== false);

		// node.debug("nodeSetExpanded(" + flag + ")");

		if((node.expanded && flag) || (!node.expanded && !flag)){
			// Nothing to do
			// node.debug("nodeSetExpanded(" + flag + "): nothing to do");
			return _getResolvedPromise(node);
		}else if(flag && !node.lazy && !node.hasChildren() ){
			// Prevent expanding of empty nodes
			// return _getRejectedPromise(node, ["empty"]);
			return _getResolvedPromise(node);
		}else if( !flag && node.getLevel() < opts.minExpandLevel ) {
			// Prevent collapsing locked levels
			return _getRejectedPromise(node, ["locked"]);
		}else if ( !noEvents && this._triggerNodeEvent("beforeExpand", node, ctx.originalEvent) === false ){
			// Callback returned false
			return _getRejectedPromise(node, ["rejected"]);
		}
		// If this node inside a collpased node, no animation and scrolling is needed
		if( !noAnimation && !node.isVisible() ) {
			noAnimation = callOpts.noAnimation = true;
		}

		dfd = new $.Deferred();

		// Auto-collapse mode: collapse all siblings
		if( flag && !node.expanded && opts.autoCollapse ) {
			parents = node.getParentList(false, true);
			prevAC = opts.autoCollapse;
			try{
				opts.autoCollapse = false;
				for(i=0, l=parents.length; i<l; i++){
					// TODO: should return promise?
					this._callHook("nodeCollapseSiblings", parents[i], callOpts);
				}
			}finally{
				opts.autoCollapse = prevAC;
			}
		}
		// Trigger expand/collapse after expanding
		dfd.done(function(){
			var	lastChild = node.getLastChild();

			if( flag && opts.autoScroll && !noAnimation && lastChild ) {
				// Scroll down to last child, but keep current node visible
				lastChild.scrollIntoView(true, {topNode: node}).always(function(){
					if( !noEvents ) {
						ctx.tree._triggerNodeEvent(flag ? "expand" : "collapse", ctx);
					}
				});
			} else {
				if( !noEvents ) {
					ctx.tree._triggerNodeEvent(flag ? "expand" : "collapse", ctx);
				}
			}
		});
		// vvv Code below is executed after loading finished:
		_afterLoad = function(callback){
			var cn = opts._classNames,
				isVisible, isExpanded,
				effect = opts.toggleEffect;

			node.expanded = flag;
			// Create required markup, but make sure the top UL is hidden, so we
			// can animate later
			tree._callHook("nodeRender", ctx, false, false, true);

			// Hide children, if node is collapsed
			if( node.ul ) {
				isVisible = (node.ul.style.display !== "none");
				isExpanded = !!node.expanded;
				if ( isVisible === isExpanded ) {
					node.warn("nodeSetExpanded: UL.style.display already set");

				} else if ( !effect || noAnimation ) {
					node.ul.style.display = ( node.expanded || !parent ) ? "" : "none";

				} else {
					// The UI toggle() effect works with the ext-wide extension,
					// while jQuery.animate() has problems when the title span
					// has positon: absolute.
					// Since jQuery UI 1.12, the blind effect requires the parent
					// element to have 'position: relative'.
					// See #716, #717
					$(node.li).addClass(cn.animating);  // #717
//					node.info("fancytree-animating start: " + node.li.className);
					$(node.ul)
						.addClass(cn.animating)  // # 716
						.toggle(effect.effect, effect.options, effect.duration, function(){
//							node.info("fancytree-animating end: " + node.li.className);
							$(this).removeClass(cn.animating);  // #716
							$(node.li).removeClass(cn.animating);  // #717
							callback();
						});
					return;
				}
			}
			callback();
		};
		// ^^^ Code above is executed after loading finshed.

		// Load lazy nodes, if any. Then continue with _afterLoad()
		if(flag && node.lazy && node.hasChildren() === undefined){
			// node.debug("nodeSetExpanded: load start...");
			node.load().done(function(){
				// node.debug("nodeSetExpanded: load done");
				if(dfd.notifyWith){ // requires jQuery 1.6+
					dfd.notifyWith(node, ["loaded"]);
				}
				_afterLoad(function () { dfd.resolveWith(node); });
			}).fail(function(errMsg){
				_afterLoad(function () { dfd.rejectWith(node, ["load failed (" + errMsg + ")"]); });
			});
/*
			var source = tree._triggerNodeEvent("lazyLoad", node, ctx.originalEvent);
			_assert(typeof source !== "boolean", "lazyLoad event must return source in data.result");
			node.debug("nodeSetExpanded: load start...");
			this._callHook("nodeLoadChildren", ctx, source).done(function(){
				node.debug("nodeSetExpanded: load done");
				if(dfd.notifyWith){ // requires jQuery 1.6+
					dfd.notifyWith(node, ["loaded"]);
				}
				_afterLoad.call(tree);
			}).fail(function(errMsg){
				dfd.rejectWith(node, ["load failed (" + errMsg + ")"]);
			});
*/
		}else{
			_afterLoad(function () { dfd.resolveWith(node); });
		}
		// node.debug("nodeSetExpanded: returns");
		return dfd.promise();
	},
	/** Focus or blur this node.
	 * @param {EventData} ctx
	 * @param {boolean} [flag=true]
	 */
	nodeSetFocus: function(ctx, flag) {
		// ctx.node.debug("nodeSetFocus(" + flag + ")");
		var ctx2,
			tree = ctx.tree,
			node = ctx.node,
			opts = tree.options,
			// et = ctx.originalEvent && ctx.originalEvent.type,
			isInput = ctx.originalEvent ? $(ctx.originalEvent.target).is(":input") : false;

		flag = (flag !== false);

		// (node || tree).debug("nodeSetFocus(" + flag + "), event: " + et + ", isInput: "+ isInput);
		// Blur previous node if any
		if(tree.focusNode){
			if(tree.focusNode === node && flag){
				// node.debug("nodeSetFocus(" + flag + "): nothing to do");
				return;
			}
			ctx2 = $.extend({}, ctx, {node: tree.focusNode});
			tree.focusNode = null;
			this._triggerNodeEvent("blur", ctx2);
			this._callHook("nodeRenderStatus", ctx2);
		}
		// Set focus to container and node
		if(flag){
			if( !this.hasFocus() ){
				node.debug("nodeSetFocus: forcing container focus");
				this._callHook("treeSetFocus", ctx, true, {calledByNode: true});
			}
			node.makeVisible({scrollIntoView: false});
			tree.focusNode = node;
			if( opts.titlesTabbable ) {
				if( !isInput ) { // #621
					$(node.span).find(".fancytree-title").focus();
				}
			} else {
				// We cannot set KB focus to a node, so use the tree container
				// #563, #570: IE scrolls on every call to .focus(), if the container
				// is partially outside the viewport. So do it only, when absolutely
				// neccessary:
				if( $(document.activeElement).closest(".fancytree-container").length === 0 ) {
					$(tree.$container).focus();
				}
			}
			if( opts.aria ){
				// Set active descendant to node's span ID (create one, if needed)
				$(tree.$container).attr("aria-activedescendant",
					$( node.tr || node.li ).uniqueId().attr("id"));
					// "ftal_" + opts.idPrefix + node.key);
			}
//			$(node.span).find(".fancytree-title").focus();
			this._triggerNodeEvent("focus", ctx);
//          if( opts.autoActivate ){
//              tree.nodeSetActive(ctx, true);
//          }
			if( opts.autoScroll ){
				node.scrollIntoView();
			}
			this._callHook("nodeRenderStatus", ctx);
		}
	},
	/** (De)Select node, return new status (sync).
	 *
	 * @param {EventData} ctx
	 * @param {boolean} [flag=true]
	 * @param {object} [opts] additional options. Defaults to {noEvents: false,
	 *     propagateDown: null, propagateUp: null,
	 *     callback: null,
	 *     }
	 * @returns {boolean} previous status
	 */
	nodeSetSelected: function(ctx, flag, callOpts) {
		callOpts = callOpts || {};
		var node = ctx.node,
			tree = ctx.tree,
			opts = ctx.options,
			noEvents = (callOpts.noEvents === true);

		// flag defaults to true
		flag = (flag !== false);

		// node.debug("nodeSetSelected(" + flag + ")", ctx);

		// Cannot (de)select unselectable nodes directly (only by propagation or
		// by setting the `.selected` property)
		if( FT.evalOption("unselectable", node, node, opts, false) ){
			return;
		}

		// Remember the user's intent, in case down -> up propagation prevents
		// applying it to node.selected
		node._lastSelectIntent = flag;

		// Nothing to do?
		/*jshint -W018 */  // Confusing use of '!'
		if( !!node.selected === flag ){
			if( opts.selectMode === 3 && node.partsel && !flag ){
				// If propagation prevented selecting this node last time, we still
				// want to allow to apply setSelected(false) now
			}else{
				return flag;
			}
		}
		/*jshint +W018 */

		if( !noEvents &&
			this._triggerNodeEvent("beforeSelect", node, ctx.originalEvent) === false ) {
				return !!node.selected;
		}
		if(flag && opts.selectMode === 1){
			// single selection mode (we don't uncheck all tree nodes, for performance reasons)
			if(tree.lastSelectedNode){
				tree.lastSelectedNode.setSelected(false);
			}
			node.selected = flag;
		}else if(opts.selectMode === 3 && !node.parent.radiogroup && !node.radiogroup){
			// multi-hierarchical selection mode
			node.selected = flag;
			node.fixSelection3AfterClick(callOpts);
		}else if(node.parent.radiogroup){
			node.visitSiblings(function(n){
				n._changeSelectStatusAttrs(flag && n === node);
			}, true);
		}else{
			// default: selectMode: 2, multi selection mode
			node.selected = flag;
		}
		this.nodeRenderStatus(ctx);
		tree.lastSelectedNode = flag ? node : null;
		if( !noEvents ) {
			tree._triggerNodeEvent("select", ctx);
		}
	},
	/** Show node status (ok, loading, error, nodata) using styles and a dummy child node.
	 *
	 * @param {EventData} ctx
	 * @param status
	 * @param message
	 * @param details
	 * @since 2.3
	 */
	nodeSetStatus: function(ctx, status, message, details) {
		var node = ctx.node,
			tree = ctx.tree;

		function _clearStatusNode() {
			// Remove dedicated dummy node, if any
			var firstChild = ( node.children ? node.children[0] : null );
			if ( firstChild && firstChild.isStatusNode() ) {
				try{
					// I've seen exceptions here with loadKeyPath...
					if(node.ul){
						node.ul.removeChild(firstChild.li);
						firstChild.li = null; // avoid leaks (DT issue 215)
					}
				}catch(e){}
				if( node.children.length === 1 ){
					node.children = [];
				}else{
					node.children.shift();
				}
			}
		}
		function _setStatusNode(data, type) {
			// Create/modify the dedicated dummy node for 'loading...' or
			// 'error!' status. (only called for direct child of the invisible
			// system root)
			var firstChild = ( node.children ? node.children[0] : null );
			if ( firstChild && firstChild.isStatusNode() ) {
				$.extend(firstChild, data);
				firstChild.statusNodeType = type;
				tree._callHook("nodeRenderTitle", firstChild);
			} else {
				node._setChildren([data]);
				node.children[0].statusNodeType = type;
				tree.render();
			}
			return node.children[0];
		}

		switch( status ){
		case "ok":
			_clearStatusNode();
			node._isLoading = false;
			node._error = null;
			node.renderStatus();
			break;
		case "loading":
			if( !node.parent ) {
				_setStatusNode({
					title: tree.options.strings.loading + (message ? " (" + message + ")" : ""),
					// icon: true,  // needed for 'loding' icon
					checkbox: false,
					tooltip: details
				}, status);
			}
			node._isLoading = true;
			node._error = null;
			node.renderStatus();
			break;
		case "error":
			_setStatusNode({
				title: tree.options.strings.loadError + (message ? " (" + message + ")" : ""),
				// icon: false,
				checkbox: false,
				tooltip: details
			}, status);
			node._isLoading = false;
			node._error = { message: message, details: details };
			node.renderStatus();
			break;
		case "nodata":
			_setStatusNode({
				title: tree.options.strings.noData,
				// icon: false,
				checkbox: false,
				tooltip: details
			}, status);
			node._isLoading = false;
			node._error = null;
			node.renderStatus();
			break;
		default:
			$.error("invalid node status " + status);
		}
	},
	/**
	 *
	 * @param {EventData} ctx
	 */
	nodeToggleExpanded: function(ctx) {
		return this.nodeSetExpanded(ctx, !ctx.node.expanded);
	},
	/**
	 * @param {EventData} ctx
	 */
	nodeToggleSelected: function(ctx) {
		var node = ctx.node,
			flag = !node.selected;

		// In selectMode: 3 this node may be unselected+partsel, even if
		// setSelected(true) was called before, due to `unselectable` children.
		// In this case, we now toggle as `setSelected(false)`
		if( node.partsel && !node.selected && node._lastSelectIntent === true ) {
			flag = false;
			node.selected = true;  // so it is not considered 'nothing to do'
		}
		node._lastSelectIntent = flag;
		return this.nodeSetSelected(ctx, flag);
	},
	/** Remove all nodes.
	 * @param {EventData} ctx
	 */
	treeClear: function(ctx) {
		var tree = ctx.tree;
		tree.activeNode = null;
		tree.focusNode = null;
		tree.$div.find(">ul.fancytree-container").empty();
		// TODO: call destructors and remove reference loops
		tree.rootNode.children = null;
	},
	/** Widget was created (called only once, even it re-initialized).
	 * @param {EventData} ctx
	 */
	treeCreate: function(ctx) {
	},
	/** Widget was destroyed.
	 * @param {EventData} ctx
	 */
	treeDestroy: function(ctx) {
		this.$div.find(">ul.fancytree-container").remove();
		this.$source && this.$source.removeClass("ui-helper-hidden");
	},
	/** Widget was (re-)initialized.
	 * @param {EventData} ctx
	 */
	treeInit: function(ctx) {
		var tree = ctx.tree,
			opts = tree.options;

		//this.debug("Fancytree.treeInit()");
		// Add container to the TAB chain
		// See http://www.w3.org/TR/wai-aria-practices/#focus_activedescendant
		// #577: Allow to set tabindex to "0", "-1" and ""
		tree.$container.attr("tabindex", opts.tabindex);

		if( opts.rtl ) {
			tree.$container.attr("DIR", "RTL").addClass("fancytree-rtl");
		}else{
			tree.$container.removeAttr("DIR").removeClass("fancytree-rtl");
		}
		if( opts.aria ){
			tree.$container.attr("role", "tree");
			if( opts.selectMode !== 1 ) {
				tree.$container.attr("aria-multiselectable", true);
			}
		}
		this.treeLoad(ctx);
	},
	/** Parse Fancytree from source, as configured in the options.
	 * @param {EventData} ctx
	 * @param {object} [source] optional new source (use last data otherwise)
	 */
	treeLoad: function(ctx, source) {
		var metaData, type, $ul,
			tree = ctx.tree,
			$container = ctx.widget.element,
			dfd,
			// calling context for root node
			rootCtx = $.extend({}, ctx, {node: this.rootNode});

		if(tree.rootNode.children){
			this.treeClear(ctx);
		}
		source = source || this.options.source;

		if(!source){
			type = $container.data("type") || "html";
			switch(type){
			case "html":
				$ul = $container.find(">ul:first");
				$ul.addClass("ui-fancytree-source ui-helper-hidden");
				source = $.ui.fancytree.parseHtml($ul);
				// allow to init tree.data.foo from <ul data-foo=''>
				this.data = $.extend(this.data, _getElementDataAsDict($ul));
				break;
			case "json":
				source = $.parseJSON($container.text());
				// $container already contains the <ul>, but we remove the plain (json) text
				// $container.empty();
				$container.contents().filter(function(){
					return (this.nodeType === 3);
				}).remove();
				if( $.isPlainObject(source) ){
					// We got {foo: 'abc', children: [...]}
					// Copy extra properties to tree.data.foo
					_assert($.isArray(source.children), "if an object is passed as source, it must contain a 'children' array (all other properties are added to 'tree.data')");
					metaData = source;
					source = source.children;
					delete metaData.children;
					$.extend(tree.data, metaData);
				}
				break;
			default:
				$.error("Invalid data-type: " + type);
			}
		}else if(typeof source === "string"){
			// TODO: source is an element ID
			$.error("Not implemented");
		}

		// Trigger fancytreeinit after nodes have been loaded
		dfd = this.nodeLoadChildren(rootCtx, source).done(function(){
			tree.render();
			if( ctx.options.selectMode === 3 ){
				tree.rootNode.fixSelection3FromEndNodes();
			}
			if( tree.activeNode && tree.options.activeVisible ) {
				tree.activeNode.makeVisible();
			}
			tree._triggerTreeEvent("init", null, { status: true });
		}).fail(function(){
			tree.render();
			tree._triggerTreeEvent("init", null, { status: false });
		});
		return dfd;
	},
	/** Node was inserted into or removed from the tree.
	 * @param {EventData} ctx
	 * @param {boolean} add
	 * @param {FancytreeNode} node
	 */
	treeRegisterNode: function(ctx, add, node) {
	},
	/** Widget got focus.
	 * @param {EventData} ctx
	 * @param {boolean} [flag=true]
	 */
	treeSetFocus: function(ctx, flag, callOpts) {
		function ensureTreeFocus(thisTree) {
			if (!thisTree.activeNode && thisTree.getFirstChild()) {
				thisTree.getFirstChild().setFocus();
			}
		}

		flag = (flag !== false);

		// this.debug("treeSetFocus(" + flag + "), callOpts: ", callOpts, this.hasFocus());
		// this.debug("    focusNode: " + this.focusNode);
		// this.debug("    activeNode: " + this.activeNode);
		if( flag !== this.hasFocus() ){
			this._hasFocus = flag;
			if( !flag && this.focusNode ) {
				// Node also looses focus if widget blurs
				this.focusNode.setFocus(false);
			} else if ( flag && (!callOpts || !callOpts.calledByNode) ) {
				$(this.$container).focus();
			}
			this.$container.toggleClass("fancytree-treefocus", flag);
			this._triggerTreeEvent(flag ? "focusTree" : "blurTree");
			if( flag ) {
				// Check after timeout to ensure mousedown processing is complete
				// and the clicked node is already activated
				var thisTree = this;
				setTimeout(function() { ensureTreeFocus(thisTree); }, 0);
			}
		}
	},
	/** Widget option was set using `$().fancytree("option", "foo", "bar")`.
	 * @param {EventData} ctx
	 * @param {string} key option name
	 * @param {any} value option value
	 */
	treeSetOption: function(ctx, key, value) {
		var tree = ctx.tree,
			callDefault = true,
			rerender = false;

		switch( key ) {
		case "aria":
		case "checkbox":
		case "icon":
		case "minExpandLevel":
		case "tabindex":
			tree._callHook("treeCreate", tree);
			rerender = true;
			break;
		case "escapeTitles":
		case "tooltip":
			rerender = true;
			break;
		case "rtl":
			if( value === false ) {
				tree.$container.removeAttr("DIR").removeClass("fancytree-rtl");
			}else{
				tree.$container.attr("DIR", "RTL").addClass("fancytree-rtl");
			}
			rerender = true;
			break;
		case "source":
			callDefault = false;
			tree._callHook("treeLoad", tree, value);
			rerender = true;
			break;
		}
		tree.debug("set option " + key + "=" + value + " <" + typeof(value) + ">");
		if(callDefault){
			if( this.widget._super ) {
				// jQuery UI 1.9+
				this.widget._super.call( this.widget, key, value );
			} else {
				// jQuery UI <= 1.8, we have to manually invoke the _setOption method from the base widget
				$.Widget.prototype._setOption.call(this.widget, key, value);
			}
		}
		if(rerender){
			tree.render(true, false);  // force, not-deep
		}
	}
});


/* ******************************************************************************
 * jQuery UI widget boilerplate
 */

/**
 * The plugin (derrived from <a href=" http://api.jqueryui.com/jQuery.widget/">jQuery.Widget</a>).<br>
 * This constructor is not called directly. Use `$(selector).fancytree({})`
 * to initialize the plugin instead.<br>
 * <pre class="sh_javascript sunlight-highlight-javascript">// Access widget methods and members:
 * var tree = $("#tree").fancytree("getTree");
 * var node = $("#tree").fancytree("getActiveNode", "1234");
 * </pre>
 *
 * @mixin Fancytree_Widget
 */

$.widget("ui.fancytree",
	/** @lends Fancytree_Widget# */
	{
	/**These options will be used as defaults
	 * @type {FancytreeOptions}
	 */
	options:
	{
		activeVisible: true,
		ajax: {
			type: "GET",
			cache: false, // false: Append random '_' argument to the request url to prevent caching.
//          timeout: 0, // >0: Make sure we get an ajax error if server is unreachable
			dataType: "json" // Expect json format and pass json object to callbacks.
		},  //
		aria: true,
		autoActivate: true,
		autoCollapse: false,
		autoScroll: false,
		checkbox: false,
		clickFolderMode: 4,
		debugLevel: null, // 0..2 (null: use global setting $.ui.fancytree.debugInfo)
		disabled: false, // TODO: required anymore?
		enableAspx: true, // TODO: document
		escapeTitles: false,
		extensions: [],
		// fx: { height: "toggle", duration: 200 },
		// toggleEffect: { effect: "drop", options: {direction: "left"}, duration: 200 },
		// toggleEffect: { effect: "slide", options: {direction: "up"}, duration: 200 },
		toggleEffect: { effect: "blind", options: {direction: "vertical", scale: "box"}, duration: 200 },
		generateIds: false,
		icon: true,
		idPrefix: "ft_",
		focusOnSelect: false,
		keyboard: true,
		keyPathSeparator: "/",
		minExpandLevel: 1,
		quicksearch: false,
		rtl: false,
		scrollOfs: {top: 0, bottom: 0},
		scrollParent: null,
		selectMode: 2,
		strings: {
			loading: "Loading...",  // &#8230; would be escaped when escapeTitles is true
			loadError: "Load error!",
			moreData: "More...",
			noData: "No data."
		},
		tabindex: "0",
		titlesTabbable: false,
		tooltip: false,
		_classNames: {
			node: "fancytree-node",
			folder: "fancytree-folder",
			animating: "fancytree-animating",
			combinedExpanderPrefix: "fancytree-exp-",
			combinedIconPrefix: "fancytree-ico-",
			hasChildren: "fancytree-has-children",
			active: "fancytree-active",
			selected: "fancytree-selected",
			expanded: "fancytree-expanded",
			lazy: "fancytree-lazy",
			focused: "fancytree-focused",
			partload: "fancytree-partload",
			partsel: "fancytree-partsel",
			radio: "fancytree-radio",
			// radiogroup: "fancytree-radiogroup",
			unselectable: "fancytree-unselectable",
			lastsib: "fancytree-lastsib",
			loading: "fancytree-loading",
			error: "fancytree-error",
			statusNodePrefix: "fancytree-statusnode-"
		},
		// events
		lazyLoad: null,
		postProcess: null
	},
	/* Set up the widget, Called on first $().fancytree() */
	_create: function() {
		this.tree = new Fancytree(this);

		this.$source = this.source || this.element.data("type") === "json" ? this.element
			: this.element.find(">ul:first");
		// Subclass Fancytree instance with all enabled extensions
		var extension, extName, i,
			opts = this.options,
			extensions = opts.extensions,
			base = this.tree;

		for(i=0; i<extensions.length; i++){
			extName = extensions[i];
			extension = $.ui.fancytree._extensions[extName];
			if(!extension){
				$.error("Could not apply extension '" + extName + "' (it is not registered, did you forget to include it?)");
			}
			// Add extension options as tree.options.EXTENSION
//			_assert(!this.tree.options[extName], "Extension name must not exist as option name: " + extName);
			this.tree.options[extName] = $.extend(true, {}, extension.options, this.tree.options[extName]);
			// Add a namespace tree.ext.EXTENSION, to hold instance data
			_assert(this.tree.ext[extName] === undefined, "Extension name must not exist as Fancytree.ext attribute: '" + extName + "'");
//			this.tree[extName] = extension;
			this.tree.ext[extName] = {};
			// Subclass Fancytree methods using proxies.
			_subclassObject(this.tree, base, extension, extName);
			// current extension becomes base for the next extension
			base = extension;
		}
		//
		if( opts.icons !== undefined ) {  // 2015-11-16
			if( opts.icon !== true ) {
				$.error("'icons' tree option is deprecated since v2.14.0: use 'icon' only instead");
			} else {
				this.tree.warn("'icons' tree option is deprecated since v2.14.0: use 'icon' instead");
				opts.icon = opts.icons;
			}
		}
		if( opts.iconClass !== undefined ) {  // 2015-11-16
			if( opts.icon ) {
				$.error("'iconClass' tree option is deprecated since v2.14.0: use 'icon' only instead");
			} else {
				this.tree.warn("'iconClass' tree option is deprecated since v2.14.0: use 'icon' instead");
				opts.icon = opts.iconClass;
			}
		}
		if( opts.tabbable !== undefined ) {  // 2016-04-04
			opts.tabindex = opts.tabbable ? "0" : "-1";
			this.tree.warn("'tabbable' tree option is deprecated since v2.17.0: use 'tabindex='" + opts.tabindex + "' instead");
		}
		//
		this.tree._callHook("treeCreate", this.tree);
		// Note: 'fancytreecreate' event is fired by widget base class
//        this.tree._triggerTreeEvent("create");
	},

	/* Called on every $().fancytree() */
	_init: function() {
		this.tree._callHook("treeInit", this.tree);
		// TODO: currently we call bind after treeInit, because treeInit
		// might change tree.$container.
		// It would be better, to move ebent binding into hooks altogether
		this._bind();
	},

	/* Use the _setOption method to respond to changes to options */
	_setOption: function(key, value) {
		return this.tree._callHook("treeSetOption", this.tree, key, value);
	},

	/** Use the destroy method to clean up any modifications your widget has made to the DOM */
	destroy: function() {
		this._unbind();
		this.tree._callHook("treeDestroy", this.tree);
		// In jQuery UI 1.8, you must invoke the destroy method from the base widget
		$.Widget.prototype.destroy.call(this);
		// TODO: delete tree and nodes to make garbage collect easier?
		// TODO: In jQuery UI 1.9 and above, you would define _destroy instead of destroy and not call the base method
	},

	// -------------------------------------------------------------------------

	/* Remove all event handlers for our namespace */
	_unbind: function() {
		var ns = this.tree._ns;
		this.element.off(ns);
		this.tree.$container.off(ns);
		$(document).off(ns);
	},
	/* Add mouse and kyboard handlers to the container */
	_bind: function() {
		var that = this,
			opts = this.options,
			tree = this.tree,
			ns = tree._ns
			// selstartEvent = ( $.support.selectstart ? "selectstart" : "mousedown" )
			;

		// Remove all previuous handlers for this tree
		this._unbind();

		//alert("keydown" + ns + "foc=" + tree.hasFocus() + tree.$container);
		// tree.debug("bind events; container: ", tree.$container);
		tree.$container.on("focusin" + ns + " focusout" + ns, function(event){
			var node = FT.getNode(event),
				flag = (event.type === "focusin");
			// tree.debug("Tree container got event " + event.type, node, event);
			// tree.treeOnFocusInOut.call(tree, event);
			if(node){
				// For example clicking into an <input> that is part of a node
				tree._callHook("nodeSetFocus", tree._makeHookContext(node, event), flag);
				// tree._callHook("nodeSetFocus", node, flag);
			}else{
				tree._callHook("treeSetFocus", tree, flag);
			}
		}).on("selectstart" + ns, "span.fancytree-title", function(event){
			// prevent mouse-drags to select text ranges
			// tree.debug("<span title> got event " + event.type);
			event.preventDefault();
		}).on("keydown" + ns, function(event){
			// TODO: also bind keyup and keypress
			// tree.debug("got event " + event.type + ", hasFocus:" + tree.hasFocus());
			// if(opts.disabled || opts.keyboard === false || !tree.hasFocus() ){
			if(opts.disabled || opts.keyboard === false ){
				return true;
			}
			var res,
				node = tree.focusNode, // node may be null
				ctx = tree._makeHookContext(node || tree, event),
				prevPhase = tree.phase;

			try {
				tree.phase = "userEvent";
				// If a 'fancytreekeydown' handler returns false, skip the default
				// handling (implemented by tree.nodeKeydown()).
				if(node){
					res = tree._triggerNodeEvent("keydown", node, event);
				}else{
					res = tree._triggerTreeEvent("keydown", event);
				}
				if ( res === "preventNav" ){
					res = true; // prevent keyboard navigation, but don't prevent default handling of embedded input controls
				} else if ( res !== false ){
					res = tree._callHook("nodeKeydown", ctx);
				}
				return res;
			} finally {
				tree.phase = prevPhase;
			}
		}).on("mousedown" + ns + " dblclick" + ns, function(event){
			// that.tree.debug("event(" + event + "): !");
			if(opts.disabled){
				return true;
			}
			var ctx,
				et = FT.getEventTarget(event),
				node = et.node,
				tree = that.tree,
				prevPhase = tree.phase;

			// that.tree.debug("event(" + event.type + "): node: ", node);
			if( !node ){
				return true;  // Allow bubbling of other events
			}
			ctx = tree._makeHookContext(node, event);
			// that.tree.debug("event(" + event.type + "): node: ", node);
			try {
				tree.phase = "userEvent";
				switch(event.type) {
				case "mousedown":
					ctx.targetType = et.type;
					if( node.isPagingNode() ) {
						return tree._triggerNodeEvent("clickPaging", ctx, event) === true;
					}
					return ( tree._triggerNodeEvent("click", ctx, event) === false ) ? false : tree._callHook("nodeClick", ctx);
				case "dblclick":
					ctx.targetType = et.type;
					return ( tree._triggerNodeEvent("dblclick", ctx, event) === false ) ? false : tree._callHook("nodeDblclick", ctx);
				}
//             } catch(e) {
// //                var _ = null; // DT issue 117 // TODO
//                 $.error(e);
			} finally {
				tree.phase = prevPhase;
			}
		});
	},
	/** Return the active node or null.
	 * @returns {FancytreeNode}
	 */
	getActiveNode: function() {
		return this.tree.activeNode;
	},
	/** Return the matching node or null.
	 * @param {string} key
	 * @returns {FancytreeNode}
	 */
	getNodeByKey: function(key) {
		return this.tree.getNodeByKey(key);
	},
	/** Return the invisible system root node.
	 * @returns {FancytreeNode}
	 */
	getRootNode: function() {
		return this.tree.rootNode;
	},
	/** Return the current tree instance.
	 * @returns {Fancytree}
	 */
	getTree: function() {
		return this.tree;
	}
});

// $.ui.fancytree was created by the widget factory. Create a local shortcut:
FT = $.ui.fancytree;

/**
 * Static members in the `$.ui.fancytree` namespace.<br>
 * <br>
 * <pre class="sh_javascript sunlight-highlight-javascript">// Access static members:
 * var node = $.ui.fancytree.getNode(element);
 * alert($.ui.fancytree.version);
 * </pre>
 *
 * @mixin Fancytree_Static
 */
$.extend($.ui.fancytree,
	/** @lends Fancytree_Static# */
	{
	/** @type {string} */
	version: "2.23.0",      // Set to semver by 'grunt release'
	/** @type {string} */
	buildType: "production", // Set to 'production' by 'grunt build'
	/** @type {int} */
	debugLevel: 1,            // Set to 1 by 'grunt build'
							  // Used by $.ui.fancytree.debug() and as default for tree.options.debugLevel

	_nextId: 1,
	_nextNodeKey: 1,
	_extensions: {},
	// focusTree: null,

	/** Expose class object as $.ui.fancytree._FancytreeClass */
	_FancytreeClass: Fancytree,
	/** Expose class object as $.ui.fancytree._FancytreeNodeClass */
	_FancytreeNodeClass: FancytreeNode,
	/* Feature checks to provide backwards compatibility */
	jquerySupports: {
		// http://jqueryui.com/upgrade-guide/1.9/#deprecated-offset-option-merged-into-my-and-at
		positionMyOfs: isVersionAtLeast($.ui.version, 1, 9)
		},
	/** Throw an error if condition fails (debug method).
	 * @param {boolean} cond
	 * @param {string} msg
	 */
	assert: function(cond, msg){
		return _assert(cond, msg);
	},
	/** Return a function that executes *fn* at most every *timeout* ms.
	 * @param {integer} timeout
	 * @param {function} fn
	 * @param {boolean} [invokeAsap=false]
	 * @param {any} [ctx]
	 */
	debounce: function(timeout, fn, invokeAsap, ctx) {
		var timer;
		if(arguments.length === 3 && typeof invokeAsap !== "boolean") {
			ctx = invokeAsap;
			invokeAsap = false;
		}
		return function() {
			var args = arguments;
			ctx = ctx || this;
			invokeAsap && !timer && fn.apply(ctx, args);
			clearTimeout(timer);
			timer = setTimeout(function() {
				invokeAsap || fn.apply(ctx, args);
				timer = null;
			}, timeout);
		};
	},
	/** Write message to console if debugLevel >= 2
	 * @param {string} msg
	 */
	debug: function(msg){
		/*jshint expr:true */
		($.ui.fancytree.debugLevel >= 2) && consoleApply("log", arguments);
	},
	/** Write error message to console.
	 * @param {string} msg
	 */
	error: function(msg){
		consoleApply("error", arguments);
	},
	/** Convert &lt;, &gt;, &amp;, &quot;, &#39;, &#x2F; to the equivalent entities.
	 *
	 * @param {string} s
	 * @returns {string}
	 */
	escapeHtml: _escapeHtml,
	/** Make jQuery.position() arguments backwards compatible, i.e. if
	 * jQuery UI version <= 1.8, convert
	 *   { my: "left+3 center", at: "left bottom", of: $target }
	 * to
	 *   { my: "left center", at: "left bottom", of: $target, offset: "3  0" }
	 *
	 * See http://jqueryui.com/upgrade-guide/1.9/#deprecated-offset-option-merged-into-my-and-at
	 * and http://jsfiddle.net/mar10/6xtu9a4e/
	 */
	fixPositionOptions: function(opts) {
		if( opts.offset || ("" + opts.my + opts.at ).indexOf("%") >= 0 ) {
		   $.error("expected new position syntax (but '%' is not supported)");
		}
		if( ! $.ui.fancytree.jquerySupports.positionMyOfs ) {
			var // parse 'left+3 center' into ['left+3 center', 'left', '+3', 'center', undefined]
				myParts = /(\w+)([+-]?\d+)?\s+(\w+)([+-]?\d+)?/.exec(opts.my),
				atParts = /(\w+)([+-]?\d+)?\s+(\w+)([+-]?\d+)?/.exec(opts.at),
				// convert to numbers
				dx = (myParts[2] ? (+myParts[2]) : 0) + (atParts[2] ? (+atParts[2]) : 0),
				dy = (myParts[4] ? (+myParts[4]) : 0) + (atParts[4] ? (+atParts[4]) : 0);

			opts = $.extend({}, opts, { // make a copy and overwrite
				my: myParts[1] + " " + myParts[3],
				at: atParts[1] + " " + atParts[3]
			});
			if( dx || dy ) {
				opts.offset = "" + dx + " " + dy;
			}
		}
		return opts;
	},
	/** Return a {node: FancytreeNode, type: TYPE} object for a mouse event.
	 *
	 * @param {Event} event Mouse event, e.g. click, ...
	 * @returns {string} 'title' | 'prefix' | 'expander' | 'checkbox' | 'icon' | undefined
	 */
	getEventTargetType: function(event){
		return this.getEventTarget(event).type;
	},
	/** Return a {node: FancytreeNode, type: TYPE} object for a mouse event.
	 *
	 * @param {Event} event Mouse event, e.g. click, ...
	 * @returns {object} Return a {node: FancytreeNode, type: TYPE} object
	 *     TYPE: 'title' | 'prefix' | 'expander' | 'checkbox' | 'icon' | undefined
	 */
	getEventTarget: function(event){
		var tcn = event && event.target ? event.target.className : "",
			res = {node: this.getNode(event.target), type: undefined};
		// We use a fast version of $(res.node).hasClass()
		// See http://jsperf.com/test-for-classname/2
		if( /\bfancytree-title\b/.test(tcn) ){
			res.type = "title";
		}else if( /\bfancytree-expander\b/.test(tcn) ){
			res.type = (res.node.hasChildren() === false ? "prefix" : "expander");
		// }else if( /\bfancytree-checkbox\b/.test(tcn) || /\bfancytree-radio\b/.test(tcn) ){
		}else if( /\bfancytree-checkbox\b/.test(tcn) ){
			res.type = "checkbox";
		}else if( /\bfancytree-icon\b/.test(tcn) ){
			res.type = "icon";
		}else if( /\bfancytree-node\b/.test(tcn) ){
			// Somewhere near the title
			res.type = "title";
		}else if( event && event.target && $(event.target).closest(".fancytree-title").length ) {
			// #228: clicking an embedded element inside a title
			res.type = "title";
		}
		return res;
	},
	/** Return a FancytreeNode instance from element, event, or jQuery object.
	 *
	 * @param {Element | jQueryObject | Event} el
	 * @returns {FancytreeNode} matching node or null
	 */
	getNode: function(el){
		if(el instanceof FancytreeNode){
			return el; // el already was a FancytreeNode
		}else if( el instanceof jQuery ){
			el = el[0]; // el was a jQuery object: use the DOM element
		}else if(el.originalEvent !== undefined){
			el = el.target; // el was an Event
		}
		while( el ) {
			if(el.ftnode) {
				return el.ftnode;
			}
			el = el.parentNode;
		}
		return null;
	},
	/** Return a Fancytree instance, from element, index, event, or jQueryObject.
	 *
	 * @param {Element | jQueryObject | Event | integer | string} [el]
	 * @returns {Fancytree} matching tree or null
	 * @example
	 * $.ui.fancytree.getTree();   // Get first Fancytree instance on page
	 * $.ui.fancytree.getTree(1);  // Get second Fancytree instance on page
	 * $.ui.fancytree.getTree("#tree"); // Get tree for this matching element
	 *
	 * @since 2.13
	 */
	getTree: function(el){
		var widget;

		if( el instanceof Fancytree ) {
			return el; // el already was a Fancytree
		}
		if( el === undefined ) {
			el = 0;  // get first tree
		}
		if( typeof el === "number" ) {
			el = $(".fancytree-container").eq(el); // el was an integer: return nth instance
		} else if( typeof el === "string" ) {
			el = $(el).eq(0); // el was a selector: use first match
		} else if( el.selector !== undefined ) {
			el = el.eq(0); // el was a jQuery object: use the first DOM element
		} else if( el.originalEvent !== undefined ) {
			el = $(el.target); // el was an Event
		}
		el = el.closest(":ui-fancytree");
		widget = el.data("ui-fancytree") || el.data("fancytree"); // the latter is required by jQuery <= 1.8
		return widget ? widget.tree : null;
	},
	/** Return an option value that has a default, but may be overridden by a
	 * callback or a node instance attribute.
	 *
	 * Evaluation sequence:<br>
	 *
	 * If tree.options.<optionName> is a callback that returns something, use that.<br>
	 * Else if node.<optionName> is defined, use that.<br>
	 * Else if tree.options.<optionName> is a value, use that.<br>
	 * Else use `defaultValue`.
	 *
	 * @param {string} optionName name of the option property (on node and tree)
	 * @param {FancytreeNode} node passed to the callback
	 * @param {object} nodeObject where to look for the local option property, e.g. `node` or `node.data`
	 * @param {object} treeOption where to look for the tree option, e.g. `tree.options` or `tree.options.dnd5`
	 * @param {any} [defaultValue]
	 * @returns {any}
	 *
	 * @example
	 * // Check for node.foo, tree,options.foo(), and tree.options.foo:
	 * $.ui.fancytree.evalOption("foo", node, node, tree.options);
	 * // Check for node.data.bar, tree,options.qux.bar(), and tree.options.qux.bar:
	 * $.ui.fancytree.evalOption("bar", node, node.data, tree.options.qux);
	 *
	 * @since 2.22
	 */
	evalOption: function(optionName, node, nodeObject, treeOptions, defaultValue) {
		var ctx, res,
			tree = node.tree,
			treeOpt = treeOptions[optionName],
			nodeOpt = nodeObject[optionName];

		if( $.isFunction(treeOpt) ) {
			ctx = { node: node, tree: tree, widget: tree.widget, options: tree.widget.options };
			res = treeOpt.call(tree, {type: optionName}, ctx);
			if( res == null ) {
				res = nodeOpt;
			}
		} else {
			res = (nodeOpt != null) ? nodeOpt : treeOpt;
		}
		if( res == null ) {
			res = defaultValue;  // no option set at all: return default
		}
		return res;
	},
	/** Convert a keydown or mouse event to a canonical string like 'ctrl+a',
	 * 'ctrl+shift+f2', 'shift+leftdblclick'.
	 *
	 * This is especially handy for switch-statements in event handlers.
	 *
	 * @param {event}
	 * @returns {string}
	 *
	 * @example

	switch( $.ui.fancytree.eventToString(event) ) {
		case "-":
			tree.nodeSetExpanded(ctx, false);
			break;
		case "shift+return":
			tree.nodeSetActive(ctx, true);
			break;
		case "down":
			res = node.navigate(event.which, activate, true);
			break;
		default:
			handled = false;
	}
	if( handled ){
		event.preventDefault();
	}
	 */
	eventToString: function(event) {
		// Poor-man's hotkeys. See here for a complete implementation:
		//   https://github.com/jeresig/jquery.hotkeys
		var which = event.which,
			et = event.type,
			s = [];

		if( event.altKey ) { s.push("alt"); }
		if( event.ctrlKey ) { s.push("ctrl"); }
		if( event.metaKey ) { s.push("meta"); }
		if( event.shiftKey ) { s.push("shift"); }

		if( et === "click" || et === "dblclick" ) {
			s.push(MOUSE_BUTTONS[event.button] + et);
		} else {
			if( !IGNORE_KEYCODES[which] ) {
				s.push( SPECIAL_KEYCODES[which] || String.fromCharCode(which).toLowerCase() );
			}
		}
		return s.join("+");
	},
	/** Write message to console if debugLevel >= 1
	 * @param {string} msg
	 */
	info: function(msg){
		/*jshint expr:true */
		($.ui.fancytree.debugLevel >= 1) && consoleApply("info", arguments);
	},
	/* @deprecated: use eventToString(event) instead.
	 */
	keyEventToString: function(event) {
		this.warn("keyEventToString() is deprecated: use eventToString()");
		return this.eventToString(event);
	},
	/** Return a wrapped handler method, that provides `this.super`.
	 *
	 * @example
		// Implement `opts.createNode` event to add the 'draggable' attribute
		$.ui.fancytree.overrideMethod(ctx.options, "createNode", function(event, data) {
			// Default processing if any
			this._super.apply(this, arguments);
			// Add 'draggable' attribute
			data.node.span.draggable = true;
		});
	 *
	 * @param {object} instance
	 * @param {string} methodName
	 * @param {function} handler
	 */
	overrideMethod: function(instance, methodName, handler){
		var prevSuper,
			_super = instance[methodName] || $.noop;

		// context = context || this;

		instance[methodName] = function() {
			try {
				prevSuper = this._super;
				this._super = _super;
				return handler.apply(this, arguments);
			} finally {
				this._super = prevSuper;
			}
		};
	},
	/**
	 * Parse tree data from HTML <ul> markup
	 *
	 * @param {jQueryObject} $ul
	 * @returns {NodeData[]}
	 */
	parseHtml: function($ul) {
		// TODO: understand this:
		/*jshint validthis:true */
		var classes, className, extraClasses, i, iPos, l, tmp, tmp2,
			$children = $ul.find(">li"),
			children = [];

		$children.each(function() {
			var allData, lowerCaseAttr,
				$li = $(this),
				$liSpan = $li.find(">span:first", this),
				$liA = $liSpan.length ? null : $li.find(">a:first"),
				d = { tooltip: null, data: {} };

			if( $liSpan.length ) {
				d.title = $liSpan.html();

			} else if( $liA && $liA.length ) {
				// If a <li><a> tag is specified, use it literally and extract href/target.
				d.title = $liA.html();
				d.data.href = $liA.attr("href");
				d.data.target = $liA.attr("target");
				d.tooltip = $liA.attr("title");

			} else {
				// If only a <li> tag is specified, use the trimmed string up to
				// the next child <ul> tag.
				d.title = $li.html();
				iPos = d.title.search(/<ul/i);
				if( iPos >= 0 ){
					d.title = d.title.substring(0, iPos);
				}
			}
			d.title = $.trim(d.title);

			// Make sure all fields exist
			for(i=0, l=CLASS_ATTRS.length; i<l; i++){
				d[CLASS_ATTRS[i]] = undefined;
			}
			// Initialize to `true`, if class is set and collect extraClasses
			classes = this.className.split(" ");
			extraClasses = [];
			for(i=0, l=classes.length; i<l; i++){
				className = classes[i];
				if(CLASS_ATTR_MAP[className]){
					d[className] = true;
				}else{
					extraClasses.push(className);
				}
			}
			d.extraClasses = extraClasses.join(" ");

			// Parse node options from ID, title and class attributes
			tmp = $li.attr("title");
			if( tmp ){
				d.tooltip = tmp; // overrides <a title='...'>
			}
			tmp = $li.attr("id");
			if( tmp ){
				d.key = tmp;
			}
			// Translate hideCheckbox -> checkbox:false
			if( $li.attr("hideCheckbox") ){
				d.checkbox = false;
			}
			// Add <li data-NAME='...'> as node.data.NAME
			allData = _getElementDataAsDict($li);
			if( allData && !$.isEmptyObject(allData) ) {
				// #507: convert data-hidecheckbox (lower case) to hideCheckbox
				for( lowerCaseAttr in NODE_ATTR_LOWERCASE_MAP ) {
					if( allData.hasOwnProperty(lowerCaseAttr) ) {
						allData[NODE_ATTR_LOWERCASE_MAP[lowerCaseAttr]] = allData[lowerCaseAttr];
						delete allData[lowerCaseAttr];
					}
				}
				// #56: Allow to set special node.attributes from data-...
				for(i=0, l=NODE_ATTRS.length; i<l; i++){
					tmp = NODE_ATTRS[i];
					tmp2 = allData[tmp];
					if( tmp2 != null ) {
						delete allData[tmp];
						d[tmp] = tmp2;
					}
				}
				// All other data-... goes to node.data...
				$.extend(d.data, allData);
			}
			// Recursive reading of child nodes, if LI tag contains an UL tag
			$ul = $li.find(">ul:first");
			if( $ul.length ) {
				d.children = $.ui.fancytree.parseHtml($ul);
			}else{
				d.children = d.lazy ? undefined : null;
			}
			children.push(d);
//            FT.debug("parse ", d, children);
		});
		return children;
	},
	/** Add Fancytree extension definition to the list of globally available extensions.
	 *
	 * @param {object} definition
	 */
	registerExtension: function(definition){
		_assert(definition.name != null, "extensions must have a `name` property.");
		_assert(definition.version != null, "extensions must have a `version` property.");
		$.ui.fancytree._extensions[definition.name] = definition;
	},
	/** Inverse of escapeHtml().
	 *
	 * @param {string} s
	 * @returns {string}
	 */
	unescapeHtml: function(s){
		var e = document.createElement("div");
		e.innerHTML = s;
		return e.childNodes.length === 0 ? "" : e.childNodes[0].nodeValue;
	},
	/** Write warning message to console.
	 * @param {string} msg
	 */
	warn: function(msg){
		consoleApply("warn", arguments);
	}
});

}(jQuery, window, document));

// Extending Fancytree
// ===================
//
// See also the [live demo](http://wwwendt.de/tech/fancytree/demo/sample-ext-childcounter.html) of this code.
//
// Every extension should have a comment header containing some information
// about the author, copyright and licensing. Also a pointer to the latest
// source code.
// Prefix with `/*!` so the comment is not removed by the minifier.

/*!
 * jquery.fancytree.childcounter.js
 *
 * Add a child counter bubble to tree nodes.
 * (Extension module for jquery.fancytree.js: https://github.com/mar10/fancytree/)
 *
 * Copyright (c) 2008-2017, Martin Wendt (http://wwWendt.de)
 *
 * Released under the MIT license
 * https://github.com/mar10/fancytree/wiki/LicenseInfo
 *
 * @version 2.23.0
 * @date 2017-05-27T20:09:38Z
 */

// To keep the global namespace clean, we wrap everything in a closure

;(function($, undefined) {

// Consider to use [strict mode](http://ejohn.org/blog/ecmascript-5-strict-mode-json-and-more/)
"use strict";

// The [coding guidelines](http://contribute.jquery.org/style-guide/js/)
// require jshint compliance.
// But for this sample, we want to allow unused variables for demonstration purpose.

/*jshint unused:false */


// Adding methods
// --------------

// New member functions can be added to the `Fancytree` class.
// This function will be available for every tree instance:
//
//     var tree = $("#tree").fancytree("getTree");
//     tree.countSelected(false);

$.ui.fancytree._FancytreeClass.prototype.countSelected = function(topOnly){
	var tree = this,
		treeOptions = tree.options;

	return tree.getSelectedNodes(topOnly).length;
};


// The `FancytreeNode` class can also be easily extended. This would be called
// like
//     node.updateCounters();
//
// It is also good practice to add a docstring comment.
/**
 * [ext-childcounter] Update counter badges for `node` and its parents.
 * May be called in the `loadChildren` event, to update parents of lazy loaded
 * nodes.
 * @alias FancytreeNode#updateCounters
 * @requires jquery.fancytree.childcounters.js
 */
$.ui.fancytree._FancytreeNodeClass.prototype.updateCounters = function(){
	var node = this,
		$badge = $("span.fancytree-childcounter", node.span),
		extOpts = node.tree.options.childcounter,
		count = node.countChildren(extOpts.deep);

	node.data.childCounter = count;
	if( (count || !extOpts.hideZeros) && (!node.isExpanded() || !extOpts.hideExpanded) ) {
		if( !$badge.length ) {
			$badge = $("<span class='fancytree-childcounter'/>").appendTo($("span.fancytree-icon", node.span));
		}
		$badge.text(count);
	} else {
		$badge.remove();
	}
	if( extOpts.deep && !node.isTopLevel() && !node.isRoot() ) {
		node.parent.updateCounters();
	}
};


// Finally, we can extend the widget API and create functions that are called
// like so:
//
//     $("#tree").fancytree("widgetMethod1", "abc");

$.ui.fancytree.prototype.widgetMethod1 = function(arg1){
	var tree = this.tree;
	return arg1;
};


// Register a Fancytree extension
// ------------------------------
// A full blown extension, extension is available for all trees and can be
// enabled like so (see also the [live demo](http://wwwendt.de/tech/fancytree/demo/sample-ext-childcounter.html)):
//
//    <script src="../src/jquery.fancytree.js"></script>
//    <script src="../src/jquery.fancytree.childcounter.js"></script>
//    ...
//
//     $("#tree").fancytree({
//         extensions: ["childcounter"],
//         childcounter: {
//             hideExpanded: true
//         },
//         ...
//     });
//


/* 'childcounter' extension */
$.ui.fancytree.registerExtension({
// Every extension must be registered by a unique name.
	name: "childcounter",
// Version information should be compliant with [semver](http://semver.org)
	version: "2.23.0",

// Extension specific options and their defaults.
// This options will be available as `tree.options.childcounter.hideExpanded`

	options: {
		deep: true,
		hideZeros: true,
		hideExpanded: false
	},

// Attributes other than `options` (or functions) can be defined here, and
// will be added to the tree.ext.EXTNAME namespace, in this case `tree.ext.childcounter.foo`.
// They can also be accessed as `this._local.foo` from within the extension
// methods.
	foo: 42,

// Local functions are prefixed with an underscore '_'.
// Callable as `this._local._appendCounter()`.

	_appendCounter: function(bar){
		var tree = this;
	},

// **Override virtual methods for this extension.**
//
// Fancytree implements a number of 'hook methods', prefixed by 'node...' or 'tree...'.
// with a `ctx` argument (see [EventData](http://www.wwwendt.de/tech/fancytree/doc/jsdoc/global.html#EventData)
// for details) and an extended calling context:<br>
// `this`       : the Fancytree instance<br>
// `this._local`: the namespace that contains extension attributes and private methods (same as this.ext.EXTNAME)<br>
// `this._super`: the virtual function that was overridden (member of previous extension or Fancytree)
//
// See also the [complete list of available hook functions](http://www.wwwendt.de/tech/fancytree/doc/jsdoc/Fancytree_Hooks.html).

	/* Init */
// `treeInit` is triggered when a tree is initalized. We can set up classes or
// bind event handlers here...
	treeInit: function(ctx){
		var tree = this, // same as ctx.tree,
			opts = ctx.options,
			extOpts = ctx.options.childcounter;
// Optionally check for dependencies with other extensions
		/* this._requireExtension("glyph", false, false); */
// Call the base implementation
		this._superApply(arguments);
// Add a class to the tree container
		this.$container.addClass("fancytree-ext-childcounter");
	},

// Destroy this tree instance (we only call the default implementation, so
// this method could as well be omitted).

	treeDestroy: function(ctx){
		this._superApply(arguments);
	},

// Overload the `renderTitle` hook, to append a counter badge
	nodeRenderTitle: function(ctx, title) {
		var node = ctx.node,
			extOpts = ctx.options.childcounter,
			count = (node.data.childCounter == null) ? node.countChildren(extOpts.deep) : +node.data.childCounter;
// Let the base implementation render the title
// We use `_super()` instead of `_superApply()` here, since it is a little bit
// more performant when called often
		this._super(ctx, title);
// Append a counter badge
		if( (count || ! extOpts.hideZeros) && (!node.isExpanded() || !extOpts.hideExpanded) ){
			$("span.fancytree-icon", node.span).append($("<span class='fancytree-childcounter'/>").text(count));
		}
	},
// Overload the `setExpanded` hook, so the counters are updated
	nodeSetExpanded: function(ctx, flag, callOpts) {
		var tree = ctx.tree,
			node = ctx.node;
// Let the base implementation expand/collapse the node, then redraw the title
// after the animation has finished
		return this._superApply(arguments).always(function(){
			tree.nodeRenderTitle(ctx);
		});
	}

// End of extension definition
});
// End of namespace closure
}(jQuery));

/*!
 *
 * jquery.fancytree.clones.js
 * Support faster lookup of nodes by key and shared ref-ids.
 * (Extension module for jquery.fancytree.js: https://github.com/mar10/fancytree/)
 *
 * Copyright (c) 2008-2017, Martin Wendt (http://wwWendt.de)
 *
 * Released under the MIT license
 * https://github.com/mar10/fancytree/wiki/LicenseInfo
 *
 * @version 2.23.0
 * @date 2017-05-27T20:09:38Z
 */

;(function($, window, document, undefined) {

"use strict";

/*******************************************************************************
 * Private functions and variables
 */
function _assert(cond, msg){
	// TODO: see qunit.js extractStacktrace()
	if(!cond){
		msg = msg ? ": " + msg : "";
		$.error("Assertion failed" + msg);
	}
}


/* Return first occurrence of member from array. */
function _removeArrayMember(arr, elem) {
	// TODO: use Array.indexOf for IE >= 9
	var i;
	for (i = arr.length - 1; i >= 0; i--) {
		if (arr[i] === elem) {
			arr.splice(i, 1);
			return true;
		}
	}
	return false;
}


// /**
//  * Calculate a 32 bit FNV-1a hash
//  * Found here: https://gist.github.com/vaiorabbit/5657561
//  * Ref.: http://isthe.com/chongo/tech/comp/fnv/
//  *
//  * @param {string} str the input value
//  * @param {boolean} [asString=false] set to true to return the hash value as
//  *     8-digit hex string instead of an integer
//  * @param {integer} [seed] optionally pass the hash of the previous chunk
//  * @returns {integer | string}
//  */
// function hashFnv32a(str, asString, seed) {
// 	/*jshint bitwise:false */
// 	var i, l,
// 		hval = (seed === undefined) ? 0x811c9dc5 : seed;

// 	for (i = 0, l = str.length; i < l; i++) {
// 		hval ^= str.charCodeAt(i);
// 		hval += (hval << 1) + (hval << 4) + (hval << 7) + (hval << 8) + (hval << 24);
// 	}
// 	if( asString ){
// 		// Convert to 8 digit hex string
// 		return ("0000000" + (hval >>> 0).toString(16)).substr(-8);
// 	}
// 	return hval >>> 0;
// }


/**
 * JS Implementation of MurmurHash3 (r136) (as of May 20, 2011)
 *
 * @author <a href="mailto:gary.court@gmail.com">Gary Court</a>
 * @see http://github.com/garycourt/murmurhash-js
 * @author <a href="mailto:aappleby@gmail.com">Austin Appleby</a>
 * @see http://sites.google.com/site/murmurhash/
 *
 * @param {string} key ASCII only
 * @param {boolean} [asString=false]
 * @param {number} seed Positive integer only
 * @return {number} 32-bit positive integer hash
 */
function hashMurmur3(key, asString, seed) {
	/*jshint bitwise:false */
	var h1b, k1,
		remainder = key.length & 3,
		bytes = key.length - remainder,
		h1 = seed,
		c1 = 0xcc9e2d51,
		c2 = 0x1b873593,
		i = 0;

	while (i < bytes) {
		k1 =
			((key.charCodeAt(i) & 0xff)) |
			((key.charCodeAt(++i) & 0xff) << 8) |
			((key.charCodeAt(++i) & 0xff) << 16) |
			((key.charCodeAt(++i) & 0xff) << 24);
		++i;

		k1 = ((((k1 & 0xffff) * c1) + ((((k1 >>> 16) * c1) & 0xffff) << 16))) & 0xffffffff;
		k1 = (k1 << 15) | (k1 >>> 17);
		k1 = ((((k1 & 0xffff) * c2) + ((((k1 >>> 16) * c2) & 0xffff) << 16))) & 0xffffffff;

		h1 ^= k1;
		h1 = (h1 << 13) | (h1 >>> 19);
		h1b = ((((h1 & 0xffff) * 5) + ((((h1 >>> 16) * 5) & 0xffff) << 16))) & 0xffffffff;
		h1 = (((h1b & 0xffff) + 0x6b64) + ((((h1b >>> 16) + 0xe654) & 0xffff) << 16));
	}

	k1 = 0;

	switch (remainder) {
		/*jshint -W086:true */
		case 3: k1 ^= (key.charCodeAt(i + 2) & 0xff) << 16;
		case 2: k1 ^= (key.charCodeAt(i + 1) & 0xff) << 8;
		case 1: k1 ^= (key.charCodeAt(i) & 0xff);

		k1 = (((k1 & 0xffff) * c1) + ((((k1 >>> 16) * c1) & 0xffff) << 16)) & 0xffffffff;
		k1 = (k1 << 15) | (k1 >>> 17);
		k1 = (((k1 & 0xffff) * c2) + ((((k1 >>> 16) * c2) & 0xffff) << 16)) & 0xffffffff;
		h1 ^= k1;
	}

	h1 ^= key.length;

	h1 ^= h1 >>> 16;
	h1 = (((h1 & 0xffff) * 0x85ebca6b) + ((((h1 >>> 16) * 0x85ebca6b) & 0xffff) << 16)) & 0xffffffff;
	h1 ^= h1 >>> 13;
	h1 = ((((h1 & 0xffff) * 0xc2b2ae35) + ((((h1 >>> 16) * 0xc2b2ae35) & 0xffff) << 16))) & 0xffffffff;
	h1 ^= h1 >>> 16;

	if( asString ){
		// Convert to 8 digit hex string
		return ("0000000" + (h1 >>> 0).toString(16)).substr(-8);
	}
	return h1 >>> 0;
}

// console.info(hashMurmur3("costarring"));
// console.info(hashMurmur3("costarring", true));
// console.info(hashMurmur3("liquid"));
// console.info(hashMurmur3("liquid", true));


/*
 * Return a unique key for node by calculationg the hash of the parents refKey-list
 */
function calcUniqueKey(node) {
	var key,
		path = $.map(node.getParentList(false, true), function(e){ return e.refKey || e.key; });
	path = path.join("/");
	key = "id_" + hashMurmur3(path, true);
	// node.debug(path + " -> " + key);
	return key;
}


/**
 * [ext-clones] Return a list of clone-nodes or null.
 * @param {boolean} [includeSelf=false]
 * @returns {FancytreeNode[] | null}
 *
 * @alias FancytreeNode#getCloneList
 * @requires jquery.fancytree.clones.js
 */
$.ui.fancytree._FancytreeNodeClass.prototype.getCloneList = function(includeSelf){
	var key,
		tree = this.tree,
		refList = tree.refMap[this.refKey] || null,
		keyMap = tree.keyMap;

	if( refList ) {
		key = this.key;
		// Convert key list to node list
		if( includeSelf ) {
			refList = $.map(refList, function(val){ return keyMap[val]; });
		} else {
			refList = $.map(refList, function(val){ return val === key ? null : keyMap[val]; });
			if( refList.length < 1 ) {
				refList = null;
			}
		}
	}
	return refList;
};


/**
 * [ext-clones] Return true if this node has at least another clone with same refKey.
 * @returns {boolean}
 *
 * @alias FancytreeNode#isClone
 * @requires jquery.fancytree.clones.js
 */
$.ui.fancytree._FancytreeNodeClass.prototype.isClone = function(){
	var refKey = this.refKey || null,
		refList = refKey && this.tree.refMap[refKey] || null;
	return !!(refList && refList.length > 1);
};


/**
 * [ext-clones] Update key and/or refKey for an existing node.
 * @param {string} key
 * @param {string} refKey
 * @returns {boolean}
 *
 * @alias FancytreeNode#reRegister
 * @requires jquery.fancytree.clones.js
 */
$.ui.fancytree._FancytreeNodeClass.prototype.reRegister = function(key, refKey){
	key = (key == null) ? null :  "" + key;
	refKey = (refKey == null) ? null :  "" + refKey;
	// this.debug("reRegister", key, refKey);

	var tree = this.tree,
		prevKey = this.key,
		prevRefKey = this.refKey,
		keyMap = tree.keyMap,
		refMap = tree.refMap,
		refList = refMap[prevRefKey] || null,
//		curCloneKeys = refList ? node.getCloneList(true),
		modified = false;

	// Key has changed: update all references
	if( key != null && key !== this.key ) {
		if( keyMap[key] ) {
			$.error("[ext-clones] reRegister(" + key + "): already exists: " + this);
		}
		// Update keyMap
		delete keyMap[prevKey];
		keyMap[key] = this;
		// Update refMap
		if( refList ) {
			refMap[prevRefKey] = $.map(refList, function(e){
				return e === prevKey ? key : e;
			});
		}
		this.key = key;
		modified = true;
	}

	// refKey has changed
	if( refKey != null && refKey !== this.refKey ) {
		// Remove previous refKeys
		if( refList ){
			if( refList.length === 1 ){
				delete refMap[prevRefKey];
			}else{
				refMap[prevRefKey] = $.map(refList, function(e){
					return e === prevKey ? null : e;
				});
			}
		}
		// Add refKey
		if( refMap[refKey] ) {
			refMap[refKey].append(key);
		}else{
			refMap[refKey] = [ this.key ];
		}
		this.refKey = refKey;
		modified = true;
	}
	return modified;
};


/**
 * [ext-clones] Define a refKey for an existing node.
 * @param {string} refKey
 * @returns {boolean}
 *
 * @alias FancytreeNode#setRefKey
 * @requires jquery.fancytree.clones.js
 * @since 2.16
 */
$.ui.fancytree._FancytreeNodeClass.prototype.setRefKey = function(refKey){
	return this.reRegister(null, refKey);
};


/**
 * [ext-clones] Return all nodes with a given refKey (null if not found).
 * @param {string} refKey
 * @param {FancytreeNode} [rootNode] optionally restrict results to descendants of this node
 * @returns {FancytreeNode[] | null}
 * @alias Fancytree#getNodesByRef
 * @requires jquery.fancytree.clones.js
 */
$.ui.fancytree._FancytreeClass.prototype.getNodesByRef = function(refKey, rootNode){
	var keyMap = this.keyMap,
		refList = this.refMap[refKey] || null;

	if( refList ) {
		// Convert key list to node list
		if( rootNode ) {
			refList = $.map(refList, function(val){
				var node = keyMap[val];
				return node.isDescendantOf(rootNode) ? node : null;
			});
		}else{
			refList = $.map(refList, function(val){ return keyMap[val]; });
		}
		if( refList.length < 1 ) {
			refList = null;
		}
	}
	return refList;
};


/**
 * [ext-clones] Replace a refKey with a new one.
 * @param {string} oldRefKey
 * @param {string} newRefKey
 * @alias Fancytree#changeRefKey
 * @requires jquery.fancytree.clones.js
 */
$.ui.fancytree._FancytreeClass.prototype.changeRefKey = function(oldRefKey, newRefKey) {
	var i, node,
		keyMap = this.keyMap,
		refList = this.refMap[oldRefKey] || null;

	if (refList) {
		for (i = 0; i < refList.length; i++) {
			node = keyMap[refList[i]];
			node.refKey = newRefKey;
		}
		delete this.refMap[oldRefKey];
		this.refMap[newRefKey] = refList;
	}
};


/*******************************************************************************
 * Extension code
 */
$.ui.fancytree.registerExtension({
	name: "clones",
	version: "2.23.0",
	// Default options for this extension.
	options: {
		highlightActiveClones: true, // set 'fancytree-active-clone' on active clones and all peers
		highlightClones: false       // set 'fancytree-clone' class on any node that has at least one clone
	},

	treeCreate: function(ctx){
		this._superApply(arguments);
		ctx.tree.refMap = {};
		ctx.tree.keyMap = {};
	},
	treeInit: function(ctx){
		this.$container.addClass("fancytree-ext-clones");
		_assert(ctx.options.defaultKey == null);
		// Generate unique / reproducible default keys
		ctx.options.defaultKey = function(node){
			return calcUniqueKey(node);
		};
		// The default implementation loads initial data
		this._superApply(arguments);
	},
	treeClear: function(ctx){
		ctx.tree.refMap = {};
		ctx.tree.keyMap = {};
		return this._superApply(arguments);
	},
	treeRegisterNode: function(ctx, add, node) {
		var refList, len,
			tree = ctx.tree,
			keyMap = tree.keyMap,
			refMap = tree.refMap,
			key = node.key,
			refKey = (node && node.refKey != null) ? "" + node.refKey : null;

//		ctx.tree.debug("clones.treeRegisterNode", add, node);

		if( node.isStatusNode() ){
			return this._super(ctx, add, node);
		}

		if( add ) {
			if( keyMap[node.key] != null ) {
				$.error("clones.treeRegisterNode: node.key already exists: " + node);
			}
			keyMap[key] = node;
			if( refKey ) {
				refList = refMap[refKey];
				if( refList ) {
					refList.push(key);
					if( refList.length === 2 && ctx.options.clones.highlightClones ) {
						// Mark peer node, if it just became a clone (no need to
						// mark current node, since it will be rendered later anyway)
						keyMap[refList[0]].renderStatus();
					}
				} else {
					refMap[refKey] = [key];
				}
				// node.debug("clones.treeRegisterNode: add clone =>", refMap[refKey]);
			}
		}else {
			if( keyMap[key] == null ) {
				$.error("clones.treeRegisterNode: node.key not registered: " + node.key);
			}
			delete keyMap[key];
			if( refKey ) {
				refList = refMap[refKey];
				// node.debug("clones.treeRegisterNode: remove clone BEFORE =>", refMap[refKey]);
				if( refList ) {
					len = refList.length;
					if( len <= 1 ){
						_assert(len === 1);
						_assert(refList[0] === key);
						delete refMap[refKey];
					}else{
						_removeArrayMember(refList, key);
						// Unmark peer node, if this was the only clone
						if( len === 2 && ctx.options.clones.highlightClones ) {
//							node.debug("clones.treeRegisterNode: last =>", node.getCloneList());
							keyMap[refList[0]].renderStatus();
						}
					}
					// node.debug("clones.treeRegisterNode: remove clone =>", refMap[refKey]);
				}
			}
		}
		return this._super(ctx, add, node);
	},
	nodeRenderStatus: function(ctx) {
		var $span, res,
			node = ctx.node;

		res = this._super(ctx);

		if( ctx.options.clones.highlightClones ) {
			$span = $(node[ctx.tree.statusClassPropName]);
			// Only if span already exists
			if( $span.length && node.isClone() ){
//				node.debug("clones.nodeRenderStatus: ", ctx.options.clones.highlightClones);
				$span.addClass("fancytree-clone");
			}
		}
		return res;
	},
	nodeSetActive: function(ctx, flag, callOpts) {
		var res,
			scpn = ctx.tree.statusClassPropName,
			node = ctx.node;

		res = this._superApply(arguments);

		if( ctx.options.clones.highlightActiveClones && node.isClone() ) {
			$.each(node.getCloneList(true), function(idx, n){
				// n.debug("clones.nodeSetActive: ", flag !== false);
				$(n[scpn]).toggleClass("fancytree-active-clone", flag !== false);
			});
		}
		return res;
	}
});
}(jQuery, window, document));

/*!
 * jquery.fancytree.dnd.js
 *
 * Drag-and-drop support (jQuery UI draggable/droppable).
 * (Extension module for jquery.fancytree.js: https://github.com/mar10/fancytree/)
 *
 * Copyright (c) 2008-2017, Martin Wendt (http://wwWendt.de)
 *
 * Released under the MIT license
 * https://github.com/mar10/fancytree/wiki/LicenseInfo
 *
 * @version 2.23.0
 * @date 2017-05-27T20:09:38Z
 */

;(function($, window, document, undefined) {

"use strict";

/* *****************************************************************************
 * Private functions and variables
 */
var didRegisterDnd = false,
	classDropAccept = "fancytree-drop-accept",
	classDropAfter = "fancytree-drop-after",
	classDropBefore = "fancytree-drop-before",
	classDropOver = "fancytree-drop-over",
	classDropReject = "fancytree-drop-reject",
	classDropTarget = "fancytree-drop-target";

/* Convert number to string and prepend +/-; return empty string for 0.*/
function offsetString(n){
	return n === 0 ? "" : (( n > 0 ) ? ("+" + n) : ("" + n));
}

//--- Extend ui.draggable event handling --------------------------------------

function _registerDnd() {
	if(didRegisterDnd){
		return;
	}

	// Register proxy-functions for draggable.start/drag/stop

	$.ui.plugin.add("draggable", "connectToFancytree", {
		start: function(event, ui) {
			// 'draggable' was renamed to 'ui-draggable' since jQueryUI 1.10
			var draggable = $(this).data("ui-draggable") || $(this).data("draggable"),
				sourceNode = ui.helper.data("ftSourceNode") || null;

			if(sourceNode) {
				// Adjust helper offset, so cursor is slightly outside top/left corner
				draggable.offset.click.top = -2;
				draggable.offset.click.left = + 16;
				// Trigger dragStart event
				// TODO: when called as connectTo..., the return value is ignored(?)
				return sourceNode.tree.ext.dnd._onDragEvent("start", sourceNode, null, event, ui, draggable);
			}
		},
		drag: function(event, ui) {
			var ctx, isHelper, logObject,
				// 'draggable' was renamed to 'ui-draggable' since jQueryUI 1.10
				draggable = $(this).data("ui-draggable") || $(this).data("draggable"),
				sourceNode = ui.helper.data("ftSourceNode") || null,
				prevTargetNode = ui.helper.data("ftTargetNode") || null,
				targetNode = $.ui.fancytree.getNode(event.target),
				dndOpts = sourceNode && sourceNode.tree.options.dnd;

			// logObject = sourceNode || prevTargetNode || $.ui.fancytree;
			// logObject.debug("Drag event:", event, event.shiftKey);
			if(event.target && !targetNode){
				// We got a drag event, but the targetNode could not be found
				// at the event location. This may happen,
				// 1. if the mouse jumped over the drag helper,
				// 2. or if a non-fancytree element is dragged
				// We ignore it:
				isHelper = $(event.target).closest("div.fancytree-drag-helper,#fancytree-drop-marker").length > 0;
				if(isHelper){
					logObject = sourceNode || prevTargetNode || $.ui.fancytree;
					logObject.debug("Drag event over helper: ignored.");
					return;
				}
			}
			ui.helper.data("ftTargetNode", targetNode);

			if( dndOpts && dndOpts.updateHelper ) {
				ctx = sourceNode.tree._makeHookContext(sourceNode, event, {
					otherNode: targetNode,
					ui: ui,
					draggable: draggable,
					dropMarker: $("#fancytree-drop-marker")
				});
				dndOpts.updateHelper.call(sourceNode.tree, sourceNode, ctx);
			}

			// Leaving a tree node
			if(prevTargetNode && prevTargetNode !== targetNode ) {
				prevTargetNode.tree.ext.dnd._onDragEvent("leave", prevTargetNode, sourceNode, event, ui, draggable);
			}
			if(targetNode){
				if(!targetNode.tree.options.dnd.dragDrop) {
					// not enabled as drop target
				} else if(targetNode === prevTargetNode) {
					// Moving over same node
					targetNode.tree.ext.dnd._onDragEvent("over", targetNode, sourceNode, event, ui, draggable);
				}else{
					// Entering this node first time
					targetNode.tree.ext.dnd._onDragEvent("enter", targetNode, sourceNode, event, ui, draggable);
					targetNode.tree.ext.dnd._onDragEvent("over", targetNode, sourceNode, event, ui, draggable);
				}
			}
			// else go ahead with standard event handling
		},
		stop: function(event, ui) {
			var logObject,
				// 'draggable' was renamed to 'ui-draggable' since jQueryUI 1.10:
				draggable = $(this).data("ui-draggable") || $(this).data("draggable"),
				sourceNode = ui.helper.data("ftSourceNode") || null,
				targetNode = ui.helper.data("ftTargetNode") || null,
				dropped = (event.type === "mouseup" && event.which === 1);

			if(!dropped){
				logObject = sourceNode || targetNode || $.ui.fancytree;
				logObject.debug("Drag was cancelled");
			}
			if(targetNode) {
				if(dropped){
					targetNode.tree.ext.dnd._onDragEvent("drop", targetNode, sourceNode, event, ui, draggable);
				}
				targetNode.tree.ext.dnd._onDragEvent("leave", targetNode, sourceNode, event, ui, draggable);
			}
			if(sourceNode){
				sourceNode.tree.ext.dnd._onDragEvent("stop", sourceNode, null, event, ui, draggable);
			}
		}
	});

	didRegisterDnd = true;
}


/* *****************************************************************************
 * Drag and drop support
 */
function _initDragAndDrop(tree) {
	var dnd = tree.options.dnd || null,
		glyph = tree.options.glyph || null;

	// Register 'connectToFancytree' option with ui.draggable
	if( dnd ) {
		_registerDnd();
	}
	// Attach ui.draggable to this Fancytree instance
	if(dnd && dnd.dragStart ) {
		tree.widget.element.draggable($.extend({
			addClasses: false,
			// DT issue 244: helper should be child of scrollParent:
			appendTo: tree.$container,
//			appendTo: "body",
			containment: false,
//			containment: "parent",
			delay: 0,
			distance: 4,
			revert: false,
			scroll: true, // to disable, also set css 'position: inherit' on ul.fancytree-container
			scrollSpeed: 7,
			scrollSensitivity: 10,
			// Delegate draggable.start, drag, and stop events to our handler
			connectToFancytree: true,
			// Let source tree create the helper element
			helper: function(event) {
				var $helper, $nodeTag, opts,
					sourceNode = $.ui.fancytree.getNode(event.target);

				if(!sourceNode){
					// #405, DT issue 211: might happen, if dragging a table *header*
					return "<div>ERROR?: helper requested but sourceNode not found</div>";
				}
				opts = sourceNode.tree.options.dnd;
				$nodeTag = $(sourceNode.span);
				// Only event and node argument is available
				$helper = $("<div class='fancytree-drag-helper'><span class='fancytree-drag-helper-img' /></div>")
					.css({zIndex: 3, position: "relative"}) // so it appears above ext-wide selection bar
					.append($nodeTag.find("span.fancytree-title").clone());

				// Attach node reference to helper object
				$helper.data("ftSourceNode", sourceNode);

				// Support glyph symbols instead of icons
				if( glyph ) {
					$helper.find(".fancytree-drag-helper-img")
						.addClass(glyph.map.dragHelper);
				}
				// Allow to modify the helper, e.g. to add multi-node-drag feedback
				if( opts.initHelper ) {
					opts.initHelper.call(sourceNode.tree, sourceNode, {
						node: sourceNode,
						tree: sourceNode.tree,
						originalEvent: event,
						ui: { helper: $helper }
					});
				}
				// We return an unconnected element, so `draggable` will add this
				// to the parent specified as `appendTo` option
				return $helper;
			},
			start: function(event, ui) {
				var sourceNode = ui.helper.data("ftSourceNode");
				return !!sourceNode; // Abort dragging if no node could be found
			}
		}, tree.options.dnd.draggable));
	}
	// Attach ui.droppable to this Fancytree instance
	if(dnd && dnd.dragDrop) {
		tree.widget.element.droppable($.extend({
			addClasses: false,
			tolerance: "intersect",
			greedy: false
/*
			activate: function(event, ui) {
				tree.debug("droppable - activate", event, ui, this);
			},
			create: function(event, ui) {
				tree.debug("droppable - create", event, ui);
			},
			deactivate: function(event, ui) {
				tree.debug("droppable - deactivate", event, ui);
			},
			drop: function(event, ui) {
				tree.debug("droppable - drop", event, ui);
			},
			out: function(event, ui) {
				tree.debug("droppable - out", event, ui);
			},
			over: function(event, ui) {
				tree.debug("droppable - over", event, ui);
			}
*/
		}, tree.options.dnd.droppable));
	}
}


/* *****************************************************************************
 *
 */

$.ui.fancytree.registerExtension({
	name: "dnd",
	version: "2.23.0",
	// Default options for this extension.
	options: {
		// Make tree nodes accept draggables
		autoExpandMS: 1000,  // Expand nodes after n milliseconds of hovering.
		draggable: null,     // Additional options passed to jQuery draggable
		droppable: null,     // Additional options passed to jQuery droppable
		focusOnClick: false, // Focus, although draggable cancels mousedown event (#270)
		preventVoidMoves: true, 	// Prevent dropping nodes 'before self', etc.
		preventRecursiveMoves: true,// Prevent dropping nodes on own descendants
		smartRevert: true,   // set draggable.revert = true if drop was rejected
		dropMarkerOffsetX: -24,			// absolute position offset for .fancytree-drop-marker relatively to ..fancytree-title (icon/img near a node accepting drop)
		dropMarkerInsertOffsetX: -16,	// additional offset for drop-marker with hitMode = "before"/"after"
		// Events (drag support)
		dragStart: null,     // Callback(sourceNode, data), return true, to enable dnd
		dragStop: null,      // Callback(sourceNode, data)
		initHelper: null,    // Callback(sourceNode, data)
		updateHelper: null,  // Callback(sourceNode, data)
		// Events (drop support)
		dragEnter: null,     // Callback(targetNode, data)
		dragOver: null,      // Callback(targetNode, data)
		dragExpand: null,    // Callback(targetNode, data), return false to prevent autoExpand
		dragDrop: null,      // Callback(targetNode, data)
		dragLeave: null      // Callback(targetNode, data)
	},

	treeInit: function(ctx){
		var tree = ctx.tree;
		this._superApply(arguments);
		// issue #270: draggable eats mousedown events
		if( tree.options.dnd.dragStart ){
			tree.$container.on("mousedown", function(event){
//				if( !tree.hasFocus() && ctx.options.dnd.focusOnClick ) {
				if( ctx.options.dnd.focusOnClick ) {  // #270
					var node = $.ui.fancytree.getNode(event);
					if (node){
						node.debug("Re-enable focus that was prevented by jQuery UI draggable.");
						// node.setFocus();
						// $(node.span).closest(":tabbable").focus();
						// $(event.target).trigger("focus");
						// $(event.target).closest(":tabbable").trigger("focus");
					}
					setTimeout(function() { // #300
						$(event.target).closest(":tabbable").focus();
					}, 10);
				}
			});
		}
		_initDragAndDrop(tree);
	},
	/* Display drop marker according to hitMode ('after', 'before', 'over'). */
	_setDndStatus: function(sourceNode, targetNode, helper, hitMode, accept) {
		var markerOffsetX,
			markerAt = "center",
			instData = this._local,
			dndOpt = this.options.dnd ,
			glyphOpt = this.options.glyph,
			$source = sourceNode ? $(sourceNode.span) : null,
			$target = $(targetNode.span),
			$targetTitle = $target.find("span.fancytree-title");

		if( !instData.$dropMarker ) {
			instData.$dropMarker = $("<div id='fancytree-drop-marker'></div>")
				.hide()
				.css({"z-index": 1000})
				.prependTo($(this.$div).parent());
//                .prependTo("body");

			if( glyphOpt ) {
				// instData.$dropMarker.addClass(glyph.map.dragHelper);
				instData.$dropMarker
					.addClass(glyphOpt.map.dropMarker);
			}
		}
		if( hitMode === "after" || hitMode === "before" || hitMode === "over" ){
			markerOffsetX = dndOpt.dropMarkerOffsetX || 0;
			switch(hitMode){
			case "before":
				markerAt = "top";
				markerOffsetX += (dndOpt.dropMarkerInsertOffsetX || 0);
				break;
			case "after":
				markerAt = "bottom";
				markerOffsetX += (dndOpt.dropMarkerInsertOffsetX || 0);
				break;
			}

			instData.$dropMarker
				.toggleClass(classDropAfter, hitMode === "after")
				.toggleClass(classDropOver, hitMode === "over")
				.toggleClass(classDropBefore, hitMode === "before")
				.show()
				.position($.ui.fancytree.fixPositionOptions({
					my: "left" + offsetString(markerOffsetX) + " center",
					at: "left " + markerAt,
					of: $targetTitle
					}));
		} else {
			instData.$dropMarker.hide();
		}
		if( $source ){
			$source
				.toggleClass(classDropAccept, accept === true)
				.toggleClass(classDropReject, accept === false);
		}
		$target
			.toggleClass(classDropTarget, hitMode === "after" || hitMode === "before" || hitMode === "over")
			.toggleClass(classDropAfter, hitMode === "after")
			.toggleClass(classDropBefore, hitMode === "before")
			.toggleClass(classDropAccept, accept === true)
			.toggleClass(classDropReject, accept === false);

		helper
			.toggleClass(classDropAccept, accept === true)
			.toggleClass(classDropReject, accept === false);
	},

	/*
	 * Handles drag'n'drop functionality.
	 *
	 * A standard jQuery drag-and-drop process may generate these calls:
	 *
	 * start:
	 *     _onDragEvent("start", sourceNode, null, event, ui, draggable);
	 * drag:
	 *     _onDragEvent("leave", prevTargetNode, sourceNode, event, ui, draggable);
	 *     _onDragEvent("over", targetNode, sourceNode, event, ui, draggable);
	 *     _onDragEvent("enter", targetNode, sourceNode, event, ui, draggable);
	 * stop:
	 *     _onDragEvent("drop", targetNode, sourceNode, event, ui, draggable);
	 *     _onDragEvent("leave", targetNode, sourceNode, event, ui, draggable);
	 *     _onDragEvent("stop", sourceNode, null, event, ui, draggable);
	 */
	_onDragEvent: function(eventName, node, otherNode, event, ui, draggable) {
		// if(eventName !== "over"){
		// 	this.debug("tree.ext.dnd._onDragEvent(%s, %o, %o) - %o", eventName, node, otherNode, this);
		// }
		var accept, nodeOfs, parentRect, rect, relPos, relPos2,
			enterResponse, hitMode, r,
			opts = this.options,
			dnd = opts.dnd,
			ctx = this._makeHookContext(node, event, {otherNode: otherNode, ui: ui, draggable: draggable}),
			res = null,
			that = this,
			$nodeTag = $(node.span);

		if( dnd.smartRevert ) {
			draggable.options.revert = "invalid";
		}

		switch (eventName) {

		case "start":
			if( node.isStatusNode() ) {
				res = false;
			} else if(dnd.dragStart) {
				res = dnd.dragStart(node, ctx);
			}
			if(res === false) {
				this.debug("tree.dragStart() cancelled");
				//draggable._clear();
				// NOTE: the return value seems to be ignored (drag is not cancelled, when false is returned)
				// TODO: call this._cancelDrag()?
				ui.helper.trigger("mouseup")
					.hide();
			} else {
				if( dnd.smartRevert ) {
					// #567, #593: fix revert position
					// rect = node.li.getBoundingClientRect();
					rect = node[ctx.tree.nodeContainerAttrName].getBoundingClientRect();
					parentRect = $(draggable.options.appendTo)[0].getBoundingClientRect();
					draggable.originalPosition.left = Math.max(0, rect.left - parentRect.left);
					draggable.originalPosition.top = Math.max(0, rect.top - parentRect.top);
				}
				$nodeTag.addClass("fancytree-drag-source");
				// Register global handlers to allow cancel
				$(document)
					.on("keydown.fancytree-dnd,mousedown.fancytree-dnd", function(event){
						// node.tree.debug("dnd global event", event.type, event.which);
						if( event.type === "keydown" && event.which === $.ui.keyCode.ESCAPE ) {
							that.ext.dnd._cancelDrag();
						} else if( event.type === "mousedown" ) {
							that.ext.dnd._cancelDrag();
						}
					});
			}
			break;

		case "enter":
			if(dnd.preventRecursiveMoves && node.isDescendantOf(otherNode)){
				r = false;
			}else{
				r = dnd.dragEnter ? dnd.dragEnter(node, ctx) : null;
			}
			if(!r){
				// convert null, undefined, false to false
				res = false;
			}else if ( $.isArray(r) ) {
				// TODO: also accept passing an object of this format directly
				res = {
					over: ($.inArray("over", r) >= 0),
					before: ($.inArray("before", r) >= 0),
					after: ($.inArray("after", r) >= 0)
				};
			}else{
				res = {
					over: ((r === true) || (r === "over")),
					before: ((r === true) || (r === "before")),
					after: ((r === true) || (r === "after"))
				};
			}
			ui.helper.data("enterResponse", res);
			// this.debug("helper.enterResponse: %o", res);
			break;

		case "over":
			enterResponse = ui.helper.data("enterResponse");
			hitMode = null;
			if(enterResponse === false){
				// Don't call dragOver if onEnter returned false.
//                break;
			} else if(typeof enterResponse === "string") {
				// Use hitMode from onEnter if provided.
				hitMode = enterResponse;
			} else {
				// Calculate hitMode from relative cursor position.
				nodeOfs = $nodeTag.offset();
				relPos = { x: event.pageX - nodeOfs.left,
						   y: event.pageY - nodeOfs.top };
				relPos2 = { x: relPos.x / $nodeTag.width(),
							y: relPos.y / $nodeTag.height() };

				if( enterResponse.after && relPos2.y > 0.75 ){
					hitMode = "after";
				} else if(!enterResponse.over && enterResponse.after && relPos2.y > 0.5 ){
					hitMode = "after";
				} else if(enterResponse.before && relPos2.y <= 0.25) {
					hitMode = "before";
				} else if(!enterResponse.over && enterResponse.before && relPos2.y <= 0.5) {
					hitMode = "before";
				} else if(enterResponse.over) {
					hitMode = "over";
				}
				// Prevent no-ops like 'before source node'
				// TODO: these are no-ops when moving nodes, but not in copy mode
				if( dnd.preventVoidMoves ){
					if(node === otherNode){
						this.debug("    drop over source node prevented");
						hitMode = null;
					}else if(hitMode === "before" && otherNode && node === otherNode.getNextSibling()){
						this.debug("    drop after source node prevented");
						hitMode = null;
					}else if(hitMode === "after" && otherNode && node === otherNode.getPrevSibling()){
						this.debug("    drop before source node prevented");
						hitMode = null;
					}else if(hitMode === "over" && otherNode && otherNode.parent === node && otherNode.isLastSibling() ){
						this.debug("    drop last child over own parent prevented");
						hitMode = null;
					}
				}
//                this.debug("hitMode: %s - %s - %s", hitMode, (node.parent === otherNode), node.isLastSibling());
				ui.helper.data("hitMode", hitMode);
			}
			// Auto-expand node (only when 'over' the node, not 'before', or 'after')
			if(hitMode !== "before" && hitMode !== "after" && dnd.autoExpandMS &&
				node.hasChildren() !== false && !node.expanded &&
				(!dnd.dragExpand || dnd.dragExpand(node, ctx) !== false)
				) {
				node.scheduleAction("expand", dnd.autoExpandMS);
			}
			if(hitMode && dnd.dragOver){
				// TODO: http://code.google.com/p/dynatree/source/detail?r=625
				ctx.hitMode = hitMode;
				res = dnd.dragOver(node, ctx);
			}
			accept = (res !== false && hitMode !== null);
			if( dnd.smartRevert ) {
				draggable.options.revert = !accept;
			}
			this._local._setDndStatus(otherNode, node, ui.helper, hitMode, accept);
			break;

		case "drop":
			hitMode = ui.helper.data("hitMode");
			if(hitMode && dnd.dragDrop){
				ctx.hitMode = hitMode;
				dnd.dragDrop(node, ctx);
			}
			break;

		case "leave":
			// Cancel pending expand request
			node.scheduleAction("cancel");
			ui.helper.data("enterResponse", null);
			ui.helper.data("hitMode", null);
			this._local._setDndStatus(otherNode, node, ui.helper, "out", undefined);
			if(dnd.dragLeave){
				dnd.dragLeave(node, ctx);
			}
			break;

		case "stop":
			$nodeTag.removeClass("fancytree-drag-source");
			$(document).off(".fancytree-dnd");
			if(dnd.dragStop){
				dnd.dragStop(node, ctx);
			}
			break;

		default:
			$.error("Unsupported drag event: " + eventName);
		}
		return res;
	},

	_cancelDrag: function() {
		 var dd = $.ui.ddmanager.current;
		 if(dd){
			 dd.cancel();
		 }
	}
});
}(jQuery, window, document));

/*!
 * jquery.fancytree.dnd5.js
 *
 * Drag-and-drop support (native HTML5).
 * (Extension module for jquery.fancytree.js: https://github.com/mar10/fancytree/)
 *
 * Copyright (c) 2008-2017, Martin Wendt (http://wwWendt.de)
 *
 * Released under the MIT license
 * https://github.com/mar10/fancytree/wiki/LicenseInfo
 *
 * @version 2.23.0
 * @date 2017-05-27T20:09:38Z
 */


 /*
 #TODO
   - glyph

	Compatiblity when dragging between *separate* windows:

		   Drag from Chrome   Edge    FF    IE11    Safari
	  To Chrome      ok       ok      ok    NO      ?
		 Edge        ok       ok      ok    NO      ?
		 FF          ok       ok      ok    NO      ?
		 IE 11       ok       ok      ok    ok      ?
		 Safari      ?        ?       ?     ?       ok

 */

;(function($, window, document, undefined) {

"use strict";

/* *****************************************************************************
 * Private functions and variables
 */
var
	classDragSource = "fancytree-drag-source",
	classDragRemove = "fancytree-drag-remove",
	classDropAccept = "fancytree-drop-accept",
	classDropAfter = "fancytree-drop-after",
	classDropBefore = "fancytree-drop-before",
	classDropOver = "fancytree-drop-over",
	classDropReject = "fancytree-drop-reject",
	classDropTarget = "fancytree-drop-target",
	nodeMimeType = "application/x-fancytree-node",
	$dropMarker = null,
	SOURCE_NODE = null,
	DRAG_ENTER_RESPONSE = null,
	LAST_HIT_MODE = null;

/* Convert number to string and prepend +/-; return empty string for 0.*/
function offsetString(n){
	return n === 0 ? "" : (( n > 0 ) ? ("+" + n) : ("" + n));
}

/* Convert a dragEnter() or dragOver() response to a canonical form.
 * Return false or plain object
 * @param {string|object|boolean} r
 * @return {object|false}
 */
function normalizeDragEnterResponse(r) {
	var res;

	if( !r ){
		return false;
	}
	if ( $.isPlainObject(r) ) {
		res = {
			over: !!r.over,
			before: !!r.before,
			after: !!r.after
		};
	}else if ( $.isArray(r) ) {
		res = {
			over: ($.inArray("over", r) >= 0),
			before: ($.inArray("before", r) >= 0),
			after: ($.inArray("after", r) >= 0)
		};
	}else{
		res = {
			over: ((r === true) || (r === "over")),
			before: ((r === true) || (r === "before")),
			after: ((r === true) || (r === "after"))
		};
	}
	if( Object.keys(res).length === 0 ) {
		return false;
	}
	// if( Object.keys(res).length === 1 ) {
	// 	res.unique = res[0];
	// }
	return res;
}

/* Implement auto scrolling when drag cursor is in top/bottom area of scroll parent. */
function autoScroll(tree, event) {
	var spOfs, scrollTop, delta,
		dndOpts = tree.options.dnd5,
		sp = tree.$scrollParent[0],
		sensitivity = dndOpts.scrollSensitivity,
		speed = dndOpts.scrollSpeed,
		scrolled = 0;

	if ( sp !== document && sp.tagName !== "HTML" ) {
		spOfs = tree.$scrollParent.offset();
		scrollTop = sp.scrollTop;
		if ( spOfs.top + sp.offsetHeight - event.pageY < sensitivity ) {
			delta = (sp.scrollHeight - tree.$scrollParent.innerHeight() - scrollTop);
			// console.log ("sp.offsetHeight: " + sp.offsetHeight
			// 	+ ", spOfs.top: " + spOfs.top
			// 	+ ", scrollTop: " + scrollTop
			// 	+ ", innerHeight: " + tree.$scrollParent.innerHeight()
			// 	+ ", scrollHeight: " + sp.scrollHeight
			// 	+ ", delta: " + delta
			// 	);
			if( delta > 0 ) {
				sp.scrollTop = scrolled = scrollTop + speed;
			}
		} else if ( scrollTop > 0 && event.pageY - spOfs.top < sensitivity ) {
			sp.scrollTop = scrolled = scrollTop - speed;
		}
	} else {
		scrollTop = $(document).scrollTop();
		if (scrollTop > 0 && event.pageY - scrollTop < sensitivity) {
			scrolled = scrollTop - speed;
			$(document).scrollTop(scrolled);
		} else if ($(window).height() - (event.pageY - scrollTop) < sensitivity) {
			scrolled = scrollTop + speed;
			$(document).scrollTop(scrolled);
		}
	}
	if( scrolled ) {
		tree.debug("autoScroll: " + scrolled + "px");
	}
	return scrolled;
}

/* Handle dragover event (fired every x ms) and return hitMode. */
function handleDragOver(event, data) {
	// Implement auto-scrolling
	if ( data.options.dnd5.scroll ) {
		autoScroll(data.tree, event);
	}
	// Bail out with previous response if we get an invalid dragover
	if( !data.node ) {
		data.tree.warn("Ignore dragover for non-node");  //, event, data);
		return LAST_HIT_MODE;
	}

	var markerOffsetX, nodeOfs, relPosY, //res,
		// eventHash = getEventHash(event),
		hitMode = null,
		tree = data.tree,
		options = tree.options,
		dndOpts = options.dnd5,
		targetNode = data.node,
		sourceNode = data.otherNode,
		markerAt = "center",
		// glyph = options.glyph || null,
		// $source = sourceNode ? $(sourceNode.span) : null,
		$target = $(targetNode.span),
		$targetTitle = $target.find("span.fancytree-title");

	if(DRAG_ENTER_RESPONSE === false){
		tree.warn("Ignore dragover, since dragenter returned false");  //, event, data);
		// $.error("assert failed: dragenter returned false");
		return false;
	} else if(typeof DRAG_ENTER_RESPONSE === "string") {
		$.error("assert failed: dragenter returned string");
		// Use hitMode from onEnter if provided.
		// hitMode = DRAG_ENTER_RESPONSE;
	} else {
		// Calculate hitMode from relative cursor position.
		nodeOfs = $target.offset();
		relPosY = (event.pageY - nodeOfs.top) / $target.height();

		if( DRAG_ENTER_RESPONSE.after && relPosY > 0.75 ){
			hitMode = "after";
		} else if(!DRAG_ENTER_RESPONSE.over && DRAG_ENTER_RESPONSE.after && relPosY > 0.5 ){
			hitMode = "after";
		} else if(DRAG_ENTER_RESPONSE.before && relPosY <= 0.25) {
			hitMode = "before";
		} else if(!DRAG_ENTER_RESPONSE.over && DRAG_ENTER_RESPONSE.before && relPosY <= 0.5) {
			hitMode = "before";
		} else if(DRAG_ENTER_RESPONSE.over) {
			hitMode = "over";
		}
		// Prevent no-ops like 'before source node'
		// TODO: these are no-ops when moving nodes, but not in copy mode
		if( dndOpts.preventVoidMoves ){
			if(targetNode === sourceNode){
				targetNode.debug("drop over source node prevented");
				hitMode = null;
			}else if(hitMode === "before" && sourceNode && targetNode === sourceNode.getNextSibling()){
				targetNode.debug("drop after source node prevented");
				hitMode = null;
			}else if(hitMode === "after" && sourceNode && targetNode === sourceNode.getPrevSibling()){
				targetNode.debug("drop before source node prevented");
				hitMode = null;
			}else if(hitMode === "over" && sourceNode && sourceNode.parent === targetNode && sourceNode.isLastSibling() ){
				targetNode.debug("drop last child over own parent prevented");
				hitMode = null;
			}
		}
	}
	// Let callback modify the calculated hitMode
	data.hitMode = hitMode;
	if(hitMode && dndOpts.dragOver){
		// TODO: http://code.google.com/p/dynatree/source/detail?r=625
		dndOpts.dragOver(targetNode, data);
		hitMode = data.hitMode;
	}
	// LAST_DROP_EFFECT = data.dataTransfer.dropEffect;
	// LAST_EFFECT_ALLOWED = data.dataTransfer.effectAllowed;
	LAST_HIT_MODE = hitMode;
	//
	if( hitMode === "after" || hitMode === "before" || hitMode === "over" ){
		markerOffsetX = dndOpts.dropMarkerOffsetX || 0;
		switch(hitMode){
		case "before":
			markerAt = "top";
			markerOffsetX += (dndOpts.dropMarkerInsertOffsetX || 0);
			break;
		case "after":
			markerAt = "bottom";
			markerOffsetX += (dndOpts.dropMarkerInsertOffsetX || 0);
			break;
		}

		$dropMarker
			.toggleClass(classDropAfter, hitMode === "after")
			.toggleClass(classDropOver, hitMode === "over")
			.toggleClass(classDropBefore, hitMode === "before")
			.show()
			.position($.ui.fancytree.fixPositionOptions({
				my: "left" + offsetString(markerOffsetX) + " center",
				at: "left " + markerAt,
				of: $targetTitle
				}));
	} else {
		$dropMarker.hide();
		// console.log("hide dropmarker")
	}
	// if( $source ){
	// 	$source.toggleClass(classDragRemove, isMove);
	// }
	$(targetNode.span)
		.toggleClass(classDropTarget, hitMode === "after" || hitMode === "before" || hitMode === "over")
		.toggleClass(classDropAfter, hitMode === "after")
		.toggleClass(classDropBefore, hitMode === "before")
		.toggleClass(classDropAccept, hitMode === "over")
		.toggleClass(classDropReject, hitMode === false);

	return hitMode;
}

/* *****************************************************************************
 *
 */

$.ui.fancytree.registerExtension({
	name: "dnd5",
	version: "2.23.0",
	// Default options for this extension.
	options: {
		autoExpandMS: 1500,          // Expand nodes after n milliseconds of hovering
		setTextTypeJson: false,      // Allow dragging of nodes to different IE windows
		preventForeignNodes: false,  // Prevent dropping nodes from different Fancytrees
		preventNonNodes: false,      // Prevent dropping items other than Fancytree nodes
		preventRecursiveMoves: true, // Prevent dropping nodes on own descendants
		preventVoidMoves: true,      // Prevent dropping nodes 'before self', etc.
		scroll: true,                // Enable auto-scrolling while dragging
		scrollSensitivity: 20,       // Active top/bottom margin in pixel
		scrollSpeed: 5,              // Pixel per event
		dropMarkerOffsetX: -24,		 // absolute position offset for .fancytree-drop-marker relatively to ..fancytree-title (icon/img near a node accepting drop)
		dropMarkerInsertOffsetX: -16,// additional offset for drop-marker with hitMode = "before"/"after"
		// Events (drag support)
		dragStart: null,       // Callback(sourceNode, data), return true, to enable dnd drag
		dragDrag: $.noop,      // Callback(sourceNode, data)
		dragEnd: $.noop,       // Callback(sourceNode, data)
		// Events (drop support)
		dragEnter: null,       // Callback(targetNode, data), return true, to enable dnd drop
		dragOver: $.noop,      // Callback(targetNode, data)
		dragExpand: $.noop,    // Callback(targetNode, data), return false to prevent autoExpand
		dragDrop: $.noop,      // Callback(targetNode, data)
		dragLeave: $.noop      // Callback(targetNode, data)
	},

	treeInit: function(ctx){
		var tree = ctx.tree,
			opts = ctx.options,
			dndOpts = opts.dnd5,
			getNode = $.ui.fancytree.getNode;

		if( $.inArray("dnd", opts.extensions) >= 0 ) {
			$.error("Extensions 'dnd' and 'dnd5' are mutually exclusive.");
		}
		if( dndOpts.dragStop ) {
			$.error("dragStop is not used by ext-dnd5. Use dragEnd instead.");
		}

		// Implement `opts.createNode` event to add the 'draggable' attribute
		// #680: this must happen before calling super.treeInit()
		if( dndOpts.dragStart ) {
			$.ui.fancytree.overrideMethod(ctx.options, "createNode", function(event, data) {
				// Default processing if any
				this._super.apply(this, arguments);

				data.node.span.draggable = true;
			});
		}
		this._superApply(arguments);

		this.$container.addClass("fancytree-ext-dnd5");

		// Store the current scroll parent, which may be the tree
		// container, any enclosing div, or the document
		this.$scrollParent = this.$container.children(":first").scrollParent();

		$dropMarker = $("#fancytree-drop-marker");
		if( !$dropMarker.length ) {
			$dropMarker = $("<div id='fancytree-drop-marker'></div>")
				.hide()
				.css({
					"z-index": 1000,
					// Drop marker should not steal dragenter/dragover events:
					"pointer-events": "none"
				}).prependTo("body");
				// if( glyph ) {
					// instData.$dropMarker
						// .addClass(glyph.map.dropMarker);
				// }
		}
		// Enable drag support if dragStart() is specified:
		if( dndOpts.dragStart ) {
			// Bind drag event handlers
			tree.$container.on("dragstart drag dragend", function(event){
				var json,
					node = getNode(event),
					dataTransfer = event.dataTransfer || event.originalEvent.dataTransfer,
					isMove = dataTransfer.dropEffect === "move",
					$source = node ? $(node.span) : null,
					data = {
						node: node,
						tree: tree,
						options: tree.options,
						originalEvent: event,
						dataTransfer: dataTransfer,
//						dropEffect: undefined,  // set by dragend
						isCancelled: undefined  // set by dragend
					};

				switch( event.type ) {

				case "dragstart":
					$(node.span).addClass(classDragSource);

					// Store current source node in different formats
					SOURCE_NODE = node;

					// Set payload
					// Note:
					// Transfer data is only accessible on dragstart and drop!
					// For all other events the formats and kinds in the drag
					// data store list of items representing dragged data can be
					// enumerated, but the data itself is unavailable and no new
					// data can be added.
					json = JSON.stringify(node.toDict());
					try {
						dataTransfer.setData(nodeMimeType, json);
						dataTransfer.setData("text/html", $(node.span).html());
						dataTransfer.setData("text/plain", node.title);
					} catch(ex) {
						// IE only accepts 'text' type
						tree.warn("Could not set data (IE only accepts 'text') - " + ex);
					}
					// We always need to set the 'text' type if we want to drag
					// Because IE 11 only accepts this single type.
					// If we pass JSON here, IE can can access all node properties,
					// even when the source lives in another window. (D'n'd inside
					// the same window will always work.)
					// The drawback is, that in this case ALL browsers will see
					// the JSON representation as 'text', so dragging
					// to a text field will insert the JSON string instead of
					// the node title.
					if( dndOpts.setTextTypeJson ) {
						dataTransfer.setData("text", json);
					} else {
						dataTransfer.setData("text", node.title);
					}

					// Set the allowed and current drag mode (move, copy, or link)
					dataTransfer.effectAllowed = "all";  // "copyMove"
					// dataTransfer.dropEffect = "move";

					// Set the title as drag image (otherwise it would contain the expander)
					if( dataTransfer.setDragImage ) {
						// IE 11 does not support this
						dataTransfer.setDragImage($(node.span).find(".fancytree-title")[0], -10, -10);
						// dataTransfer.setDragImage($(node.span)[0], -10, -10);
					}
					// Let user modify above settings
					return dndOpts.dragStart(node, data) !== false;

				case "drag":
					// Called every few miliseconds
					$source.toggleClass(classDragRemove, isMove);
					dndOpts.dragDrag(node, data);
					break;

				case "dragend":
					$(node.span).removeClass(classDragSource + " " + classDragRemove);
					SOURCE_NODE = null;
					DRAG_ENTER_RESPONSE = null;
//					data.dropEffect = dataTransfer.dropEffect;
					data.isCancelled = (dataTransfer.dropEffect === "none");
					$dropMarker.hide();
					dndOpts.dragEnd(node, data);
					break;
				}
			});
		}
		// Enable drop support if dragEnter() is specified:
		if( dndOpts.dragEnter ) {
			// Bind drop event handlers
			tree.$container.on("dragenter dragover dragleave drop", function(event){
				var json, nodeData, r, res,
					allowDrop = null,
					node = getNode(event),
					dataTransfer = event.dataTransfer || event.originalEvent.dataTransfer,
					// glyph = opts.glyph || null,
					data = {
						node: node,
						tree: tree,
						options: tree.options,
						hitMode: DRAG_ENTER_RESPONSE,
						originalEvent: event,
						dataTransfer: dataTransfer,
						otherNode: SOURCE_NODE || null,
						otherNodeData: null,    // set by drop event
						dropEffect: undefined,  // set by drop event
						isCancelled: undefined  // set by drop event
					};

				switch( event.type ) {

				case "dragenter":
					// The dragenter event is fired when a dragged element or
					// text selection enters a valid drop target.

					if( !node ) {
						// Sometimes we get dragenter for the container element
						tree.debug("Ignore non-node " + event.type + ": " + event.target.tagName + "." + event.target.className);
						DRAG_ENTER_RESPONSE = false;
						break;
					}

					$(node.span)
						.addClass(classDropOver)
						.removeClass(classDropAccept + " " + classDropReject);

					if( dndOpts.preventNonNodes && !nodeData ) {
						node.debug("Reject dropping a non-node");
						DRAG_ENTER_RESPONSE = false;
						break;
					} else if( dndOpts.preventForeignNodes && (!SOURCE_NODE || SOURCE_NODE.tree !== node.tree ) ) {
						node.debug("Reject dropping a foreign node");
						DRAG_ENTER_RESPONSE = false;
						break;
					}

					// NOTE: dragenter is fired BEFORE the dragleave event
					// of the previous element!
					// https://www.w3.org/Bugs/Public/show_bug.cgi?id=19041
					setTimeout(function(){
						// node.info("DELAYED " + event.type, event.target, DRAG_ENTER_RESPONSE);
						// Auto-expand node (only when 'over' the node, not 'before', or 'after')
						if( dndOpts.autoExpandMS &&
							node.hasChildren() !== false && !node.expanded &&
							(!dndOpts.dragExpand || dndOpts.dragExpand(node, data) !== false)
							) {
							node.scheduleAction("expand", dndOpts.autoExpandMS);
						}
					}, 0);

					$dropMarker.show();

					// Call dragEnter() to figure out if (and where) dropping is allowed
					if( dndOpts.preventRecursiveMoves && node.isDescendantOf(data.otherNode) ){
						res = false;
					}else{
						r = dndOpts.dragEnter(node, data);
						res = normalizeDragEnterResponse(r);
					}
					DRAG_ENTER_RESPONSE = res;

					allowDrop = res && ( res.over || res.before || res.after );
					break;

				case "dragover":
					// The dragover event is fired when an element or text
					// selection is being dragged over a valid drop target
					// (every few hundred milliseconds).
					LAST_HIT_MODE = handleDragOver(event, data);
					allowDrop = !!LAST_HIT_MODE;
					break;

				case "dragleave":
					// NOTE: dragleave is fired AFTER the dragenter event of the
					// FOLLOWING element.
					if( !node ) {
						tree.debug("Ignore non-node " + event.type + ": " + event.target.tagName + "." + event.target.className);
						break;
					}
					if( !$(node.span).hasClass(classDropOver) ) {
						node.debug("Ignore dragleave (multi)"); //, event.currentTarget);
						break;
					}
					$(node.span).removeClass(classDropOver + " " + classDropAccept + " " + classDropReject);
					node.scheduleAction("cancel");
					dndOpts.dragLeave(node, data);
					$dropMarker.hide();
					break;

				case "drop":
					// Data is only readable in the (dragenter and) drop event:

					if( $.inArray(nodeMimeType, dataTransfer.types) >= 0 ) {
						nodeData = dataTransfer.getData(nodeMimeType);
						tree.info(event.type + ": getData('application/x-fancytree-node'): '" + nodeData + "'");
					}
					if( !nodeData ) {
						// 1. Source is not a Fancytree node, or
						// 2. If the FT mime type was set, but returns '', this
						//    is probably IE 11 (which only supports 'text')
						nodeData = dataTransfer.getData("text");
						tree.info(event.type + ": getData('text'): '" + nodeData + "'");
					}
					if( nodeData ) {
						try {
							// 'text' type may contain JSON if IE is involved
							// and setTextTypeJson option was set
							json = JSON.parse(nodeData);
							if( json.title !== undefined ) {
								data.otherNodeData = json;
							}
						} catch(ex) {
							// assume 'text' type contains plain text, so `otherNodeData`
							// should not be set
						}
					}
					tree.debug(event.type + ": nodeData: '" + nodeData + "', otherNodeData: ", data.otherNodeData);

					$(node.span).removeClass(classDropOver + " " + classDropAccept + " " + classDropReject);
					$dropMarker.hide();

					data.hitMode = LAST_HIT_MODE;
					data.dropEffect = dataTransfer.dropEffect;
					data.isCancelled = data.dropEffect === "none";

					// Let user implement the actual drop operation
					dndOpts.dragDrop(node, data);

					// Prevent browser's default drop handling
					event.preventDefault();
					break;
				}
				// Dnd API madness: we must PREVENT default handling to enable dropping
				if( allowDrop ) {
					event.preventDefault();
					return false;
				}
			});
		}
	}
});
}(jQuery, window, document));

/*!
 * jquery.fancytree.edit.js
 *
 * Make node titles editable.
 * (Extension module for jquery.fancytree.js: https://github.com/mar10/fancytree/)
 *
 * Copyright (c) 2008-2017, Martin Wendt (http://wwWendt.de)
 *
 * Released under the MIT license
 * https://github.com/mar10/fancytree/wiki/LicenseInfo
 *
 * @version 2.23.0
 * @date 2017-05-27T20:09:38Z
 */

;(function($, window, document, undefined) {

"use strict";


/*******************************************************************************
 * Private functions and variables
 */

var isMac = /Mac/.test(navigator.platform),
	escapeHtml = $.ui.fancytree.escapeHtml,
	unescapeHtml = $.ui.fancytree.unescapeHtml;

/**
 * [ext-edit] Start inline editing of current node title.
 *
 * @alias FancytreeNode#editStart
 * @requires Fancytree
 */
$.ui.fancytree._FancytreeNodeClass.prototype.editStart = function(){
	var $input,
		node = this,
		tree = this.tree,
		local = tree.ext.edit,
		instOpts = tree.options.edit,
		$title = $(".fancytree-title", node.span),
		eventData = {
			node: node,
			tree: tree,
			options: tree.options,
			isNew: $(node[tree.statusClassPropName]).hasClass("fancytree-edit-new"),
			orgTitle: node.title,
			input: null,
			dirty: false
			};

	// beforeEdit may want to modify the title before editing
	if( instOpts.beforeEdit.call(node, {type: "beforeEdit"}, eventData) === false ) {
		return false;
	}
	$.ui.fancytree.assert(!local.currentNode, "recursive edit");
	local.currentNode = this;
	local.eventData = eventData;

	// Disable standard Fancytree mouse- and key handling
	tree.widget._unbind();
	// #116: ext-dnd prevents the blur event, so we have to catch outer clicks
	$(document).on("mousedown.fancytree-edit", function(event){
		if( ! $(event.target).hasClass("fancytree-edit-input") ){
			node.editEnd(true, event);
		}
	});

	// Replace node with <input>
	$input = $("<input />", {
		"class": "fancytree-edit-input",
		type: "text",
		value: tree.options.escapeTitles ? eventData.orgTitle : unescapeHtml(eventData.orgTitle)
	});
	local.eventData.input = $input;
	if ( instOpts.adjustWidthOfs != null ) {
		$input.width($title.width() + instOpts.adjustWidthOfs);
	}
	if ( instOpts.inputCss != null ) {
		$input.css(instOpts.inputCss);
	}

	$title.html($input);

	// Focus <input> and bind keyboard handler
	$input
		.focus()
		.change(function(event){
			$input.addClass("fancytree-edit-dirty");
		}).keydown(function(event){
			switch( event.which ) {
			case $.ui.keyCode.ESCAPE:
				node.editEnd(false, event);
				break;
			case $.ui.keyCode.ENTER:
				node.editEnd(true, event);
				return false; // so we don't start editmode on Mac
			}
			event.stopPropagation();
		}).blur(function(event){
			return node.editEnd(true, event);
		});

	instOpts.edit.call(node, {type: "edit"}, eventData);
};


/**
 * [ext-edit] Stop inline editing.
 * @param {Boolean} [applyChanges=false] false: cancel edit, true: save (if modified)
 * @alias FancytreeNode#editEnd
 * @requires jquery.fancytree.edit.js
 */
$.ui.fancytree._FancytreeNodeClass.prototype.editEnd = function(applyChanges, _event){
	var newVal,
		node = this,
		tree = this.tree,
		local = tree.ext.edit,
		eventData = local.eventData,
		instOpts = tree.options.edit,
		$title = $(".fancytree-title", node.span),
		$input = $title.find("input.fancytree-edit-input");

	if( instOpts.trim ) {
		$input.val($.trim($input.val()));
	}
	newVal = $input.val();

	eventData.dirty = ( newVal !== node.title );
	eventData.originalEvent = _event;

	// Find out, if saving is required
	if( applyChanges === false ) {
		// If true/false was passed, honor this (except in rename mode, if unchanged)
		eventData.save = false;
	} else if( eventData.isNew ) {
		// In create mode, we save everyting, except for empty text
		eventData.save = (newVal !== "");
	} else {
		// In rename mode, we save everyting, except for empty or unchanged text
		eventData.save = eventData.dirty && (newVal !== "");
	}
	// Allow to break (keep editor open), modify input, or re-define data.save
	if( instOpts.beforeClose.call(node, {type: "beforeClose"}, eventData) === false){
		return false;
	}
	if( eventData.save && instOpts.save.call(node, {type: "save"}, eventData) === false){
		return false;
	}
	$input
		.removeClass("fancytree-edit-dirty")
		.off();
	// Unbind outer-click handler
	$(document).off(".fancytree-edit");

	if( eventData.save ) {
		// # 171: escape user input (not required if global escaping is on)
		node.setTitle( tree.options.escapeTitles ? newVal : escapeHtml(newVal) );
		node.setFocus();
	}else{
		if( eventData.isNew ) {
			node.remove();
			node = eventData.node = null;
			local.relatedNode.setFocus();
		} else {
			node.renderTitle();
			node.setFocus();
		}
	}
	local.eventData = null;
	local.currentNode = null;
	local.relatedNode = null;
	// Re-enable mouse and keyboard handling
	tree.widget._bind();
	// Set keyboard focus, even if setFocus() claims 'nothing to do'
	$(tree.$container).focus();
	eventData.input = null;
	instOpts.close.call(node, {type: "close"}, eventData);
	return true;
};


/**
* [ext-edit] Create a new child or sibling node and start edit mode.
*
* @param {String} [mode='child'] 'before', 'after', or 'child'
* @param {Object} [init] NodeData (or simple title string)
* @alias FancytreeNode#editCreateNode
* @requires jquery.fancytree.edit.js
* @since 2.4
*/
$.ui.fancytree._FancytreeNodeClass.prototype.editCreateNode = function(mode, init){
	var newNode,
		tree = this.tree,
		self = this;

	mode = mode || "child";
	if( init == null ) {
		init = { title: "" };
	} else if( typeof init === "string" ) {
		init = { title: init };
	} else {
		$.ui.fancytree.assert($.isPlainObject(init));
	}
	// Make sure node is expanded (and loaded) in 'child' mode
	if( mode === "child" && !this.isExpanded() && this.hasChildren() !== false ) {
		this.setExpanded().done(function(){
			self.editCreateNode(mode, init);
		});
		return;
	}
	newNode = this.addNode(init, mode);

	// #644: Don't filter new nodes.
	newNode.match = true;
	$(newNode[tree.statusClassPropName])
		.removeClass("fancytree-hide")
		.addClass("fancytree-match");

	newNode.makeVisible(/*{noAnimation: true}*/).done(function(){
		$(newNode[tree.statusClassPropName]).addClass("fancytree-edit-new");
		self.tree.ext.edit.relatedNode = self;
		newNode.editStart();
	});
};


/**
 * [ext-edit] Check if any node in this tree  in edit mode.
 *
 * @returns {FancytreeNode | null}
 * @alias Fancytree#isEditing
 * @requires jquery.fancytree.edit.js
 */
$.ui.fancytree._FancytreeClass.prototype.isEditing = function(){
	return this.ext.edit ? this.ext.edit.currentNode : null;
};


/**
 * [ext-edit] Check if this node is in edit mode.
 * @returns {Boolean} true if node is currently beeing edited
 * @alias FancytreeNode#isEditing
 * @requires jquery.fancytree.edit.js
 */
$.ui.fancytree._FancytreeNodeClass.prototype.isEditing = function(){
	return this.tree.ext.edit ? this.tree.ext.edit.currentNode === this : false;
};


/*******************************************************************************
 * Extension code
 */
$.ui.fancytree.registerExtension({
	name: "edit",
	version: "2.23.0",
	// Default options for this extension.
	options: {
		adjustWidthOfs: 4,   // null: don't adjust input size to content
		allowEmpty: false,   // Prevent empty input
		inputCss: {minWidth: "3em"},
		// triggerCancel: ["esc", "tab", "click"],
		// triggerStart: ["f2", "dblclick", "shift+click", "mac+enter"],
		triggerStart: ["f2", "shift+click", "mac+enter"],
		trim: true,          // Trim whitespace before save
		// Events:
		beforeClose: $.noop, // Return false to prevent cancel/save (data.input is available)
		beforeEdit: $.noop,  // Return false to prevent edit mode
		close: $.noop,       // Editor was removed
		edit: $.noop,        // Editor was opened (available as data.input)
//		keypress: $.noop,    // Not yet implemented
		save: $.noop         // Save data.input.val() or return false to keep editor open
	},
	// Local attributes
	currentNode: null,

	treeInit: function(ctx){
		this._superApply(arguments);
		this.$container.addClass("fancytree-ext-edit");
	},
	nodeClick: function(ctx) {
		if( $.inArray("shift+click", ctx.options.edit.triggerStart) >= 0 ){
			if( ctx.originalEvent.shiftKey ){
				ctx.node.editStart();
				return false;
			}
		}
		return this._superApply(arguments);
	},
	nodeDblclick: function(ctx) {
		if( $.inArray("dblclick", ctx.options.edit.triggerStart) >= 0 ){
			ctx.node.editStart();
			return false;
		}
		return this._superApply(arguments);
	},
	nodeKeydown: function(ctx) {
		switch( ctx.originalEvent.which ) {
		case 113: // [F2]
			if( $.inArray("f2", ctx.options.edit.triggerStart) >= 0 ){
				ctx.node.editStart();
				return false;
			}
			break;
		case $.ui.keyCode.ENTER:
			if( $.inArray("mac+enter", ctx.options.edit.triggerStart) >= 0 && isMac ){
				ctx.node.editStart();
				return false;
			}
			break;
		}
		return this._superApply(arguments);
	}
});
}(jQuery, window, document));

/*!
 * jquery.fancytree.filter.js
 *
 * Remove or highlight tree nodes, based on a filter.
 * (Extension module for jquery.fancytree.js: https://github.com/mar10/fancytree/)
 *
 * Copyright (c) 2008-2017, Martin Wendt (http://wwWendt.de)
 *
 * Released under the MIT license
 * https://github.com/mar10/fancytree/wiki/LicenseInfo
 *
 * @version 2.23.0
 * @date 2017-05-27T20:09:38Z
 */

;(function($, window, document, undefined) {

"use strict";


/*******************************************************************************
 * Private functions and variables
 */

var KeyNoData = "__not_found__",
	escapeHtml = $.ui.fancytree.escapeHtml;

function _escapeRegex(str){
	/*jshint regexdash:true */
	return (str + "").replace(/([.?*+\^\$\[\]\\(){}|-])/g, "\\$1");
}

function extractHtmlText(s){
	if( s.indexOf(">") >= 0 ) {
		return $("<div/>").html(s).text();
	}
	return s;
}

$.ui.fancytree._FancytreeClass.prototype._applyFilterImpl = function(filter, branchMode, _opts){
	var match, statusNode, re, reHighlight,
		count = 0,
		treeOpts = this.options,
		escapeTitles = treeOpts.escapeTitles,
		prevAutoCollapse = treeOpts.autoCollapse,
		opts = $.extend({}, treeOpts.filter, _opts),
		hideMode = opts.mode === "hide",
		leavesOnly = !!opts.leavesOnly && !branchMode;

	// Default to 'match title substring (not case sensitive)'
	if(typeof filter === "string"){
		// console.log("rex", filter.split('').join('\\w*').replace(/\W/, ""))
		if( opts.fuzzy ) {
			// See https://codereview.stackexchange.com/questions/23899/faster-javascript-fuzzy-string-matching-function/23905#23905
			// and http://www.quora.com/How-is-the-fuzzy-search-algorithm-in-Sublime-Text-designed
			// and http://www.dustindiaz.com/autocomplete-fuzzy-matching
			match = filter.split("").reduce(function(a, b) {
				return a + "[^" + b + "]*" + b;
			});
		} else {
			match = _escapeRegex(filter); // make sure a '.' is treated literally
		}
		re = new RegExp(".*" + match + ".*", "i");
		reHighlight = new RegExp(_escapeRegex(filter), "gi");
		filter = function(node){
			var display,
				text = escapeTitles ? node.title : extractHtmlText(node.title),
				res = !!re.test(text);

			if( res && opts.highlight ) {
				display = escapeTitles ? escapeHtml(node.title) : text;
				node.titleWithHighlight = display.replace(reHighlight, function(s){
					return "<mark>" + s + "</mark>";
				});
				// node.debug("filter", escapeTitles, text, node.titleWithHighlight);
			}
			return res;
		};
	}

	this.enableFilter = true;
	this.lastFilterArgs = arguments;

	this.$div.addClass("fancytree-ext-filter");
	if( hideMode ){
		this.$div.addClass("fancytree-ext-filter-hide");
	} else {
		this.$div.addClass("fancytree-ext-filter-dimm");
	}
	this.$div.toggleClass("fancytree-ext-filter-hide-expanders", !!opts.hideExpanders);
	// Reset current filter
	this.visit(function(node){
		delete node.match;
		delete node.titleWithHighlight;
		node.subMatchCount = 0;
	});
	statusNode = this.getRootNode()._findDirectChild(KeyNoData);
	if( statusNode ) {
		statusNode.remove();
	}

	// Adjust node.hide, .match, and .subMatchCount properties
	treeOpts.autoCollapse = false;  // #528

	this.visit(function(node){
		if ( leavesOnly && node.children != null ) {
			return;
		}
		var res = filter(node),
			matchedByBranch = false;

		if( res === "skip" ) {
			node.visit(function(c){
				c.match = false;
			}, true);
			return "skip";
		}
		if( !res && (branchMode || res === "branch") && node.parent.match ) {
			res = true;
			matchedByBranch = true;
		}
		if( res ) {
			count++;
			node.match = true;
			node.visitParents(function(p){
				p.subMatchCount += 1;
				// Expand match (unless this is no real match, but only a node in a matched branch)
				if( opts.autoExpand && !matchedByBranch && !p.expanded ) {
					p.setExpanded(true, {noAnimation: true, noEvents: true, scrollIntoView: false});
					p._filterAutoExpanded = true;
				}
			});
		}
	});
	treeOpts.autoCollapse = prevAutoCollapse;

	if( count === 0 && opts.nodata && hideMode ) {
		statusNode = opts.nodata;
		if( $.isFunction(statusNode) ) {
			statusNode = statusNode();
		}
		if( statusNode === true ) {
			statusNode = {};
		} else if( typeof statusNode === "string" ) {
			statusNode = { title: statusNode };
		}
		statusNode = $.extend({
			statusNodeType: "nodata",
			key: KeyNoData,
			title: this.options.strings.noData
		}, statusNode);

		this.getRootNode().addNode(statusNode).match = true;
	}
	// Redraw whole tree
	this.render();
	return count;
};

/**
 * [ext-filter] Dimm or hide nodes.
 *
 * @param {function | string} filter
 * @param {boolean} [opts={autoExpand: false, leavesOnly: false}]
 * @returns {integer} count
 * @alias Fancytree#filterNodes
 * @requires jquery.fancytree.filter.js
 */
$.ui.fancytree._FancytreeClass.prototype.filterNodes = function(filter, opts) {
	if( typeof opts === "boolean" ) {
		opts = { leavesOnly: opts };
		this.warn("Fancytree.filterNodes() leavesOnly option is deprecated since 2.9.0 / 2015-04-19. Use opts.leavesOnly instead.");
	}
	return this._applyFilterImpl(filter, false, opts);
};

/**
 * @deprecated
 */
$.ui.fancytree._FancytreeClass.prototype.applyFilter = function(filter){
	this.warn("Fancytree.applyFilter() is deprecated since 2.1.0 / 2014-05-29. Use .filterNodes() instead.");
	return this.filterNodes.apply(this, arguments);
};

/**
 * [ext-filter] Dimm or hide whole branches.
 *
 * @param {function | string} filter
 * @param {boolean} [opts={autoExpand: false}]
 * @returns {integer} count
 * @alias Fancytree#filterBranches
 * @requires jquery.fancytree.filter.js
 */
$.ui.fancytree._FancytreeClass.prototype.filterBranches = function(filter, opts){
	return this._applyFilterImpl(filter, true, opts);
};


/**
 * [ext-filter] Reset the filter.
 *
 * @alias Fancytree#clearFilter
 * @requires jquery.fancytree.filter.js
 */
$.ui.fancytree._FancytreeClass.prototype.clearFilter = function(){
	var $title,
		statusNode = this.getRootNode()._findDirectChild(KeyNoData),
		escapeTitles = this.options.escapeTitles,
		enhanceTitle = this.options.enhanceTitle;

	if( statusNode ) {
		statusNode.remove();
	}
	this.visit(function(node){
		if( node.match && node.span ) {  // #491, #601
			$title = $(node.span).find(">span.fancytree-title");
			if( escapeTitles ) {
				$title.text(node.title);
			} else {
				$title.html(node.title);
			}
			if( enhanceTitle ) {
				enhanceTitle({type: "enhanceTitle"}, {node: node, $title: $title});
			}
		}
		delete node.match;
		delete node.subMatchCount;
		delete node.titleWithHighlight;
		if ( node.$subMatchBadge ) {
			node.$subMatchBadge.remove();
			delete node.$subMatchBadge;
		}
		if( node._filterAutoExpanded && node.expanded ) {
			node.setExpanded(false, {noAnimation: true, noEvents: true, scrollIntoView: false});
		}
		delete node._filterAutoExpanded;
	});
	this.enableFilter = false;
	this.lastFilterArgs = null;
	this.$div.removeClass("fancytree-ext-filter fancytree-ext-filter-dimm fancytree-ext-filter-hide");
	this.render();
};


/**
 * [ext-filter] Return true if a filter is currently applied.
 *
 * @returns {Boolean}
 * @alias Fancytree#isFilterActive
 * @requires jquery.fancytree.filter.js
 * @since 2.13
 */
$.ui.fancytree._FancytreeClass.prototype.isFilterActive = function(){
	return !!this.enableFilter;
};


/**
 * [ext-filter] Return true if this node is matched by current filter (or no filter is active).
 *
 * @returns {Boolean}
 * @alias FancytreeNode#isMatched
 * @requires jquery.fancytree.filter.js
 * @since 2.13
 */
$.ui.fancytree._FancytreeNodeClass.prototype.isMatched = function(){
	return !(this.tree.enableFilter && !this.match);
};


/*******************************************************************************
 * Extension code
 */
$.ui.fancytree.registerExtension({
	name: "filter",
	version: "2.23.0",
	// Default options for this extension.
	options: {
		autoApply: true,   // Re-apply last filter if lazy data is loaded
		autoExpand: false, // Expand all branches that contain matches while filtered
		counter: true,     // Show a badge with number of matching child nodes near parent icons
		fuzzy: false,      // Match single characters in order, e.g. 'fb' will match 'FooBar'
		hideExpandedCounter: true,  // Hide counter badge if parent is expanded
		hideExpanders: false,       // Hide expanders if all child nodes are hidden by filter
		highlight: true,   // Highlight matches by wrapping inside <mark> tags
		leavesOnly: false, // Match end nodes only
		nodata: true,      // Display a 'no data' status node if result is empty
		mode: "dimm"       // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
	},
	nodeLoadChildren: function(ctx, source) {
		return this._superApply(arguments).done(function() {
			if( ctx.tree.enableFilter && ctx.tree.lastFilterArgs && ctx.options.filter.autoApply ) {
				ctx.tree._applyFilterImpl.apply(ctx.tree, ctx.tree.lastFilterArgs);
			}
		});
	},
	nodeSetExpanded: function(ctx, flag, callOpts) {
		delete ctx.node._filterAutoExpanded;
		// Make sure counter badge is displayed again, when node is beeing collapsed
		if( !flag && ctx.options.filter.hideExpandedCounter && ctx.node.$subMatchBadge ) {
			ctx.node.$subMatchBadge.show();
		}
		return this._superApply(arguments);
	},
	nodeRenderStatus: function(ctx) {
		// Set classes for current status
		var res,
			node = ctx.node,
			tree = ctx.tree,
			opts = ctx.options.filter,
			$title = $(node.span).find("span.fancytree-title"),
			$span = $(node[tree.statusClassPropName]),
			enhanceTitle = ctx.options.enhanceTitle,
			escapeTitles = ctx.options.escapeTitles;

		res = this._super(ctx);
		// nothing to do, if node was not yet rendered
		if( !$span.length || !tree.enableFilter ) {
			return res;
		}
		$span
			.toggleClass("fancytree-match", !!node.match)
			.toggleClass("fancytree-submatch", !!node.subMatchCount)
			.toggleClass("fancytree-hide", !(node.match || node.subMatchCount));
		// Add/update counter badge
		if( opts.counter && node.subMatchCount && (!node.isExpanded() || !opts.hideExpandedCounter) ) {
			if( !node.$subMatchBadge ) {
				node.$subMatchBadge = $("<span class='fancytree-childcounter'/>");
				$("span.fancytree-icon, span.fancytree-custom-icon", node.span).append(node.$subMatchBadge);
			}
			node.$subMatchBadge.show().text(node.subMatchCount);
		} else if ( node.$subMatchBadge ) {
			node.$subMatchBadge.hide();
		}
		// node.debug("nodeRenderStatus", node.titleWithHighlight, node.title)
		// #601: also chek for $title.length, because we don't need to render
		// if node.span is null (i.e. not rendered)
		if( node.span && (!node.isEditing || !node.isEditing.call(node)) ) {
			if( node.titleWithHighlight ) {
				$title.html(node.titleWithHighlight);
			} else if ( escapeTitles ) {
				$title.text(node.title);
			} else {
				$title.html(node.title);
			}
			if( enhanceTitle ) {
				enhanceTitle({type: "enhanceTitle"}, {node: node, $title: $title});
			}
		}
		return res;
	}
});
}(jQuery, window, document));

/*!
 * jquery.fancytree.glyph.js
 *
 * Use glyph fonts as instead of icon sprites.
 * (Extension module for jquery.fancytree.js: https://github.com/mar10/fancytree/)
 *
 * Copyright (c) 2008-2017, Martin Wendt (http://wwWendt.de)
 *
 * Released under the MIT license
 * https://github.com/mar10/fancytree/wiki/LicenseInfo
 *
 * @version 2.23.0
 * @date 2017-05-27T20:09:38Z
 */

;(function($, window, document, undefined) {

"use strict";

/* *****************************************************************************
 * Private functions and variables
 */

function _getIcon(opts, type){
	return opts.map[type];
}

$.ui.fancytree.registerExtension({
	name: "glyph",
	version: "2.23.0",
	// Default options for this extension.
	options: {
		map: {
			// Samples from Font Awesome 3.2
			//   http://fortawesome.github.io/Font-Awesome/3.2.1/icons/
			// See here for alternatives:
			//   http://fortawesome.github.io/Font-Awesome/icons/
			//   http://getbootstrap.com/components/
			checkbox: "icon-check-empty",
			checkboxSelected: "icon-check",
			checkboxUnknown: "icon-check icon-muted",
			error: "icon-exclamation-sign",
			expanderClosed: "icon-caret-right",
			expanderLazy: "icon-angle-right",
			expanderOpen: "icon-caret-down",
			nodata: "icon-meh",
			noExpander: "",
			dragHelper: "icon-caret-right",
			dropMarker: "icon-caret-right",
			// Default node icons.
			// (Use tree.options.icon callback to define custom icons
			// based on node data)
			doc: "icon-file-alt",
			docOpen: "icon-file-alt",
			loading: "icon-refresh icon-spin",
			folder: "icon-folder-close-alt",
			folderOpen: "icon-folder-open-alt"
		}
	},

	treeInit: function(ctx){
		var tree = ctx.tree;
		this._superApply(arguments);
		tree.$container.addClass("fancytree-ext-glyph");
	},
	nodeRenderStatus: function(ctx) {
		var icon, res, span,
			node = ctx.node,
			$span = $(node.span),
			opts = ctx.options.glyph,
			map = opts.map;

		res = this._super(ctx);

		if( node.isRoot() ){
			return res;
		}
		span = $span.children("span.fancytree-expander").get(0);
		if( span ){
			// if( node.isLoading() ){
				// icon = "loading";
			if( node.expanded && node.hasChildren() ){
				icon = "expanderOpen";
			}else if( node.isUndefined() ){
				icon = "expanderLazy";
			}else if( node.hasChildren() ){
				icon = "expanderClosed";
			}else{
				icon = "noExpander";
			}
			span.className = "fancytree-expander " + map[icon];
		}

		if( node.tr ){
			span = $("td", node.tr).find("span.fancytree-checkbox").get(0);
		}else{
			span = $span.children("span.fancytree-checkbox").get(0);
		}
		if( span ){
			icon = node.selected ? "checkboxSelected" : (node.partsel ? "checkboxUnknown" : "checkbox");
			span.className = "fancytree-checkbox " + map[icon];
		}

		// Standard icon (note that this does not match .fancytree-custom-icon,
		// that might be set by opts.icon callbacks)
		span = $span.children("span.fancytree-icon").get(0);
		if( span ){
			if( node.statusNodeType ){
				icon = _getIcon(opts, node.statusNodeType); // loading, error
			}else if( node.folder ){
				icon = node.expanded && node.hasChildren() ? _getIcon(opts, "folderOpen") : _getIcon(opts, "folder");
			}else{
				icon = node.expanded ? _getIcon(opts, "docOpen") : _getIcon(opts, "doc");
			}
			span.className = "fancytree-icon " + icon;
		}
		return res;
	},
	nodeSetStatus: function(ctx, status, message, details) {
		var res, span,
			opts = ctx.options.glyph,
			node = ctx.node;

		res = this._superApply(arguments);

		if( status === "error" || status === "loading" || status === "nodata" ){
			if(node.parent){
				span = $("span.fancytree-expander", node.span).get(0);
				if( span ) {
					span.className = "fancytree-expander " + _getIcon(opts, status);
				}
			}else{ //
				span = $(".fancytree-statusnode-" + status, node[this.nodeContainerAttrName])
					.find("span.fancytree-icon").get(0);
				if( span ) {
					span.className = "fancytree-icon " + _getIcon(opts, status);
				}
			}
		}
		return res;
	}
});
}(jQuery, window, document));

/*!
 * jquery.fancytree.gridnav.js
 *
 * Support keyboard navigation for trees with embedded input controls.
 * (Extension module for jquery.fancytree.js: https://github.com/mar10/fancytree/)
 *
 * Copyright (c) 2008-2017, Martin Wendt (http://wwWendt.de)
 *
 * Released under the MIT license
 * https://github.com/mar10/fancytree/wiki/LicenseInfo
 *
 * @version 2.23.0
 * @date 2017-05-27T20:09:38Z
 */

;(function($, window, document, undefined) {

"use strict";


/*******************************************************************************
 * Private functions and variables
 */

// Allow these navigation keys even when input controls are focused

var	KC = $.ui.keyCode,
	// which keys are *not* handled by embedded control, but passed to tree
	// navigation handler:
	NAV_KEYS = {
		"text": [KC.UP, KC.DOWN],
		"checkbox": [KC.UP, KC.DOWN, KC.LEFT, KC.RIGHT],
		"link": [KC.UP, KC.DOWN, KC.LEFT, KC.RIGHT],
		"radiobutton": [KC.UP, KC.DOWN, KC.LEFT, KC.RIGHT],
		"select-one": [KC.LEFT, KC.RIGHT],
		"select-multiple": [KC.LEFT, KC.RIGHT]
	};


/* Calculate TD column index (considering colspans).*/
function getColIdx($tr, $td) {
	var colspan,
		td = $td.get(0),
		idx = 0;

	$tr.children().each(function () {
		if( this === td ) {
			return false;
		}
		colspan = $(this).prop("colspan");
		idx += colspan ? colspan : 1;
	});
	return idx;
}


/* Find TD at given column index (considering colspans).*/
function findTdAtColIdx($tr, colIdx) {
	var colspan,
		res = null,
		idx = 0;

	$tr.children().each(function () {
		if( idx >= colIdx ) {
			res = $(this);
			return false;
		}
		colspan = $(this).prop("colspan");
		idx += colspan ? colspan : 1;
	});
	return res;
}


/* Find adjacent cell for a given direction. Skip empty cells and consider merged cells */
function findNeighbourTd($target, keyCode){
	var $tr, colIdx,
		$td = $target.closest("td"),
		$tdNext = null;

	switch( keyCode ){
		case KC.LEFT:
			$tdNext = $td.prev();
			break;
		case KC.RIGHT:
			$tdNext = $td.next();
			break;
		case KC.UP:
		case KC.DOWN:
			$tr = $td.parent();
			colIdx = getColIdx($tr, $td);
			while( true ) {
				$tr = (keyCode === KC.UP) ? $tr.prev() : $tr.next();
				if( !$tr.length ) {
					break;
				}
				// Skip hidden rows
				if( $tr.is(":hidden") ) {
					continue;
				}
				// Find adjacent cell in the same column
				$tdNext = findTdAtColIdx($tr, colIdx);
				// Skip cells that don't conatain a focusable element
				if( $tdNext && $tdNext.find(":input,a").length ) {
					break;
				}
			}
			break;
	}
	return $tdNext;
}


/*******************************************************************************
 * Extension code
 */
$.ui.fancytree.registerExtension({
	name: "gridnav",
	version: "2.23.0",
	// Default options for this extension.
	options: {
		autofocusInput:   false,  // Focus first embedded input if node gets activated
		handleCursorKeys: true   // Allow UP/DOWN in inputs to move to prev/next node
	},

	treeInit: function(ctx){
		// gridnav requires the table extension to be loaded before itself
		this._requireExtension("table", true, true);
		this._superApply(arguments);

		this.$container.addClass("fancytree-ext-gridnav");

		// Activate node if embedded input gets focus (due to a click)
		this.$container.on("focusin", function(event){
			var ctx2,
				node = $.ui.fancytree.getNode(event.target);

			if( node && !node.isActive() ){
				// Call node.setActive(), but also pass the event
				ctx2 = ctx.tree._makeHookContext(node, event);
				ctx.tree._callHook("nodeSetActive", ctx2, true);
			}
		});
	},
	nodeSetActive: function(ctx, flag, callOpts) {
		var $outer,
			opts = ctx.options.gridnav,
			node = ctx.node,
			event = ctx.originalEvent || {},
			triggeredByInput = $(event.target).is(":input");

		flag = (flag !== false);

		this._superApply(arguments);

		if( flag ){
			if( ctx.options.titlesTabbable ){
				if( !triggeredByInput ) {
					$(node.span).find("span.fancytree-title").focus();
					node.setFocus();
				}
				// If one node is tabbable, the container no longer needs to be
				ctx.tree.$container.attr("tabindex", "-1");
				// ctx.tree.$container.removeAttr("tabindex");
			} else if( opts.autofocusInput && !triggeredByInput ){
				// Set focus to input sub input (if node was clicked, but not
				// when TAB was pressed )
				$outer = $(node.tr || node.span);
				$outer.find(":input:enabled:first").focus();
			}
		}
	},
	nodeKeydown: function(ctx) {
		var inputType, handleKeys, $td,
			opts = ctx.options.gridnav,
			event = ctx.originalEvent,
			$target = $(event.target);

		if( $target.is(":input:enabled") ) {
			inputType = $target.prop("type");
		} else if( $target.is("a") ) {
			inputType = "link";
		}
		// ctx.tree.debug("ext-gridnav nodeKeydown", event, inputType);

		if( inputType && opts.handleCursorKeys ){
			handleKeys = NAV_KEYS[inputType];
			if( handleKeys && $.inArray(event.which, handleKeys) >= 0 ){
				$td = findNeighbourTd($target, event.which);
				if( $td && $td.length ) {
					// ctx.node.debug("ignore keydown in input", event.which, handleKeys);
					$td.find(":input:enabled,a").focus();
					// Prevent Fancytree default navigation
					return false;
				}
			}
			return true;
		}
		// ctx.tree.debug("ext-gridnav NOT HANDLED", event, inputType);
		return this._superApply(arguments);
	}
});
}(jQuery, window, document));

/*!
 * jquery.fancytree.persist.js
 *
 * Persist tree status in cookiesRemove or highlight tree nodes, based on a filter.
 * (Extension module for jquery.fancytree.js: https://github.com/mar10/fancytree/)
 *
 * @depends: js-cookie or jquery-cookie
 *
 * Copyright (c) 2008-2017, Martin Wendt (http://wwWendt.de)
 *
 * Released under the MIT license
 * https://github.com/mar10/fancytree/wiki/LicenseInfo
 *
 * @version 2.23.0
 * @date 2017-05-27T20:09:38Z
 */

;(function($, window, document, undefined) {

"use strict";
/* global Cookies:false */

/*******************************************************************************
 * Private functions and variables
 */
var cookieGetter, cookieRemover, cookieSetter,
	_assert = $.ui.fancytree.assert,
	ACTIVE = "active",
	EXPANDED = "expanded",
	FOCUS = "focus",
	SELECTED = "selected";

if( typeof Cookies === "function" ) {
	// Assume https://github.com/js-cookie/js-cookie
	cookieSetter = Cookies.set;
	cookieGetter = Cookies.get;
	cookieRemover = Cookies.remove;
} else {
	// Fall back to https://github.com/carhartl/jquery-cookie
	cookieSetter = cookieGetter = $.cookie;
	cookieRemover = $.removeCookie;
}

/* Recursively load lazy nodes
 * @param {string} mode 'load', 'expand', false
 */
function _loadLazyNodes(tree, local, keyList, mode, dfd) {
	var i, key, l, node,
		foundOne = false,
		expandOpts = tree.options.persist.expandOpts,
		deferredList = [],
		missingKeyList = [];

	keyList = keyList || [];
	dfd = dfd || $.Deferred();

	for( i=0, l=keyList.length; i<l; i++ ) {
		key = keyList[i];
		node = tree.getNodeByKey(key);
		if( node ) {
			if( mode && node.isUndefined() ) {
				foundOne = true;
				tree.debug("_loadLazyNodes: " + node + " is lazy: loading...");
				if( mode === "expand" ) {
					deferredList.push(node.setExpanded(true, expandOpts));
				} else {
					deferredList.push(node.load());
				}
			} else {
				tree.debug("_loadLazyNodes: " + node + " already loaded.");
				node.setExpanded(true, expandOpts);
			}
		} else {
			missingKeyList.push(key);
			tree.debug("_loadLazyNodes: " + node + " was not yet found.");
		}
	}

	$.when.apply($, deferredList).always(function(){
		// All lazy-expands have finished
		if( foundOne && missingKeyList.length > 0 ) {
			// If we read new nodes from server, try to resolve yet-missing keys
			_loadLazyNodes(tree, local, missingKeyList, mode, dfd);
		} else {
			if( missingKeyList.length ) {
				tree.warn("_loadLazyNodes: could not load those keys: ", missingKeyList);
				for( i=0, l=missingKeyList.length; i<l; i++ ) {
					key = keyList[i];
					local._appendKey(EXPANDED, keyList[i], false);
				}
			}
			dfd.resolve();
		}
	});
	return dfd;
}


/**
 * [ext-persist] Remove persistence cookies of the given type(s).
 * Called like
 *     $("#tree").fancytree("getTree").clearCookies("active expanded focus selected");
 *
 * @alias Fancytree#clearCookies
 * @requires jquery.fancytree.persist.js
 */
$.ui.fancytree._FancytreeClass.prototype.clearCookies = function(types){
	var local = this.ext.persist,
		prefix = local.cookiePrefix;

	types = types || "active expanded focus selected";
	if(types.indexOf(ACTIVE) >= 0){
		local._data(prefix + ACTIVE, null);
	}
	if(types.indexOf(EXPANDED) >= 0){
		local._data(prefix + EXPANDED, null);
	}
	if(types.indexOf(FOCUS) >= 0){
		local._data(prefix + FOCUS, null);
	}
	if(types.indexOf(SELECTED) >= 0){
		local._data(prefix + SELECTED, null);
	}
};


/**
 * [ext-persist] Return persistence information from cookies
 *
 * Called like
 *     $("#tree").fancytree("getTree").getPersistData();
 *
 * @alias Fancytree#getPersistData
 * @requires jquery.fancytree.persist.js
 */
$.ui.fancytree._FancytreeClass.prototype.getPersistData = function(){
	var local = this.ext.persist,
		prefix = local.cookiePrefix,
		delim = local.cookieDelimiter,
		res = {};

	res[ACTIVE] = local._data(prefix + ACTIVE);
	res[EXPANDED] = (local._data(prefix + EXPANDED) || "").split(delim);
	res[SELECTED] = (local._data(prefix + SELECTED) || "").split(delim);
	res[FOCUS] = local._data(prefix + FOCUS);
	return res;
};


/* *****************************************************************************
 * Extension code
 */
$.ui.fancytree.registerExtension({
	name: "persist",
	version: "2.23.0",
	// Default options for this extension.
	options: {
		cookieDelimiter: "~",
		cookiePrefix: undefined, // 'fancytree-<treeId>-' by default
		cookie: {
			raw: false,
			expires: "",
			path: "",
			domain: "",
			secure: false
		},
		expandLazy: false,     // true: recursively expand and load lazy nodes
		expandOpts: undefined, // optional `opts` argument passed to setExpanded()
		fireActivate: true,    // false: suppress `activate` event after active node was restored
		overrideSource: true,  // true: cookie takes precedence over `source` data attributes.
		store: "auto",         // 'cookie': force cookie, 'local': force localStore, 'session': force sessionStore
		types: "active expanded focus selected"
	},

	/* Generic read/write string data to cookie, sessionStorage or localStorage. */
	_data: function(key, value){
		var ls = this._local.localStorage; // null, sessionStorage, or localStorage

		if( value === undefined ) {
			return ls ? ls.getItem(key) : cookieGetter(key);
		} else if ( value === null ) {
			if( ls ) {
				ls.removeItem(key);
			} else {
				cookieRemover(key);
			}
		} else {
			if( ls ) {
				ls.setItem(key, value);
			} else {
				cookieSetter(key, value, this.options.persist.cookie);
			}
		}
	},

	/* Append `key` to a cookie. */
	_appendKey: function(type, key, flag){
		key = "" + key; // #90
		var local = this._local,
			instOpts = this.options.persist,
			delim = instOpts.cookieDelimiter,
			cookieName = local.cookiePrefix + type,
			data = local._data(cookieName),
			keyList = data ? data.split(delim) : [],
			idx = $.inArray(key, keyList);
		// Remove, even if we add a key,  so the key is always the last entry
		if(idx >= 0){
			keyList.splice(idx, 1);
		}
		// Append key to cookie
		if(flag){
			keyList.push(key);
		}
		local._data(cookieName, keyList.join(delim));
	},

	treeInit: function(ctx){
		var tree = ctx.tree,
			opts = ctx.options,
			local = this._local,
			instOpts = this.options.persist;

		// For 'auto' or 'cookie' mode, the cookie plugin must be available
		_assert((instOpts.store !== "auto" && instOpts.store !== "cookie") || cookieGetter,
			"Missing required plugin for 'persist' extension: js.cookie.js or jquery.cookie.js");

		local.cookiePrefix = instOpts.cookiePrefix || ("fancytree-" + tree._id + "-");
		local.storeActive = instOpts.types.indexOf(ACTIVE) >= 0;
		local.storeExpanded = instOpts.types.indexOf(EXPANDED) >= 0;
		local.storeSelected = instOpts.types.indexOf(SELECTED) >= 0;
		local.storeFocus = instOpts.types.indexOf(FOCUS) >= 0;
		if( instOpts.store === "cookie" || !window.localStorage ) {
			local.localStorage = null;
		} else {
			local.localStorage = (instOpts.store === "local") ? window.localStorage : window.sessionStorage;
		}

		// Bind init-handler to apply cookie state
		tree.$div.bind("fancytreeinit", function(event){
			if ( tree._triggerTreeEvent("beforeRestore", null, {}) === false ) {
				return;
			}

			var cookie, dfd, i, keyList, node,
				prevFocus = local._data(local.cookiePrefix + FOCUS), // record this before node.setActive() overrides it;
				noEvents = instOpts.fireActivate === false;

			// tree.debug("document.cookie:", document.cookie);

			cookie = local._data(local.cookiePrefix + EXPANDED);
			keyList = cookie && cookie.split(instOpts.cookieDelimiter);

			if( local.storeExpanded ) {
				// Recursively load nested lazy nodes if expandLazy is 'expand' or 'load'
				// Also remove expand-cookies for unmatched nodes
				dfd = _loadLazyNodes(tree, local, keyList, instOpts.expandLazy ? "expand" : false , null);
			} else {
				// nothing to do
				dfd = new $.Deferred().resolve();
			}

			dfd.done(function(){
				if(local.storeSelected){
					cookie = local._data(local.cookiePrefix + SELECTED);
					if(cookie){
						keyList = cookie.split(instOpts.cookieDelimiter);
						for(i=0; i<keyList.length; i++){
							node = tree.getNodeByKey(keyList[i]);
							if(node){
								if(node.selected === undefined || instOpts.overrideSource && (node.selected === false)){
//									node.setSelected();
									node.selected = true;
									node.renderStatus();
								}
							}else{
								// node is no longer member of the tree: remove from cookie also
								local._appendKey(SELECTED, keyList[i], false);
							}
						}
					}
					// In selectMode 3 we have to fix the child nodes, since we
					// only stored the selected *top* nodes
					if( tree.options.selectMode === 3 ){
						tree.visit(function(n){
							if( n.selected ) {
								n.fixSelection3AfterClick();
								return "skip";
							}
						});
					}
				}
				if(local.storeActive){
					cookie = local._data(local.cookiePrefix + ACTIVE);
					if(cookie && (opts.persist.overrideSource || !tree.activeNode)){
						node = tree.getNodeByKey(cookie);
						if(node){
							node.debug("persist: set active", cookie);
							// We only want to set the focus if the container
							// had the keyboard focus before
							node.setActive(true, {
								noFocus: true,
								noEvents: noEvents
							});
						}
					}
				}
				if(local.storeFocus && prevFocus){
					node = tree.getNodeByKey(prevFocus);
					if(node){
						// node.debug("persist: set focus", cookie);
						if( tree.options.titlesTabbable ) {
							$(node.span).find(".fancytree-title").focus();
						} else {
							$(tree.$container).focus();
						}
						// node.setFocus();
					}
				}
				tree._triggerTreeEvent("restore", null, {});
			});
		});
		// Init the tree
		return this._superApply(arguments);
	},
	nodeSetActive: function(ctx, flag, callOpts) {
		var res,
			local = this._local;

		flag = (flag !== false);
		res = this._superApply(arguments);

		if(local.storeActive){
			local._data(local.cookiePrefix + ACTIVE, this.activeNode ? this.activeNode.key : null);
		}
		return res;
	},
	nodeSetExpanded: function(ctx, flag, callOpts) {
		var res,
			node = ctx.node,
			local = this._local;

		flag = (flag !== false);
		res = this._superApply(arguments);

		if(local.storeExpanded){
			local._appendKey(EXPANDED, node.key, flag);
		}
		return res;
	},
	nodeSetFocus: function(ctx, flag) {
		var res,
			local = this._local;

		flag = (flag !== false);
		res = this._superApply(arguments);

		if( local.storeFocus ) {
			local._data(local.cookiePrefix + FOCUS, this.focusNode ? this.focusNode.key : null);
		}
		return res;
	},
	nodeSetSelected: function(ctx, flag, callOpts) {
		var res, selNodes,
			tree = ctx.tree,
			node = ctx.node,
			local = this._local;

		flag = (flag !== false);
		res = this._superApply(arguments);

		if(local.storeSelected){
			if( tree.options.selectMode === 3 ){
				// In selectMode 3 we only store the the selected *top* nodes.
				// De-selecting a node may also de-select some parents, so we
				// calculate the current status again
				selNodes = $.map(tree.getSelectedNodes(true), function(n){
					return n.key;
				});
				selNodes = selNodes.join(ctx.options.persist.cookieDelimiter);
				local._data(local.cookiePrefix + SELECTED, selNodes);
			} else {
				// beforeSelect can prevent the change - flag doesn't reflect the node.selected state
				local._appendKey(SELECTED, node.key, node.selected);
			}
		}
		return res;
	}
});
}(jQuery, window, document));

/*!
 * jquery.fancytree.table.js
 *
 * Render tree as table (aka 'tree grid', 'table tree').
 * (Extension module for jquery.fancytree.js: https://github.com/mar10/fancytree/)
 *
 * Copyright (c) 2008-2017, Martin Wendt (http://wwWendt.de)
 *
 * Released under the MIT license
 * https://github.com/mar10/fancytree/wiki/LicenseInfo
 *
 * @version 2.23.0
 * @date 2017-05-27T20:09:38Z
 */

;(function($, window, document, undefined) {

"use strict";

/* *****************************************************************************
 * Private functions and variables
 */
function _assert(cond, msg){
	msg = msg || "";
	if(!cond){
		$.error("Assertion failed " + msg);
	}
}

function insertFirstChild(referenceNode, newNode) {
	referenceNode.insertBefore(newNode, referenceNode.firstChild);
}

function insertSiblingAfter(referenceNode, newNode) {
	referenceNode.parentNode.insertBefore(newNode, referenceNode.nextSibling);
}

/* Show/hide all rows that are structural descendants of `parent`. */
function setChildRowVisibility(parent, flag) {
	parent.visit(function(node){
		var tr = node.tr;
		// currentFlag = node.hide ? false : flag; // fix for ext-filter
		if(tr){
			tr.style.display = (node.hide || !flag) ? "none" : "";
		}
		if(!node.expanded){
			return "skip";
		}
	});
}

/* Find node that is rendered in previous row. */
function findPrevRowNode(node){
	var i, last, prev,
		parent = node.parent,
		siblings = parent ? parent.children : null;

	if(siblings && siblings.length > 1 && siblings[0] !== node){
		// use the lowest descendant of the preceeding sibling
		i = $.inArray(node, siblings);
		prev = siblings[i - 1];
		_assert(prev.tr);
		// descend to lowest child (with a <tr> tag)
		while(prev.children && prev.children.length){
			last = prev.children[prev.children.length - 1];
			if(!last.tr){
				break;
			}
			prev = last;
		}
	}else{
		// if there is no preceding sibling, use the direct parent
		prev = parent;
	}
	return prev;
}

/* Render callback for 'wide' mode. */
// function _renderStatusNodeWide(event, data) {
// 	var node = data.node,
// 		nodeColumnIdx = data.options.table.nodeColumnIdx,
// 		$tdList = $(node.tr).find(">td");

// 	$tdList.eq(nodeColumnIdx).attr("colspan", data.tree.columnCount);
// 	$tdList.not(":eq(" + nodeColumnIdx + ")").remove();
// }


$.ui.fancytree.registerExtension({
	name: "table",
	version: "2.23.0",
	// Default options for this extension.
	options: {
		checkboxColumnIdx: null, // render the checkboxes into the this column index (default: nodeColumnIdx)
		// customStatus: false,	 // true: generate renderColumns events for status nodes
		indentation: 16,         // indent every node level by 16px
		nodeColumnIdx: 0         // render node expander, icon, and title to this column (default: #0)
	},
	// Overide virtual methods for this extension.
	// `this`       : is this extension object
	// `this._super`: the virtual function that was overriden (member of prev. extension or Fancytree)
	treeInit: function(ctx){
		var i, columnCount, n, $row, $tbody,
			tree = ctx.tree,
			opts = ctx.options,
			tableOpts = opts.table,
			$table = tree.widget.element;

		if( tableOpts.customStatus != null ) {
			if( opts.renderStatusColumns != null) {
				$.error("The 'customStatus' option is deprecated since v2.15.0. Use 'renderStatusColumns' only instead.");
			} else {
				tree.warn("The 'customStatus' option is deprecated since v2.15.0. Use 'renderStatusColumns' instead.");
				opts.renderStatusColumns = tableOpts.customStatus;
			}
		}
		if( opts.renderStatusColumns ) {
			if( opts.renderStatusColumns === true ) {
				opts.renderStatusColumns = opts.renderColumns;
			// } else if( opts.renderStatusColumns === "wide" ) {
			// 	opts.renderStatusColumns = _renderStatusNodeWide;
			}
		}

		$table.addClass("fancytree-container fancytree-ext-table");
		tree.tbody = $table.find(">tbody")[0];
		$tbody = $(tree.tbody);

		// Prepare row templates:
		// Determine column count from table header if any
		columnCount = $("thead >tr:last >th", $table).length;
		// Read TR templates from tbody if any
		$row = $tbody.children("tr:first");
		if( $row.length ) {
			n = $row.children("td").length;
			if( columnCount && n !== columnCount ) {
				tree.warn("Column count mismatch between thead (" + columnCount + ") and tbody (" + n + "): using tbody.");
				columnCount = n;
			}
			$row = $row.clone();
		} else {
			// Only thead is defined: create default row markup
			_assert(columnCount >= 1, "Need either <thead> or <tbody> with <td> elements to determine column count.");
			$row = $("<tr />");
			for(i=0; i<columnCount; i++) {
				$row.append("<td />");
			}
		}
		$row.find(">td").eq(tableOpts.nodeColumnIdx)
			.html("<span class='fancytree-node' />");
		if( opts.aria ) {
			$row.attr("role", "row");
			$row.find("td").attr("role", "gridcell");
		}
		tree.rowFragment = document.createDocumentFragment();
		tree.rowFragment.appendChild($row.get(0));

		// // If tbody contains a second row, use this as status node template
		// $row = $tbody.children("tr:eq(1)");
		// if( $row.length === 0 ) {
		// 	tree.statusRowFragment = tree.rowFragment;
		// } else {
		// 	$row = $row.clone();
		// 	tree.statusRowFragment = document.createDocumentFragment();
		// 	tree.statusRowFragment.appendChild($row.get(0));
		// }
		//
		$tbody.empty();

		// Make sure that status classes are set on the node's <tr> elements
		tree.statusClassPropName = "tr";
		tree.ariaPropName = "tr";
		this.nodeContainerAttrName = "tr";

		// #489: make sure $container is set to <table>, even if ext-dnd is listed before ext-table
		tree.$container = $table;

		this._superApply(arguments);

		// standard Fancytree created a root UL
		$(tree.rootNode.ul).remove();
		tree.rootNode.ul = null;

		// Add container to the TAB chain
		// #577: Allow to set tabindex to "0", "-1" and ""
		this.$container.attr("tabindex", opts.tabindex);
		// this.$container.attr("tabindex", opts.tabbable ? "0" : "-1");
		if(opts.aria) {
			tree.$container
				.attr("role", "treegrid")
				.attr("aria-readonly", true);
		}
	},
	nodeRemoveChildMarkup: function(ctx) {
		var node = ctx.node;
//		node.debug("nodeRemoveChildMarkup()");
		node.visit(function(n){
			if(n.tr){
				$(n.tr).remove();
				n.tr = null;
			}
		});
	},
	nodeRemoveMarkup: function(ctx) {
		var node = ctx.node;
//		node.debug("nodeRemoveMarkup()");
		if(node.tr){
			$(node.tr).remove();
			node.tr = null;
		}
		this.nodeRemoveChildMarkup(ctx);
	},
	/* Override standard render. */
	nodeRender: function(ctx, force, deep, collapsed, _recursive) {
		var children, firstTr, i, l, newRow, prevNode, prevTr, subCtx,
			tree = ctx.tree,
			node = ctx.node,
			opts = ctx.options,
			isRootNode = !node.parent;

		if( tree._enableUpdate === false ) {
			// $.ui.fancytree.debug("*** nodeRender _enableUpdate: false");
			return;
		}
		if( !_recursive ){
			ctx.hasCollapsedParents = node.parent && !node.parent.expanded;
		}
		// $.ui.fancytree.debug("*** nodeRender " + node + ", isRoot=" + isRootNode, "tr=" + node.tr, "hcp=" + ctx.hasCollapsedParents, "parent.tr=" + (node.parent && node.parent.tr));
		if( !isRootNode ){
			if( node.tr && force ) {
				this.nodeRemoveMarkup(ctx);
			}
			if( !node.tr ) {
				if( ctx.hasCollapsedParents && !deep ) {
					// #166: we assume that the parent will be (recursively) rendered
					// later anyway.
					// node.debug("nodeRender ignored due to unrendered parent");
					return;
				}
				// Create new <tr> after previous row
				// if( node.isStatusNode() ) {
				// 	newRow = tree.statusRowFragment.firstChild.cloneNode(true);
				// } else {
				newRow = tree.rowFragment.firstChild.cloneNode(true);
				// }
				prevNode = findPrevRowNode(node);
				// $.ui.fancytree.debug("*** nodeRender " + node + ": prev: " + prevNode.key);
				_assert(prevNode);
				if(collapsed === true && _recursive){
					// hide all child rows, so we can use an animation to show it later
					newRow.style.display = "none";
				}else if(deep && ctx.hasCollapsedParents){
					// also hide this row if deep === true but any parent is collapsed
					newRow.style.display = "none";
//					newRow.style.color = "red";
				}
				if(!prevNode.tr){
					_assert(!prevNode.parent, "prev. row must have a tr, or be system root");
					// tree.tbody.appendChild(newRow);
					insertFirstChild(tree.tbody, newRow);  // #675
				}else{
					insertSiblingAfter(prevNode.tr, newRow);
				}
				node.tr = newRow;
				if( node.key && opts.generateIds ){
					node.tr.id = opts.idPrefix + node.key;
				}
				node.tr.ftnode = node;
				// if(opts.aria){
				// 	$(node.tr).attr("aria-labelledby", "ftal_" + opts.idPrefix + node.key);
				// }
				node.span = $("span.fancytree-node", node.tr).get(0);
				// Set icon, link, and title (normally this is only required on initial render)
				this.nodeRenderTitle(ctx);
				// Allow tweaking, binding, after node was created for the first time
//				tree._triggerNodeEvent("createNode", ctx);
				if ( opts.createNode ){
					opts.createNode.call(tree, {type: "createNode"}, ctx);
				}
			} else {
				if( force ) {
					// Set icon, link, and title (normally this is only required on initial render)
					this.nodeRenderTitle(ctx); // triggers renderColumns()
				} else {
					// Update element classes according to node state
					this.nodeRenderStatus(ctx);
				}
			}
		}
		// Allow tweaking after node state was rendered
//		tree._triggerNodeEvent("renderNode", ctx);
		if ( opts.renderNode ){
			opts.renderNode.call(tree, {type: "renderNode"}, ctx);
		}
		// Visit child nodes
		// Add child markup
		children = node.children;
		if(children && (isRootNode || deep || node.expanded)){
			for(i=0, l=children.length; i<l; i++) {
				subCtx = $.extend({}, ctx, {node: children[i]});
				subCtx.hasCollapsedParents = subCtx.hasCollapsedParents || !node.expanded;
				this.nodeRender(subCtx, force, deep, collapsed, true);
			}
		}
		// Make sure, that <tr> order matches node.children order.
		if(children && !_recursive){ // we only have to do it once, for the root branch
			prevTr = node.tr || null;
			firstTr = tree.tbody.firstChild;
			// Iterate over all descendants
			node.visit(function(n){
				if(n.tr){
					if(!n.parent.expanded && n.tr.style.display !== "none"){
						// fix after a node was dropped over a collapsed
						n.tr.style.display = "none";
						setChildRowVisibility(n, false);
					}
					if(n.tr.previousSibling !== prevTr){
						node.debug("_fixOrder: mismatch at node: " + n);
						var nextTr = prevTr ? prevTr.nextSibling : firstTr;
						tree.tbody.insertBefore(n.tr, nextTr);
					}
					prevTr = n.tr;
				}
			});
		}
		// Update element classes according to node state
		// if(!isRootNode){
		// 	this.nodeRenderStatus(ctx);
		// }
	},
	nodeRenderTitle: function(ctx, title) {
		var $cb, res,
			node = ctx.node,
			opts = ctx.options,
			isStatusNode = node.isStatusNode();

		res = this._super(ctx, title);

		if( node.isRootNode() ) {
			return res;
		}
		// Move checkbox to custom column
		if(opts.checkbox && !isStatusNode && opts.table.checkboxColumnIdx != null ){
			$cb = $("span.fancytree-checkbox", node.span); //.detach();
			$(node.tr).find("td").eq(+opts.table.checkboxColumnIdx).html($cb);
		}
		// Update element classes according to node state
		this.nodeRenderStatus(ctx);

		if( isStatusNode ) {
			if( opts.renderStatusColumns ) {
				// Let user code write column content
				opts.renderStatusColumns.call(ctx.tree, {type: "renderStatusColumns"}, ctx);
			} // else: default rendering for status node: leave other cells empty
		} else if ( opts.renderColumns ) {
			opts.renderColumns.call(ctx.tree, {type: "renderColumns"}, ctx);
		}
		return res;
	},
	nodeRenderStatus: function(ctx) {
		var indent,
			node = ctx.node,
			opts = ctx.options;

		this._super(ctx);

		$(node.tr).removeClass("fancytree-node");
		// indent
		indent = (node.getLevel() - 1) * opts.table.indentation;
		$(node.span).css({paddingLeft: indent + "px"});  // #460
		// $(node.span).css({marginLeft: indent + "px"});
	 },
	/* Expand node, return Deferred.promise. */
	nodeSetExpanded: function(ctx, flag, callOpts) {
		// flag defaults to true
		flag = (flag !== false);

		if((ctx.node.expanded && flag) || (!ctx.node.expanded && !flag)) {
			// Expanded state isn't changed - just call base implementation
			return this._superApply(arguments);
		}

		var dfd = new $.Deferred(),
			subOpts = $.extend({}, callOpts, {noEvents: true, noAnimation: true});

		callOpts = callOpts || {};

		function _afterExpand(ok) {
			setChildRowVisibility(ctx.node, flag);
			if( ok ) {
				if( flag && ctx.options.autoScroll && !callOpts.noAnimation && ctx.node.hasChildren() ) {
					// Scroll down to last child, but keep current node visible
					ctx.node.getLastChild().scrollIntoView(true, {topNode: ctx.node}).always(function(){
						if( !callOpts.noEvents ) {
							ctx.tree._triggerNodeEvent(flag ? "expand" : "collapse", ctx);
						}
						dfd.resolveWith(ctx.node);
					});
				} else {
					if( !callOpts.noEvents ) {
						ctx.tree._triggerNodeEvent(flag ? "expand" : "collapse", ctx);
					}
					dfd.resolveWith(ctx.node);
				}
			} else {
				if( !callOpts.noEvents ) {
					ctx.tree._triggerNodeEvent(flag ? "expand" : "collapse", ctx);
				}
				dfd.rejectWith(ctx.node);
			}
		}
		// Call base-expand with disabled events and animation
		this._super(ctx, flag, subOpts).done(function () {
			_afterExpand(true);
		}).fail(function () {
			_afterExpand(false);
		});
		return dfd.promise();
	},
	nodeSetStatus: function(ctx, status, message, details) {
		if(status === "ok"){
			var node = ctx.node,
				firstChild = ( node.children ? node.children[0] : null );
			if ( firstChild && firstChild.isStatusNode() ) {
				$(firstChild.tr).remove();
			}
		}
		return this._superApply(arguments);
	},
	treeClear: function(ctx) {
		this.nodeRemoveChildMarkup(this._makeHookContext(this.rootNode));
		return this._superApply(arguments);
	},
	treeDestroy: function(ctx) {
		this.$container.find("tbody").empty();
		this.$source && this.$source.removeClass("ui-helper-hidden");
		return this._superApply(arguments);
	}
	/*,
	treeSetFocus: function(ctx, flag) {
//	        alert("treeSetFocus" + ctx.tree.$container);
		ctx.tree.$container.focus();
		$.ui.fancytree.focusTree = ctx.tree;
	}*/
});
}(jQuery, window, document));

/*!
 * jquery.fancytree.themeroller.js
 *
 * Enable jQuery UI ThemeRoller styles.
 * (Extension module for jquery.fancytree.js: https://github.com/mar10/fancytree/)
 *
 * @see http://jqueryui.com/themeroller/
 *
 * Copyright (c) 2008-2017, Martin Wendt (http://wwWendt.de)
 *
 * Released under the MIT license
 * https://github.com/mar10/fancytree/wiki/LicenseInfo
 *
 * @version 2.23.0
 * @date 2017-05-27T20:09:38Z
 */

;(function($, window, document, undefined) {

"use strict";

/*******************************************************************************
 * Extension code
 */
$.ui.fancytree.registerExtension({
	name: "themeroller",
	version: "2.23.0",
	// Default options for this extension.
	options: {
		activeClass: "ui-state-active",      // Class added to active node
		// activeClass: "ui-state-highlight",
		addClass: "ui-corner-all",           // Class added to all nodes
		focusClass: "ui-state-focus",        // Class added to focused node
		hoverClass: "ui-state-hover",        // Class added to hovered node
		selectedClass: "ui-state-highlight"  // Class added to selected nodes
		// selectedClass: "ui-state-active"
	},

	treeInit: function(ctx){
		var $el = ctx.widget.element,
			opts = ctx.options.themeroller;

		this._superApply(arguments);

		if($el[0].nodeName === "TABLE"){
			$el.addClass("ui-widget ui-corner-all");
			$el.find(">thead tr").addClass("ui-widget-header");
			$el.find(">tbody").addClass("ui-widget-conent");
		}else{
			$el.addClass("ui-widget ui-widget-content ui-corner-all");
		}

		$el.delegate(".fancytree-node", "mouseenter mouseleave", function(event){
			var node = $.ui.fancytree.getNode(event.target),
				flag = (event.type === "mouseenter");

			$(node.tr ? node.tr : node.span)
				.toggleClass(opts.hoverClass + " " + opts.addClass, flag);
		});
	},
	treeDestroy: function(ctx){
		this._superApply(arguments);
		ctx.widget.element.removeClass("ui-widget ui-widget-content ui-corner-all");
	},
	nodeRenderStatus: function(ctx){
		var classes = {},
			node = ctx.node,
			$el = $(node.tr ? node.tr : node.span),
			opts = ctx.options.themeroller;

		this._super(ctx);
/*
		.ui-state-highlight: Class to be applied to highlighted or selected elements. Applies "highlight" container styles to an element and its child text, links, and icons.
		.ui-state-error: Class to be applied to error messaging container elements. Applies "error" container styles to an element and its child text, links, and icons.
		.ui-state-error-text: An additional class that applies just the error text color without background. Can be used on form labels for instance. Also applies error icon color to child icons.

		.ui-state-default: Class to be applied to clickable button-like elements. Applies "clickable default" container styles to an element and its child text, links, and icons.
		.ui-state-hover: Class to be applied on mouseover to clickable button-like elements. Applies "clickable hover" container styles to an element and its child text, links, and icons.
		.ui-state-focus: Class to be applied on keyboard focus to clickable button-like elements. Applies "clickable hover" container styles to an element and its child text, links, and icons.
		.ui-state-active: Class to be applied on mousedown to clickable button-like elements. Applies "clickable active" container styles to an element and its child text, links, and icons.
*/
		// Set ui-state-* class (handle the case that the same class is assigned
		// to different states)
		classes[opts.activeClass] = false;
		classes[opts.focusClass] = false;
		classes[opts.selectedClass] = false;
		if( node.isActive() ) { classes[opts.activeClass] = true; }
		if( node.hasFocus() ) { classes[opts.focusClass]  = true; }
		// activeClass takes precedence before selectedClass:
		if( node.isSelected() && !node.isActive() ) { classes[opts.selectedClass]  = true; }
		$el.toggleClass(opts.activeClass, classes[opts.activeClass]);
		$el.toggleClass(opts.focusClass, classes[opts.focusClass]);
		$el.toggleClass(opts.selectedClass, classes[opts.selectedClass]);
		// Additional classes (e.g. 'ui-corner-all')
		$el.addClass(opts.addClass);
	}
});
}(jQuery, window, document));

/*!
 * jquery.fancytree.wide.js
 * Support for 100% wide selection bars.
 * (Extension module for jquery.fancytree.js: https://github.com/mar10/fancytree/)
 *
 * Copyright (c) 2008-2017, Martin Wendt (http://wwWendt.de)
 *
 * Released under the MIT license
 * https://github.com/mar10/fancytree/wiki/LicenseInfo
 *
 * @version 2.23.0
 * @date 2017-05-27T20:09:38Z
 */

;(function($, window, document, undefined) {

"use strict";

var reNumUnit = /^([+-]?(?:\d+|\d*\.\d+))([a-z]*|%)$/; // split "1.5em" to ["1.5", "em"]

/*******************************************************************************
 * Private functions and variables
 */
// var _assert = $.ui.fancytree.assert;

/* Calculate inner width without scrollbar */
// function realInnerWidth($el) {
// 	// http://blog.jquery.com/2012/08/16/jquery-1-8-box-sizing-width-csswidth-and-outerwidth/
// //	inst.contWidth = parseFloat(this.$container.css("width"), 10);
// 	// 'Client width without scrollbar' - 'padding'
// 	return $el[0].clientWidth - ($el.innerWidth() -  parseFloat($el.css("width"), 10));
// }

/* Create a global embedded CSS style for the tree. */
function defineHeadStyleElement(id, cssText) {
	id = "fancytree-style-" + id;
	var $headStyle = $("#" + id);

	if( !cssText ) {
		$headStyle.remove();
		return null;
	}
	if( !$headStyle.length ) {
		$headStyle = $("<style />")
			.attr("id", id)
			.addClass("fancytree-style")
			.prop("type", "text/css")
			.appendTo("head");
	}
	try {
		$headStyle.html(cssText);
	} catch ( e ) {
		// fix for IE 6-8
		$headStyle[0].styleSheet.cssText = cssText;
	}
	return $headStyle;
}

/* Calculate the CSS rules that indent title spans. */
function renderLevelCss(containerId, depth, levelOfs, lineOfs, labelOfs, measureUnit)
{
	var i,
		prefix = "#" + containerId + " span.fancytree-level-",
		rules = [];

	for(i = 0; i < depth; i++) {
		rules.push(prefix + (i + 1) + " span.fancytree-title { padding-left: " +
			(i * levelOfs + lineOfs) + measureUnit + "; }");
	}
	// Some UI animations wrap the UL inside a DIV and set position:relative on both.
	// This breaks the left:0 and padding-left:nn settings of the title
	rules.push(
		"#" + containerId + " div.ui-effects-wrapper ul li span.fancytree-title, " +
		"#" + containerId + " ul.fancytree-animating span.fancytree-title " +  // #716
		"{ padding-left: " + labelOfs + measureUnit + "; position: static; width: auto; }");
	return rules.join("\n");
}


// /**
//  * [ext-wide] Recalculate the width of the selection bar after the tree container
//  * was resized.<br>
//  * May be called explicitly on container resize, since there is no resize event
//  * for DIV tags.
//  *
//  * @alias Fancytree#wideUpdate
//  * @requires jquery.fancytree.wide.js
//  */
// $.ui.fancytree._FancytreeClass.prototype.wideUpdate = function(){
// 	var inst = this.ext.wide,
// 		prevCw = inst.contWidth,
// 		prevLo = inst.lineOfs;

// 	inst.contWidth = realInnerWidth(this.$container);
// 	// Each title is precceeded by 2 or 3 icons (16px + 3 margin)
// 	//     + 1px title border and 3px title padding
// 	// TODO: use code from treeInit() below
// 	inst.lineOfs = (this.options.checkbox ? 3 : 2) * 19;
// 	if( prevCw !== inst.contWidth || prevLo !== inst.lineOfs ) {
// 		this.debug("wideUpdate: " + inst.contWidth);
// 		this.visit(function(node){
// 			node.tree._callHook("nodeRenderTitle", node);
// 		});
// 	}
// };

/*******************************************************************************
 * Extension code
 */
$.ui.fancytree.registerExtension({
	name: "wide",
	version: "2.23.0",
	// Default options for this extension.
	options: {
		iconWidth: null,     // Adjust this if @fancy-icon-width != "16px"
		iconSpacing: null,   // Adjust this if @fancy-icon-spacing != "3px"
		labelSpacing: null,  // Adjust this if padding between icon and label != "3px"
		levelOfs: null       // Adjust this if ul padding != "16px"
	},

	treeCreate: function(ctx){
		this._superApply(arguments);
		this.$container.addClass("fancytree-ext-wide");

		var containerId, cssText, iconSpacingUnit, labelSpacingUnit, iconWidthUnit, levelOfsUnit,
			instOpts = ctx.options.wide,
			// css sniffing
			$dummyLI = $("<li id='fancytreeTemp'><span class='fancytree-node'><span class='fancytree-icon' /><span class='fancytree-title' /></span><ul />")
				.appendTo(ctx.tree.$container),
			$dummyIcon = $dummyLI.find(".fancytree-icon"),
			$dummyUL = $dummyLI.find("ul"),
			// $dummyTitle = $dummyLI.find(".fancytree-title"),
			iconSpacing = instOpts.iconSpacing || $dummyIcon.css("margin-left"),
			iconWidth = instOpts.iconWidth || $dummyIcon.css("width"),
			labelSpacing = instOpts.labelSpacing || "3px",
			levelOfs = instOpts.levelOfs || $dummyUL.css("padding-left");

		$dummyLI.remove();

		iconSpacingUnit = iconSpacing.match(reNumUnit)[2];
		iconSpacing = parseFloat(iconSpacing, 10);
		labelSpacingUnit = labelSpacing.match(reNumUnit)[2];
		labelSpacing = parseFloat(labelSpacing, 10);
		iconWidthUnit = iconWidth.match(reNumUnit)[2];
		iconWidth = parseFloat(iconWidth, 10);
		levelOfsUnit = levelOfs.match(reNumUnit)[2];
		if( iconSpacingUnit !== iconWidthUnit || levelOfsUnit !== iconWidthUnit || labelSpacingUnit !== iconWidthUnit ) {
			$.error("iconWidth, iconSpacing, and levelOfs must have the same css measure unit");
		}
		this._local.measureUnit = iconWidthUnit;
		this._local.levelOfs = parseFloat(levelOfs);
		this._local.lineOfs = (1 + (ctx.options.checkbox ? 1 : 0) +
				(ctx.options.icon === false ? 0 : 1)) * (iconWidth + iconSpacing) +
				iconSpacing;
		this._local.labelOfs = labelSpacing;
		this._local.maxDepth = 10;

		// Get/Set a unique Id on the container (if not already exists)
		containerId = this.$container.uniqueId().attr("id");
		// Generated css rules for some levels (extended on demand)
		cssText = renderLevelCss(containerId, this._local.maxDepth,
			this._local.levelOfs, this._local.lineOfs, this._local.labelOfs,
			this._local.measureUnit);
		defineHeadStyleElement(containerId, cssText);
	},
	treeDestroy: function(ctx){
		// Remove generated css rules
		defineHeadStyleElement(this.$container.attr("id"), null);
		return this._superApply(arguments);
	},
	nodeRenderStatus: function(ctx) {
		var containerId, cssText, res,
			node = ctx.node,
			level = node.getLevel();

		res = this._super(ctx);
		// Generate some more level-n rules if required
		if( level > this._local.maxDepth ) {
			containerId = this.$container.attr("id");
			this._local.maxDepth *= 2;
			node.debug("Define global ext-wide css up to level " + this._local.maxDepth);
			cssText = renderLevelCss(containerId, this._local.maxDepth,
				this._local.levelOfs, this._local.lineOfs, this._local.labelSpacing,
				this._local.measureUnit);
			defineHeadStyleElement(containerId, cssText);
		}
		// Add level-n class to apply indentation padding.
		// (Setting element style would not work, since it cannot easily be
		// overriden while animations run)
		$(node.span).addClass("fancytree-level-" + level);
		return res;
	}
});
}(jQuery, window, document));

/**!
 * jquery.fancytree.contextmenu.js
 * 3rd party jQuery Context menu extension for jQuery Fancytree
 *
 * Authors: Rodney Rehm, Addy Osmani (patches for FF)
 * Web: http://medialize.github.com/jQuery-contextMenu/
 *
 * Copyright (c) 2012, Martin Wendt (http://wwWendt.de)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://code.google.com/p/fancytree/wiki/LicenseInfe
 */
(function($, document) {
	"use strict";

	var initContextMenu = function(tree, selector, menu, actions) {
		tree.$container.on("mousedown.contextMenu", function(event) {
			var node = $.ui.fancytree.getNode(event);

			if(node) {
        $.contextMenu("destroy", "." + selector);

				node.setFocus(true);
				node.setActive(true);

				$.contextMenu({
					selector: "." + selector,
					build: function($trigger, e) {
						node = $.ui.fancytree.getNode($trigger);

						var menuItems = { };
						if($.isFunction(menu)) {
							menuItems = menu(node);
						} else if($.isPlainObject(menu)) {
							menuItems = menu;
						}

						return {
							callback: function(action, options) {
								if($.isFunction(actions)) {
									actions(node, action, options);
								} else if($.isPlainObject(actions)) {
									if(actions.hasOwnProperty(action) && $.isFunction(actions[action])) {
										actions[action](node, options);
									}
								}
							},
							items: menuItems
						};
					}
				});
			}
		});
	};

	$.ui.fancytree.registerExtension({
		name: "contextMenu",
		version: "1.0",
		contextMenu: {
      selector: "fancytree-title",
			menu: {},
			actions: {}
		},
		treeInit: function(ctx) {
			this._super(ctx);
			initContextMenu(ctx.tree,
                      ctx.options.contextMenu.selector || "fancytree-title",
                      ctx.options.contextMenu.menu,
                      ctx.options.contextMenu.actions);
		}
	});
}(jQuery, document));

/*!
 * jQuery Spliter Plugin version 0.28.5
 * Copyright (C) 2010-2020 Jakub T. Jankiewicz <https://jcubic.pl/me>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
/* global module, define, global, require, setTimeout */
// UMD taken from https://github.com/umdjs/umd
(function (factory, undefined) {
    var root = typeof window !== 'undefined' ? window : global;
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        // istanbul ignore next
        define(['jquery'], factory);
    } else if (typeof module === 'object' && module.exports) {
        // Node/CommonJS
        module.exports = function (root, jQuery) {
            if (jQuery === undefined) {
                // require('jQuery') returns a factory that requires window to
                // build a jQuery instance, we normalize how we use modules
                // that require this pattern but the window provided is a noop
                // if it's defined (how jquery works)
                if (window !== undefined) {
                    jQuery = require('jquery');
                } else {
                    jQuery = require('jquery')(root);
                }
            }
            factory(jQuery);
            return jQuery;
        };
    } else {
        // Browser
        // istanbul ignore next
        factory(root.jQuery);
    }
})(function ($, undefined) {
    if (typeof String.prototype.trim === 'undefined') {
        var sp_re = /(^\s+)|(\s+)$/g;
        String.prototype.trim = function () {
            return this.replace(sp_re, '');
        };
    }
    var count = 0;
    var splitter_id = null;
    var splitters = [];
    var current_splitter = null;
    var current_splitter_index = null;
    $.fn.split = function (options) {
        var data = this.data('splitter');
        if (data) {
            return data;
        }
        var panels = [];
        var $splitters = [];
        var panel_1;
        var panel_2;
        var settings = $.extend({
            limit: 100,
            orientation: 'horizontal',
            position: '50%',
            invisible: false,
            onDragStart: $.noop,
            onDragEnd: $.noop,
            onDrag: $.noop,
            percent: false
        }, options || {});
        this.settings = settings;
        function unset(prop) {
            return options && typeof options[prop] === 'undefined' || !options;
        }
        var len = this.children().length;
        if (len > 2) {
            if (unset('position')) {
                var w = 100 / len;
                settings.position = new Array(len - 1).fill(0).map(function () {
                    return w + '%';
                });
            }
            if (unset('percent')) {
                settings.percent = true;
            }
        }
        var cls;
        var children = this.children();
        if (children.length === 2) {
            if (settings.orientation == 'vertical') {
                panel_1 = children.first().addClass('left_panel');
                panel_2 = panel_1.next().addClass('right_panel');
                cls = 'vsplitter';
            } else if (settings.orientation == 'horizontal') {
                panel_1 = children.first().addClass('top_panel');
                panel_2 = panel_1.next().addClass('bottom_panel');
                cls = 'hsplitter';
            }
            panels = [panel_1, panel_2];
        } else {
            children.each(function () {
                var panel = $(this);
                if (settings.orientation == 'vertical') {
                    panel.addClass('vertical_panel');
                    cls = 'vsplitter';
                } else {
                    panel.addClass('horizontal_panel');
                    cls = 'hsplitter';
                }
                panels.push(panel);
            });
        }
        if (settings.invisible) {
            cls += ' splitter-invisible';
        }
        var width = this.width();
        var height = this.height();
        this.addClass('splitter_panel');
        var id = count++;
        panels.slice(0, -1).forEach(function (panel, i) {
            var splitter = $('<div/>').addClass(cls).on('mouseenter touchstart', function () {
                splitter_id = id;
                current_splitter_index = splitter.index() - i - 1;
            }).on('mouseleave touchend', function () {
                splitter_id = null;
                current_splitter_index = null;
            }).insertAfter(panel);
            $splitters.push(splitter);
        });
        var position;

        function get_position(position) {
            if (position instanceof Array) {
                return position.map(get_position);
            }
            if (typeof position === 'number') {
                return position;
            }
            if (typeof position === 'string') {
                var match = position.match(/^([0-9\.]+)(px|%)$/);
                if (match) {
                    if (match[2] == 'px') {
                        return +match[1];
                    } else {
                        if (settings.orientation == 'vertical') {
                            return (width * +match[1]) / 100;
                        } else if (settings.orientation == 'horizontal') {
                            return (height * +match[1]) / 100;
                        }
                    }
                } else {
                    //throw position + ' is invalid value';
                }
            } else {
                //throw 'position have invalid type';
            }
        }

        function set_limit(limit) {
            if (!isNaN(parseFloat(limit)) && isFinite(limit)) {
                return {
                    leftUpper: limit,
                    rightBottom: limit
                };
            }
            return limit;
        }

        var self = $.extend(this, {
            refresh: function () {
                var new_width = this.width();
                var new_height = this.height();
                if (width != new_width || height != new_height) {
                    width = this.width();
                    height = this.height();
                    self.position(position);
                }
            },
            option: function (name, value) {
                if (name === 'position') {
                    return self.position(value);
                } else if (typeof value === 'undefined') {
                    return settings[name];
                } else {
                    settings[name] = value;
                }
                return self;
            },
            position: (function () {
                function make_sizer(dim_name, pos_name) {
                    return function (n, silent) {
                        if (n === undefined) {
                            return position;
                        } else {
                            position = get_position(n);
                            if (!(position instanceof Array)) {
                                position = [position];
                            }
                            if (position.length !== panels.length - 1) {
                                throw new Error('position array need to equal splitters length');
                            }
                            var outer_name = 'outer';
                            outer_name += dim_name[0].toUpperCase() + dim_name.substring(1);
                            var dim_px = self.css('visiblity', 'hidden')[dim_name]();
                            var pw = 0;
                            var sw_sum = 0;
                            for (var i = 0; i < position.length; ++i) {
                                var splitter = $splitters[i];
                                var panel = panels[i];
                                var pos = position[i];
                                var splitter_dim = splitter[dim_name]();
                                var sw2 = splitter_dim / 2;
                                if (settings.invisible) {
                                    pw += panel[dim_name](pos)[outer_name]();
                                    splitter.css(pos_name, pw - (sw2 * (i + 1)));
                                } else if (settings.percent) {
                                    var w1 = (pos - sw2) / dim_px * 100;
                                    var l1 = (pw + sw_sum) / dim_px * 100;
                                    panel.css(pos_name, l1 + '%');
                                    pw += panel.css(dim_name, w1 + '%')[outer_name]();
                                    splitter.css(pos_name, (pw + sw_sum) / dim_px * 100 + '%');
                                } else {
                                    panel.css(pos_name, pw + sw_sum);
                                    pw += panel.css(dim_name, pos - sw2)[outer_name]();
                                    splitter.css(pos_name, pw + sw_sum);
                                }
                                sw_sum += splitter_dim;
                            }
                            var panel_last = panels[i];
                            if (settings.invisible) {
                                panel_last.height(height - pw);
                            } else {
                                var s_sum = splitter_dim * i;
                                var props = {};
                                if (settings.percent) {
                                    props[dim_name] = (dim_px - pw - sw_sum) / dim_px * 100 + '%';
                                    props[pos_name] = (pw + sw_sum) / dim_px * 100 + '%';
                                } else {
                                    props[dim_name] = dim_px - pw - sw_sum;
                                    props[pos_name] = pw + sw_sum;
                                }
                                panel_last.css(props);
                            }
                            self.css('visiblity', '');
                        }
                        if (!silent) {
                            self.trigger('splitter.resize');
                            self.find('.splitter_panel').trigger('splitter.resize');
                        }
                        return self;
                    };
                }
                if (settings.orientation == 'vertical') {
                    return make_sizer('width', 'left');
                } else if (settings.orientation == 'horizontal') {
                    return make_sizer('height', 'top');
                } else {
                    return $.noop;
                }
            })(),
            _splitters: $splitters,
            _panels: panels,
            orientation: settings.orientation,
            limit: set_limit(settings.limit),
            isActive: function () {
                return splitter_id === id;
            },
            destroy: function () {
                self.removeClass('splitter_panel');
                clear_attr(self, 'class');
                function clear_attr(item, attr) {
                    var val = item.attr(attr);
                    if (typeof val === 'string' && !val.trim()) {
                        item.removeAttr(attr);
                    }
                }
                panels.forEach(function ($panel) {
                    $panel.css({
                        height: '',
                        width: '',
                        left: '',
                        top: ''
                    });
                    var cls = $panel.attr('class').replace(/\w+_panel/g, '').trim();
                    $panel.attr('class', cls);
                    clear_attr($panel, 'class');
                    clear_attr($panel, 'style');
                });
                self.off('splitter.resize');
                self.trigger('splitter.resize');
                self.find('.splitter_panel').trigger('splitter.resize');
                splitters[id] = null;
                count--;
                $splitters.forEach(function ($splitter) {
                    var splitter = $(this);
                    $splitter.off('mouseenter');
                    $splitter.off('mouseleave');
                    $splitter.off('touchstart');
                    $splitter.off('touchmove');
                    $splitter.off('touchend');
                    $splitter.off('touchleave');
                    $splitter.off('touchcancel');
                    $splitter.remove();
                });
                self.removeData('splitter');
                var not_null = false;
                for (var i = splitters.length; i--;) {
                    if (splitters[i] !== null) {
                        not_null = true;
                        break;
                    }
                }
                //remove document events when no splitters
                if (!not_null) {
                    $(document.documentElement).off('.splitter');
                    $(window).off('resize.splitter');
                    splitters = [];
                    count = 0;
                }
            }
        });
        self.on('splitter.resize', function (e) {
            var pos = self.position();
            if (self.orientation == 'vertical' &&
                pos > self.width()) {
                pos = self.width() - self.limit.rightBottom - 1;
            } else if (self.orientation == 'horizontal' &&
                pos > self.height()) {
                pos = self.height() - self.limit.rightBottom - 1;
            }
            if (pos < self.limit.leftUpper) {
                pos = self.limit.leftUpper + 1;
            }
            e.stopPropagation();
            self.position(pos, true);
        });
        //inital position of splitter
        var pos;
        if (settings.orientation == 'vertical') {
            if (pos > width - settings.limit.rightBottom) {
                pos = width - settings.limit.rightBottom;
            } else {
                pos = get_position(settings.position);
            }
        } else if (settings.orientation == 'horizontal') {
            //position = height/2;
            if (pos > height - settings.limit.rightBottom) {
                pos = height - settings.limit.rightBottom;
            } else {
                pos = get_position(settings.position);
            }
        }
        if (pos < settings.limit.leftUpper) {
            pos = settings.limit.leftUpper;
        }
        if (panels.length > 2 && !(pos instanceof Array && pos.length == $splitters.length)) {
            throw new Error('position need to be array equal to $splitters length');
        }
        self.position(pos, true);
        /*
        // disable this is not needed, if higher in tree there is splitter
        // the parets need to have height: 100%
        var parent = this.closest('.splitter_panel');
        if (parent.length) {
            this.height(parent.height());
        }
        */
        function calc_pos(pos, x) {
            var new_pos = pos.slice(0, current_splitter.index);
            var p;
            if (new_pos.length) {
                p = x - new_pos.reduce(function (a, b) {
                    return a + b;
                });
            } else {
                p = x;
            }
            var diff = pos[current_splitter.index] - p;
            new_pos.push(p);
            if (current_splitter.index < pos.length - 1) {
                var rest = pos.slice(current_splitter.index + 1);
                rest[0] += diff;
                new_pos = new_pos.concat(rest);
            }
            return new_pos;
        }
        // ------------------------------------------------------------------------------------
        // bind events to document if no splitters
        if (splitters.filter(Boolean).length === 0) {
            $(window).on('resize.splitter', function () {
                $.each(splitters, function (i, splitter) {
                    if (splitter) {
                        splitter.refresh();
                    }
                });
            });
            $(document.documentElement).on('mousedown.splitter touchstart.splitter', function (e) {
                if (splitter_id !== null) {
                    e.preventDefault();
                    current_splitter = {
                        node: splitters[splitter_id],
                        index: current_splitter_index
                    };
                    // ignore right click
                    if (e.originalEvent.button !== 2) {
                        setTimeout(function () {
                            if (!current_splitter) {
                                return;
                            }
                            $('<div class="splitterMask"></div>').
                                css('cursor', current_splitter.node.children().eq(1).css('cursor')).
                                insertAfter(current_splitter.node);
                        });
                    }
                    current_splitter.node.settings.onDragStart(e);
                }
            }).on('mouseup.splitter touchend.splitter touchleave.splitter touchcancel.splitter', function (e) {
                if (current_splitter) {
                    setTimeout(function () {
                        $('.splitterMask').remove();
                    });
                    current_splitter.node.settings.onDragEnd(e);
                    current_splitter = null;
                }
            }).on('mousemove.splitter touchmove.splitter', function (e) {
                var pos;
                if (current_splitter !== null) {
                    var node = current_splitter.node;
                    var leftUpperLimit = node.limit.leftUpper;
                    var rightBottomLimit = node.limit.rightBottom;
                    var offset = node.offset();
                    if (node.orientation == 'vertical') {
                        var pageX = e.pageX;
                        if (e.originalEvent && e.originalEvent.changedTouches) {
                            pageX = e.originalEvent.changedTouches[0].pageX;
                        }
                        var x = pageX - offset.left;
                        if (x <= node.limit.leftUpper) {
                            x = node.limit.leftUpper + 1;
                        } else if (x >= node.width() - rightBottomLimit) {
                            x = node.width() - rightBottomLimit - 1;
                        }
                        pos = node.position();
                        if (pos.length > 1) {
                            node.position(calc_pos(pos, x), true);
                        } else if (x > node.limit.leftUpper &&
                            x < node.width() - rightBottomLimit) {
                            node.position(x, true);
                            node.trigger('splitter.resize');
                            node.find('.splitter_panel').
                                trigger('splitter.resize');
                            //e.preventDefault();
                        }
                    } else if (node.orientation == 'horizontal') {
                        var pageY = e.pageY;
                        if (e.originalEvent && e.originalEvent.changedTouches) {
                            pageY = e.originalEvent.changedTouches[0].pageY;
                        }
                        var y = pageY - offset.top;
                        if (y <= node.limit.leftUpper) {
                            y = node.limit.leftUpper + 1;
                        } else if (y >= node.height() - rightBottomLimit) {
                            y = node.height() - rightBottomLimit - 1;
                        }
                        pos = node.position();
                        if (pos.length > 1) {
                            node.position(calc_pos(pos, y), true);
                        } else if (y > node.limit.leftUpper &&
                            y < node.height() - rightBottomLimit) {
                            node.position(y, true);
                            node.trigger('splitter.resize');
                            node.find('.splitter_panel').
                                trigger('splitter.resize');
                            //e.preventDefault();
                        }
                    }
                    node.settings.onDrag(e);
                }
            });//*/
        }
        splitters[id] = self;
        self.data('splitter', self);
        return self;
    };
});
/*
CryptoJS v3.1.2
code.google.com/p/crypto-js
(c) 2009-2013 by Jeff Mott. All rights reserved.
code.google.com/p/crypto-js/wiki/License
*/
var CryptoJS = CryptoJS || function (u, p) {
    var d = {}, l = d.lib = {}, s = function () { }, t = l.Base = { extend: function (a) { s.prototype = this; var c = new s; a && c.mixIn(a); c.hasOwnProperty("init") || (c.init = function () { c.$super.init.apply(this, arguments) }); c.init.prototype = c; c.$super = this; return c }, create: function () { var a = this.extend(); a.init.apply(a, arguments); return a }, init: function () { }, mixIn: function (a) { for (var c in a) a.hasOwnProperty(c) && (this[c] = a[c]); a.hasOwnProperty("toString") && (this.toString = a.toString) }, clone: function () { return this.init.prototype.extend(this) } },
    r = l.WordArray = t.extend({
        init: function (a, c) { a = this.words = a || []; this.sigBytes = c != p ? c : 4 * a.length }, toString: function (a) { return (a || v).stringify(this) }, concat: function (a) { var c = this.words, e = a.words, j = this.sigBytes; a = a.sigBytes; this.clamp(); if (j % 4) for (var k = 0; k < a; k++)c[j + k >>> 2] |= (e[k >>> 2] >>> 24 - 8 * (k % 4) & 255) << 24 - 8 * ((j + k) % 4); else if (65535 < e.length) for (k = 0; k < a; k += 4)c[j + k >>> 2] = e[k >>> 2]; else c.push.apply(c, e); this.sigBytes += a; return this }, clamp: function () {
            var a = this.words, c = this.sigBytes; a[c >>> 2] &= 4294967295 <<
                32 - 8 * (c % 4); a.length = u.ceil(c / 4)
        }, clone: function () { var a = t.clone.call(this); a.words = this.words.slice(0); return a }, random: function (a) { for (var c = [], e = 0; e < a; e += 4)c.push(4294967296 * u.random() | 0); return new r.init(c, a) }
    }), w = d.enc = {}, v = w.Hex = {
        stringify: function (a) { var c = a.words; a = a.sigBytes; for (var e = [], j = 0; j < a; j++) { var k = c[j >>> 2] >>> 24 - 8 * (j % 4) & 255; e.push((k >>> 4).toString(16)); e.push((k & 15).toString(16)) } return e.join("") }, parse: function (a) {
            for (var c = a.length, e = [], j = 0; j < c; j += 2)e[j >>> 3] |= parseInt(a.substr(j,
                2), 16) << 24 - 4 * (j % 8); return new r.init(e, c / 2)
        }
    }, b = w.Latin1 = { stringify: function (a) { var c = a.words; a = a.sigBytes; for (var e = [], j = 0; j < a; j++)e.push(String.fromCharCode(c[j >>> 2] >>> 24 - 8 * (j % 4) & 255)); return e.join("") }, parse: function (a) { for (var c = a.length, e = [], j = 0; j < c; j++)e[j >>> 2] |= (a.charCodeAt(j) & 255) << 24 - 8 * (j % 4); return new r.init(e, c) } }, x = w.Utf8 = { stringify: function (a) { try { return decodeURIComponent(escape(b.stringify(a))) } catch (c) { throw Error("Malformed UTF-8 data"); } }, parse: function (a) { return b.parse(unescape(encodeURIComponent(a))) } },
    q = l.BufferedBlockAlgorithm = t.extend({
        reset: function () { this._data = new r.init; this._nDataBytes = 0 }, _append: function (a) { "string" == typeof a && (a = x.parse(a)); this._data.concat(a); this._nDataBytes += a.sigBytes }, _process: function (a) { var c = this._data, e = c.words, j = c.sigBytes, k = this.blockSize, b = j / (4 * k), b = a ? u.ceil(b) : u.max((b | 0) - this._minBufferSize, 0); a = b * k; j = u.min(4 * a, j); if (a) { for (var q = 0; q < a; q += k)this._doProcessBlock(e, q); q = e.splice(0, a); c.sigBytes -= j } return new r.init(q, j) }, clone: function () {
            var a = t.clone.call(this);
            a._data = this._data.clone(); return a
        }, _minBufferSize: 0
    }); l.Hasher = q.extend({
        cfg: t.extend(), init: function (a) { this.cfg = this.cfg.extend(a); this.reset() }, reset: function () { q.reset.call(this); this._doReset() }, update: function (a) { this._append(a); this._process(); return this }, finalize: function (a) { a && this._append(a); return this._doFinalize() }, blockSize: 16, _createHelper: function (a) { return function (b, e) { return (new a.init(e)).finalize(b) } }, _createHmacHelper: function (a) {
            return function (b, e) {
                return (new n.HMAC.init(a,
                    e)).finalize(b)
            }
        }
    }); var n = d.algo = {}; return d
}(Math);
(function () {
    var u = CryptoJS, p = u.lib.WordArray; u.enc.Base64 = {
        stringify: function (d) { var l = d.words, p = d.sigBytes, t = this._map; d.clamp(); d = []; for (var r = 0; r < p; r += 3)for (var w = (l[r >>> 2] >>> 24 - 8 * (r % 4) & 255) << 16 | (l[r + 1 >>> 2] >>> 24 - 8 * ((r + 1) % 4) & 255) << 8 | l[r + 2 >>> 2] >>> 24 - 8 * ((r + 2) % 4) & 255, v = 0; 4 > v && r + 0.75 * v < p; v++)d.push(t.charAt(w >>> 6 * (3 - v) & 63)); if (l = t.charAt(64)) for (; d.length % 4;)d.push(l); return d.join("") }, parse: function (d) {
            var l = d.length, s = this._map, t = s.charAt(64); t && (t = d.indexOf(t), -1 != t && (l = t)); for (var t = [], r = 0, w = 0; w <
                l; w++)if (w % 4) { var v = s.indexOf(d.charAt(w - 1)) << 2 * (w % 4), b = s.indexOf(d.charAt(w)) >>> 6 - 2 * (w % 4); t[r >>> 2] |= (v | b) << 24 - 8 * (r % 4); r++ } return p.create(t, r)
        }, _map: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/="
    }
})();
(function (u) {
    function p(b, n, a, c, e, j, k) { b = b + (n & a | ~n & c) + e + k; return (b << j | b >>> 32 - j) + n } function d(b, n, a, c, e, j, k) { b = b + (n & c | a & ~c) + e + k; return (b << j | b >>> 32 - j) + n } function l(b, n, a, c, e, j, k) { b = b + (n ^ a ^ c) + e + k; return (b << j | b >>> 32 - j) + n } function s(b, n, a, c, e, j, k) { b = b + (a ^ (n | ~c)) + e + k; return (b << j | b >>> 32 - j) + n } for (var t = CryptoJS, r = t.lib, w = r.WordArray, v = r.Hasher, r = t.algo, b = [], x = 0; 64 > x; x++)b[x] = 4294967296 * u.abs(u.sin(x + 1)) | 0; r = r.MD5 = v.extend({
        _doReset: function () { this._hash = new w.init([1732584193, 4023233417, 2562383102, 271733878]) },
        _doProcessBlock: function (q, n) {
            for (var a = 0; 16 > a; a++) { var c = n + a, e = q[c]; q[c] = (e << 8 | e >>> 24) & 16711935 | (e << 24 | e >>> 8) & 4278255360 } var a = this._hash.words, c = q[n + 0], e = q[n + 1], j = q[n + 2], k = q[n + 3], z = q[n + 4], r = q[n + 5], t = q[n + 6], w = q[n + 7], v = q[n + 8], A = q[n + 9], B = q[n + 10], C = q[n + 11], u = q[n + 12], D = q[n + 13], E = q[n + 14], x = q[n + 15], f = a[0], m = a[1], g = a[2], h = a[3], f = p(f, m, g, h, c, 7, b[0]), h = p(h, f, m, g, e, 12, b[1]), g = p(g, h, f, m, j, 17, b[2]), m = p(m, g, h, f, k, 22, b[3]), f = p(f, m, g, h, z, 7, b[4]), h = p(h, f, m, g, r, 12, b[5]), g = p(g, h, f, m, t, 17, b[6]), m = p(m, g, h, f, w, 22, b[7]),
                f = p(f, m, g, h, v, 7, b[8]), h = p(h, f, m, g, A, 12, b[9]), g = p(g, h, f, m, B, 17, b[10]), m = p(m, g, h, f, C, 22, b[11]), f = p(f, m, g, h, u, 7, b[12]), h = p(h, f, m, g, D, 12, b[13]), g = p(g, h, f, m, E, 17, b[14]), m = p(m, g, h, f, x, 22, b[15]), f = d(f, m, g, h, e, 5, b[16]), h = d(h, f, m, g, t, 9, b[17]), g = d(g, h, f, m, C, 14, b[18]), m = d(m, g, h, f, c, 20, b[19]), f = d(f, m, g, h, r, 5, b[20]), h = d(h, f, m, g, B, 9, b[21]), g = d(g, h, f, m, x, 14, b[22]), m = d(m, g, h, f, z, 20, b[23]), f = d(f, m, g, h, A, 5, b[24]), h = d(h, f, m, g, E, 9, b[25]), g = d(g, h, f, m, k, 14, b[26]), m = d(m, g, h, f, v, 20, b[27]), f = d(f, m, g, h, D, 5, b[28]), h = d(h, f,
                    m, g, j, 9, b[29]), g = d(g, h, f, m, w, 14, b[30]), m = d(m, g, h, f, u, 20, b[31]), f = l(f, m, g, h, r, 4, b[32]), h = l(h, f, m, g, v, 11, b[33]), g = l(g, h, f, m, C, 16, b[34]), m = l(m, g, h, f, E, 23, b[35]), f = l(f, m, g, h, e, 4, b[36]), h = l(h, f, m, g, z, 11, b[37]), g = l(g, h, f, m, w, 16, b[38]), m = l(m, g, h, f, B, 23, b[39]), f = l(f, m, g, h, D, 4, b[40]), h = l(h, f, m, g, c, 11, b[41]), g = l(g, h, f, m, k, 16, b[42]), m = l(m, g, h, f, t, 23, b[43]), f = l(f, m, g, h, A, 4, b[44]), h = l(h, f, m, g, u, 11, b[45]), g = l(g, h, f, m, x, 16, b[46]), m = l(m, g, h, f, j, 23, b[47]), f = s(f, m, g, h, c, 6, b[48]), h = s(h, f, m, g, w, 10, b[49]), g = s(g, h, f, m,
                        E, 15, b[50]), m = s(m, g, h, f, r, 21, b[51]), f = s(f, m, g, h, u, 6, b[52]), h = s(h, f, m, g, k, 10, b[53]), g = s(g, h, f, m, B, 15, b[54]), m = s(m, g, h, f, e, 21, b[55]), f = s(f, m, g, h, v, 6, b[56]), h = s(h, f, m, g, x, 10, b[57]), g = s(g, h, f, m, t, 15, b[58]), m = s(m, g, h, f, D, 21, b[59]), f = s(f, m, g, h, z, 6, b[60]), h = s(h, f, m, g, C, 10, b[61]), g = s(g, h, f, m, j, 15, b[62]), m = s(m, g, h, f, A, 21, b[63]); a[0] = a[0] + f | 0; a[1] = a[1] + m | 0; a[2] = a[2] + g | 0; a[3] = a[3] + h | 0
        }, _doFinalize: function () {
            var b = this._data, n = b.words, a = 8 * this._nDataBytes, c = 8 * b.sigBytes; n[c >>> 5] |= 128 << 24 - c % 32; var e = u.floor(a /
                4294967296); n[(c + 64 >>> 9 << 4) + 15] = (e << 8 | e >>> 24) & 16711935 | (e << 24 | e >>> 8) & 4278255360; n[(c + 64 >>> 9 << 4) + 14] = (a << 8 | a >>> 24) & 16711935 | (a << 24 | a >>> 8) & 4278255360; b.sigBytes = 4 * (n.length + 1); this._process(); b = this._hash; n = b.words; for (a = 0; 4 > a; a++)c = n[a], n[a] = (c << 8 | c >>> 24) & 16711935 | (c << 24 | c >>> 8) & 4278255360; return b
        }, clone: function () { var b = v.clone.call(this); b._hash = this._hash.clone(); return b }
    }); t.MD5 = v._createHelper(r); t.HmacMD5 = v._createHmacHelper(r)
})(Math);
(function () {
    var u = CryptoJS, p = u.lib, d = p.Base, l = p.WordArray, p = u.algo, s = p.EvpKDF = d.extend({ cfg: d.extend({ keySize: 4, hasher: p.MD5, iterations: 1 }), init: function (d) { this.cfg = this.cfg.extend(d) }, compute: function (d, r) { for (var p = this.cfg, s = p.hasher.create(), b = l.create(), u = b.words, q = p.keySize, p = p.iterations; u.length < q;) { n && s.update(n); var n = s.update(d).finalize(r); s.reset(); for (var a = 1; a < p; a++)n = s.finalize(n), s.reset(); b.concat(n) } b.sigBytes = 4 * q; return b } }); u.EvpKDF = function (d, l, p) {
        return s.create(p).compute(d,
            l)
    }
})();
CryptoJS.lib.Cipher || function (u) {
    var p = CryptoJS, d = p.lib, l = d.Base, s = d.WordArray, t = d.BufferedBlockAlgorithm, r = p.enc.Base64, w = p.algo.EvpKDF, v = d.Cipher = t.extend({
        cfg: l.extend(), createEncryptor: function (e, a) { return this.create(this._ENC_XFORM_MODE, e, a) }, createDecryptor: function (e, a) { return this.create(this._DEC_XFORM_MODE, e, a) }, init: function (e, a, b) { this.cfg = this.cfg.extend(b); this._xformMode = e; this._key = a; this.reset() }, reset: function () { t.reset.call(this); this._doReset() }, process: function (e) { this._append(e); return this._process() },
        finalize: function (e) { e && this._append(e); return this._doFinalize() }, keySize: 4, ivSize: 4, _ENC_XFORM_MODE: 1, _DEC_XFORM_MODE: 2, _createHelper: function (e) { return { encrypt: function (b, k, d) { return ("string" == typeof k ? c : a).encrypt(e, b, k, d) }, decrypt: function (b, k, d) { return ("string" == typeof k ? c : a).decrypt(e, b, k, d) } } }
    }); d.StreamCipher = v.extend({ _doFinalize: function () { return this._process(!0) }, blockSize: 1 }); var b = p.mode = {}, x = function (e, a, b) {
        var c = this._iv; c ? this._iv = u : c = this._prevBlock; for (var d = 0; d < b; d++)e[a + d] ^=
            c[d]
    }, q = (d.BlockCipherMode = l.extend({ createEncryptor: function (e, a) { return this.Encryptor.create(e, a) }, createDecryptor: function (e, a) { return this.Decryptor.create(e, a) }, init: function (e, a) { this._cipher = e; this._iv = a } })).extend(); q.Encryptor = q.extend({ processBlock: function (e, a) { var b = this._cipher, c = b.blockSize; x.call(this, e, a, c); b.encryptBlock(e, a); this._prevBlock = e.slice(a, a + c) } }); q.Decryptor = q.extend({
        processBlock: function (e, a) {
            var b = this._cipher, c = b.blockSize, d = e.slice(a, a + c); b.decryptBlock(e, a); x.call(this,
                e, a, c); this._prevBlock = d
        }
    }); b = b.CBC = q; q = (p.pad = {}).Pkcs7 = { pad: function (a, b) { for (var c = 4 * b, c = c - a.sigBytes % c, d = c << 24 | c << 16 | c << 8 | c, l = [], n = 0; n < c; n += 4)l.push(d); c = s.create(l, c); a.concat(c) }, unpad: function (a) { a.sigBytes -= a.words[a.sigBytes - 1 >>> 2] & 255 } }; d.BlockCipher = v.extend({
        cfg: v.cfg.extend({ mode: b, padding: q }), reset: function () {
            v.reset.call(this); var a = this.cfg, b = a.iv, a = a.mode; if (this._xformMode == this._ENC_XFORM_MODE) var c = a.createEncryptor; else c = a.createDecryptor, this._minBufferSize = 1; this._mode = c.call(a,
                this, b && b.words)
        }, _doProcessBlock: function (a, b) { this._mode.processBlock(a, b) }, _doFinalize: function () { var a = this.cfg.padding; if (this._xformMode == this._ENC_XFORM_MODE) { a.pad(this._data, this.blockSize); var b = this._process(!0) } else b = this._process(!0), a.unpad(b); return b }, blockSize: 4
    }); var n = d.CipherParams = l.extend({ init: function (a) { this.mixIn(a) }, toString: function (a) { return (a || this.formatter).stringify(this) } }), b = (p.format = {}).OpenSSL = {
        stringify: function (a) {
            var b = a.ciphertext; a = a.salt; return (a ? s.create([1398893684,
                1701076831]).concat(a).concat(b) : b).toString(r)
        }, parse: function (a) { a = r.parse(a); var b = a.words; if (1398893684 == b[0] && 1701076831 == b[1]) { var c = s.create(b.slice(2, 4)); b.splice(0, 4); a.sigBytes -= 16 } return n.create({ ciphertext: a, salt: c }) }
    }, a = d.SerializableCipher = l.extend({
        cfg: l.extend({ format: b }), encrypt: function (a, b, c, d) { d = this.cfg.extend(d); var l = a.createEncryptor(c, d); b = l.finalize(b); l = l.cfg; return n.create({ ciphertext: b, key: c, iv: l.iv, algorithm: a, mode: l.mode, padding: l.padding, blockSize: a.blockSize, formatter: d.format }) },
        decrypt: function (a, b, c, d) { d = this.cfg.extend(d); b = this._parse(b, d.format); return a.createDecryptor(c, d).finalize(b.ciphertext) }, _parse: function (a, b) { return "string" == typeof a ? b.parse(a, this) : a }
    }), p = (p.kdf = {}).OpenSSL = { execute: function (a, b, c, d) { d || (d = s.random(8)); a = w.create({ keySize: b + c }).compute(a, d); c = s.create(a.words.slice(b), 4 * c); a.sigBytes = 4 * b; return n.create({ key: a, iv: c, salt: d }) } }, c = d.PasswordBasedCipher = a.extend({
        cfg: a.cfg.extend({ kdf: p }), encrypt: function (b, c, d, l) {
            l = this.cfg.extend(l); d = l.kdf.execute(d,
                b.keySize, b.ivSize); l.iv = d.iv; b = a.encrypt.call(this, b, c, d.key, l); b.mixIn(d); return b
        }, decrypt: function (b, c, d, l) { l = this.cfg.extend(l); c = this._parse(c, l.format); d = l.kdf.execute(d, b.keySize, b.ivSize, c.salt); l.iv = d.iv; return a.decrypt.call(this, b, c, d.key, l) }
    })
}();
(function () {
    for (var u = CryptoJS, p = u.lib.BlockCipher, d = u.algo, l = [], s = [], t = [], r = [], w = [], v = [], b = [], x = [], q = [], n = [], a = [], c = 0; 256 > c; c++)a[c] = 128 > c ? c << 1 : c << 1 ^ 283; for (var e = 0, j = 0, c = 0; 256 > c; c++) { var k = j ^ j << 1 ^ j << 2 ^ j << 3 ^ j << 4, k = k >>> 8 ^ k & 255 ^ 99; l[e] = k; s[k] = e; var z = a[e], F = a[z], G = a[F], y = 257 * a[k] ^ 16843008 * k; t[e] = y << 24 | y >>> 8; r[e] = y << 16 | y >>> 16; w[e] = y << 8 | y >>> 24; v[e] = y; y = 16843009 * G ^ 65537 * F ^ 257 * z ^ 16843008 * e; b[k] = y << 24 | y >>> 8; x[k] = y << 16 | y >>> 16; q[k] = y << 8 | y >>> 24; n[k] = y; e ? (e = z ^ a[a[a[G ^ z]]], j ^= a[a[j]]) : e = j = 1 } var H = [0, 1, 2, 4, 8,
        16, 32, 64, 128, 27, 54], d = d.AES = p.extend({
            _doReset: function () {
                for (var a = this._key, c = a.words, d = a.sigBytes / 4, a = 4 * ((this._nRounds = d + 6) + 1), e = this._keySchedule = [], j = 0; j < a; j++)if (j < d) e[j] = c[j]; else { var k = e[j - 1]; j % d ? 6 < d && 4 == j % d && (k = l[k >>> 24] << 24 | l[k >>> 16 & 255] << 16 | l[k >>> 8 & 255] << 8 | l[k & 255]) : (k = k << 8 | k >>> 24, k = l[k >>> 24] << 24 | l[k >>> 16 & 255] << 16 | l[k >>> 8 & 255] << 8 | l[k & 255], k ^= H[j / d | 0] << 24); e[j] = e[j - d] ^ k } c = this._invKeySchedule = []; for (d = 0; d < a; d++)j = a - d, k = d % 4 ? e[j] : e[j - 4], c[d] = 4 > d || 4 >= j ? k : b[l[k >>> 24]] ^ x[l[k >>> 16 & 255]] ^ q[l[k >>>
                    8 & 255]] ^ n[l[k & 255]]
            }, encryptBlock: function (a, b) { this._doCryptBlock(a, b, this._keySchedule, t, r, w, v, l) }, decryptBlock: function (a, c) { var d = a[c + 1]; a[c + 1] = a[c + 3]; a[c + 3] = d; this._doCryptBlock(a, c, this._invKeySchedule, b, x, q, n, s); d = a[c + 1]; a[c + 1] = a[c + 3]; a[c + 3] = d }, _doCryptBlock: function (a, b, c, d, e, j, l, f) {
                for (var m = this._nRounds, g = a[b] ^ c[0], h = a[b + 1] ^ c[1], k = a[b + 2] ^ c[2], n = a[b + 3] ^ c[3], p = 4, r = 1; r < m; r++)var q = d[g >>> 24] ^ e[h >>> 16 & 255] ^ j[k >>> 8 & 255] ^ l[n & 255] ^ c[p++], s = d[h >>> 24] ^ e[k >>> 16 & 255] ^ j[n >>> 8 & 255] ^ l[g & 255] ^ c[p++], t =
                    d[k >>> 24] ^ e[n >>> 16 & 255] ^ j[g >>> 8 & 255] ^ l[h & 255] ^ c[p++], n = d[n >>> 24] ^ e[g >>> 16 & 255] ^ j[h >>> 8 & 255] ^ l[k & 255] ^ c[p++], g = q, h = s, k = t; q = (f[g >>> 24] << 24 | f[h >>> 16 & 255] << 16 | f[k >>> 8 & 255] << 8 | f[n & 255]) ^ c[p++]; s = (f[h >>> 24] << 24 | f[k >>> 16 & 255] << 16 | f[n >>> 8 & 255] << 8 | f[g & 255]) ^ c[p++]; t = (f[k >>> 24] << 24 | f[n >>> 16 & 255] << 16 | f[g >>> 8 & 255] << 8 | f[h & 255]) ^ c[p++]; n = (f[n >>> 24] << 24 | f[g >>> 16 & 255] << 16 | f[h >>> 8 & 255] << 8 | f[k & 255]) ^ c[p++]; a[b] = q; a[b + 1] = s; a[b + 2] = t; a[b + 3] = n
            }, keySize: 8
        }); u.AES = p._createHelper(d)
})();
'use strict';

(function iife($) {

    $.DurationPicker = function DurationPicker(mainElement, options) {

        var defaults = {
            translations: {
                day: 'day',
                hour: 'hour',
                minute: 'minute',
                second: 'second',
                days: 'days',
                hours: 'hours',
                minutes: 'minutes',
                seconds: 'seconds'
            },
            showSeconds: false,
            showDays: true
        };

        var plugin = this;

        plugin.settings = {};

        var mainInput = $(mainElement);

        plugin.init = function init() {
            plugin.settings = $.extend({}, defaults, options);

            var mainInputReplacer = $('<div>', {
                class: 'bdp-input',
                html: [buildDisplayBlock('days', !plugin.settings.showDays), buildDisplayBlock('hours', false, plugin.settings.showDays ? 23 : 99999), buildDisplayBlock('minutes', false, 59), buildDisplayBlock('seconds', !plugin.settings.showSeconds, 59)]
            });

            mainInput.after(mainInputReplacer).hide();

            if (mainInput.val() === '') mainInput.val(0);
            setValue(mainInput.val(), true);
        };

        var inputs = [];
        var labels = [];
        var disabled = mainInput.hasClass('disabled') || mainInput.attr('disabled') === 'disabled';

        var days = 0;
        var hours = 0;
        var minutes = 0;
        var seconds = 0;

        //
        // private methods
        //
        function translate(id) {
            if (id === 'days') {

                return 'D';
            }
            else if (id === 'hours') {
                return 'H';
            }

            return 'M';
        }
        function getTitle(id) {
            if (id === 'days') {

                return 'Days';
            }
            else if (id === 'hours') {
                return 'Hours';
            }

            return 'Minutes';
        }
        

        function updateWordLabel(value, label) {
            var text = value === 1 ? label.substring(0, label.length - 1) : label;
           // labels[label].text(translate(text));
        }

        function updateUI() {
            var isInitializing = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : false;

            var total = seconds + minutes * 60 + hours * 60 * 60 + days * 24 * 60 * 60;
            mainInput.val(total);
            mainInput.change();

            updateWordLabel(days, 'days');
            updateWordLabel(hours, 'hours');
            updateWordLabel(minutes, 'minutes');
            updateWordLabel(seconds, 'seconds');

            inputs.days.val(days);
            inputs.hours.val(hours);
            inputs.minutes.val(minutes);
            inputs.seconds.val(seconds);

            if (typeof plugin.settings.onChanged === 'function') {
                plugin.settings.onChanged(mainInput.val(), isInitializing);
            }
        }

        function durationPickerChanged() {
            days = parseInt(inputs.days.val(), 10) || 0;
            hours = parseInt(inputs.hours.val(), 10) || 0;
            minutes = parseInt(inputs.minutes.val(), 10) || 0;
            seconds = parseInt(inputs.seconds.val(), 10) || 0;
            updateUI();
        }

        function buildDisplayBlock(id, hidden, max) {
            var input = $('<input>', {
                class: 'bdp-input-sm',
                type: 'number',
                min: 0,
                value: 0,
                disabled: disabled
            }).change(durationPickerChanged);

            if (max) {
                input.attr('max', max);
            }
            inputs[id] = input;

            var label = $('<span>', {
                class: 'bdp-label',
                id: 'bdp-' + id + '-label',
                text: translate(id)
              
            });
            labels[id] = label;

            return $('<div>', {
                class: 'bdp-block ' + (hidden ? 'bdp-hidden' : ''),
                title: getTitle(id),
                html: [label, input]
            });
        }

        function setValue(value, isInitializing) {
            mainInput.val(value);

            var total = parseInt(value, 10);
            seconds = total % 60;
            total = Math.floor(total / 60);
            minutes = total % 60;
            total = Math.floor(total / 60);

            if (plugin.settings.showDays) {
                hours = total % 24;
                days = Math.floor(total / 24);
            } else {
                hours = total;
                days = 0;
            }

            updateUI(isInitializing);
        }

        //
        // public methods
        //
        plugin.setValue = function (value) {
            setValue(value, true);
        };

        plugin.destroy = function () {
            mainInput.next('.bdp-input').remove();
            mainInput.data('durationPicker', null).show();
        };

        plugin.init();
    };

    // eslint-disable-next-line no-param-reassign
    $.fn.durationPicker = function durationPicker(options) {
        return this.each(function () {
            if (undefined === $(this).data('durationPicker')) {
                var plugin = new $.DurationPicker(this, options);
                $(this).data('durationPicker', plugin);
            }
        });
    };
})(jQuery); // eslint-disable-line no-undef
/*! jsTree - v3.3.12 - 2021-09-03 - (MIT) */
!function(a){"use strict";"function"==typeof define&&define.amd?define(["jquery"],a):"undefined"!=typeof module&&module.exports?module.exports=a(require("jquery")):a(jQuery)}(function(a,b){"use strict";if(!a.jstree){var c=0,d=!1,e=!1,f=!1,g=[],h=a("script:last").attr("src"),i=window.document,j=window.setImmediate,k=window.Promise;!j&&k&&(j=function(a,b){k.resolve(b).then(a)}),a.jstree={version:"3.3.12",defaults:{plugins:[]},plugins:{},path:h&&-1!==h.indexOf("/")?h.replace(/\/[^\/]+$/,""):"",idregex:/[\\:&!^|()\[\]<>@*'+~#";.,=\- \/${}%?`]/g,root:"#"},a.jstree.create=function(b,d){var e=new a.jstree.core(++c),f=d;return d=a.extend(!0,{},a.jstree.defaults,d),f&&f.plugins&&(d.plugins=f.plugins),a.each(d.plugins,function(a,b){"core"!==a&&(e=e.plugin(b,d[b]))}),a(b).data("jstree",e),e.init(b,d),e},a.jstree.destroy=function(){a(".jstree:jstree").jstree("destroy"),a(i).off(".jstree")},a.jstree.core=function(a){this._id=a,this._cnt=0,this._wrk=null,this._data={core:{themes:{name:!1,dots:!1,icons:!1,ellipsis:!1},selected:[],last_error:{},working:!1,worker_queue:[],focused:null}}},a.jstree.reference=function(b){var c=null,d=null;if(!b||!b.id||b.tagName&&b.nodeType||(b=b.id),!d||!d.length)try{d=a(b)}catch(e){}if(!d||!d.length)try{d=a("#"+b.replace(a.jstree.idregex,"\\$&"))}catch(e){}return d&&d.length&&(d=d.closest(".jstree")).length&&(d=d.data("jstree"))?c=d:a(".jstree").each(function(){var d=a(this).data("jstree");return d&&d._model.data[b]?(c=d,!1):void 0}),c},a.fn.jstree=function(c){var d="string"==typeof c,e=Array.prototype.slice.call(arguments,1),f=null;return c!==!0||this.length?(this.each(function(){var g=a.jstree.reference(this),h=d&&g?g[c]:null;return f=d&&h?h.apply(g,e):null,g||d||c!==b&&!a.isPlainObject(c)||a.jstree.create(this,c),(g&&!d||c===!0)&&(f=g||!1),null!==f&&f!==b?!1:void 0}),null!==f&&f!==b?f:this):!1},a.expr.pseudos.jstree=a.expr.createPseudo(function(c){return function(c){return a(c).hasClass("jstree")&&a(c).data("jstree")!==b}}),a.jstree.defaults.core={data:!1,strings:!1,check_callback:!1,error:a.noop,animation:200,multiple:!0,themes:{name:!1,url:!1,dir:!1,dots:!0,icons:!0,ellipsis:!1,stripes:!1,variant:!1,responsive:!1},expand_selected_onload:!0,worker:!0,force_text:!1,dblclick_toggle:!0,loaded_state:!1,restore_focus:!0,compute_elements_positions:!1,keyboard:{"ctrl-space":function(b){b.type="click",a(b.currentTarget).trigger(b)},enter:function(b){b.type="click",a(b.currentTarget).trigger(b)},left:function(b){if(b.preventDefault(),this.is_open(b.currentTarget))this.close_node(b.currentTarget);else{var c=this.get_parent(b.currentTarget);c&&c.id!==a.jstree.root&&this.get_node(c,!0).children(".jstree-anchor").trigger("focus")}},up:function(a){a.preventDefault();var b=this.get_prev_dom(a.currentTarget);b&&b.length&&b.children(".jstree-anchor").trigger("focus")},right:function(b){if(b.preventDefault(),this.is_closed(b.currentTarget))this.open_node(b.currentTarget,function(a){this.get_node(a,!0).children(".jstree-anchor").trigger("focus")});else if(this.is_open(b.currentTarget)){var c=this.get_node(b.currentTarget,!0).children(".jstree-children")[0];c&&a(this._firstChild(c)).children(".jstree-anchor").trigger("focus")}},down:function(a){a.preventDefault();var b=this.get_next_dom(a.currentTarget);b&&b.length&&b.children(".jstree-anchor").trigger("focus")},"*":function(a){this.open_all()},home:function(b){b.preventDefault();var c=this._firstChild(this.get_container_ul()[0]);c&&a(c).children(".jstree-anchor").filter(":visible").trigger("focus")},end:function(a){a.preventDefault(),this.element.find(".jstree-anchor").filter(":visible").last().trigger("focus")},f2:function(a){a.preventDefault(),this.edit(a.currentTarget)}}},a.jstree.core.prototype={plugin:function(b,c){var d=a.jstree.plugins[b];return d?(this._data[b]={},d.prototype=this,new d(c,this)):this},init:function(b,c){this._model={data:{},changed:[],force_full_redraw:!1,redraw_timeout:!1,default_state:{loaded:!0,opened:!1,selected:!1,disabled:!1}},this._model.data[a.jstree.root]={id:a.jstree.root,parent:null,parents:[],children:[],children_d:[],state:{loaded:!1}},this.element=a(b).addClass("jstree jstree-"+this._id),this.settings=c,this._data.core.ready=!1,this._data.core.loaded=!1,this._data.core.rtl="rtl"===this.element.css("direction"),this.element[this._data.core.rtl?"addClass":"removeClass"]("jstree-rtl"),this.element.attr("role","tree"),this.settings.core.multiple&&this.element.attr("aria-multiselectable",!0),this.element.attr("tabindex")||this.element.attr("tabindex","0"),this.bind(),this.trigger("init"),this._data.core.original_container_html=this.element.find(" > ul > li").clone(!0),this._data.core.original_container_html.find("li").addBack().contents().filter(function(){return 3===this.nodeType&&(!this.nodeValue||/^\s+$/.test(this.nodeValue))}).remove(),this.element.html("<ul class='jstree-container-ul jstree-children' role='group'><li id='j"+this._id+"_loading' class='jstree-initial-node jstree-loading jstree-leaf jstree-last' role='none'><i class='jstree-icon jstree-ocl'></i><a class='jstree-anchor' role='treeitem' href='#'><i class='jstree-icon jstree-themeicon-hidden'></i>"+this.get_string("Loading ...")+"</a></li></ul>"),this.element.attr("aria-activedescendant","j"+this._id+"_loading"),this._data.core.li_height=this.get_container_ul().children("li").first().outerHeight()||24,this._data.core.node=this._create_prototype_node(),this.trigger("loading"),this.load_node(a.jstree.root)},destroy:function(a){if(this.trigger("destroy"),this._wrk)try{window.URL.revokeObjectURL(this._wrk),this._wrk=null}catch(b){}a||this.element.empty(),this.teardown()},_create_prototype_node:function(){var a=i.createElement("LI"),b,c;return a.setAttribute("role","none"),b=i.createElement("I"),b.className="jstree-icon jstree-ocl",b.setAttribute("role","presentation"),a.appendChild(b),b=i.createElement("A"),b.className="jstree-anchor",b.setAttribute("href","#"),b.setAttribute("tabindex","-1"),b.setAttribute("role","treeitem"),c=i.createElement("I"),c.className="jstree-icon jstree-themeicon",c.setAttribute("role","presentation"),b.appendChild(c),a.appendChild(b),b=c=null,a},_kbevent_to_func:function(a){var b={8:"Backspace",9:"Tab",13:"Enter",19:"Pause",27:"Esc",32:"Space",33:"PageUp",34:"PageDown",35:"End",36:"Home",37:"Left",38:"Up",39:"Right",40:"Down",44:"Print",45:"Insert",46:"Delete",96:"Numpad0",97:"Numpad1",98:"Numpad2",99:"Numpad3",100:"Numpad4",101:"Numpad5",102:"Numpad6",103:"Numpad7",104:"Numpad8",105:"Numpad9","-13":"NumpadEnter",112:"F1",113:"F2",114:"F3",115:"F4",116:"F5",117:"F6",118:"F7",119:"F8",120:"F9",121:"F10",122:"F11",123:"F12",144:"Numlock",145:"Scrolllock",16:"Shift",17:"Ctrl",18:"Alt",48:"0",49:"1",50:"2",51:"3",52:"4",53:"5",54:"6",55:"7",56:"8",57:"9",59:";",61:"=",65:"a",66:"b",67:"c",68:"d",69:"e",70:"f",71:"g",72:"h",73:"i",74:"j",75:"k",76:"l",77:"m",78:"n",79:"o",80:"p",81:"q",82:"r",83:"s",84:"t",85:"u",86:"v",87:"w",88:"x",89:"y",90:"z",107:"+",109:"-",110:".",186:";",187:"=",188:",",189:"-",190:".",191:"/",192:"`",219:"[",220:"\\",221:"]",222:"'",111:"/",106:"*",173:"-"},c=[];if(a.ctrlKey&&c.push("ctrl"),a.altKey&&c.push("alt"),a.shiftKey&&c.push("shift"),c.push(b[a.which]||a.which),c=c.sort().join("-").toLowerCase(),"shift-shift"===c||"ctrl-ctrl"===c||"alt-alt"===c)return null;var d=this.settings.core.keyboard,e,f;for(e in d)if(d.hasOwnProperty(e)&&(f=e,"-"!==f&&"+"!==f&&(f=f.replace("--","-MINUS").replace("+-","-MINUS").replace("++","-PLUS").replace("-+","-PLUS"),f=f.split(/-|\+/).sort().join("-").replace("MINUS","-").replace("PLUS","+").toLowerCase()),f===c))return d[e];return null},teardown:function(){this.unbind(),this.element.removeClass("jstree").removeData("jstree").find("[class^='jstree']").addBack().attr("class",function(){return this.className.replace(/jstree[^ ]*|$/gi,"")}),this.element=null},bind:function(){var b="",c=null,d=0;this.element.on("dblclick.jstree",function(a){if(a.target.tagName&&"input"===a.target.tagName.toLowerCase())return!0;if(i.selection&&i.selection.empty)i.selection.empty();else if(window.getSelection){var b=window.getSelection();try{b.removeAllRanges(),b.collapse()}catch(c){}}}).on("mousedown.jstree",function(a){a.target===this.element[0]&&(a.preventDefault(),d=+new Date)}.bind(this)).on("mousedown.jstree",".jstree-ocl",function(a){a.preventDefault()}).on("click.jstree",".jstree-ocl",function(a){this.toggle_node(a.target)}.bind(this)).on("dblclick.jstree",".jstree-anchor",function(a){return a.target.tagName&&"input"===a.target.tagName.toLowerCase()?!0:void(this.settings.core.dblclick_toggle&&this.toggle_node(a.target))}.bind(this)).on("click.jstree",".jstree-anchor",function(b){b.preventDefault(),b.currentTarget!==i.activeElement&&a(b.currentTarget).trigger("focus"),this.activate_node(b.currentTarget,b)}.bind(this)).on("keydown.jstree",".jstree-anchor",function(a){if(a.target.tagName&&"input"===a.target.tagName.toLowerCase())return!0;this._data.core.rtl&&(37===a.which?a.which=39:39===a.which&&(a.which=37));var b=this._kbevent_to_func(a);if(b){var c=b.call(this,a);if(c===!1||c===!0)return c}}.bind(this)).on("load_node.jstree",function(b,c){c.status&&(c.node.id!==a.jstree.root||this._data.core.loaded||(this._data.core.loaded=!0,this._firstChild(this.get_container_ul()[0])&&this.element.attr("aria-activedescendant",this._firstChild(this.get_container_ul()[0]).id),this.trigger("loaded")),this._data.core.ready||setTimeout(function(){if(this.element&&!this.get_container_ul().find(".jstree-loading").length){if(this._data.core.ready=!0,this._data.core.selected.length){if(this.settings.core.expand_selected_onload){var b=[],c,d;for(c=0,d=this._data.core.selected.length;d>c;c++)b=b.concat(this._model.data[this._data.core.selected[c]].parents);for(b=a.vakata.array_unique(b),c=0,d=b.length;d>c;c++)this.open_node(b[c],!1,0)}this.trigger("changed",{action:"ready",selected:this._data.core.selected})}this.trigger("ready")}}.bind(this),0))}.bind(this)).on("keypress.jstree",function(d){if(d.target.tagName&&"input"===d.target.tagName.toLowerCase())return!0;c&&clearTimeout(c),c=setTimeout(function(){b=""},500);var e=String.fromCharCode(d.which).toLowerCase(),f=this.element.find(".jstree-anchor").filter(":visible"),g=f.index(i.activeElement)||0,h=!1;if(b+=e,b.length>1){if(f.slice(g).each(function(c,d){return 0===a(d).text().toLowerCase().indexOf(b)?(a(d).trigger("focus"),h=!0,!1):void 0}.bind(this)),h)return;if(f.slice(0,g).each(function(c,d){return 0===a(d).text().toLowerCase().indexOf(b)?(a(d).trigger("focus"),h=!0,!1):void 0}.bind(this)),h)return}if(new RegExp("^"+e.replace(/[-\/\\^$*+?.()|[\]{}]/g,"\\$&")+"+$").test(b)){if(f.slice(g+1).each(function(b,c){return a(c).text().toLowerCase().charAt(0)===e?(a(c).trigger("focus"),h=!0,!1):void 0}.bind(this)),h)return;if(f.slice(0,g+1).each(function(b,c){return a(c).text().toLowerCase().charAt(0)===e?(a(c).trigger("focus"),h=!0,!1):void 0}.bind(this)),h)return}}.bind(this)).on("init.jstree",function(){var a=this.settings.core.themes;this._data.core.themes.dots=a.dots,this._data.core.themes.stripes=a.stripes,this._data.core.themes.icons=a.icons,this._data.core.themes.ellipsis=a.ellipsis,this.set_theme(a.name||"default",a.url),this.set_theme_variant(a.variant)}.bind(this)).on("loading.jstree",function(){this[this._data.core.themes.dots?"show_dots":"hide_dots"](),this[this._data.core.themes.icons?"show_icons":"hide_icons"](),this[this._data.core.themes.stripes?"show_stripes":"hide_stripes"](),this[this._data.core.themes.ellipsis?"show_ellipsis":"hide_ellipsis"]()}.bind(this)).on("blur.jstree",".jstree-anchor",function(b){this._data.core.focused=null,a(b.currentTarget).filter(".jstree-hovered").trigger("mouseleave"),this.element.attr("tabindex","0")}.bind(this)).on("focus.jstree",".jstree-anchor",function(b){var c=this.get_node(b.currentTarget);c&&c.id&&(this._data.core.focused=c.id),this.element.find(".jstree-hovered").not(b.currentTarget).trigger("mouseleave"),a(b.currentTarget).trigger("mouseenter"),this.element.attr("tabindex","-1")}.bind(this)).on("focus.jstree",function(){if(+new Date-d>500&&!this._data.core.focused&&this.settings.core.restore_focus){d=0;var a=this.get_node(this.element.attr("aria-activedescendant"),!0);a&&a.find("> .jstree-anchor").trigger("focus")}}.bind(this)).on("mouseenter.jstree",".jstree-anchor",function(a){this.hover_node(a.currentTarget)}.bind(this)).on("mouseleave.jstree",".jstree-anchor",function(a){this.dehover_node(a.currentTarget)}.bind(this))},unbind:function(){this.element.off(".jstree"),a(i).off(".jstree-"+this._id)},trigger:function(a,b){b||(b={}),b.instance=this,this.element.triggerHandler(a.replace(".jstree","")+".jstree",b)},get_container:function(){return this.element},get_container_ul:function(){return this.element.children(".jstree-children").first()},get_string:function(b){var c=this.settings.core.strings;return a.vakata.is_function(c)?c.call(this,b):c&&c[b]?c[b]:b},_firstChild:function(a){a=a?a.firstChild:null;while(null!==a&&1!==a.nodeType)a=a.nextSibling;return a},_nextSibling:function(a){a=a?a.nextSibling:null;while(null!==a&&1!==a.nodeType)a=a.nextSibling;return a},_previousSibling:function(a){a=a?a.previousSibling:null;while(null!==a&&1!==a.nodeType)a=a.previousSibling;return a},get_node:function(b,c){b&&b.id&&(b=b.id),b instanceof a&&b.length&&b[0].id&&(b=b[0].id);var d;try{if(this._model.data[b])b=this._model.data[b];else if("string"==typeof b&&this._model.data[b.replace(/^#/,"")])b=this._model.data[b.replace(/^#/,"")];else if("string"==typeof b&&(d=a("#"+b.replace(a.jstree.idregex,"\\$&"),this.element)).length&&this._model.data[d.closest(".jstree-node").attr("id")])b=this._model.data[d.closest(".jstree-node").attr("id")];else if((d=this.element.find(b)).length&&this._model.data[d.closest(".jstree-node").attr("id")])b=this._model.data[d.closest(".jstree-node").attr("id")];else{if(!(d=this.element.find(b)).length||!d.hasClass("jstree"))return!1;b=this._model.data[a.jstree.root]}return c&&(b=b.id===a.jstree.root?this.element:a("#"+b.id.replace(a.jstree.idregex,"\\$&"),this.element)),b}catch(e){return!1}},get_path:function(b,c,d){if(b=b.parents?b:this.get_node(b),!b||b.id===a.jstree.root||!b.parents)return!1;var e,f,g=[];for(g.push(d?b.id:b.text),e=0,f=b.parents.length;f>e;e++)g.push(d?b.parents[e]:this.get_text(b.parents[e]));return g=g.reverse().slice(1),c?g.join(c):g},get_next_dom:function(b,c){var d;if(b=this.get_node(b,!0),b[0]===this.element[0]){d=this._firstChild(this.get_container_ul()[0]);while(d&&0===d.offsetHeight)d=this._nextSibling(d);return d?a(d):!1}if(!b||!b.length)return!1;if(c){d=b[0];do d=this._nextSibling(d);while(d&&0===d.offsetHeight);return d?a(d):!1}if(b.hasClass("jstree-open")){d=this._firstChild(b.children(".jstree-children")[0]);while(d&&0===d.offsetHeight)d=this._nextSibling(d);if(null!==d)return a(d)}d=b[0];do d=this._nextSibling(d);while(d&&0===d.offsetHeight);return null!==d?a(d):b.parentsUntil(".jstree",".jstree-node").nextAll(".jstree-node:visible").first()},get_prev_dom:function(b,c){var d;if(b=this.get_node(b,!0),b[0]===this.element[0]){d=this.get_container_ul()[0].lastChild;while(d&&0===d.offsetHeight)d=this._previousSibling(d);return d?a(d):!1}if(!b||!b.length)return!1;if(c){d=b[0];do d=this._previousSibling(d);while(d&&0===d.offsetHeight);return d?a(d):!1}d=b[0];do d=this._previousSibling(d);while(d&&0===d.offsetHeight);if(null!==d){b=a(d);while(b.hasClass("jstree-open"))b=b.children(".jstree-children").first().children(".jstree-node:visible:last");return b}return d=b[0].parentNode.parentNode,d&&d.className&&-1!==d.className.indexOf("jstree-node")?a(d):!1},get_parent:function(b){return b=this.get_node(b),b&&b.id!==a.jstree.root?b.parent:!1},get_children_dom:function(a){return a=this.get_node(a,!0),a[0]===this.element[0]?this.get_container_ul().children(".jstree-node"):a&&a.length?a.children(".jstree-children").children(".jstree-node"):!1},is_parent:function(a){return a=this.get_node(a),a&&(a.state.loaded===!1||a.children.length>0)},is_loaded:function(a){return a=this.get_node(a),a&&a.state.loaded},is_loading:function(a){return a=this.get_node(a),a&&a.state&&a.state.loading},is_open:function(a){return a=this.get_node(a),a&&a.state.opened},is_closed:function(a){return a=this.get_node(a),a&&this.is_parent(a)&&!a.state.opened},is_leaf:function(a){return!this.is_parent(a)},load_node:function(b,c){var d,e,f,g,h;if(a.vakata.is_array(b))return this._load_nodes(b.slice(),c),!0;if(b=this.get_node(b),!b)return c&&c.call(this,b,!1),!1;if(b.state.loaded){for(b.state.loaded=!1,f=0,g=b.parents.length;g>f;f++)this._model.data[b.parents[f]].children_d=a.vakata.array_filter(this._model.data[b.parents[f]].children_d,function(c){return-1===a.inArray(c,b.children_d)});for(d=0,e=b.children_d.length;e>d;d++)this._model.data[b.children_d[d]].state.selected&&(h=!0),delete this._model.data[b.children_d[d]];h&&(this._data.core.selected=a.vakata.array_filter(this._data.core.selected,function(c){return-1===a.inArray(c,b.children_d)})),b.children=[],b.children_d=[],h&&this.trigger("changed",{action:"load_node",node:b,selected:this._data.core.selected})}return b.state.failed=!1,b.state.loading=!0,this.get_node(b,!0).addClass("jstree-loading").attr("aria-busy",!0),this._load_node(b,function(a){b=this._model.data[b.id],b.state.loading=!1,b.state.loaded=a,b.state.failed=!b.state.loaded;var d=this.get_node(b,!0),e=0,f=0,g=this._model.data,h=!1;for(e=0,f=b.children.length;f>e;e++)if(g[b.children[e]]&&!g[b.children[e]].state.hidden){h=!0;break}b.state.loaded&&d&&d.length&&(d.removeClass("jstree-closed jstree-open jstree-leaf"),h?"#"!==b.id&&d.addClass(b.state.opened?"jstree-open":"jstree-closed"):d.addClass("jstree-leaf")),d.removeClass("jstree-loading").attr("aria-busy",!1),this.trigger("load_node",{node:b,status:a}),c&&c.call(this,b,a)}.bind(this)),!0},_load_nodes:function(a,b,c,d){var e=!0,f=function(){this._load_nodes(a,b,!0)},g=this._model.data,h,i,j=[];for(h=0,i=a.length;i>h;h++)g[a[h]]&&(!g[a[h]].state.loaded&&!g[a[h]].state.failed||!c&&d)&&(this.is_loading(a[h])||this.load_node(a[h],f),e=!1);if(e){for(h=0,i=a.length;i>h;h++)g[a[h]]&&g[a[h]].state.loaded&&j.push(a[h]);b&&!b.done&&(b.call(this,j),b.done=!0)}},load_all:function(b,c){if(b||(b=a.jstree.root),b=this.get_node(b),!b)return!1;var d=[],e=this._model.data,f=e[b.id].children_d,g,h;for(b.state&&!b.state.loaded&&d.push(b.id),g=0,h=f.length;h>g;g++)e[f[g]]&&e[f[g]].state&&!e[f[g]].state.loaded&&d.push(f[g]);d.length?this._load_nodes(d,function(){this.load_all(b,c)}):(c&&c.call(this,b),this.trigger("load_all",{node:b}))},_load_node:function(b,c){var d=this.settings.core.data,e,f=function g(){return 3!==this.nodeType&&8!==this.nodeType};return d?a.vakata.is_function(d)?d.call(this,b,function(d){d===!1?c.call(this,!1):this["string"==typeof d?"_append_html_data":"_append_json_data"](b,"string"==typeof d?a(a.parseHTML(d)).filter(f):d,function(a){c.call(this,a)})}.bind(this)):"object"==typeof d?d.url?(d=a.extend(!0,{},d),a.vakata.is_function(d.url)&&(d.url=d.url.call(this,b)),a.vakata.is_function(d.data)&&(d.data=d.data.call(this,b)),a.ajax(d).done(function(d,e,g){var h=g.getResponseHeader("Content-Type");return h&&-1!==h.indexOf("json")||"object"==typeof d?this._append_json_data(b,d,function(a){c.call(this,a)}):h&&-1!==h.indexOf("html")||"string"==typeof d?this._append_html_data(b,a(a.parseHTML(d)).filter(f),function(a){c.call(this,a)}):(this._data.core.last_error={error:"ajax",plugin:"core",id:"core_04",reason:"Could not load node",data:JSON.stringify({id:b.id,xhr:g})},this.settings.core.error.call(this,this._data.core.last_error),c.call(this,!1))}.bind(this)).fail(function(a){this._data.core.last_error={error:"ajax",plugin:"core",id:"core_04",reason:"Could not load node",data:JSON.stringify({id:b.id,xhr:a})},c.call(this,!1),this.settings.core.error.call(this,this._data.core.last_error)}.bind(this))):(e=a.vakata.is_array(d)?a.extend(!0,[],d):a.isPlainObject(d)?a.extend(!0,{},d):d,b.id===a.jstree.root?this._append_json_data(b,e,function(a){c.call(this,a)}):(this._data.core.last_error={error:"nodata",plugin:"core",id:"core_05",reason:"Could not load node",data:JSON.stringify({id:b.id})},this.settings.core.error.call(this,this._data.core.last_error),c.call(this,!1))):"string"==typeof d?b.id===a.jstree.root?this._append_html_data(b,a(a.parseHTML(d)).filter(f),function(a){c.call(this,a)}):(this._data.core.last_error={error:"nodata",plugin:"core",id:"core_06",reason:"Could not load node",data:JSON.stringify({id:b.id})},this.settings.core.error.call(this,this._data.core.last_error),c.call(this,!1)):c.call(this,!1):b.id===a.jstree.root?this._append_html_data(b,this._data.core.original_container_html.clone(!0),function(a){c.call(this,a)}):c.call(this,!1)},_node_changed:function(b){b=this.get_node(b),b&&-1===a.inArray(b.id,this._model.changed)&&this._model.changed.push(b.id)},_append_html_data:function(b,c,d){b=this.get_node(b),b.children=[],b.children_d=[];var e=c.is("ul")?c.children():c,f=b.id,g=[],h=[],i=this._model.data,j=i[f],k=this._data.core.selected.length,l,m,n;for(e.each(function(b,c){l=this._parse_model_from_html(a(c),f,j.parents.concat()),l&&(g.push(l),h.push(l),i[l].children_d.length&&(h=h.concat(i[l].children_d)))}.bind(this)),j.children=g,j.children_d=h,m=0,n=j.parents.length;n>m;m++)i[j.parents[m]].children_d=i[j.parents[m]].children_d.concat(h);this.trigger("model",{nodes:h,parent:f}),f!==a.jstree.root?(this._node_changed(f),this.redraw()):(this.get_container_ul().children(".jstree-initial-node").remove(),this.redraw(!0)),this._data.core.selected.length!==k&&this.trigger("changed",{action:"model",selected:this._data.core.selected}),d.call(this,!0)},_append_json_data:function(b,c,d,e){if(null!==this.element){b=this.get_node(b),b.children=[],b.children_d=[],c.d&&(c=c.d,"string"==typeof c&&(c=JSON.parse(c))),a.vakata.is_array(c)||(c=[c]);var f=null,g={df:this._model.default_state,dat:c,par:b.id,m:this._model.data,t_id:this._id,t_cnt:this._cnt,sel:this._data.core.selected},h=this,i=function(a,b){a.data&&(a=a.data);var c=a.dat,d=a.par,e=[],f=[],g=[],i=a.df,j=a.t_id,k=a.t_cnt,l=a.m,m=l[d],n=a.sel,o,p,q,r,s=function(a,c,d){d=d?d.concat():[],c&&d.unshift(c);var e=a.id.toString(),f,h,j,k,m={id:e,text:a.text||"",icon:a.icon!==b?a.icon:!0,parent:c,parents:d,children:a.children||[],children_d:a.children_d||[],data:a.data,state:{},li_attr:{id:!1},a_attr:{href:"#"},original:!1};for(f in i)i.hasOwnProperty(f)&&(m.state[f]=i[f]);if(a&&a.data&&a.data.jstree&&a.data.jstree.icon&&(m.icon=a.data.jstree.icon),(m.icon===b||null===m.icon||""===m.icon)&&(m.icon=!0),a&&a.data&&(m.data=a.data,a.data.jstree))for(f in a.data.jstree)a.data.jstree.hasOwnProperty(f)&&(m.state[f]=a.data.jstree[f]);if(a&&"object"==typeof a.state)for(f in a.state)a.state.hasOwnProperty(f)&&(m.state[f]=a.state[f]);if(a&&"object"==typeof a.li_attr)for(f in a.li_attr)a.li_attr.hasOwnProperty(f)&&(m.li_attr[f]=a.li_attr[f]);if(m.li_attr.id||(m.li_attr.id=e),a&&"object"==typeof a.a_attr)for(f in a.a_attr)a.a_attr.hasOwnProperty(f)&&(m.a_attr[f]=a.a_attr[f]);for(a&&a.children&&a.children===!0&&(m.state.loaded=!1,m.children=[],m.children_d=[]),l[m.id]=m,f=0,h=m.children.length;h>f;f++)j=s(l[m.children[f]],m.id,d),k=l[j],m.children_d.push(j),k.children_d.length&&(m.children_d=m.children_d.concat(k.children_d));return delete a.data,delete a.children,l[m.id].original=a,m.state.selected&&g.push(m.id),m.id},t=function(a,c,d){d=d?d.concat():[],c&&d.unshift(c);var e=!1,f,h,m,n,o;do e="j"+j+"_"+ ++k;while(l[e]);o={id:!1,text:"string"==typeof a?a:"",icon:"object"==typeof a&&a.icon!==b?a.icon:!0,parent:c,parents:d,children:[],children_d:[],data:null,state:{},li_attr:{id:!1},a_attr:{href:"#"},original:!1};for(f in i)i.hasOwnProperty(f)&&(o.state[f]=i[f]);if(a&&a.id&&(o.id=a.id.toString()),a&&a.text&&(o.text=a.text),a&&a.data&&a.data.jstree&&a.data.jstree.icon&&(o.icon=a.data.jstree.icon),(o.icon===b||null===o.icon||""===o.icon)&&(o.icon=!0),a&&a.data&&(o.data=a.data,a.data.jstree))for(f in a.data.jstree)a.data.jstree.hasOwnProperty(f)&&(o.state[f]=a.data.jstree[f]);if(a&&"object"==typeof a.state)for(f in a.state)a.state.hasOwnProperty(f)&&(o.state[f]=a.state[f]);if(a&&"object"==typeof a.li_attr)for(f in a.li_attr)a.li_attr.hasOwnProperty(f)&&(o.li_attr[f]=a.li_attr[f]);if(o.li_attr.id&&!o.id&&(o.id=o.li_attr.id.toString()),o.id||(o.id=e),o.li_attr.id||(o.li_attr.id=o.id),a&&"object"==typeof a.a_attr)for(f in a.a_attr)a.a_attr.hasOwnProperty(f)&&(o.a_attr[f]=a.a_attr[f]);if(a&&a.children&&a.children.length){for(f=0,h=a.children.length;h>f;f++)m=t(a.children[f],o.id,d),n=l[m],o.children.push(m),n.children_d.length&&(o.children_d=o.children_d.concat(n.children_d));o.children_d=o.children_d.concat(o.children)}return a&&a.children&&a.children===!0&&(o.state.loaded=!1,o.children=[],o.children_d=[]),delete a.data,delete a.children,o.original=a,l[o.id]=o,o.state.selected&&g.push(o.id),o.id};if(c.length&&c[0].id!==b&&c[0].parent!==b){for(p=0,q=c.length;q>p;p++)c[p].children||(c[p].children=[]),c[p].state||(c[p].state={}),l[c[p].id.toString()]=c[p];for(p=0,q=c.length;q>p;p++)l[c[p].parent.toString()]?(l[c[p].parent.toString()].children.push(c[p].id.toString()),m.children_d.push(c[p].id.toString())):"undefined"!=typeof h&&(h._data.core.last_error={error:"parse",plugin:"core",id:"core_07",reason:"Node with invalid parent",data:JSON.stringify({id:c[p].id.toString(),parent:c[p].parent.toString()})},h.settings.core.error.call(h,h._data.core.last_error));for(p=0,q=m.children.length;q>p;p++)o=s(l[m.children[p]],d,m.parents.concat()),f.push(o),l[o].children_d.length&&(f=f.concat(l[o].children_d));for(p=0,q=m.parents.length;q>p;p++)l[m.parents[p]].children_d=l[m.parents[p]].children_d.concat(f);r={cnt:k,mod:l,sel:n,par:d,dpc:f,add:g}}else{for(p=0,q=c.length;q>p;p++)o=t(c[p],d,m.parents.concat()),o&&(e.push(o),f.push(o),l[o].children_d.length&&(f=f.concat(l[o].children_d)));for(m.children=e,m.children_d=f,p=0,q=m.parents.length;q>p;p++)l[m.parents[p]].children_d=l[m.parents[p]].children_d.concat(f);r={cnt:k,mod:l,sel:n,par:d,dpc:f,add:g}}return"undefined"!=typeof window&&"undefined"!=typeof window.document?r:void postMessage(r)},k=function(b,c){if(null!==this.element){this._cnt=b.cnt;var e,f=this._model.data;for(e in f)f.hasOwnProperty(e)&&f[e].state&&f[e].state.loading&&b.mod[e]&&(b.mod[e].state.loading=!0);if(this._model.data=b.mod,c){var g,i=b.add,k=b.sel,l=this._data.core.selected.slice();if(f=this._model.data,k.length!==l.length||a.vakata.array_unique(k.concat(l)).length!==k.length){for(e=0,g=k.length;g>e;e++)-1===a.inArray(k[e],i)&&-1===a.inArray(k[e],l)&&(f[k[e]].state.selected=!1);for(e=0,g=l.length;g>e;e++)-1===a.inArray(l[e],k)&&(f[l[e]].state.selected=!0)}}b.add.length&&(this._data.core.selected=this._data.core.selected.concat(b.add)),this.trigger("model",{nodes:b.dpc,parent:b.par}),b.par!==a.jstree.root?(this._node_changed(b.par),this.redraw()):this.redraw(!0),b.add.length&&this.trigger("changed",{action:"model",selected:this._data.core.selected}),!c&&j?j(function(){d.call(h,!0)}):d.call(h,!0)}};if(this.settings.core.worker&&window.Blob&&window.URL&&window.Worker)try{null===this._wrk&&(this._wrk=window.URL.createObjectURL(new window.Blob(["self.onmessage = "+i.toString()],{type:"text/javascript"}))),!this._data.core.working||e?(this._data.core.working=!0,f=new window.Worker(this._wrk),f.onmessage=function(a){k.call(this,a.data,!0);try{f.terminate(),f=null}catch(b){}this._data.core.worker_queue.length?this._append_json_data.apply(this,this._data.core.worker_queue.shift()):this._data.core.working=!1}.bind(this),g.par?f.postMessage(g):this._data.core.worker_queue.length?this._append_json_data.apply(this,this._data.core.worker_queue.shift()):this._data.core.working=!1):this._data.core.worker_queue.push([b,c,d,!0])}catch(l){k.call(this,i(g),!1),this._data.core.worker_queue.length?this._append_json_data.apply(this,this._data.core.worker_queue.shift()):this._data.core.working=!1}else k.call(this,i(g),!1)}},_parse_model_from_html:function(c,d,e){e=e?[].concat(e):[],d&&e.unshift(d);var f,g,h=this._model.data,i={id:!1,text:!1,icon:!0,parent:d,parents:e,children:[],children_d:[],data:null,state:{},li_attr:{id:!1},a_attr:{href:"#"},original:!1},j,k,l;for(j in this._model.default_state)this._model.default_state.hasOwnProperty(j)&&(i.state[j]=this._model.default_state[j]);if(k=a.vakata.attributes(c,!0),a.each(k,function(b,c){return c=a.vakata.trim(c),c.length?(i.li_attr[b]=c,void("id"===b&&(i.id=c.toString()))):!0}),k=c.children("a").first(),k.length&&(k=a.vakata.attributes(k,!0),a.each(k,function(b,c){c=a.vakata.trim(c),c.length&&(i.a_attr[b]=c)})),k=c.children("a").first().length?c.children("a").first().clone():c.clone(),k.children("ins, i, ul").remove(),k=k.html(),k=a("<div></div>").html(k),i.text=this.settings.core.force_text?k.text():k.html(),k=c.data(),i.data=k?a.extend(!0,{},k):null,i.state.opened=c.hasClass("jstree-open"),i.state.selected=c.children("a").hasClass("jstree-clicked"),i.state.disabled=c.children("a").hasClass("jstree-disabled"),i.data&&i.data.jstree)for(j in i.data.jstree)i.data.jstree.hasOwnProperty(j)&&(i.state[j]=i.data.jstree[j]);k=c.children("a").children(".jstree-themeicon"),k.length&&(i.icon=k.hasClass("jstree-themeicon-hidden")?!1:k.attr("rel")),i.state.icon!==b&&(i.icon=i.state.icon),(i.icon===b||null===i.icon||""===i.icon)&&(i.icon=!0),k=c.children("ul").children("li");do l="j"+this._id+"_"+ ++this._cnt;while(h[l]);return i.id=i.li_attr.id?i.li_attr.id.toString():l,k.length?(k.each(function(b,c){f=this._parse_model_from_html(a(c),i.id,e),g=this._model.data[f],i.children.push(f),g.children_d.length&&(i.children_d=i.children_d.concat(g.children_d))}.bind(this)),i.children_d=i.children_d.concat(i.children)):c.hasClass("jstree-closed")&&(i.state.loaded=!1),i.li_attr["class"]&&(i.li_attr["class"]=i.li_attr["class"].replace("jstree-closed","").replace("jstree-open","")),i.a_attr["class"]&&(i.a_attr["class"]=i.a_attr["class"].replace("jstree-clicked","").replace("jstree-disabled","")),h[i.id]=i,i.state.selected&&this._data.core.selected.push(i.id),i.id},_parse_model_from_flat_json:function(a,c,d){d=d?d.concat():[],c&&d.unshift(c);var e=a.id.toString(),f=this._model.data,g=this._model.default_state,h,i,j,k,l={id:e,text:a.text||"",icon:a.icon!==b?a.icon:!0,parent:c,parents:d,children:a.children||[],children_d:a.children_d||[],data:a.data,state:{},li_attr:{id:!1},a_attr:{href:"#"},original:!1};for(h in g)g.hasOwnProperty(h)&&(l.state[h]=g[h]);if(a&&a.data&&a.data.jstree&&a.data.jstree.icon&&(l.icon=a.data.jstree.icon),(l.icon===b||null===l.icon||""===l.icon)&&(l.icon=!0),a&&a.data&&(l.data=a.data,a.data.jstree))for(h in a.data.jstree)a.data.jstree.hasOwnProperty(h)&&(l.state[h]=a.data.jstree[h]);if(a&&"object"==typeof a.state)for(h in a.state)a.state.hasOwnProperty(h)&&(l.state[h]=a.state[h]);if(a&&"object"==typeof a.li_attr)for(h in a.li_attr)a.li_attr.hasOwnProperty(h)&&(l.li_attr[h]=a.li_attr[h]);if(l.li_attr.id||(l.li_attr.id=e),a&&"object"==typeof a.a_attr)for(h in a.a_attr)a.a_attr.hasOwnProperty(h)&&(l.a_attr[h]=a.a_attr[h]);for(a&&a.children&&a.children===!0&&(l.state.loaded=!1,l.children=[],l.children_d=[]),f[l.id]=l,h=0,i=l.children.length;i>h;h++)j=this._parse_model_from_flat_json(f[l.children[h]],l.id,d),k=f[j],l.children_d.push(j),k.children_d.length&&(l.children_d=l.children_d.concat(k.children_d));return delete a.data,delete a.children,f[l.id].original=a,l.state.selected&&this._data.core.selected.push(l.id),l.id},_parse_model_from_json:function(a,c,d){d=d?d.concat():[],c&&d.unshift(c);var e=!1,f,g,h,i,j=this._model.data,k=this._model.default_state,l;do e="j"+this._id+"_"+ ++this._cnt;while(j[e]);l={id:!1,text:"string"==typeof a?a:"",icon:"object"==typeof a&&a.icon!==b?a.icon:!0,parent:c,parents:d,children:[],children_d:[],data:null,state:{},li_attr:{id:!1},a_attr:{href:"#"},original:!1};for(f in k)k.hasOwnProperty(f)&&(l.state[f]=k[f]);if(a&&a.id&&(l.id=a.id.toString()),a&&a.text&&(l.text=a.text),a&&a.data&&a.data.jstree&&a.data.jstree.icon&&(l.icon=a.data.jstree.icon),(l.icon===b||null===l.icon||""===l.icon)&&(l.icon=!0),
a&&a.data&&(l.data=a.data,a.data.jstree))for(f in a.data.jstree)a.data.jstree.hasOwnProperty(f)&&(l.state[f]=a.data.jstree[f]);if(a&&"object"==typeof a.state)for(f in a.state)a.state.hasOwnProperty(f)&&(l.state[f]=a.state[f]);if(a&&"object"==typeof a.li_attr)for(f in a.li_attr)a.li_attr.hasOwnProperty(f)&&(l.li_attr[f]=a.li_attr[f]);if(l.li_attr.id&&!l.id&&(l.id=l.li_attr.id.toString()),l.id||(l.id=e),l.li_attr.id||(l.li_attr.id=l.id),a&&"object"==typeof a.a_attr)for(f in a.a_attr)a.a_attr.hasOwnProperty(f)&&(l.a_attr[f]=a.a_attr[f]);if(a&&a.children&&a.children.length){for(f=0,g=a.children.length;g>f;f++)h=this._parse_model_from_json(a.children[f],l.id,d),i=j[h],l.children.push(h),i.children_d.length&&(l.children_d=l.children_d.concat(i.children_d));l.children_d=l.children.concat(l.children_d)}return a&&a.children&&a.children===!0&&(l.state.loaded=!1,l.children=[],l.children_d=[]),delete a.data,delete a.children,l.original=a,j[l.id]=l,l.state.selected&&this._data.core.selected.push(l.id),l.id},_redraw:function(){var b=this._model.force_full_redraw?this._model.data[a.jstree.root].children.concat([]):this._model.changed.concat([]),c=i.createElement("UL"),d,e,f,g=this._data.core.focused;for(e=0,f=b.length;f>e;e++)d=this.redraw_node(b[e],!0,this._model.force_full_redraw),d&&this._model.force_full_redraw&&c.appendChild(d);this._model.force_full_redraw&&(c.className=this.get_container_ul()[0].className,c.setAttribute("role","group"),this.element.empty().append(c)),null!==g&&this.settings.core.restore_focus&&(d=this.get_node(g,!0),d&&d.length&&d.children(".jstree-anchor")[0]!==i.activeElement?d.children(".jstree-anchor").trigger("focus"):this._data.core.focused=null),this._model.force_full_redraw=!1,this._model.changed=[],this.trigger("redraw",{nodes:b})},redraw:function(a){a&&(this._model.force_full_redraw=!0),this._redraw()},draw_children:function(b){var c=this.get_node(b),d=!1,e=!1,f=!1,g=i;if(!c)return!1;if(c.id===a.jstree.root)return this.redraw(!0);if(b=this.get_node(b,!0),!b||!b.length)return!1;if(b.children(".jstree-children").remove(),b=b[0],c.children.length&&c.state.loaded){for(f=g.createElement("UL"),f.setAttribute("role","group"),f.className="jstree-children",d=0,e=c.children.length;e>d;d++)f.appendChild(this.redraw_node(c.children[d],!0,!0));b.appendChild(f)}},redraw_node:function(b,c,d,e){var f=this.get_node(b),g=!1,h=!1,j=!1,k=!1,l=!1,m=!1,n="",o=i,p=this._model.data,q=!1,r=!1,s=null,t=0,u=0,v=!1,w=!1;if(!f)return!1;if(f.id===a.jstree.root)return this.redraw(!0);if(c=c||0===f.children.length,b=i.querySelector?this.element[0].querySelector("#"+(-1!=="0123456789".indexOf(f.id[0])?"\\3"+f.id[0]+" "+f.id.substr(1).replace(a.jstree.idregex,"\\$&"):f.id.replace(a.jstree.idregex,"\\$&"))):i.getElementById(f.id))b=a(b),d||(g=b.parent().parent()[0],g===this.element[0]&&(g=null),h=b.index()),c||!f.children.length||b.children(".jstree-children").length||(c=!0),c||(j=b.children(".jstree-children")[0]),q=b.children(".jstree-anchor")[0]===i.activeElement,b.remove();else if(c=!0,!d){if(g=f.parent!==a.jstree.root?a("#"+f.parent.replace(a.jstree.idregex,"\\$&"),this.element)[0]:null,!(null===g||g&&p[f.parent].state.opened))return!1;h=a.inArray(f.id,null===g?p[a.jstree.root].children:p[f.parent].children)}b=this._data.core.node.cloneNode(!0),n="jstree-node ";for(k in f.li_attr)if(f.li_attr.hasOwnProperty(k)){if("id"===k)continue;"class"!==k?b.setAttribute(k,f.li_attr[k]):n+=f.li_attr[k]}for(f.a_attr.id||(f.a_attr.id=f.id+"_anchor"),b.childNodes[1].setAttribute("aria-selected",!!f.state.selected),b.childNodes[1].setAttribute("aria-level",f.parents.length),this.settings.core.compute_elements_positions&&(b.childNodes[1].setAttribute("aria-setsize",p[f.parent].children.length),b.childNodes[1].setAttribute("aria-posinset",p[f.parent].children.indexOf(f.id)+1)),f.state.disabled&&b.childNodes[1].setAttribute("aria-disabled",!0),k=0,l=f.children.length;l>k;k++)if(!p[f.children[k]].state.hidden){v=!0;break}if(null!==f.parent&&p[f.parent]&&!f.state.hidden&&(k=a.inArray(f.id,p[f.parent].children),w=f.id,-1!==k))for(k++,l=p[f.parent].children.length;l>k;k++)if(p[p[f.parent].children[k]].state.hidden||(w=p[f.parent].children[k]),w!==f.id)break;f.state.hidden&&(n+=" jstree-hidden"),f.state.loading&&(n+=" jstree-loading"),f.state.loaded&&!v?n+=" jstree-leaf":(n+=f.state.opened&&f.state.loaded?" jstree-open":" jstree-closed",b.childNodes[1].setAttribute("aria-expanded",f.state.opened&&f.state.loaded)),w===f.id&&(n+=" jstree-last"),b.id=f.id,b.className=n,n=(f.state.selected?" jstree-clicked":"")+(f.state.disabled?" jstree-disabled":"");for(l in f.a_attr)if(f.a_attr.hasOwnProperty(l)){if("href"===l&&"#"===f.a_attr[l])continue;"class"!==l?b.childNodes[1].setAttribute(l,f.a_attr[l]):n+=" "+f.a_attr[l]}if(n.length&&(b.childNodes[1].className="jstree-anchor "+n),(f.icon&&f.icon!==!0||f.icon===!1)&&(f.icon===!1?b.childNodes[1].childNodes[0].className+=" jstree-themeicon-hidden":-1===f.icon.indexOf("/")&&-1===f.icon.indexOf(".")?b.childNodes[1].childNodes[0].className+=" "+f.icon+" jstree-themeicon-custom":(b.childNodes[1].childNodes[0].style.backgroundImage='url("'+f.icon+'")',b.childNodes[1].childNodes[0].style.backgroundPosition="center center",b.childNodes[1].childNodes[0].style.backgroundSize="auto",b.childNodes[1].childNodes[0].className+=" jstree-themeicon-custom")),this.settings.core.force_text?b.childNodes[1].appendChild(o.createTextNode(f.text)):b.childNodes[1].innerHTML+=f.text,c&&f.children.length&&(f.state.opened||e)&&f.state.loaded){for(m=o.createElement("UL"),m.setAttribute("role","group"),m.className="jstree-children",k=0,l=f.children.length;l>k;k++)m.appendChild(this.redraw_node(f.children[k],c,!0));b.appendChild(m)}if(j&&b.appendChild(j),!d){for(g||(g=this.element[0]),k=0,l=g.childNodes.length;l>k;k++)if(g.childNodes[k]&&g.childNodes[k].className&&-1!==g.childNodes[k].className.indexOf("jstree-children")){s=g.childNodes[k];break}s||(s=o.createElement("UL"),s.setAttribute("role","group"),s.className="jstree-children",g.appendChild(s)),g=s,h<g.childNodes.length?g.insertBefore(b,g.childNodes[h]):g.appendChild(b),q&&(t=this.element[0].scrollTop,u=this.element[0].scrollLeft,b.childNodes[1].focus(),this.element[0].scrollTop=t,this.element[0].scrollLeft=u)}return f.state.opened&&!f.state.loaded&&(f.state.opened=!1,setTimeout(function(){this.open_node(f.id,!1,0)}.bind(this),0)),b},open_node:function(c,d,e){var f,g,h,i;if(a.vakata.is_array(c)){for(c=c.slice(),f=0,g=c.length;g>f;f++)this.open_node(c[f],d,e);return!0}return c=this.get_node(c),c&&c.id!==a.jstree.root?(e=e===b?this.settings.core.animation:e,this.is_closed(c)?this.is_loaded(c)?(h=this.get_node(c,!0),i=this,h.length&&(e&&h.children(".jstree-children").length&&h.children(".jstree-children").stop(!0,!0),c.children.length&&!this._firstChild(h.children(".jstree-children")[0])&&this.draw_children(c),e?(this.trigger("before_open",{node:c}),h.children(".jstree-children").css("display","none").end().removeClass("jstree-closed").addClass("jstree-open").children(".jstree-anchor").attr("aria-expanded",!0).end().children(".jstree-children").stop(!0,!0).slideDown(e,function(){this.style.display="",i.element&&i.trigger("after_open",{node:c})})):(this.trigger("before_open",{node:c}),h[0].className=h[0].className.replace("jstree-closed","jstree-open"),h[0].childNodes[1].setAttribute("aria-expanded",!0))),c.state.opened=!0,d&&d.call(this,c,!0),h.length||this.trigger("before_open",{node:c}),this.trigger("open_node",{node:c}),e&&h.length||this.trigger("after_open",{node:c}),!0):this.is_loading(c)?setTimeout(function(){this.open_node(c,d,e)}.bind(this),500):void this.load_node(c,function(a,b){return b?this.open_node(a,d,e):d?d.call(this,a,!1):!1}):(d&&d.call(this,c,!1),!1)):!1},_open_to:function(b){if(b=this.get_node(b),!b||b.id===a.jstree.root)return!1;var c,d,e=b.parents;for(c=0,d=e.length;d>c;c+=1)c!==a.jstree.root&&this.open_node(e[c],!1,0);return a("#"+b.id.replace(a.jstree.idregex,"\\$&"),this.element)},close_node:function(c,d){var e,f,g,h;if(a.vakata.is_array(c)){for(c=c.slice(),e=0,f=c.length;f>e;e++)this.close_node(c[e],d);return!0}return c=this.get_node(c),c&&c.id!==a.jstree.root?this.is_closed(c)?!1:(d=d===b?this.settings.core.animation:d,g=this,h=this.get_node(c,!0),c.state.opened=!1,this.trigger("close_node",{node:c}),void(h.length?d?h.children(".jstree-children").attr("style","display:block !important").end().removeClass("jstree-open").addClass("jstree-closed").children(".jstree-anchor").attr("aria-expanded",!1).end().children(".jstree-children").stop(!0,!0).slideUp(d,function(){this.style.display="",h.children(".jstree-children").remove(),g.element&&g.trigger("after_close",{node:c})}):(h[0].className=h[0].className.replace("jstree-open","jstree-closed"),h.children(".jstree-anchor").attr("aria-expanded",!1),h.children(".jstree-children").remove(),this.trigger("after_close",{node:c})):this.trigger("after_close",{node:c}))):!1},toggle_node:function(b){var c,d;if(a.vakata.is_array(b)){for(b=b.slice(),c=0,d=b.length;d>c;c++)this.toggle_node(b[c]);return!0}return this.is_closed(b)?this.open_node(b):this.is_open(b)?this.close_node(b):void 0},open_all:function(b,c,d){if(b||(b=a.jstree.root),b=this.get_node(b),!b)return!1;var e=b.id===a.jstree.root?this.get_container_ul():this.get_node(b,!0),f,g,h;if(!e.length){for(f=0,g=b.children_d.length;g>f;f++)this.is_closed(this._model.data[b.children_d[f]])&&(this._model.data[b.children_d[f]].state.opened=!0);return this.trigger("open_all",{node:b})}d=d||e,h=this,e=this.is_closed(b)?e.find(".jstree-closed").addBack():e.find(".jstree-closed"),e.each(function(){h.open_node(this,function(a,b){b&&this.is_parent(a)&&this.open_all(a,c,d)},c||0)}),0===d.find(".jstree-closed").length&&this.trigger("open_all",{node:this.get_node(d)})},close_all:function(b,c){if(b||(b=a.jstree.root),b=this.get_node(b),!b)return!1;var d=b.id===a.jstree.root?this.get_container_ul():this.get_node(b,!0),e=this,f,g;for(d.length&&(d=this.is_open(b)?d.find(".jstree-open").addBack():d.find(".jstree-open"),a(d.get().reverse()).each(function(){e.close_node(this,c||0)})),f=0,g=b.children_d.length;g>f;f++)this._model.data[b.children_d[f]].state.opened=!1;this.trigger("close_all",{node:b})},is_disabled:function(a){return a=this.get_node(a),a&&a.state&&a.state.disabled},enable_node:function(b){var c,d;if(a.vakata.is_array(b)){for(b=b.slice(),c=0,d=b.length;d>c;c++)this.enable_node(b[c]);return!0}return b=this.get_node(b),b&&b.id!==a.jstree.root?(b.state.disabled=!1,this.get_node(b,!0).children(".jstree-anchor").removeClass("jstree-disabled").attr("aria-disabled",!1),void this.trigger("enable_node",{node:b})):!1},disable_node:function(b){var c,d;if(a.vakata.is_array(b)){for(b=b.slice(),c=0,d=b.length;d>c;c++)this.disable_node(b[c]);return!0}return b=this.get_node(b),b&&b.id!==a.jstree.root?(b.state.disabled=!0,this.get_node(b,!0).children(".jstree-anchor").addClass("jstree-disabled").attr("aria-disabled",!0),void this.trigger("disable_node",{node:b})):!1},is_hidden:function(a){return a=this.get_node(a),a.state.hidden===!0},hide_node:function(b,c){var d,e;if(a.vakata.is_array(b)){for(b=b.slice(),d=0,e=b.length;e>d;d++)this.hide_node(b[d],!0);return c||this.redraw(),!0}return b=this.get_node(b),b&&b.id!==a.jstree.root?void(b.state.hidden||(b.state.hidden=!0,this._node_changed(b.parent),c||this.redraw(),this.trigger("hide_node",{node:b}))):!1},show_node:function(b,c){var d,e;if(a.vakata.is_array(b)){for(b=b.slice(),d=0,e=b.length;e>d;d++)this.show_node(b[d],!0);return c||this.redraw(),!0}return b=this.get_node(b),b&&b.id!==a.jstree.root?void(b.state.hidden&&(b.state.hidden=!1,this._node_changed(b.parent),c||this.redraw(),this.trigger("show_node",{node:b}))):!1},hide_all:function(b){var c,d=this._model.data,e=[];for(c in d)d.hasOwnProperty(c)&&c!==a.jstree.root&&!d[c].state.hidden&&(d[c].state.hidden=!0,e.push(c));return this._model.force_full_redraw=!0,b||this.redraw(),this.trigger("hide_all",{nodes:e}),e},show_all:function(b){var c,d=this._model.data,e=[];for(c in d)d.hasOwnProperty(c)&&c!==a.jstree.root&&d[c].state.hidden&&(d[c].state.hidden=!1,e.push(c));return this._model.force_full_redraw=!0,b||this.redraw(),this.trigger("show_all",{nodes:e}),e},activate_node:function(a,c){if(this.is_disabled(a))return!1;if(c&&"object"==typeof c||(c={}),this._data.core.last_clicked=this._data.core.last_clicked&&this._data.core.last_clicked.id!==b?this.get_node(this._data.core.last_clicked.id):null,this._data.core.last_clicked&&!this._data.core.last_clicked.state.selected&&(this._data.core.last_clicked=null),!this._data.core.last_clicked&&this._data.core.selected.length&&(this._data.core.last_clicked=this.get_node(this._data.core.selected[this._data.core.selected.length-1])),this.settings.core.multiple&&(c.metaKey||c.ctrlKey||c.shiftKey)&&(!c.shiftKey||this._data.core.last_clicked&&this.get_parent(a)&&this.get_parent(a)===this._data.core.last_clicked.parent))if(c.shiftKey){var d=this.get_node(a).id,e=this._data.core.last_clicked.id,f=this.get_node(this._data.core.last_clicked.parent).children,g=!1,h,i;for(h=0,i=f.length;i>h;h+=1)f[h]===d&&(g=!g),f[h]===e&&(g=!g),this.is_disabled(f[h])||!g&&f[h]!==d&&f[h]!==e?this.deselect_node(f[h],!0,c):this.is_hidden(f[h])||this.select_node(f[h],!0,!1,c);this.trigger("changed",{action:"select_node",node:this.get_node(a),selected:this._data.core.selected,event:c})}else this.is_selected(a)?this.deselect_node(a,!1,c):this.select_node(a,!1,!1,c);else!this.settings.core.multiple&&(c.metaKey||c.ctrlKey||c.shiftKey)&&this.is_selected(a)?this.deselect_node(a,!1,c):(this.deselect_all(!0),this.select_node(a,!1,!1,c),this._data.core.last_clicked=this.get_node(a));this.trigger("activate_node",{node:this.get_node(a),event:c})},hover_node:function(a){if(a=this.get_node(a,!0),!a||!a.length||a.children(".jstree-hovered").length)return!1;var b=this.element.find(".jstree-hovered"),c=this.element;b&&b.length&&this.dehover_node(b),a.children(".jstree-anchor").addClass("jstree-hovered"),this.trigger("hover_node",{node:this.get_node(a)}),setTimeout(function(){c.attr("aria-activedescendant",a[0].id)},0)},dehover_node:function(a){return a=this.get_node(a,!0),a&&a.length&&a.children(".jstree-hovered").length?(a.children(".jstree-anchor").removeClass("jstree-hovered"),void this.trigger("dehover_node",{node:this.get_node(a)})):!1},select_node:function(b,c,d,e){var f,g,h,i;if(a.vakata.is_array(b)){for(b=b.slice(),g=0,h=b.length;h>g;g++)this.select_node(b[g],c,d,e);return!0}return b=this.get_node(b),b&&b.id!==a.jstree.root?(f=this.get_node(b,!0),void(b.state.selected||(b.state.selected=!0,this._data.core.selected.push(b.id),d||(f=this._open_to(b)),f&&f.length&&f.children(".jstree-anchor").addClass("jstree-clicked").attr("aria-selected",!0),this.trigger("select_node",{node:b,selected:this._data.core.selected,event:e}),c||this.trigger("changed",{action:"select_node",node:b,selected:this._data.core.selected,event:e})))):!1},deselect_node:function(b,c,d){var e,f,g;if(a.vakata.is_array(b)){for(b=b.slice(),e=0,f=b.length;f>e;e++)this.deselect_node(b[e],c,d);return!0}return b=this.get_node(b),b&&b.id!==a.jstree.root?(g=this.get_node(b,!0),void(b.state.selected&&(b.state.selected=!1,this._data.core.selected=a.vakata.array_remove_item(this._data.core.selected,b.id),g.length&&g.children(".jstree-anchor").removeClass("jstree-clicked").attr("aria-selected",!1),this.trigger("deselect_node",{node:b,selected:this._data.core.selected,event:d}),c||this.trigger("changed",{action:"deselect_node",node:b,selected:this._data.core.selected,event:d})))):!1},select_all:function(b){var c=this._data.core.selected.concat([]),d,e;for(this._data.core.selected=this._model.data[a.jstree.root].children_d.concat(),d=0,e=this._data.core.selected.length;e>d;d++)this._model.data[this._data.core.selected[d]]&&(this._model.data[this._data.core.selected[d]].state.selected=!0);this.redraw(!0),this.trigger("select_all",{selected:this._data.core.selected}),b||this.trigger("changed",{action:"select_all",selected:this._data.core.selected,old_selection:c})},deselect_all:function(a){var b=this._data.core.selected.concat([]),c,d;for(c=0,d=this._data.core.selected.length;d>c;c++)this._model.data[this._data.core.selected[c]]&&(this._model.data[this._data.core.selected[c]].state.selected=!1);this._data.core.selected=[],this.element.find(".jstree-clicked").removeClass("jstree-clicked").attr("aria-selected",!1),this.trigger("deselect_all",{selected:this._data.core.selected,node:b}),a||this.trigger("changed",{action:"deselect_all",selected:this._data.core.selected,old_selection:b})},is_selected:function(b){return b=this.get_node(b),b&&b.id!==a.jstree.root?b.state.selected:!1},get_selected:function(b){return b?a.map(this._data.core.selected,function(a){return this.get_node(a)}.bind(this)):this._data.core.selected.slice()},get_top_selected:function(b){var c=this.get_selected(!0),d={},e,f,g,h;for(e=0,f=c.length;f>e;e++)d[c[e].id]=c[e];for(e=0,f=c.length;f>e;e++)for(g=0,h=c[e].children_d.length;h>g;g++)d[c[e].children_d[g]]&&delete d[c[e].children_d[g]];c=[];for(e in d)d.hasOwnProperty(e)&&c.push(e);return b?a.map(c,function(a){return this.get_node(a)}.bind(this)):c},get_bottom_selected:function(b){var c=this.get_selected(!0),d=[],e,f;for(e=0,f=c.length;f>e;e++)c[e].children.length||d.push(c[e].id);return b?a.map(d,function(a){return this.get_node(a)}.bind(this)):d},get_state:function(){var b={core:{open:[],loaded:[],scroll:{left:this.element.scrollLeft(),top:this.element.scrollTop()},selected:[]}},c;for(c in this._model.data)this._model.data.hasOwnProperty(c)&&c!==a.jstree.root&&(this._model.data[c].state.loaded&&this.settings.core.loaded_state&&b.core.loaded.push(c),this._model.data[c].state.opened&&b.core.open.push(c),this._model.data[c].state.selected&&b.core.selected.push(c));return b},set_state:function(c,d){if(c){if(c.core&&c.core.selected&&c.core.initial_selection===b&&(c.core.initial_selection=this._data.core.selected.concat([]).sort().join(",")),c.core){var e,f,g,h,i;if(c.core.loaded)return this.settings.core.loaded_state&&a.vakata.is_array(c.core.loaded)&&c.core.loaded.length?this._load_nodes(c.core.loaded,function(a){delete c.core.loaded,this.set_state(c,d)}):(delete c.core.loaded,this.set_state(c,d)),!1;if(c.core.open)return a.vakata.is_array(c.core.open)&&c.core.open.length?this._load_nodes(c.core.open,function(a){this.open_node(a,!1,0),delete c.core.open,this.set_state(c,d)}):(delete c.core.open,this.set_state(c,d)),!1;if(c.core.scroll)return c.core.scroll&&c.core.scroll.left!==b&&this.element.scrollLeft(c.core.scroll.left),c.core.scroll&&c.core.scroll.top!==b&&this.element.scrollTop(c.core.scroll.top),delete c.core.scroll,this.set_state(c,d),!1;if(c.core.selected)return h=this,(c.core.initial_selection===b||c.core.initial_selection===this._data.core.selected.concat([]).sort().join(","))&&(this.deselect_all(),a.each(c.core.selected,function(a,b){h.select_node(b,!1,!0)})),delete c.core.initial_selection,delete c.core.selected,this.set_state(c,d),!1;for(i in c)c.hasOwnProperty(i)&&"core"!==i&&-1===a.inArray(i,this.settings.plugins)&&delete c[i];if(a.isEmptyObject(c.core))return delete c.core,this.set_state(c,d),!1}return a.isEmptyObject(c)?(c=null,d&&d.call(this),this.trigger("set_state"),!1):!0}return!1},refresh:function(b,c){this._data.core.state=c===!0?{}:this.get_state(),c&&a.vakata.is_function(c)&&(this._data.core.state=c.call(this,this._data.core.state)),this._cnt=0,this._model.data={},this._model.data[a.jstree.root]={id:a.jstree.root,parent:null,parents:[],children:[],children_d:[],state:{loaded:!1}},this._data.core.selected=[],this._data.core.last_clicked=null,this._data.core.focused=null;var d=this.get_container_ul()[0].className;b||(this.element.html("<ul class='"+d+"' role='group'><li class='jstree-initial-node jstree-loading jstree-leaf jstree-last' role='none' id='j"+this._id+"_loading'><i class='jstree-icon jstree-ocl'></i><a class='jstree-anchor' role='treeitem' href='#'><i class='jstree-icon jstree-themeicon-hidden'></i>"+this.get_string("Loading ...")+"</a></li></ul>"),this.element.attr("aria-activedescendant","j"+this._id+"_loading")),this.load_node(a.jstree.root,function(b,c){c&&(this.get_container_ul()[0].className=d,this._firstChild(this.get_container_ul()[0])&&this.element.attr("aria-activedescendant",this._firstChild(this.get_container_ul()[0]).id),this.set_state(a.extend(!0,{},this._data.core.state),function(){this.trigger("refresh")})),this._data.core.state=null})},refresh_node:function(b){if(b=this.get_node(b),!b||b.id===a.jstree.root)return!1;var c=[],d=[],e=this._data.core.selected.concat([]);d.push(b.id),b.state.opened===!0&&c.push(b.id),this.get_node(b,!0).find(".jstree-open").each(function(){d.push(this.id),c.push(this.id)}),this._load_nodes(d,function(a){this.open_node(c,!1,0),this.select_node(e),this.trigger("refresh_node",{node:b,nodes:a})}.bind(this),!1,!0)},set_id:function(b,c){if(b=this.get_node(b),!b||b.id===a.jstree.root)return!1;var d,e,f=this._model.data,g=b.id;for(c=c.toString(),f[b.parent].children[a.inArray(b.id,f[b.parent].children)]=c,d=0,e=b.parents.length;e>d;d++)f[b.parents[d]].children_d[a.inArray(b.id,f[b.parents[d]].children_d)]=c;for(d=0,e=b.children.length;e>d;d++)f[b.children[d]].parent=c;for(d=0,e=b.children_d.length;e>d;d++)f[b.children_d[d]].parents[a.inArray(b.id,f[b.children_d[d]].parents)]=c;return d=a.inArray(b.id,this._data.core.selected),-1!==d&&(this._data.core.selected[d]=c),d=this.get_node(b.id,!0),d&&(d.attr("id",c),this.element.attr("aria-activedescendant")===b.id&&this.element.attr("aria-activedescendant",c)),delete f[b.id],b.id=c,b.li_attr.id=c,f[c]=b,this.trigger("set_id",{node:b,"new":b.id,old:g}),!0},get_text:function(b){return b=this.get_node(b),b&&b.id!==a.jstree.root?b.text:!1},set_text:function(b,c){var d,e;if(a.vakata.is_array(b)){for(b=b.slice(),d=0,e=b.length;e>d;d++)this.set_text(b[d],c);return!0}return b=this.get_node(b),b&&b.id!==a.jstree.root?(b.text=c,this.get_node(b,!0).length&&this.redraw_node(b.id),this.trigger("set_text",{obj:b,text:c}),!0):!1},get_json:function(b,c,d){if(b=this.get_node(b||a.jstree.root),!b)return!1;c&&c.flat&&!d&&(d=[]);var e={id:b.id,text:b.text,icon:this.get_icon(b),li_attr:a.extend(!0,{},b.li_attr),a_attr:a.extend(!0,{},b.a_attr),state:{},data:c&&c.no_data?!1:a.extend(!0,a.vakata.is_array(b.data)?[]:{},b.data)},f,g;if(c&&c.flat?e.parent=b.parent:e.children=[],c&&c.no_state)delete e.state;else for(f in b.state)b.state.hasOwnProperty(f)&&(e.state[f]=b.state[f]);if(c&&c.no_li_attr&&delete e.li_attr,c&&c.no_a_attr&&delete e.a_attr,c&&c.no_id&&(delete e.id,e.li_attr&&e.li_attr.id&&delete e.li_attr.id,e.a_attr&&e.a_attr.id&&delete e.a_attr.id),c&&c.flat&&b.id!==a.jstree.root&&d.push(e),!c||!c.no_children)for(f=0,g=b.children.length;g>f;f++)c&&c.flat?this.get_json(b.children[f],c,d):e.children.push(this.get_json(b.children[f],c));return c&&c.flat?d:b.id===a.jstree.root?e.children:e},create_node:function(c,d,e,f,g){if(null===c&&(c=a.jstree.root),c=this.get_node(c),!c)return!1;if(e=e===b?"last":e,!e.toString().match(/^(before|after)$/)&&!g&&!this.is_loaded(c))return this.load_node(c,function(){this.create_node(c,d,e,f,!0)});d||(d={text:this.get_string("New node")}),d="string"==typeof d?{text:d}:a.extend(!0,{},d),d.text===b&&(d.text=this.get_string("New node"));var h,i,j,k;switch(c.id===a.jstree.root&&("before"===e&&(e="first"),"after"===e&&(e="last")),e){case"before":h=this.get_node(c.parent),e=a.inArray(c.id,h.children),c=h;break;case"after":h=this.get_node(c.parent),e=a.inArray(c.id,h.children)+1,c=h;break;case"inside":case"first":e=0;break;case"last":e=c.children.length;break;default:e||(e=0)}if(e>c.children.length&&(e=c.children.length),d.id||(d.id=!0),!this.check("create_node",d,c,e))return this.settings.core.error.call(this,this._data.core.last_error),!1;if(d.id===!0&&delete d.id,d=this._parse_model_from_json(d,c.id,c.parents.concat()),!d)return!1;for(h=this.get_node(d),i=[],i.push(d),i=i.concat(h.children_d),this.trigger("model",{nodes:i,parent:c.id}),c.children_d=c.children_d.concat(i),j=0,k=c.parents.length;k>j;j++)this._model.data[c.parents[j]].children_d=this._model.data[c.parents[j]].children_d.concat(i);for(d=h,h=[],j=0,k=c.children.length;k>j;j++)h[j>=e?j+1:j]=c.children[j];return h[e]=d.id,c.children=h,this.redraw_node(c,!0),this.trigger("create_node",{node:this.get_node(d),parent:c.id,position:e}),f&&f.call(this,this.get_node(d)),d.id},rename_node:function(b,c){var d,e,f;if(a.vakata.is_array(b)){for(b=b.slice(),d=0,e=b.length;e>d;d++)this.rename_node(b[d],c);return!0}return b=this.get_node(b),b&&b.id!==a.jstree.root?(f=b.text,this.check("rename_node",b,this.get_parent(b),c)?(this.set_text(b,c),this.trigger("rename_node",{node:b,text:c,old:f}),!0):(this.settings.core.error.call(this,this._data.core.last_error),!1)):!1},delete_node:function(b){var c,d,e,f,g,h,i,j,k,l,m,n;if(a.vakata.is_array(b)){for(b=b.slice(),c=0,d=b.length;d>c;c++)this.delete_node(b[c]);return!0}if(b=this.get_node(b),!b||b.id===a.jstree.root)return!1;if(e=this.get_node(b.parent),f=a.inArray(b.id,e.children),l=!1,!this.check("delete_node",b,e,f))return this.settings.core.error.call(this,this._data.core.last_error),!1;for(-1!==f&&(e.children=a.vakata.array_remove(e.children,f)),g=b.children_d.concat([]),g.push(b.id),h=0,i=b.parents.length;i>h;h++)this._model.data[b.parents[h]].children_d=a.vakata.array_filter(this._model.data[b.parents[h]].children_d,function(b){return-1===a.inArray(b,g)});for(j=0,k=g.length;k>j;j++)if(this._model.data[g[j]].state.selected){l=!0;break}for(l&&(this._data.core.selected=a.vakata.array_filter(this._data.core.selected,function(b){return-1===a.inArray(b,g)})),this.trigger("delete_node",{node:b,parent:e.id}),l&&this.trigger("changed",{action:"delete_node",node:b,selected:this._data.core.selected,parent:e.id}),j=0,k=g.length;k>j;j++)delete this._model.data[g[j]];return-1!==a.inArray(this._data.core.focused,g)&&(this._data.core.focused=null,m=this.element[0].scrollTop,n=this.element[0].scrollLeft,e.id===a.jstree.root?this._model.data[a.jstree.root].children[0]&&this.get_node(this._model.data[a.jstree.root].children[0],!0).children(".jstree-anchor").triger("focus"):this.get_node(e,!0).children(".jstree-anchor").trigger("focus"),this.element[0].scrollTop=m,this.element[0].scrollLeft=n),this.redraw_node(e,!0),!0},check:function(b,c,d,e,f){c=c&&c.id?c:this.get_node(c),d=d&&d.id?d:this.get_node(d);var g=b.match(/^move_node|copy_node|create_node$/i)?d:c,h=this.settings.core.check_callback;if("move_node"===b||"copy_node"===b){if(!(f&&f.is_multi||"move_node"!==b||a.inArray(c.id,d.children)!==e))return this._data.core.last_error={error:"check",plugin:"core",id:"core_08",reason:"Moving node to its current position",data:JSON.stringify({chk:b,pos:e,obj:c&&c.id?c.id:!1,par:d&&d.id?d.id:!1})},!1;if(!(f&&f.is_multi||c.id!==d.id&&("move_node"!==b||a.inArray(c.id,d.children)!==e)&&-1===a.inArray(d.id,c.children_d)))return this._data.core.last_error={error:"check",plugin:"core",id:"core_01",reason:"Moving parent inside child",data:JSON.stringify({chk:b,pos:e,obj:c&&c.id?c.id:!1,par:d&&d.id?d.id:!1})},!1}return g&&g.data&&(g=g.data),g&&g.functions&&(g.functions[b]===!1||g.functions[b]===!0)?(g.functions[b]===!1&&(this._data.core.last_error={error:"check",plugin:"core",id:"core_02",reason:"Node data prevents function: "+b,data:JSON.stringify({chk:b,pos:e,obj:c&&c.id?c.id:!1,par:d&&d.id?d.id:!1})}),g.functions[b]):h===!1||a.vakata.is_function(h)&&h.call(this,b,c,d,e,f)===!1||h&&h[b]===!1?(this._data.core.last_error={error:"check",plugin:"core",id:"core_03",reason:"User config for core.check_callback prevents function: "+b,data:JSON.stringify({chk:b,pos:e,obj:c&&c.id?c.id:!1,par:d&&d.id?d.id:!1})},!1):!0},last_error:function(){return this._data.core.last_error},move_node:function(c,d,e,f,g,h,i){var j,k,l,m,n,o,p,q,r,s,t,u,v,w;if(d=this.get_node(d),e=e===b?0:e,!d)return!1;if(!e.toString().match(/^(before|after)$/)&&!g&&!this.is_loaded(d))return this.load_node(d,function(){this.move_node(c,d,e,f,!0,!1,i)});if(a.vakata.is_array(c)){if(1!==c.length){for(j=0,k=c.length;k>j;j++)(r=this.move_node(c[j],d,e,f,g,!1,i))&&(d=r,e="after");return this.redraw(),!0}c=c[0]}if(c=c&&c.id?c:this.get_node(c),!c||c.id===a.jstree.root)return!1;if(l=(c.parent||a.jstree.root).toString(),n=e.toString().match(/^(before|after)$/)&&d.id!==a.jstree.root?this.get_node(d.parent):d,o=i?i:this._model.data[c.id]?this:a.jstree.reference(c.id),p=!o||!o._id||this._id!==o._id,m=o&&o._id&&l&&o._model.data[l]&&o._model.data[l].children?a.inArray(c.id,o._model.data[l].children):-1,o&&o._id&&(c=o._model.data[c.id]),p)return(r=this.copy_node(c,d,e,f,g,!1,i))?(o&&o.delete_node(c),r):!1;switch(d.id===a.jstree.root&&("before"===e&&(e="first"),"after"===e&&(e="last")),e){case"before":e=a.inArray(d.id,n.children);break;case"after":e=a.inArray(d.id,n.children)+1;break;case"inside":case"first":e=0;break;case"last":e=n.children.length;break;default:e||(e=0)}if(e>n.children.length&&(e=n.children.length),!this.check("move_node",c,n,e,{core:!0,origin:i,is_multi:o&&o._id&&o._id!==this._id,is_foreign:!o||!o._id}))return this.settings.core.error.call(this,this._data.core.last_error),!1;if(c.parent===n.id){for(q=n.children.concat(),r=a.inArray(c.id,q),-1!==r&&(q=a.vakata.array_remove(q,r),e>r&&e--),r=[],s=0,t=q.length;t>s;s++)r[s>=e?s+1:s]=q[s];r[e]=c.id,n.children=r,this._node_changed(n.id),this.redraw(n.id===a.jstree.root)}else{for(r=c.children_d.concat(),r.push(c.id),s=0,t=c.parents.length;t>s;s++){for(q=[],w=o._model.data[c.parents[s]].children_d,u=0,v=w.length;v>u;u++)-1===a.inArray(w[u],r)&&q.push(w[u]);o._model.data[c.parents[s]].children_d=q}for(o._model.data[l].children=a.vakata.array_remove_item(o._model.data[l].children,c.id),s=0,t=n.parents.length;t>s;s++)this._model.data[n.parents[s]].children_d=this._model.data[n.parents[s]].children_d.concat(r);for(q=[],s=0,t=n.children.length;t>s;s++)q[s>=e?s+1:s]=n.children[s];for(q[e]=c.id,n.children=q,n.children_d.push(c.id),n.children_d=n.children_d.concat(c.children_d),c.parent=n.id,r=n.parents.concat(),r.unshift(n.id),w=c.parents.length,c.parents=r,r=r.concat(),s=0,t=c.children_d.length;t>s;s++)this._model.data[c.children_d[s]].parents=this._model.data[c.children_d[s]].parents.slice(0,-1*w),Array.prototype.push.apply(this._model.data[c.children_d[s]].parents,r);(l===a.jstree.root||n.id===a.jstree.root)&&(this._model.force_full_redraw=!0),this._model.force_full_redraw||(this._node_changed(l),this._node_changed(n.id)),h||this.redraw()}return f&&f.call(this,c,n,e),this.trigger("move_node",{node:c,parent:n.id,position:e,old_parent:l,old_position:m,is_multi:o&&o._id&&o._id!==this._id,is_foreign:!o||!o._id,old_instance:o,new_instance:this}),c.id},copy_node:function(c,d,e,f,g,h,i){var j,k,l,m,n,o,p,q,r,s,t;if(d=this.get_node(d),e=e===b?0:e,!d)return!1;if(!e.toString().match(/^(before|after)$/)&&!g&&!this.is_loaded(d))return this.load_node(d,function(){this.copy_node(c,d,e,f,!0,!1,i)});if(a.vakata.is_array(c)){if(1!==c.length){for(j=0,k=c.length;k>j;j++)(m=this.copy_node(c[j],d,e,f,g,!0,i))&&(d=m,e="after");return this.redraw(),!0}c=c[0]}if(c=c&&c.id?c:this.get_node(c),!c||c.id===a.jstree.root)return!1;switch(q=(c.parent||a.jstree.root).toString(),r=e.toString().match(/^(before|after)$/)&&d.id!==a.jstree.root?this.get_node(d.parent):d,s=i?i:this._model.data[c.id]?this:a.jstree.reference(c.id),t=!s||!s._id||this._id!==s._id,s&&s._id&&(c=s._model.data[c.id]),d.id===a.jstree.root&&("before"===e&&(e="first"),"after"===e&&(e="last")),e){case"before":e=a.inArray(d.id,r.children);break;case"after":e=a.inArray(d.id,r.children)+1;break;case"inside":case"first":e=0;break;case"last":e=r.children.length;break;default:e||(e=0)}if(e>r.children.length&&(e=r.children.length),!this.check("copy_node",c,r,e,{core:!0,origin:i,is_multi:s&&s._id&&s._id!==this._id,is_foreign:!s||!s._id}))return this.settings.core.error.call(this,this._data.core.last_error),!1;if(p=s?s.get_json(c,{no_id:!0,no_data:!0,no_state:!0}):c,!p)return!1;if(p.id===!0&&delete p.id,p=this._parse_model_from_json(p,r.id,r.parents.concat()),
!p)return!1;for(m=this.get_node(p),c&&c.state&&c.state.loaded===!1&&(m.state.loaded=!1),l=[],l.push(p),l=l.concat(m.children_d),this.trigger("model",{nodes:l,parent:r.id}),n=0,o=r.parents.length;o>n;n++)this._model.data[r.parents[n]].children_d=this._model.data[r.parents[n]].children_d.concat(l);for(l=[],n=0,o=r.children.length;o>n;n++)l[n>=e?n+1:n]=r.children[n];return l[e]=m.id,r.children=l,r.children_d.push(m.id),r.children_d=r.children_d.concat(m.children_d),r.id===a.jstree.root&&(this._model.force_full_redraw=!0),this._model.force_full_redraw||this._node_changed(r.id),h||this.redraw(r.id===a.jstree.root),f&&f.call(this,m,r,e),this.trigger("copy_node",{node:m,original:c,parent:r.id,position:e,old_parent:q,old_position:s&&s._id&&q&&s._model.data[q]&&s._model.data[q].children?a.inArray(c.id,s._model.data[q].children):-1,is_multi:s&&s._id&&s._id!==this._id,is_foreign:!s||!s._id,old_instance:s,new_instance:this}),m.id},cut:function(b){if(b||(b=this._data.core.selected.concat()),a.vakata.is_array(b)||(b=[b]),!b.length)return!1;var c=[],g,h,i;for(h=0,i=b.length;i>h;h++)g=this.get_node(b[h]),g&&g.id&&g.id!==a.jstree.root&&c.push(g);return c.length?(d=c,f=this,e="move_node",void this.trigger("cut",{node:b})):!1},copy:function(b){if(b||(b=this._data.core.selected.concat()),a.vakata.is_array(b)||(b=[b]),!b.length)return!1;var c=[],g,h,i;for(h=0,i=b.length;i>h;h++)g=this.get_node(b[h]),g&&g.id&&g.id!==a.jstree.root&&c.push(g);return c.length?(d=c,f=this,e="copy_node",void this.trigger("copy",{node:b})):!1},get_buffer:function(){return{mode:e,node:d,inst:f}},can_paste:function(){return e!==!1&&d!==!1},paste:function(a,b){return a=this.get_node(a),a&&e&&e.match(/^(copy_node|move_node)$/)&&d?(this[e](d,a,b,!1,!1,!1,f)&&this.trigger("paste",{parent:a.id,node:d,mode:e}),d=!1,e=!1,void(f=!1)):!1},clear_buffer:function(){d=!1,e=!1,f=!1,this.trigger("clear_buffer")},edit:function(b,c,d){var e,f,g,h,j,k,l,m,n,o=!1;return(b=this.get_node(b))?this.check("edit",b,this.get_parent(b))?(n=b,c="string"==typeof c?c:b.text,this.set_text(b,""),b=this._open_to(b),n.text=c,e=this._data.core.rtl,f=this.element.width(),this._data.core.focused=n.id,g=b.children(".jstree-anchor").trigger("focus"),h=a("<span></span>"),j=c,k=a("<div></div>",{css:{position:"absolute",top:"-200px",left:e?"0px":"-1000px",visibility:"hidden"}}).appendTo(i.body),l=a("<input />",{value:j,"class":"jstree-rename-input",css:{padding:"0",border:"1px solid silver","box-sizing":"border-box",display:"inline-block",height:this._data.core.li_height+"px",lineHeight:this._data.core.li_height+"px",width:"150px"},blur:function(c){c.stopImmediatePropagation(),c.preventDefault();var e=h.children(".jstree-rename-input"),f=e.val(),i=this.settings.core.force_text,m;""===f&&(f=j),k.remove(),h.replaceWith(g),h.remove(),j=i?j:a("<div></div>").append(a.parseHTML(j)).html(),b=this.get_node(b),this.set_text(b,j),m=!!this.rename_node(b,i?a("<div></div>").text(f).text():a("<div></div>").append(a.parseHTML(f)).html()),m||this.set_text(b,j),this._data.core.focused=n.id,setTimeout(function(){var a=this.get_node(n.id,!0);a.length&&(this._data.core.focused=n.id,a.children(".jstree-anchor").trigger("focus"))}.bind(this),0),d&&d.call(this,n,m,o,f),l=null}.bind(this),keydown:function(a){var b=a.which;27===b&&(o=!0,this.value=j),(27===b||13===b||37===b||38===b||39===b||40===b||32===b)&&a.stopImmediatePropagation(),(27===b||13===b)&&(a.preventDefault(),this.blur())},click:function(a){a.stopImmediatePropagation()},mousedown:function(a){a.stopImmediatePropagation()},keyup:function(a){l.width(Math.min(k.text("pW"+this.value).width(),f))},keypress:function(a){return 13===a.which?!1:void 0}}),m={fontFamily:g.css("fontFamily")||"",fontSize:g.css("fontSize")||"",fontWeight:g.css("fontWeight")||"",fontStyle:g.css("fontStyle")||"",fontStretch:g.css("fontStretch")||"",fontVariant:g.css("fontVariant")||"",letterSpacing:g.css("letterSpacing")||"",wordSpacing:g.css("wordSpacing")||""},h.attr("class",g.attr("class")).append(g.contents().clone()).append(l),g.replaceWith(h),k.css(m),l.css(m).width(Math.min(k.text("pW"+l[0].value).width(),f))[0].select(),void a(i).one("mousedown.jstree touchstart.jstree dnd_start.vakata",function(b){l&&b.target!==l&&a(l).trigger("blur")})):(this.settings.core.error.call(this,this._data.core.last_error),!1):!1},set_theme:function(b,c){if(!b)return!1;if(c===!0){var d=this.settings.core.themes.dir;d||(d=a.jstree.path+"/themes"),c=d+"/"+b+"/style.css"}c&&-1===a.inArray(c,g)&&(a("head").append('<link rel="stylesheet" href="'+c+'" type="text/css" />'),g.push(c)),this._data.core.themes.name&&this.element.removeClass("jstree-"+this._data.core.themes.name),this._data.core.themes.name=b,this.element.addClass("jstree-"+b),this.element[this.settings.core.themes.responsive?"addClass":"removeClass"]("jstree-"+b+"-responsive"),this.trigger("set_theme",{theme:b})},get_theme:function(){return this._data.core.themes.name},set_theme_variant:function(a){this._data.core.themes.variant&&this.element.removeClass("jstree-"+this._data.core.themes.name+"-"+this._data.core.themes.variant),this._data.core.themes.variant=a,a&&this.element.addClass("jstree-"+this._data.core.themes.name+"-"+this._data.core.themes.variant)},get_theme_variant:function(){return this._data.core.themes.variant},show_stripes:function(){this._data.core.themes.stripes=!0,this.get_container_ul().addClass("jstree-striped"),this.trigger("show_stripes")},hide_stripes:function(){this._data.core.themes.stripes=!1,this.get_container_ul().removeClass("jstree-striped"),this.trigger("hide_stripes")},toggle_stripes:function(){this._data.core.themes.stripes?this.hide_stripes():this.show_stripes()},show_dots:function(){this._data.core.themes.dots=!0,this.get_container_ul().removeClass("jstree-no-dots"),this.trigger("show_dots")},hide_dots:function(){this._data.core.themes.dots=!1,this.get_container_ul().addClass("jstree-no-dots"),this.trigger("hide_dots")},toggle_dots:function(){this._data.core.themes.dots?this.hide_dots():this.show_dots()},show_icons:function(){this._data.core.themes.icons=!0,this.get_container_ul().removeClass("jstree-no-icons"),this.trigger("show_icons")},hide_icons:function(){this._data.core.themes.icons=!1,this.get_container_ul().addClass("jstree-no-icons"),this.trigger("hide_icons")},toggle_icons:function(){this._data.core.themes.icons?this.hide_icons():this.show_icons()},show_ellipsis:function(){this._data.core.themes.ellipsis=!0,this.get_container_ul().addClass("jstree-ellipsis"),this.trigger("show_ellipsis")},hide_ellipsis:function(){this._data.core.themes.ellipsis=!1,this.get_container_ul().removeClass("jstree-ellipsis"),this.trigger("hide_ellipsis")},toggle_ellipsis:function(){this._data.core.themes.ellipsis?this.hide_ellipsis():this.show_ellipsis()},set_icon:function(c,d){var e,f,g,h;if(a.vakata.is_array(c)){for(c=c.slice(),e=0,f=c.length;f>e;e++)this.set_icon(c[e],d);return!0}return c=this.get_node(c),c&&c.id!==a.jstree.root?(h=c.icon,c.icon=d===!0||null===d||d===b||""===d?!0:d,g=this.get_node(c,!0).children(".jstree-anchor").children(".jstree-themeicon"),d===!1?(g.removeClass("jstree-themeicon-custom "+h).css("background","").removeAttr("rel"),this.hide_icon(c)):d===!0||null===d||d===b||""===d?(g.removeClass("jstree-themeicon-custom "+h).css("background","").removeAttr("rel"),h===!1&&this.show_icon(c)):-1===d.indexOf("/")&&-1===d.indexOf(".")?(g.removeClass(h).css("background",""),g.addClass(d+" jstree-themeicon-custom").attr("rel",d),h===!1&&this.show_icon(c)):(g.removeClass(h).css("background",""),g.addClass("jstree-themeicon-custom").css("background","url('"+d+"') center center no-repeat").attr("rel",d),h===!1&&this.show_icon(c)),!0):!1},get_icon:function(b){return b=this.get_node(b),b&&b.id!==a.jstree.root?b.icon:!1},hide_icon:function(b){var c,d;if(a.vakata.is_array(b)){for(b=b.slice(),c=0,d=b.length;d>c;c++)this.hide_icon(b[c]);return!0}return b=this.get_node(b),b&&b!==a.jstree.root?(b.icon=!1,this.get_node(b,!0).children(".jstree-anchor").children(".jstree-themeicon").addClass("jstree-themeicon-hidden"),!0):!1},show_icon:function(b){var c,d,e;if(a.vakata.is_array(b)){for(b=b.slice(),c=0,d=b.length;d>c;c++)this.show_icon(b[c]);return!0}return b=this.get_node(b),b&&b!==a.jstree.root?(e=this.get_node(b,!0),b.icon=e.length?e.children(".jstree-anchor").children(".jstree-themeicon").attr("rel"):!0,b.icon||(b.icon=!0),e.children(".jstree-anchor").children(".jstree-themeicon").removeClass("jstree-themeicon-hidden"),!0):!1}},a.vakata={},a.vakata.attributes=function(b,c){b=a(b)[0];var d=c?{}:[];return b&&b.attributes&&a.each(b.attributes,function(b,e){-1===a.inArray(e.name.toLowerCase(),["style","contenteditable","hasfocus","tabindex"])&&null!==e.value&&""!==a.vakata.trim(e.value)&&(c?d[e.name]=e.value:d.push(e.name))}),d},a.vakata.array_unique=function(a){var c=[],d,e,f,g={};for(d=0,f=a.length;f>d;d++)g[a[d]]===b&&(c.push(a[d]),g[a[d]]=!0);return c},a.vakata.array_remove=function(a,b){return a.splice(b,1),a},a.vakata.array_remove_item=function(b,c){var d=a.inArray(c,b);return-1!==d?a.vakata.array_remove(b,d):b},a.vakata.array_filter=function(a,b,c,d,e){if(a.filter)return a.filter(b,c);d=[];for(e in a)~~e+""==e+""&&e>=0&&b.call(c,a[e],+e,a)&&d.push(a[e]);return d},a.vakata.trim=function(a){return String.prototype.trim?String.prototype.trim.call(a.toString()):a.toString().replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g,"")},a.vakata.is_function=function(a){return"function"==typeof a&&"number"!=typeof a.nodeType},a.vakata.is_array=Array.isArray||function(a){return"[object Array]"===Object.prototype.toString.call(a)},Function.prototype.bind||(Function.prototype.bind=function(){var a=this,b=arguments[0],c=Array.prototype.slice.call(arguments,1);if("function"!=typeof a)throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable");return function(){var d=c.concat(Array.prototype.slice.call(arguments));return a.apply(b,d)}}),a.jstree.plugins.changed=function(a,b){var c=[];this.trigger=function(a,d){var e,f;if(d||(d={}),"changed"===a.replace(".jstree","")){d.changed={selected:[],deselected:[]};var g={};for(e=0,f=c.length;f>e;e++)g[c[e]]=1;for(e=0,f=d.selected.length;f>e;e++)g[d.selected[e]]?g[d.selected[e]]=2:d.changed.selected.push(d.selected[e]);for(e=0,f=c.length;f>e;e++)1===g[c[e]]&&d.changed.deselected.push(c[e]);c=d.selected.slice()}b.trigger.call(this,a,d)},this.refresh=function(a,d){return c=[],b.refresh.apply(this,arguments)}};var l=i.createElement("I");l.className="jstree-icon jstree-checkbox",l.setAttribute("role","presentation"),a.jstree.defaults.checkbox={visible:!0,three_state:!0,whole_node:!0,keep_selected_style:!0,cascade:"",tie_selection:!0,cascade_to_disabled:!0,cascade_to_hidden:!0},a.jstree.plugins.checkbox=function(c,d){this.bind=function(){d.bind.call(this),this._data.checkbox.uto=!1,this._data.checkbox.selected=[],this.settings.checkbox.three_state&&(this.settings.checkbox.cascade="up+down+undetermined"),this.element.on("init.jstree",function(){this._data.checkbox.visible=this.settings.checkbox.visible,this.settings.checkbox.keep_selected_style||this.element.addClass("jstree-checkbox-no-clicked"),this.settings.checkbox.tie_selection&&this.element.addClass("jstree-checkbox-selection")}.bind(this)).on("loading.jstree",function(){this[this._data.checkbox.visible?"show_checkboxes":"hide_checkboxes"]()}.bind(this)),-1!==this.settings.checkbox.cascade.indexOf("undetermined")&&this.element.on("changed.jstree uncheck_node.jstree check_node.jstree uncheck_all.jstree check_all.jstree move_node.jstree copy_node.jstree redraw.jstree open_node.jstree",function(){this._data.checkbox.uto&&clearTimeout(this._data.checkbox.uto),this._data.checkbox.uto=setTimeout(this._undetermined.bind(this),50)}.bind(this)),this.settings.checkbox.tie_selection||this.element.on("model.jstree",function(a,b){var c=this._model.data,d=c[b.parent],e=b.nodes,f,g;for(f=0,g=e.length;g>f;f++)c[e[f]].state.checked=c[e[f]].state.checked||c[e[f]].original&&c[e[f]].original.state&&c[e[f]].original.state.checked,c[e[f]].state.checked&&this._data.checkbox.selected.push(e[f])}.bind(this)),(-1!==this.settings.checkbox.cascade.indexOf("up")||-1!==this.settings.checkbox.cascade.indexOf("down"))&&this.element.on("model.jstree",function(b,c){var d=this._model.data,e=d[c.parent],f=c.nodes,g=[],h,i,j,k,l,m,n=this.settings.checkbox.cascade,o=this.settings.checkbox.tie_selection;if(-1!==n.indexOf("down"))if(e.state[o?"selected":"checked"]){for(i=0,j=f.length;j>i;i++)d[f[i]].state[o?"selected":"checked"]=!0;this._data[o?"core":"checkbox"].selected=this._data[o?"core":"checkbox"].selected.concat(f)}else for(i=0,j=f.length;j>i;i++)if(d[f[i]].state[o?"selected":"checked"]){for(k=0,l=d[f[i]].children_d.length;l>k;k++)d[d[f[i]].children_d[k]].state[o?"selected":"checked"]=!0;this._data[o?"core":"checkbox"].selected=this._data[o?"core":"checkbox"].selected.concat(d[f[i]].children_d)}if(-1!==n.indexOf("up")){for(i=0,j=e.children_d.length;j>i;i++)d[e.children_d[i]].children.length||g.push(d[e.children_d[i]].parent);for(g=a.vakata.array_unique(g),k=0,l=g.length;l>k;k++){e=d[g[k]];while(e&&e.id!==a.jstree.root){for(h=0,i=0,j=e.children.length;j>i;i++)h+=d[e.children[i]].state[o?"selected":"checked"];if(h!==j)break;e.state[o?"selected":"checked"]=!0,this._data[o?"core":"checkbox"].selected.push(e.id),m=this.get_node(e,!0),m&&m.length&&m.attr("aria-selected",!0).children(".jstree-anchor").addClass(o?"jstree-clicked":"jstree-checked"),e=this.get_node(e.parent)}}}this._data[o?"core":"checkbox"].selected=a.vakata.array_unique(this._data[o?"core":"checkbox"].selected)}.bind(this)).on(this.settings.checkbox.tie_selection?"select_node.jstree":"check_node.jstree",function(b,c){var d=this,e=c.node,f=this._model.data,g=this.get_node(e.parent),h,i,j,k,l=this.settings.checkbox.cascade,m=this.settings.checkbox.tie_selection,n={},o=this._data[m?"core":"checkbox"].selected;for(h=0,i=o.length;i>h;h++)n[o[h]]=!0;if(-1!==l.indexOf("down")){var p=this._cascade_new_checked_state(e.id,!0),q=e.children_d.concat(e.id);for(h=0,i=q.length;i>h;h++)p.indexOf(q[h])>-1?n[q[h]]=!0:delete n[q[h]]}if(-1!==l.indexOf("up"))while(g&&g.id!==a.jstree.root){for(j=0,h=0,i=g.children.length;i>h;h++)j+=f[g.children[h]].state[m?"selected":"checked"];if(j!==i)break;g.state[m?"selected":"checked"]=!0,n[g.id]=!0,k=this.get_node(g,!0),k&&k.length&&k.attr("aria-selected",!0).children(".jstree-anchor").addClass(m?"jstree-clicked":"jstree-checked"),g=this.get_node(g.parent)}o=[];for(h in n)n.hasOwnProperty(h)&&o.push(h);this._data[m?"core":"checkbox"].selected=o}.bind(this)).on(this.settings.checkbox.tie_selection?"deselect_all.jstree":"uncheck_all.jstree",function(b,c){var d=this.get_node(a.jstree.root),e=this._model.data,f,g,h;for(f=0,g=d.children_d.length;g>f;f++)h=e[d.children_d[f]],h&&h.original&&h.original.state&&h.original.state.undetermined&&(h.original.state.undetermined=!1)}.bind(this)).on(this.settings.checkbox.tie_selection?"deselect_node.jstree":"uncheck_node.jstree",function(b,c){var d=this,e=c.node,f=this.get_node(e,!0),g,h,i,j=this.settings.checkbox.cascade,k=this.settings.checkbox.tie_selection,l=this._data[k?"core":"checkbox"].selected,m={},n=[],o=e.children_d.concat(e.id);if(-1!==j.indexOf("down")){var p=this._cascade_new_checked_state(e.id,!1);l=a.vakata.array_filter(l,function(a){return-1===o.indexOf(a)||p.indexOf(a)>-1})}if(-1!==j.indexOf("up")&&-1===l.indexOf(e.id)){for(g=0,h=e.parents.length;h>g;g++)i=this._model.data[e.parents[g]],i.state[k?"selected":"checked"]=!1,i&&i.original&&i.original.state&&i.original.state.undetermined&&(i.original.state.undetermined=!1),i=this.get_node(e.parents[g],!0),i&&i.length&&i.attr("aria-selected",!1).children(".jstree-anchor").removeClass(k?"jstree-clicked":"jstree-checked");l=a.vakata.array_filter(l,function(a){return-1===e.parents.indexOf(a)})}this._data[k?"core":"checkbox"].selected=l}.bind(this)),-1!==this.settings.checkbox.cascade.indexOf("up")&&this.element.on("delete_node.jstree",function(b,c){var d=this.get_node(c.parent),e=this._model.data,f,g,h,i,j=this.settings.checkbox.tie_selection;while(d&&d.id!==a.jstree.root&&!d.state[j?"selected":"checked"]){for(h=0,f=0,g=d.children.length;g>f;f++)h+=e[d.children[f]].state[j?"selected":"checked"];if(!(g>0&&h===g))break;d.state[j?"selected":"checked"]=!0,this._data[j?"core":"checkbox"].selected.push(d.id),i=this.get_node(d,!0),i&&i.length&&i.attr("aria-selected",!0).children(".jstree-anchor").addClass(j?"jstree-clicked":"jstree-checked"),d=this.get_node(d.parent)}}.bind(this)).on("move_node.jstree",function(b,c){var d=c.is_multi,e=c.old_parent,f=this.get_node(c.parent),g=this._model.data,h,i,j,k,l,m=this.settings.checkbox.tie_selection;if(!d){h=this.get_node(e);while(h&&h.id!==a.jstree.root&&!h.state[m?"selected":"checked"]){for(i=0,j=0,k=h.children.length;k>j;j++)i+=g[h.children[j]].state[m?"selected":"checked"];if(!(k>0&&i===k))break;h.state[m?"selected":"checked"]=!0,this._data[m?"core":"checkbox"].selected.push(h.id),l=this.get_node(h,!0),l&&l.length&&l.attr("aria-selected",!0).children(".jstree-anchor").addClass(m?"jstree-clicked":"jstree-checked"),h=this.get_node(h.parent)}}h=f;while(h&&h.id!==a.jstree.root){for(i=0,j=0,k=h.children.length;k>j;j++)i+=g[h.children[j]].state[m?"selected":"checked"];if(i===k)h.state[m?"selected":"checked"]||(h.state[m?"selected":"checked"]=!0,this._data[m?"core":"checkbox"].selected.push(h.id),l=this.get_node(h,!0),l&&l.length&&l.attr("aria-selected",!0).children(".jstree-anchor").addClass(m?"jstree-clicked":"jstree-checked"));else{if(!h.state[m?"selected":"checked"])break;h.state[m?"selected":"checked"]=!1,this._data[m?"core":"checkbox"].selected=a.vakata.array_remove_item(this._data[m?"core":"checkbox"].selected,h.id),l=this.get_node(h,!0),l&&l.length&&l.attr("aria-selected",!1).children(".jstree-anchor").removeClass(m?"jstree-clicked":"jstree-checked")}h=this.get_node(h.parent)}}.bind(this))},this.get_undetermined=function(c){if(-1===this.settings.checkbox.cascade.indexOf("undetermined"))return[];var d,e,f,g,h={},i=this._model.data,j=this.settings.checkbox.tie_selection,k=this._data[j?"core":"checkbox"].selected,l=[],m=this,n=[];for(d=0,e=k.length;e>d;d++)if(i[k[d]]&&i[k[d]].parents)for(f=0,g=i[k[d]].parents.length;g>f;f++){if(h[i[k[d]].parents[f]]!==b)break;i[k[d]].parents[f]!==a.jstree.root&&(h[i[k[d]].parents[f]]=!0,l.push(i[k[d]].parents[f]))}for(this.element.find(".jstree-closed").not(":has(.jstree-children)").each(function(){var c=m.get_node(this),j;if(c)if(c.state.loaded){for(d=0,e=c.children_d.length;e>d;d++)if(j=i[c.children_d[d]],!j.state.loaded&&j.original&&j.original.state&&j.original.state.undetermined&&j.original.state.undetermined===!0)for(h[j.id]===b&&j.id!==a.jstree.root&&(h[j.id]=!0,l.push(j.id)),f=0,g=j.parents.length;g>f;f++)h[j.parents[f]]===b&&j.parents[f]!==a.jstree.root&&(h[j.parents[f]]=!0,l.push(j.parents[f]))}else if(c.original&&c.original.state&&c.original.state.undetermined&&c.original.state.undetermined===!0)for(h[c.id]===b&&c.id!==a.jstree.root&&(h[c.id]=!0,l.push(c.id)),f=0,g=c.parents.length;g>f;f++)h[c.parents[f]]===b&&c.parents[f]!==a.jstree.root&&(h[c.parents[f]]=!0,l.push(c.parents[f]))}),d=0,e=l.length;e>d;d++)i[l[d]].state[j?"selected":"checked"]||n.push(c?i[l[d]]:l[d]);return n},this._undetermined=function(){if(null!==this.element){var a=this.get_undetermined(!1),b,c,d;for(this.element.find(".jstree-undetermined").removeClass("jstree-undetermined"),b=0,c=a.length;c>b;b++)d=this.get_node(a[b],!0),d&&d.length&&d.children(".jstree-anchor").children(".jstree-checkbox").addClass("jstree-undetermined")}},this.redraw_node=function(a,b,c,e){if(a=d.redraw_node.apply(this,arguments)){var f,g,h=null,i=null;for(f=0,g=a.childNodes.length;g>f;f++)if(a.childNodes[f]&&a.childNodes[f].className&&-1!==a.childNodes[f].className.indexOf("jstree-anchor")){h=a.childNodes[f];break}h&&(!this.settings.checkbox.tie_selection&&this._model.data[a.id].state.checked&&(h.className+=" jstree-checked"),i=l.cloneNode(!1),this._model.data[a.id].state.checkbox_disabled&&(i.className+=" jstree-checkbox-disabled"),h.insertBefore(i,h.childNodes[0]))}return c||-1===this.settings.checkbox.cascade.indexOf("undetermined")||(this._data.checkbox.uto&&clearTimeout(this._data.checkbox.uto),this._data.checkbox.uto=setTimeout(this._undetermined.bind(this),50)),a},this.show_checkboxes=function(){this._data.core.themes.checkboxes=!0,this.get_container_ul().removeClass("jstree-no-checkboxes")},this.hide_checkboxes=function(){this._data.core.themes.checkboxes=!1,this.get_container_ul().addClass("jstree-no-checkboxes")},this.toggle_checkboxes=function(){this._data.core.themes.checkboxes?this.hide_checkboxes():this.show_checkboxes()},this.is_undetermined=function(b){b=this.get_node(b);var c=this.settings.checkbox.cascade,d,e,f=this.settings.checkbox.tie_selection,g=this._data[f?"core":"checkbox"].selected,h=this._model.data;if(!b||b.state[f?"selected":"checked"]===!0||-1===c.indexOf("undetermined")||-1===c.indexOf("down")&&-1===c.indexOf("up"))return!1;if(!b.state.loaded&&b.original.state.undetermined===!0)return!0;for(d=0,e=b.children_d.length;e>d;d++)if(-1!==a.inArray(b.children_d[d],g)||!h[b.children_d[d]].state.loaded&&h[b.children_d[d]].original.state.undetermined)return!0;return!1},this.disable_checkbox=function(b){var c,d,e;if(a.vakata.is_array(b)){for(b=b.slice(),c=0,d=b.length;d>c;c++)this.disable_checkbox(b[c]);return!0}return b=this.get_node(b),b&&b.id!==a.jstree.root?(e=this.get_node(b,!0),void(b.state.checkbox_disabled||(b.state.checkbox_disabled=!0,e&&e.length&&e.children(".jstree-anchor").children(".jstree-checkbox").addClass("jstree-checkbox-disabled"),this.trigger("disable_checkbox",{node:b})))):!1},this.enable_checkbox=function(b){var c,d,e;if(a.vakata.is_array(b)){for(b=b.slice(),c=0,d=b.length;d>c;c++)this.enable_checkbox(b[c]);return!0}return b=this.get_node(b),b&&b.id!==a.jstree.root?(e=this.get_node(b,!0),void(b.state.checkbox_disabled&&(b.state.checkbox_disabled=!1,e&&e.length&&e.children(".jstree-anchor").children(".jstree-checkbox").removeClass("jstree-checkbox-disabled"),this.trigger("enable_checkbox",{node:b})))):!1},this.activate_node=function(b,c){return a(c.target).hasClass("jstree-checkbox-disabled")?!1:(this.settings.checkbox.tie_selection&&(this.settings.checkbox.whole_node||a(c.target).hasClass("jstree-checkbox"))&&(c.ctrlKey=!0),this.settings.checkbox.tie_selection||!this.settings.checkbox.whole_node&&!a(c.target).hasClass("jstree-checkbox")?d.activate_node.call(this,b,c):this.is_disabled(b)?!1:(this.is_checked(b)?this.uncheck_node(b,c):this.check_node(b,c),void this.trigger("activate_node",{node:this.get_node(b)})))},this._cascade_new_checked_state=function(a,b){var c=this,d=this.settings.checkbox.tie_selection,e=this._model.data[a],f=[],g=[],h,i,j;if(!this.settings.checkbox.cascade_to_disabled&&e.state.disabled||!this.settings.checkbox.cascade_to_hidden&&e.state.hidden)j=this.get_checked_descendants(a),e.state[d?"selected":"checked"]&&j.push(e.id),f=f.concat(j);else{if(e.children)for(h=0,i=e.children.length;i>h;h++){var k=e.children[h];j=c._cascade_new_checked_state(k,b),f=f.concat(j),j.indexOf(k)>-1&&g.push(k)}var l=c.get_node(e,!0),m=g.length>0&&g.length<e.children.length;e.original&&e.original.state&&e.original.state.undetermined&&(e.original.state.undetermined=m),m?(e.state[d?"selected":"checked"]=!1,l.attr("aria-selected",!1).children(".jstree-anchor").removeClass(d?"jstree-clicked":"jstree-checked")):b&&g.length===e.children.length?(e.state[d?"selected":"checked"]=b,f.push(e.id),l.attr("aria-selected",!0).children(".jstree-anchor").addClass(d?"jstree-clicked":"jstree-checked")):(e.state[d?"selected":"checked"]=!1,l.attr("aria-selected",!1).children(".jstree-anchor").removeClass(d?"jstree-clicked":"jstree-checked"))}return f},this.get_checked_descendants=function(b){var c=this,d=c.settings.checkbox.tie_selection,e=c._model.data[b];return a.vakata.array_filter(e.children_d,function(a){return c._model.data[a].state[d?"selected":"checked"]})},this.check_node=function(b,c){if(this.settings.checkbox.tie_selection)return this.select_node(b,!1,!0,c);var d,e,f,g;if(a.vakata.is_array(b)){for(b=b.slice(),e=0,f=b.length;f>e;e++)this.check_node(b[e],c);return!0}return b=this.get_node(b),b&&b.id!==a.jstree.root?(d=this.get_node(b,!0),void(b.state.checked||(b.state.checked=!0,this._data.checkbox.selected.push(b.id),d&&d.length&&d.children(".jstree-anchor").addClass("jstree-checked"),this.trigger("check_node",{node:b,selected:this._data.checkbox.selected,event:c})))):!1},this.uncheck_node=function(b,c){if(this.settings.checkbox.tie_selection)return this.deselect_node(b,!1,c);var d,e,f;if(a.vakata.is_array(b)){for(b=b.slice(),d=0,e=b.length;e>d;d++)this.uncheck_node(b[d],c);return!0}return b=this.get_node(b),b&&b.id!==a.jstree.root?(f=this.get_node(b,!0),void(b.state.checked&&(b.state.checked=!1,this._data.checkbox.selected=a.vakata.array_remove_item(this._data.checkbox.selected,b.id),f.length&&f.children(".jstree-anchor").removeClass("jstree-checked"),this.trigger("uncheck_node",{node:b,selected:this._data.checkbox.selected,event:c})))):!1},this.check_all=function(){if(this.settings.checkbox.tie_selection)return this.select_all();var b=this._data.checkbox.selected.concat([]),c,d;for(this._data.checkbox.selected=this._model.data[a.jstree.root].children_d.concat(),c=0,d=this._data.checkbox.selected.length;d>c;c++)this._model.data[this._data.checkbox.selected[c]]&&(this._model.data[this._data.checkbox.selected[c]].state.checked=!0);this.redraw(!0),this.trigger("check_all",{selected:this._data.checkbox.selected})},this.uncheck_all=function(){if(this.settings.checkbox.tie_selection)return this.deselect_all();var a=this._data.checkbox.selected.concat([]),b,c;for(b=0,c=this._data.checkbox.selected.length;c>b;b++)this._model.data[this._data.checkbox.selected[b]]&&(this._model.data[this._data.checkbox.selected[b]].state.checked=!1);this._data.checkbox.selected=[],this.element.find(".jstree-checked").removeClass("jstree-checked"),this.trigger("uncheck_all",{selected:this._data.checkbox.selected,node:a})},this.is_checked=function(b){return this.settings.checkbox.tie_selection?this.is_selected(b):(b=this.get_node(b),b&&b.id!==a.jstree.root?b.state.checked:!1)},this.get_checked=function(b){return this.settings.checkbox.tie_selection?this.get_selected(b):b?a.map(this._data.checkbox.selected,function(a){return this.get_node(a)}.bind(this)):this._data.checkbox.selected.slice()},this.get_top_checked=function(b){if(this.settings.checkbox.tie_selection)return this.get_top_selected(b);var c=this.get_checked(!0),d={},e,f,g,h;for(e=0,f=c.length;f>e;e++)d[c[e].id]=c[e];for(e=0,f=c.length;f>e;e++)for(g=0,h=c[e].children_d.length;h>g;g++)d[c[e].children_d[g]]&&delete d[c[e].children_d[g]];c=[];for(e in d)d.hasOwnProperty(e)&&c.push(e);return b?a.map(c,function(a){return this.get_node(a)}.bind(this)):c},this.get_bottom_checked=function(b){if(this.settings.checkbox.tie_selection)return this.get_bottom_selected(b);var c=this.get_checked(!0),d=[],e,f;for(e=0,f=c.length;f>e;e++)c[e].children.length||d.push(c[e].id);return b?a.map(d,function(a){return this.get_node(a)}.bind(this)):d},this.load_node=function(b,c){var e,f,g,h,i,j;if(!a.vakata.is_array(b)&&!this.settings.checkbox.tie_selection&&(j=this.get_node(b),j&&j.state.loaded))for(e=0,f=j.children_d.length;f>e;e++)this._model.data[j.children_d[e]].state.checked&&(i=!0,this._data.checkbox.selected=a.vakata.array_remove_item(this._data.checkbox.selected,j.children_d[e]));return d.load_node.apply(this,arguments)},this.get_state=function(){var a=d.get_state.apply(this,arguments);return this.settings.checkbox.tie_selection?a:(a.checkbox=this._data.checkbox.selected.slice(),a)},this.set_state=function(b,c){var e=d.set_state.apply(this,arguments);if(e&&b.checkbox){if(!this.settings.checkbox.tie_selection){this.uncheck_all();var f=this;a.each(b.checkbox,function(a,b){f.check_node(b)})}return delete b.checkbox,this.set_state(b,c),!1}return e},this.refresh=function(a,b){return this.settings.checkbox.tie_selection&&(this._data.checkbox.selected=[]),d.refresh.apply(this,arguments)}},a.jstree.defaults.conditionalselect=function(){return!0},a.jstree.plugins.conditionalselect=function(a,b){this.activate_node=function(a,c){return this.settings.conditionalselect.call(this,this.get_node(a),c)?b.activate_node.call(this,a,c):void 0}},a.jstree.defaults.contextmenu={select_node:!0,show_at_node:!0,items:function(b,c){return{create:{separator_before:!1,separator_after:!0,_disabled:!1,label:"Create",action:function(b){var c=a.jstree.reference(b.reference),d=c.get_node(b.reference);c.create_node(d,{},"last",function(a){try{c.edit(a)}catch(b){setTimeout(function(){c.edit(a)},0)}})}},rename:{separator_before:!1,separator_after:!1,_disabled:!1,label:"Rename",action:function(b){var c=a.jstree.reference(b.reference),d=c.get_node(b.reference);c.edit(d)}},remove:{separator_before:!1,icon:!1,separator_after:!1,_disabled:!1,label:"Delete",action:function(b){var c=a.jstree.reference(b.reference),d=c.get_node(b.reference);c.is_selected(d)?c.delete_node(c.get_selected()):c.delete_node(d)}},ccp:{separator_before:!0,icon:!1,separator_after:!1,label:"Edit",action:!1,submenu:{cut:{separator_before:!1,separator_after:!1,label:"Cut",action:function(b){var c=a.jstree.reference(b.reference),d=c.get_node(b.reference);c.is_selected(d)?c.cut(c.get_top_selected()):c.cut(d)}},copy:{separator_before:!1,icon:!1,separator_after:!1,label:"Copy",action:function(b){var c=a.jstree.reference(b.reference),d=c.get_node(b.reference);c.is_selected(d)?c.copy(c.get_top_selected()):c.copy(d)}},paste:{separator_before:!1,icon:!1,_disabled:function(b){return!a.jstree.reference(b.reference).can_paste()},separator_after:!1,label:"Paste",action:function(b){var c=a.jstree.reference(b.reference),d=c.get_node(b.reference);c.paste(d)}}}}}}},a.jstree.plugins.contextmenu=function(c,d){this.bind=function(){d.bind.call(this);var b=0,c=null,e,f;this.element.on("init.jstree loading.jstree ready.jstree",function(){this.get_container_ul().addClass("jstree-contextmenu")}.bind(this)).on("contextmenu.jstree",".jstree-anchor",function(a,d){"input"!==a.target.tagName.toLowerCase()&&(a.preventDefault(),b=a.ctrlKey?+new Date:0,(d||c)&&(b=+new Date+1e4),c&&clearTimeout(c),this.is_loading(a.currentTarget)||this.show_contextmenu(a.currentTarget,a.pageX,a.pageY,a))}.bind(this)).on("click.jstree",".jstree-anchor",function(c){this._data.contextmenu.visible&&(!b||+new Date-b>250)&&a.vakata.context.hide(),b=0}.bind(this)).on("touchstart.jstree",".jstree-anchor",function(b){b.originalEvent&&b.originalEvent.changedTouches&&b.originalEvent.changedTouches[0]&&(e=b.originalEvent.changedTouches[0].clientX,f=b.originalEvent.changedTouches[0].clientY,c=setTimeout(function(){a(b.currentTarget).trigger("contextmenu",!0)},750))}).on("touchmove.vakata.jstree",function(b){c&&b.originalEvent&&b.originalEvent.changedTouches&&b.originalEvent.changedTouches[0]&&(Math.abs(e-b.originalEvent.changedTouches[0].clientX)>10||Math.abs(f-b.originalEvent.changedTouches[0].clientY)>10)&&(clearTimeout(c),a.vakata.context.hide())}).on("touchend.vakata.jstree",function(a){c&&clearTimeout(c)}),a(i).on("context_hide.vakata.jstree",function(b,c){this._data.contextmenu.visible=!1,a(c.reference).removeClass("jstree-context")}.bind(this))},this.teardown=function(){this._data.contextmenu.visible&&a.vakata.context.hide(),a(i).off("context_hide.vakata.jstree"),d.teardown.call(this)},this.show_contextmenu=function(c,d,e,f){if(c=this.get_node(c),!c||c.id===a.jstree.root)return!1;var g=this.settings.contextmenu,h=this.get_node(c,!0),i=h.children(".jstree-anchor"),j=!1,k=!1;(g.show_at_node||d===b||e===b)&&(j=i.offset(),d=j.left,e=j.top+this._data.core.li_height),this.settings.contextmenu.select_node&&!this.is_selected(c)&&this.activate_node(c,f),k=g.items,a.vakata.is_function(k)&&(k=k.call(this,c,function(a){this._show_contextmenu(c,d,e,a)}.bind(this))),a.isPlainObject(k)&&this._show_contextmenu(c,d,e,k)},this._show_contextmenu=function(b,c,d,e){
var f=this.get_node(b,!0),g=f.children(".jstree-anchor");a(i).one("context_show.vakata.jstree",function(b,c){var d="jstree-contextmenu jstree-"+this.get_theme()+"-contextmenu";a(c.element).addClass(d),g.addClass("jstree-context")}.bind(this)),this._data.contextmenu.visible=!0,a.vakata.context.show(g,{x:c,y:d},e),this.trigger("show_contextmenu",{node:b,x:c,y:d})}},function(a){var b=!1,c={element:!1,reference:!1,position_x:0,position_y:0,items:[],html:"",is_visible:!1};a.vakata.context={settings:{hide_onmouseleave:0,icons:!0},_trigger:function(b){a(i).triggerHandler("context_"+b+".vakata",{reference:c.reference,element:c.element,position:{x:c.position_x,y:c.position_y}})},_execute:function(b){return b=c.items[b],b&&(!b._disabled||a.vakata.is_function(b._disabled)&&!b._disabled({item:b,reference:c.reference,element:c.element}))&&b.action?b.action.call(null,{item:b,reference:c.reference,element:c.element,position:{x:c.position_x,y:c.position_y}}):!1},_parse:function(b,d){if(!b)return!1;d||(c.html="",c.items=[]);var e="",f=!1,g;return d&&(e+="<ul>"),a.each(b,function(b,d){return d?(c.items.push(d),!f&&d.separator_before&&(e+="<li class='vakata-context-separator'><a href='#' "+(a.vakata.context.settings.icons?"":'class="vakata-context-no-icons"')+">&#160;</a></li>"),f=!1,e+="<li class='"+(d._class||"")+(d._disabled===!0||a.vakata.is_function(d._disabled)&&d._disabled({item:d,reference:c.reference,element:c.element})?" vakata-contextmenu-disabled ":"")+"' "+(d.shortcut?" data-shortcut='"+d.shortcut+"' ":"")+">",e+="<a href='#' rel='"+(c.items.length-1)+"' "+(d.title?"title='"+d.title+"'":"")+">",a.vakata.context.settings.icons&&(e+="<i ",d.icon&&(e+=-1!==d.icon.indexOf("/")||-1!==d.icon.indexOf(".")?" style='background:url(\""+d.icon+"\") center center no-repeat' ":" class='"+d.icon+"' "),e+="></i><span class='vakata-contextmenu-sep'>&#160;</span>"),e+=(a.vakata.is_function(d.label)?d.label({item:b,reference:c.reference,element:c.element}):d.label)+(d.shortcut?' <span class="vakata-contextmenu-shortcut vakata-contextmenu-shortcut-'+d.shortcut+'">'+(d.shortcut_label||"")+"</span>":"")+"</a>",d.submenu&&(g=a.vakata.context._parse(d.submenu,!0),g&&(e+=g)),e+="</li>",void(d.separator_after&&(e+="<li class='vakata-context-separator'><a href='#' "+(a.vakata.context.settings.icons?"":'class="vakata-context-no-icons"')+">&#160;</a></li>",f=!0))):!0}),e=e.replace(/<li class\='vakata-context-separator'\><\/li\>$/,""),d&&(e+="</ul>"),d||(c.html=e,a.vakata.context._trigger("parse")),e.length>10?e:!1},_show_submenu:function(c){if(c=a(c),c.length&&c.children("ul").length){var d=c.children("ul"),e=c.offset().left,f=e+c.outerWidth(),g=c.offset().top,h=d.width(),i=d.height(),j=a(window).width()+a(window).scrollLeft(),k=a(window).height()+a(window).scrollTop();b?c[f-(h+10+c.outerWidth())<0?"addClass":"removeClass"]("vakata-context-left"):c[f+h>j&&e>j-f?"addClass":"removeClass"]("vakata-context-right"),g+i+10>k&&d.css("bottom","-1px"),c.hasClass("vakata-context-right")?h>e&&d.css("margin-right",e-h):h>j-f&&d.css("margin-left",j-f-h),d.show()}},show:function(d,e,f){var g,h,j,k,l,m,n,o,p=!0;switch(c.element&&c.element.length&&c.element.width(""),p){case!e&&!d:return!1;case!!e&&!!d:c.reference=d,c.position_x=e.x,c.position_y=e.y;break;case!e&&!!d:c.reference=d,g=d.offset(),c.position_x=g.left+d.outerHeight(),c.position_y=g.top;break;case!!e&&!d:c.position_x=e.x,c.position_y=e.y}d&&!f&&a(d).data("vakata_contextmenu")&&(f=a(d).data("vakata_contextmenu")),a.vakata.context._parse(f)&&c.element.html(c.html),c.items.length&&(c.element.appendTo(i.body),h=c.element,j=c.position_x,k=c.position_y,l=h.width(),m=h.height(),n=a(window).width()+a(window).scrollLeft(),o=a(window).height()+a(window).scrollTop(),b&&(j-=h.outerWidth()-a(d).outerWidth(),j<a(window).scrollLeft()+20&&(j=a(window).scrollLeft()+20)),j+l+20>n&&(j=n-(l+20)),k+m+20>o&&(k=o-(m+20)),c.element.css({left:j,top:k}).show().find("a").first().trigger("focus").parent().addClass("vakata-context-hover"),c.is_visible=!0,a.vakata.context._trigger("show"))},hide:function(){c.is_visible&&(c.element.hide().find("ul").hide().end().find(":focus").trigger("blur").end().detach(),c.is_visible=!1,a.vakata.context._trigger("hide"))}},a(function(){b="rtl"===a(i.body).css("direction");var d=!1;c.element=a("<ul class='vakata-context'></ul>"),c.element.on("mouseenter","li",function(b){b.stopImmediatePropagation(),a.contains(this,b.relatedTarget)||(d&&clearTimeout(d),c.element.find(".vakata-context-hover").removeClass("vakata-context-hover").end(),a(this).siblings().find("ul").hide().end().end().parentsUntil(".vakata-context","li").addBack().addClass("vakata-context-hover"),a.vakata.context._show_submenu(this))}).on("mouseleave","li",function(b){a.contains(this,b.relatedTarget)||a(this).find(".vakata-context-hover").addBack().removeClass("vakata-context-hover")}).on("mouseleave",function(b){a(this).find(".vakata-context-hover").removeClass("vakata-context-hover"),a.vakata.context.settings.hide_onmouseleave&&(d=setTimeout(function(b){return function(){a.vakata.context.hide()}}(this),a.vakata.context.settings.hide_onmouseleave))}).on("click","a",function(b){b.preventDefault(),a(this).trigger("blur").parent().hasClass("vakata-context-disabled")||a.vakata.context._execute(a(this).attr("rel"))===!1||a.vakata.context.hide()}).on("keydown","a",function(b){var d=null;switch(b.which){case 13:case 32:b.type="click",b.preventDefault(),a(b.currentTarget).trigger(b);break;case 37:c.is_visible&&(c.element.find(".vakata-context-hover").last().closest("li").first().find("ul").hide().find(".vakata-context-hover").removeClass("vakata-context-hover").end().end().children("a").trigger("focus"),b.stopImmediatePropagation(),b.preventDefault());break;case 38:c.is_visible&&(d=c.element.find("ul:visible").addBack().last().children(".vakata-context-hover").removeClass("vakata-context-hover").prevAll("li:not(.vakata-context-separator)").first(),d.length||(d=c.element.find("ul:visible").addBack().last().children("li:not(.vakata-context-separator)").last()),d.addClass("vakata-context-hover").children("a").trigger("focus"),b.stopImmediatePropagation(),b.preventDefault());break;case 39:c.is_visible&&(c.element.find(".vakata-context-hover").last().children("ul").show().children("li:not(.vakata-context-separator)").removeClass("vakata-context-hover").first().addClass("vakata-context-hover").children("a").trigger("focus"),b.stopImmediatePropagation(),b.preventDefault());break;case 40:c.is_visible&&(d=c.element.find("ul:visible").addBack().last().children(".vakata-context-hover").removeClass("vakata-context-hover").nextAll("li:not(.vakata-context-separator)").first(),d.length||(d=c.element.find("ul:visible").addBack().last().children("li:not(.vakata-context-separator)").first()),d.addClass("vakata-context-hover").children("a").trigger("focus"),b.stopImmediatePropagation(),b.preventDefault());break;case 27:a.vakata.context.hide(),b.preventDefault()}}).on("keydown",function(a){a.preventDefault();var b=c.element.find(".vakata-contextmenu-shortcut-"+a.which).parent();b.parent().not(".vakata-context-disabled")&&b.trigger("click")}),a(i).on("mousedown.vakata.jstree",function(b){c.is_visible&&c.element[0]!==b.target&&!a.contains(c.element[0],b.target)&&a.vakata.context.hide()}).on("context_show.vakata.jstree",function(a,d){c.element.find("li:has(ul)").children("a").addClass("vakata-context-parent"),b&&c.element.addClass("vakata-context-rtl").css("direction","rtl"),c.element.find("ul").hide().end()})})}(a),a.jstree.defaults.dnd={copy:!0,open_timeout:500,is_draggable:!0,check_while_dragging:!0,always_copy:!1,inside_pos:0,drag_selection:!0,touch:!0,large_drop_target:!1,large_drag_target:!1,use_html5:!1};var m,n;a.jstree.plugins.dnd=function(b,c){this.init=function(a,b){c.init.call(this,a,b),this.settings.dnd.use_html5=this.settings.dnd.use_html5&&"draggable"in i.createElement("span")},this.bind=function(){c.bind.call(this),this.element.on(this.settings.dnd.use_html5?"dragstart.jstree":"mousedown.jstree touchstart.jstree",this.settings.dnd.large_drag_target?".jstree-node":".jstree-anchor",function(b){if(this.settings.dnd.large_drag_target&&a(b.target).closest(".jstree-node")[0]!==b.currentTarget)return!0;if("touchstart"===b.type&&(!this.settings.dnd.touch||"selected"===this.settings.dnd.touch&&!a(b.currentTarget).closest(".jstree-node").children(".jstree-anchor").hasClass("jstree-clicked")))return!0;var c=this.get_node(b.target),d=this.is_selected(c)&&this.settings.dnd.drag_selection?this.get_top_selected().length:1,e=d>1?d+" "+this.get_string("nodes"):this.get_text(b.currentTarget);if(this.settings.core.force_text&&(e=a.vakata.html.escape(e)),c&&c.id&&c.id!==a.jstree.root&&(1===b.which||"touchstart"===b.type||"dragstart"===b.type)&&(this.settings.dnd.is_draggable===!0||a.vakata.is_function(this.settings.dnd.is_draggable)&&this.settings.dnd.is_draggable.call(this,d>1?this.get_top_selected(!0):[c],b))){if(m={jstree:!0,origin:this,obj:this.get_node(c,!0),nodes:d>1?this.get_top_selected():[c.id]},n=b.currentTarget,!this.settings.dnd.use_html5)return this.element.trigger("mousedown.jstree"),a.vakata.dnd.start(b,m,'<div id="jstree-dnd" class="jstree-'+this.get_theme()+" jstree-"+this.get_theme()+"-"+this.get_theme_variant()+" "+(this.settings.core.themes.responsive?" jstree-dnd-responsive":"")+'"><i class="jstree-icon jstree-er"></i>'+e+'<ins class="jstree-copy">+</ins></div>');a.vakata.dnd._trigger("start",b,{helper:a(),element:n,data:m})}}.bind(this)),this.settings.dnd.use_html5&&this.element.on("dragover.jstree",function(b){return b.preventDefault(),a.vakata.dnd._trigger("move",b,{helper:a(),element:n,data:m}),!1}).on("drop.jstree",function(b){return b.preventDefault(),a.vakata.dnd._trigger("stop",b,{helper:a(),element:n,data:m}),!1}.bind(this))},this.redraw_node=function(a,b,d,e){if(a=c.redraw_node.apply(this,arguments),a&&this.settings.dnd.use_html5)if(this.settings.dnd.large_drag_target)a.setAttribute("draggable",!0);else{var f,g,h=null;for(f=0,g=a.childNodes.length;g>f;f++)if(a.childNodes[f]&&a.childNodes[f].className&&-1!==a.childNodes[f].className.indexOf("jstree-anchor")){h=a.childNodes[f];break}h&&h.setAttribute("draggable",!0)}return a}},a(function(){var c=!1,d=!1,e=!1,f=!1,g=a('<div id="jstree-marker">&#160;</div>').hide();a(i).on("dragover.vakata.jstree",function(b){n&&a.vakata.dnd._trigger("move",b,{helper:a(),element:n,data:m})}).on("drop.vakata.jstree",function(b){n&&(a.vakata.dnd._trigger("stop",b,{helper:a(),element:n,data:m}),n=null,m=null)}).on("dnd_start.vakata.jstree",function(a,b){c=!1,e=!1,b&&b.data&&b.data.jstree&&g.appendTo(i.body)}).on("dnd_move.vakata.jstree",function(h,i){var j=i.event.target!==e.target;if(f&&(!i.event||"dragover"!==i.event.type||j)&&clearTimeout(f),i&&i.data&&i.data.jstree&&(!i.event.target.id||"jstree-marker"!==i.event.target.id)){e=i.event;var k=a.jstree.reference(i.event.target),l=!1,m=!1,n=!1,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F;if(k&&k._data&&k._data.dnd)if(g.attr("class","jstree-"+k.get_theme()+(k.settings.core.themes.responsive?" jstree-dnd-responsive":"")),D=i.data.origin&&(i.data.origin.settings.dnd.always_copy||i.data.origin.settings.dnd.copy&&(i.event.metaKey||i.event.ctrlKey)),i.helper.children().attr("class","jstree-"+k.get_theme()+" jstree-"+k.get_theme()+"-"+k.get_theme_variant()+" "+(k.settings.core.themes.responsive?" jstree-dnd-responsive":"")).find(".jstree-copy").first()[D?"show":"hide"](),i.event.target!==k.element[0]&&i.event.target!==k.get_container_ul()[0]||0!==k.get_container_ul().children().length){if(l=k.settings.dnd.large_drop_target?a(i.event.target).closest(".jstree-node").children(".jstree-anchor"):a(i.event.target).closest(".jstree-anchor"),l&&l.length&&l.parent().is(".jstree-closed, .jstree-open, .jstree-leaf")&&(m=l.offset(),n=(i.event.pageY!==b?i.event.pageY:i.event.originalEvent.pageY)-m.top,r=l.outerHeight(),u=r/3>n?["b","i","a"]:n>r-r/3?["a","i","b"]:n>r/2?["i","a","b"]:["i","b","a"],a.each(u,function(b,e){switch(e){case"b":p=m.left-6,q=m.top,s=k.get_parent(l),t=l.parent().index(),F="jstree-below";break;case"i":B=k.settings.dnd.inside_pos,C=k.get_node(l.parent()),p=m.left-2,q=m.top+r/2+1,s=C.id,t="first"===B?0:"last"===B?C.children.length:Math.min(B,C.children.length),F="jstree-inside";break;case"a":p=m.left-6,q=m.top+r,s=k.get_parent(l),t=l.parent().index()+1,F="jstree-above"}for(v=!0,w=0,x=i.data.nodes.length;x>w;w++)if(y=i.data.origin&&(i.data.origin.settings.dnd.always_copy||i.data.origin.settings.dnd.copy&&(i.event.metaKey||i.event.ctrlKey))?"copy_node":"move_node",z=t,"move_node"===y&&"a"===e&&i.data.origin&&i.data.origin===k&&s===k.get_parent(i.data.nodes[w])&&(A=k.get_node(s),z>a.inArray(i.data.nodes[w],A.children)&&(z-=1)),v=v&&(k&&k.settings&&k.settings.dnd&&k.settings.dnd.check_while_dragging===!1||k.check(y,i.data.origin&&i.data.origin!==k?i.data.origin.get_node(i.data.nodes[w]):i.data.nodes[w],s,z,{dnd:!0,ref:k.get_node(l.parent()),pos:e,origin:i.data.origin,is_multi:i.data.origin&&i.data.origin!==k,is_foreign:!i.data.origin})),!v){k&&k.last_error&&(d=k.last_error());break}return"i"===e&&l.parent().is(".jstree-closed")&&k.settings.dnd.open_timeout&&(!i.event||"dragover"!==i.event.type||j)&&(f&&clearTimeout(f),f=setTimeout(function(a,b){return function(){a.open_node(b)}}(k,l),k.settings.dnd.open_timeout)),v?(E=k.get_node(s,!0),E.hasClass(".jstree-dnd-parent")||(a(".jstree-dnd-parent").removeClass("jstree-dnd-parent"),E.addClass("jstree-dnd-parent")),c={ins:k,par:s,pos:"i"!==e||"last"!==B||0!==t||k.is_loaded(C)?t:"last"},g.css({left:p+"px",top:q+"px"}).show(),g.removeClass("jstree-above jstree-inside jstree-below").addClass(F),i.helper.find(".jstree-icon").first().removeClass("jstree-er").addClass("jstree-ok"),i.event.originalEvent&&i.event.originalEvent.dataTransfer&&(i.event.originalEvent.dataTransfer.dropEffect=D?"copy":"move"),d={},u=!0,!1):void 0}),u===!0))return}else{for(v=!0,w=0,x=i.data.nodes.length;x>w;w++)if(v=v&&k.check(i.data.origin&&(i.data.origin.settings.dnd.always_copy||i.data.origin.settings.dnd.copy&&(i.event.metaKey||i.event.ctrlKey))?"copy_node":"move_node",i.data.origin&&i.data.origin!==k?i.data.origin.get_node(i.data.nodes[w]):i.data.nodes[w],a.jstree.root,"last",{dnd:!0,ref:k.get_node(a.jstree.root),pos:"i",origin:i.data.origin,is_multi:i.data.origin&&i.data.origin!==k,is_foreign:!i.data.origin}),!v)break;if(v)return c={ins:k,par:a.jstree.root,pos:"last"},g.hide(),i.helper.find(".jstree-icon").first().removeClass("jstree-er").addClass("jstree-ok"),void(i.event.originalEvent&&i.event.originalEvent.dataTransfer&&(i.event.originalEvent.dataTransfer.dropEffect=D?"copy":"move"))}a(".jstree-dnd-parent").removeClass("jstree-dnd-parent"),c=!1,i.helper.find(".jstree-icon").removeClass("jstree-ok").addClass("jstree-er"),i.event.originalEvent&&i.event.originalEvent.dataTransfer,g.hide()}}).on("dnd_scroll.vakata.jstree",function(a,b){b&&b.data&&b.data.jstree&&(g.hide(),c=!1,e=!1,b.helper.find(".jstree-icon").first().removeClass("jstree-ok").addClass("jstree-er"))}).on("dnd_stop.vakata.jstree",function(b,h){if(a(".jstree-dnd-parent").removeClass("jstree-dnd-parent"),f&&clearTimeout(f),h&&h.data&&h.data.jstree){g.hide().detach();var i,j,k=[];if(c){for(i=0,j=h.data.nodes.length;j>i;i++)k[i]=h.data.origin?h.data.origin.get_node(h.data.nodes[i]):h.data.nodes[i];c.ins[h.data.origin&&(h.data.origin.settings.dnd.always_copy||h.data.origin.settings.dnd.copy&&(h.event.metaKey||h.event.ctrlKey))?"copy_node":"move_node"](k,c.par,c.pos,!1,!1,!1,h.data.origin)}else i=a(h.event.target).closest(".jstree"),i.length&&d&&d.error&&"check"===d.error&&(i=i.jstree(!0),i&&i.settings.core.error.call(this,d));e=!1,c=!1}}).on("keyup.jstree keydown.jstree",function(b,h){h=a.vakata.dnd._get(),h&&h.data&&h.data.jstree&&("keyup"===b.type&&27===b.which?(f&&clearTimeout(f),c=!1,d=!1,e=!1,f=!1,g.hide().detach(),a.vakata.dnd._clean()):(h.helper.find(".jstree-copy").first()[h.data.origin&&(h.data.origin.settings.dnd.always_copy||h.data.origin.settings.dnd.copy&&(b.metaKey||b.ctrlKey))?"show":"hide"](),e&&(e.metaKey=b.metaKey,e.ctrlKey=b.ctrlKey,a.vakata.dnd._trigger("move",e))))})}),function(a){a.vakata.html={div:a("<div></div>"),escape:function(b){return a.vakata.html.div.text(b).html()},strip:function(b){return a.vakata.html.div.empty().append(a.parseHTML(b)).text()}};var c={element:!1,target:!1,is_down:!1,is_drag:!1,helper:!1,helper_w:0,data:!1,init_x:0,init_y:0,scroll_l:0,scroll_t:0,scroll_e:!1,scroll_i:!1,is_touch:!1};a.vakata.dnd={settings:{scroll_speed:10,scroll_proximity:20,helper_left:5,helper_top:10,threshold:5,threshold_touch:10},_trigger:function(c,d,e){e===b&&(e=a.vakata.dnd._get()),e.event=d,a(i).triggerHandler("dnd_"+c+".vakata",e)},_get:function(){return{data:c.data,element:c.element,helper:c.helper}},_clean:function(){c.helper&&c.helper.remove(),c.scroll_i&&(clearInterval(c.scroll_i),c.scroll_i=!1),c={element:!1,target:!1,is_down:!1,is_drag:!1,helper:!1,helper_w:0,data:!1,init_x:0,init_y:0,scroll_l:0,scroll_t:0,scroll_e:!1,scroll_i:!1,is_touch:!1},n=null,a(i).off("mousemove.vakata.jstree touchmove.vakata.jstree",a.vakata.dnd.drag),a(i).off("mouseup.vakata.jstree touchend.vakata.jstree",a.vakata.dnd.stop)},_scroll:function(b){if(!c.scroll_e||!c.scroll_l&&!c.scroll_t)return c.scroll_i&&(clearInterval(c.scroll_i),c.scroll_i=!1),!1;if(!c.scroll_i)return c.scroll_i=setInterval(a.vakata.dnd._scroll,100),!1;if(b===!0)return!1;var d=c.scroll_e.scrollTop(),e=c.scroll_e.scrollLeft();c.scroll_e.scrollTop(d+c.scroll_t*a.vakata.dnd.settings.scroll_speed),c.scroll_e.scrollLeft(e+c.scroll_l*a.vakata.dnd.settings.scroll_speed),(d!==c.scroll_e.scrollTop()||e!==c.scroll_e.scrollLeft())&&a.vakata.dnd._trigger("scroll",c.scroll_e)},start:function(b,d,e){"touchstart"===b.type&&b.originalEvent&&b.originalEvent.changedTouches&&b.originalEvent.changedTouches[0]&&(b.pageX=b.originalEvent.changedTouches[0].pageX,b.pageY=b.originalEvent.changedTouches[0].pageY,b.target=i.elementFromPoint(b.originalEvent.changedTouches[0].pageX-window.pageXOffset,b.originalEvent.changedTouches[0].pageY-window.pageYOffset)),c.is_drag&&a.vakata.dnd.stop({});try{b.currentTarget.unselectable="on",b.currentTarget.onselectstart=function(){return!1},b.currentTarget.style&&(b.currentTarget.style.touchAction="none",b.currentTarget.style.msTouchAction="none",b.currentTarget.style.MozUserSelect="none")}catch(f){}return c.init_x=b.pageX,c.init_y=b.pageY,c.data=d,c.is_down=!0,c.element=b.currentTarget,c.target=b.target,c.is_touch="touchstart"===b.type,e!==!1&&(c.helper=a("<div id='vakata-dnd'></div>").html(e).css({display:"block",margin:"0",padding:"0",position:"absolute",top:"-2000px",lineHeight:"16px",zIndex:"10000"})),a(i).on("mousemove.vakata.jstree touchmove.vakata.jstree",a.vakata.dnd.drag),a(i).on("mouseup.vakata.jstree touchend.vakata.jstree",a.vakata.dnd.stop),!1},drag:function(b){if("touchmove"===b.type&&b.originalEvent&&b.originalEvent.changedTouches&&b.originalEvent.changedTouches[0]&&(b.pageX=b.originalEvent.changedTouches[0].pageX,b.pageY=b.originalEvent.changedTouches[0].pageY,b.target=i.elementFromPoint(b.originalEvent.changedTouches[0].pageX-window.pageXOffset,b.originalEvent.changedTouches[0].pageY-window.pageYOffset)),c.is_down){if(!c.is_drag){if(!(Math.abs(b.pageX-c.init_x)>(c.is_touch?a.vakata.dnd.settings.threshold_touch:a.vakata.dnd.settings.threshold)||Math.abs(b.pageY-c.init_y)>(c.is_touch?a.vakata.dnd.settings.threshold_touch:a.vakata.dnd.settings.threshold)))return;c.helper&&(c.helper.appendTo(i.body),c.helper_w=c.helper.outerWidth()),c.is_drag=!0,a(c.target).one("click.vakata",!1),a.vakata.dnd._trigger("start",b)}var d=!1,e=!1,f=!1,g=!1,h=!1,j=!1,k=!1,l=!1,m=!1,n=!1;return c.scroll_t=0,c.scroll_l=0,c.scroll_e=!1,a(a(b.target).parentsUntil("body").addBack().get().reverse()).filter(function(){return/^auto|scroll$/.test(a(this).css("overflow"))&&(this.scrollHeight>this.offsetHeight||this.scrollWidth>this.offsetWidth)}).each(function(){var d=a(this),e=d.offset();return this.scrollHeight>this.offsetHeight&&(e.top+d.height()-b.pageY<a.vakata.dnd.settings.scroll_proximity&&(c.scroll_t=1),b.pageY-e.top<a.vakata.dnd.settings.scroll_proximity&&(c.scroll_t=-1)),this.scrollWidth>this.offsetWidth&&(e.left+d.width()-b.pageX<a.vakata.dnd.settings.scroll_proximity&&(c.scroll_l=1),b.pageX-e.left<a.vakata.dnd.settings.scroll_proximity&&(c.scroll_l=-1)),c.scroll_t||c.scroll_l?(c.scroll_e=a(this),!1):void 0}),c.scroll_e||(d=a(i),e=a(window),f=d.height(),g=e.height(),h=d.width(),j=e.width(),k=d.scrollTop(),l=d.scrollLeft(),f>g&&b.pageY-k<a.vakata.dnd.settings.scroll_proximity&&(c.scroll_t=-1),f>g&&g-(b.pageY-k)<a.vakata.dnd.settings.scroll_proximity&&(c.scroll_t=1),h>j&&b.pageX-l<a.vakata.dnd.settings.scroll_proximity&&(c.scroll_l=-1),h>j&&j-(b.pageX-l)<a.vakata.dnd.settings.scroll_proximity&&(c.scroll_l=1),(c.scroll_t||c.scroll_l)&&(c.scroll_e=d)),c.scroll_e&&a.vakata.dnd._scroll(!0),c.helper&&(m=parseInt(b.pageY+a.vakata.dnd.settings.helper_top,10),n=parseInt(b.pageX+a.vakata.dnd.settings.helper_left,10),f&&m+25>f&&(m=f-50),h&&n+c.helper_w>h&&(n=h-(c.helper_w+2)),c.helper.css({left:n+"px",top:m+"px"})),a.vakata.dnd._trigger("move",b),!1}},stop:function(b){if("touchend"===b.type&&b.originalEvent&&b.originalEvent.changedTouches&&b.originalEvent.changedTouches[0]&&(b.pageX=b.originalEvent.changedTouches[0].pageX,b.pageY=b.originalEvent.changedTouches[0].pageY,b.target=i.elementFromPoint(b.originalEvent.changedTouches[0].pageX-window.pageXOffset,b.originalEvent.changedTouches[0].pageY-window.pageYOffset)),c.is_drag)b.target!==c.target&&a(c.target).off("click.vakata"),a.vakata.dnd._trigger("stop",b);else if("touchend"===b.type&&b.target===c.target){var d=setTimeout(function(){a(b.target).trigger("click")},100);a(b.target).one("click",function(){d&&clearTimeout(d)})}return a.vakata.dnd._clean(),!1}}}(a),a.jstree.defaults.massload=null,a.jstree.plugins.massload=function(b,c){this.init=function(a,b){this._data.massload={},c.init.call(this,a,b)},this._load_nodes=function(b,d,e,f){var g=this.settings.massload,h=[],i=this._model.data,j,k,l;if(!e){for(j=0,k=b.length;k>j;j++)(!i[b[j]]||!i[b[j]].state.loaded&&!i[b[j]].state.failed||f)&&(h.push(b[j]),l=this.get_node(b[j],!0),l&&l.length&&l.addClass("jstree-loading").attr("aria-busy",!0));if(this._data.massload={},h.length){if(a.vakata.is_function(g))return g.call(this,h,function(a){var g,h;if(a)for(g in a)a.hasOwnProperty(g)&&(this._data.massload[g]=a[g]);for(g=0,h=b.length;h>g;g++)l=this.get_node(b[g],!0),l&&l.length&&l.removeClass("jstree-loading").attr("aria-busy",!1);c._load_nodes.call(this,b,d,e,f)}.bind(this));if("object"==typeof g&&g&&g.url)return g=a.extend(!0,{},g),a.vakata.is_function(g.url)&&(g.url=g.url.call(this,h)),a.vakata.is_function(g.data)&&(g.data=g.data.call(this,h)),a.ajax(g).done(function(a,g,h){var i,j;if(a)for(i in a)a.hasOwnProperty(i)&&(this._data.massload[i]=a[i]);for(i=0,j=b.length;j>i;i++)l=this.get_node(b[i],!0),l&&l.length&&l.removeClass("jstree-loading").attr("aria-busy",!1);c._load_nodes.call(this,b,d,e,f)}.bind(this)).fail(function(a){c._load_nodes.call(this,b,d,e,f)}.bind(this))}}return c._load_nodes.call(this,b,d,e,f)},this._load_node=function(b,d){var e=this._data.massload[b.id],f=null,g;return e?(f=this["string"==typeof e?"_append_html_data":"_append_json_data"](b,"string"==typeof e?a(a.parseHTML(e)).filter(function(){return 3!==this.nodeType}):e,function(a){d.call(this,a)}),g=this.get_node(b.id,!0),g&&g.length&&g.removeClass("jstree-loading").attr("aria-busy",!1),delete this._data.massload[b.id],f):c._load_node.call(this,b,d)}},a.jstree.defaults.search={ajax:!1,fuzzy:!1,case_sensitive:!1,show_only_matches:!1,show_only_matches_children:!1,close_opened_onclear:!0,search_leaves_only:!1,search_callback:!1},a.jstree.plugins.search=function(c,d){this.bind=function(){d.bind.call(this),this._data.search.str="",this._data.search.dom=a(),this._data.search.res=[],this._data.search.opn=[],this._data.search.som=!1,this._data.search.smc=!1,this._data.search.hdn=[],this.element.on("search.jstree",function(b,c){if(this._data.search.som&&c.res.length){var d=this._model.data,e,f,g=[],h,i;for(e=0,f=c.res.length;f>e;e++)if(d[c.res[e]]&&!d[c.res[e]].state.hidden&&(g.push(c.res[e]),g=g.concat(d[c.res[e]].parents),this._data.search.smc))for(h=0,i=d[c.res[e]].children_d.length;i>h;h++)d[d[c.res[e]].children_d[h]]&&!d[d[c.res[e]].children_d[h]].state.hidden&&g.push(d[c.res[e]].children_d[h]);g=a.vakata.array_remove_item(a.vakata.array_unique(g),a.jstree.root),this._data.search.hdn=this.hide_all(!0),this.show_node(g,!0),this.redraw(!0)}}.bind(this)).on("clear_search.jstree",function(a,b){this._data.search.som&&b.res.length&&(this.show_node(this._data.search.hdn,!0),this.redraw(!0))}.bind(this))},this.search=function(c,d,e,f,g,h){if(c===!1||""===a.vakata.trim(c.toString()))return this.clear_search();f=this.get_node(f),f=f&&f.id?f.id:null,c=c.toString();var i=this.settings.search,j=i.ajax?i.ajax:!1,k=this._model.data,l=null,m=[],n=[],o,p;if(this._data.search.res.length&&!g&&this.clear_search(),e===b&&(e=i.show_only_matches),h===b&&(h=i.show_only_matches_children),!d&&j!==!1)return a.vakata.is_function(j)?j.call(this,c,function(b){b&&b.d&&(b=b.d),this._load_nodes(a.vakata.is_array(b)?a.vakata.array_unique(b):[],function(){this.search(c,!0,e,f,g,h)})}.bind(this),f):(j=a.extend({},j),j.data||(j.data={}),j.data.str=c,f&&(j.data.inside=f),this._data.search.lastRequest&&this._data.search.lastRequest.abort(),this._data.search.lastRequest=a.ajax(j).fail(function(){this._data.core.last_error={error:"ajax",plugin:"search",id:"search_01",reason:"Could not load search parents",data:JSON.stringify(j)},this.settings.core.error.call(this,this._data.core.last_error)}.bind(this)).done(function(b){b&&b.d&&(b=b.d),this._load_nodes(a.vakata.is_array(b)?a.vakata.array_unique(b):[],function(){this.search(c,!0,e,f,g,h)})}.bind(this)),this._data.search.lastRequest);if(g||(this._data.search.str=c,this._data.search.dom=a(),this._data.search.res=[],this._data.search.opn=[],this._data.search.som=e,this._data.search.smc=h),l=new a.vakata.search(c,!0,{caseSensitive:i.case_sensitive,fuzzy:i.fuzzy}),a.each(k[f?f:a.jstree.root].children_d,function(a,b){var d=k[b];d.text&&!d.state.hidden&&(!i.search_leaves_only||d.state.loaded&&0===d.children.length)&&(i.search_callback&&i.search_callback.call(this,c,d)||!i.search_callback&&l.search(d.text).isMatch)&&(m.push(b),n=n.concat(d.parents))}),m.length){for(n=a.vakata.array_unique(n),o=0,p=n.length;p>o;o++)n[o]!==a.jstree.root&&k[n[o]]&&this.open_node(n[o],null,0)===!0&&this._data.search.opn.push(n[o]);g?(this._data.search.dom=this._data.search.dom.add(a(this.element[0].querySelectorAll("#"+a.map(m,function(b){return-1!=="0123456789".indexOf(b[0])?"\\3"+b[0]+" "+b.substr(1).replace(a.jstree.idregex,"\\$&"):b.replace(a.jstree.idregex,"\\$&")}).join(", #")))),this._data.search.res=a.vakata.array_unique(this._data.search.res.concat(m))):(this._data.search.dom=a(this.element[0].querySelectorAll("#"+a.map(m,function(b){return-1!=="0123456789".indexOf(b[0])?"\\3"+b[0]+" "+b.substr(1).replace(a.jstree.idregex,"\\$&"):b.replace(a.jstree.idregex,"\\$&")}).join(", #"))),this._data.search.res=m),this._data.search.dom.children(".jstree-anchor").addClass("jstree-search")}this.trigger("search",{nodes:this._data.search.dom,str:c,res:this._data.search.res,show_only_matches:e})},this.clear_search=function(){this.settings.search.close_opened_onclear&&this.close_node(this._data.search.opn,0),this.trigger("clear_search",{nodes:this._data.search.dom,str:this._data.search.str,res:this._data.search.res}),this._data.search.res.length&&(this._data.search.dom=a(this.element[0].querySelectorAll("#"+a.map(this._data.search.res,function(b){return-1!=="0123456789".indexOf(b[0])?"\\3"+b[0]+" "+b.substr(1).replace(a.jstree.idregex,"\\$&"):b.replace(a.jstree.idregex,"\\$&")}).join(", #"))),this._data.search.dom.children(".jstree-anchor").removeClass("jstree-search")),this._data.search.str="",this._data.search.res=[],this._data.search.opn=[],this._data.search.dom=a()},this.redraw_node=function(b,c,e,f){if(b=d.redraw_node.apply(this,arguments),b&&-1!==a.inArray(b.id,this._data.search.res)){var g,h,i=null;for(g=0,h=b.childNodes.length;h>g;g++)if(b.childNodes[g]&&b.childNodes[g].className&&-1!==b.childNodes[g].className.indexOf("jstree-anchor")){i=b.childNodes[g];break}i&&(i.className+=" jstree-search")}return b}},function(a){a.vakata.search=function(b,c,d){d=d||{},d=a.extend({},a.vakata.search.defaults,d),d.fuzzy!==!1&&(d.fuzzy=!0),b=d.caseSensitive?b:b.toLowerCase();var e=d.location,f=d.distance,g=d.threshold,h=b.length,i,j,k,l;return h>32&&(d.fuzzy=!1),d.fuzzy&&(i=1<<h-1,j=function(){var a={},c=0;for(c=0;h>c;c++)a[b.charAt(c)]=0;for(c=0;h>c;c++)a[b.charAt(c)]|=1<<h-c-1;return a}(),k=function(a,b){var c=a/h,d=Math.abs(e-b);return f?c+d/f:d?1:c}),l=function(a){if(a=d.caseSensitive?a:a.toLowerCase(),b===a||-1!==a.indexOf(b))return{isMatch:!0,score:0};if(!d.fuzzy)return{isMatch:!1,score:1};var c,f,l=a.length,m=g,n=a.indexOf(b,e),o,p,q=h+l,r,s,t,u,v,w=1,x=[];for(-1!==n&&(m=Math.min(k(0,n),m),n=a.lastIndexOf(b,e+h),-1!==n&&(m=Math.min(k(0,n),m))),n=-1,c=0;h>c;c++){o=0,p=q;while(p>o)k(c,e+p)<=m?o=p:q=p,p=Math.floor((q-o)/2+o);for(q=p,s=Math.max(1,e-p+1),t=Math.min(e+p,l)+h,u=new Array(t+2),u[t+1]=(1<<c)-1,f=t;f>=s;f--)if(v=j[a.charAt(f-1)],0===c?u[f]=(u[f+1]<<1|1)&v:u[f]=(u[f+1]<<1|1)&v|((r[f+1]|r[f])<<1|1)|r[f+1],u[f]&i&&(w=k(c,f-1),m>=w)){if(m=w,n=f-1,x.push(n),!(n>e))break;s=Math.max(1,2*e-n)}if(k(c+1,e)>m)break;r=u}return{isMatch:n>=0,score:w}},c===!0?{search:l}:l(c)},a.vakata.search.defaults={location:0,distance:100,threshold:.6,fuzzy:!1,caseSensitive:!1}}(a),a.jstree.defaults.sort=function(a,b){return this.get_text(a)>this.get_text(b)?1:-1},a.jstree.plugins.sort=function(a,b){this.bind=function(){b.bind.call(this),this.element.on("model.jstree",function(a,b){this.sort(b.parent,!0)}.bind(this)).on("rename_node.jstree create_node.jstree",function(a,b){this.sort(b.parent||b.node.parent,!1),this.redraw_node(b.parent||b.node.parent,!0)}.bind(this)).on("move_node.jstree copy_node.jstree",function(a,b){this.sort(b.parent,!1),this.redraw_node(b.parent,!0)}.bind(this))},this.sort=function(a,b){var c,d;if(a=this.get_node(a),a&&a.children&&a.children.length&&(a.children.sort(this.settings.sort.bind(this)),b))for(c=0,d=a.children_d.length;d>c;c++)this.sort(a.children_d[c],!1)}};var o=!1;a.jstree.defaults.state={key:"jstree",events:"changed.jstree open_node.jstree close_node.jstree check_node.jstree uncheck_node.jstree",ttl:!1,filter:!1,preserve_loaded:!1},a.jstree.plugins.state=function(b,c){this.bind=function(){c.bind.call(this);var a=function(){this.element.on(this.settings.state.events,function(){o&&clearTimeout(o),o=setTimeout(function(){this.save_state()}.bind(this),100)}.bind(this)),this.trigger("state_ready")}.bind(this);this.element.on("ready.jstree",function(b,c){this.element.one("restore_state.jstree",a),this.restore_state()||a()}.bind(this))},this.save_state=function(){var b=this.get_state();this.settings.state.preserve_loaded||delete b.core.loaded;var c={state:b,ttl:this.settings.state.ttl,sec:+new Date};a.vakata.storage.set(this.settings.state.key,JSON.stringify(c))},this.restore_state=function(){var b=a.vakata.storage.get(this.settings.state.key);if(b)try{b=JSON.parse(b)}catch(c){return!1}return b&&b.ttl&&b.sec&&+new Date-b.sec>b.ttl?!1:(b&&b.state&&(b=b.state),b&&a.vakata.is_function(this.settings.state.filter)&&(b=this.settings.state.filter.call(this,b)),b?(this.settings.state.preserve_loaded||delete b.core.loaded,this.element.one("set_state.jstree",function(c,d){d.instance.trigger("restore_state",{state:a.extend(!0,{},b)})}),this.set_state(b),!0):!1)},this.clear_state=function(){return a.vakata.storage.del(this.settings.state.key)}},function(a,b){a.vakata.storage={set:function(a,b){return window.localStorage.setItem(a,b)},get:function(a){return window.localStorage.getItem(a)},del:function(a){return window.localStorage.removeItem(a)}}}(a),a.jstree.defaults.types={
"default":{}},a.jstree.defaults.types[a.jstree.root]={},a.jstree.plugins.types=function(c,d){this.init=function(c,e){var f,g;if(e&&e.types&&e.types["default"])for(f in e.types)if("default"!==f&&f!==a.jstree.root&&e.types.hasOwnProperty(f))for(g in e.types["default"])e.types["default"].hasOwnProperty(g)&&e.types[f][g]===b&&(e.types[f][g]=e.types["default"][g]);d.init.call(this,c,e),this._model.data[a.jstree.root].type=a.jstree.root},this.refresh=function(b,c){d.refresh.call(this,b,c),this._model.data[a.jstree.root].type=a.jstree.root},this.bind=function(){this.element.on("model.jstree",function(c,d){var e=this._model.data,f=d.nodes,g=this.settings.types,h,i,j="default",k;for(h=0,i=f.length;i>h;h++){if(j="default",e[f[h]].original&&e[f[h]].original.type&&g[e[f[h]].original.type]&&(j=e[f[h]].original.type),e[f[h]].data&&e[f[h]].data.jstree&&e[f[h]].data.jstree.type&&g[e[f[h]].data.jstree.type]&&(j=e[f[h]].data.jstree.type),e[f[h]].type=j,e[f[h]].icon===!0&&g[j].icon!==b&&(e[f[h]].icon=g[j].icon),g[j].li_attr!==b&&"object"==typeof g[j].li_attr)for(k in g[j].li_attr)if(g[j].li_attr.hasOwnProperty(k)){if("id"===k)continue;e[f[h]].li_attr[k]===b?e[f[h]].li_attr[k]=g[j].li_attr[k]:"class"===k&&(e[f[h]].li_attr["class"]=g[j].li_attr["class"]+" "+e[f[h]].li_attr["class"])}if(g[j].a_attr!==b&&"object"==typeof g[j].a_attr)for(k in g[j].a_attr)if(g[j].a_attr.hasOwnProperty(k)){if("id"===k)continue;e[f[h]].a_attr[k]===b?e[f[h]].a_attr[k]=g[j].a_attr[k]:"href"===k&&"#"===e[f[h]].a_attr[k]?e[f[h]].a_attr.href=g[j].a_attr.href:"class"===k&&(e[f[h]].a_attr["class"]=g[j].a_attr["class"]+" "+e[f[h]].a_attr["class"])}}e[a.jstree.root].type=a.jstree.root}.bind(this)),d.bind.call(this)},this.get_json=function(b,c,e){var f,g,h=this._model.data,i=c?a.extend(!0,{},c,{no_id:!1}):{},j=d.get_json.call(this,b,i,e);if(j===!1)return!1;if(a.vakata.is_array(j))for(f=0,g=j.length;g>f;f++)j[f].type=j[f].id&&h[j[f].id]&&h[j[f].id].type?h[j[f].id].type:"default",c&&c.no_id&&(delete j[f].id,j[f].li_attr&&j[f].li_attr.id&&delete j[f].li_attr.id,j[f].a_attr&&j[f].a_attr.id&&delete j[f].a_attr.id);else j.type=j.id&&h[j.id]&&h[j.id].type?h[j.id].type:"default",c&&c.no_id&&(j=this._delete_ids(j));return j},this._delete_ids=function(b){if(a.vakata.is_array(b)){for(var c=0,d=b.length;d>c;c++)b[c]=this._delete_ids(b[c]);return b}return delete b.id,b.li_attr&&b.li_attr.id&&delete b.li_attr.id,b.a_attr&&b.a_attr.id&&delete b.a_attr.id,b.children&&a.vakata.is_array(b.children)&&(b.children=this._delete_ids(b.children)),b},this.check=function(c,e,f,g,h){if(d.check.call(this,c,e,f,g,h)===!1)return!1;e=e&&e.id?e:this.get_node(e),f=f&&f.id?f:this.get_node(f);var i=e&&e.id?h&&h.origin?h.origin:a.jstree.reference(e.id):null,j,k,l,m;switch(i=i&&i._model&&i._model.data?i._model.data:null,c){case"create_node":case"move_node":case"copy_node":if("move_node"!==c||-1===a.inArray(e.id,f.children)){if(j=this.get_rules(f),j.max_children!==b&&-1!==j.max_children&&j.max_children===f.children.length)return this._data.core.last_error={error:"check",plugin:"types",id:"types_01",reason:"max_children prevents function: "+c,data:JSON.stringify({chk:c,pos:g,obj:e&&e.id?e.id:!1,par:f&&f.id?f.id:!1})},!1;if(j.valid_children!==b&&-1!==j.valid_children&&-1===a.inArray(e.type||"default",j.valid_children))return this._data.core.last_error={error:"check",plugin:"types",id:"types_02",reason:"valid_children prevents function: "+c,data:JSON.stringify({chk:c,pos:g,obj:e&&e.id?e.id:!1,par:f&&f.id?f.id:!1})},!1;if(i&&e.children_d&&e.parents){for(k=0,l=0,m=e.children_d.length;m>l;l++)k=Math.max(k,i[e.children_d[l]].parents.length);k=k-e.parents.length+1}(0>=k||k===b)&&(k=1);do{if(j.max_depth!==b&&-1!==j.max_depth&&j.max_depth<k)return this._data.core.last_error={error:"check",plugin:"types",id:"types_03",reason:"max_depth prevents function: "+c,data:JSON.stringify({chk:c,pos:g,obj:e&&e.id?e.id:!1,par:f&&f.id?f.id:!1})},!1;f=this.get_node(f.parent),j=this.get_rules(f),k++}while(f)}}return!0},this.get_rules=function(a){if(a=this.get_node(a),!a)return!1;var c=this.get_type(a,!0);return c.max_depth===b&&(c.max_depth=-1),c.max_children===b&&(c.max_children=-1),c.valid_children===b&&(c.valid_children=-1),c},this.get_type=function(b,c){return b=this.get_node(b),b?c?a.extend({type:b.type},this.settings.types[b.type]):b.type:!1},this.set_type=function(c,d){var e=this._model.data,f,g,h,i,j,k,l,m;if(a.vakata.is_array(c)){for(c=c.slice(),g=0,h=c.length;h>g;g++)this.set_type(c[g],d);return!0}if(f=this.settings.types,c=this.get_node(c),!f[d]||!c)return!1;if(l=this.get_node(c,!0),l&&l.length&&(m=l.children(".jstree-anchor")),i=c.type,j=this.get_icon(c),c.type=d,(j===!0||!f[i]||f[i].icon!==b&&j===f[i].icon)&&this.set_icon(c,f[d].icon!==b?f[d].icon:!0),f[i]&&f[i].li_attr!==b&&"object"==typeof f[i].li_attr)for(k in f[i].li_attr)if(f[i].li_attr.hasOwnProperty(k)){if("id"===k)continue;"class"===k?(e[c.id].li_attr["class"]=(e[c.id].li_attr["class"]||"").replace(f[i].li_attr[k],""),l&&l.removeClass(f[i].li_attr[k])):e[c.id].li_attr[k]===f[i].li_attr[k]&&(e[c.id].li_attr[k]=null,l&&l.removeAttr(k))}if(f[i]&&f[i].a_attr!==b&&"object"==typeof f[i].a_attr)for(k in f[i].a_attr)if(f[i].a_attr.hasOwnProperty(k)){if("id"===k)continue;"class"===k?(e[c.id].a_attr["class"]=(e[c.id].a_attr["class"]||"").replace(f[i].a_attr[k],""),m&&m.removeClass(f[i].a_attr[k])):e[c.id].a_attr[k]===f[i].a_attr[k]&&("href"===k?(e[c.id].a_attr[k]="#",m&&m.attr("href","#")):(delete e[c.id].a_attr[k],m&&m.removeAttr(k)))}if(f[d].li_attr!==b&&"object"==typeof f[d].li_attr)for(k in f[d].li_attr)if(f[d].li_attr.hasOwnProperty(k)){if("id"===k)continue;e[c.id].li_attr[k]===b?(e[c.id].li_attr[k]=f[d].li_attr[k],l&&("class"===k?l.addClass(f[d].li_attr[k]):l.attr(k,f[d].li_attr[k]))):"class"===k&&(e[c.id].li_attr["class"]=f[d].li_attr[k]+" "+e[c.id].li_attr["class"],l&&l.addClass(f[d].li_attr[k]))}if(f[d].a_attr!==b&&"object"==typeof f[d].a_attr)for(k in f[d].a_attr)if(f[d].a_attr.hasOwnProperty(k)){if("id"===k)continue;e[c.id].a_attr[k]===b?(e[c.id].a_attr[k]=f[d].a_attr[k],m&&("class"===k?m.addClass(f[d].a_attr[k]):m.attr(k,f[d].a_attr[k]))):"href"===k&&"#"===e[c.id].a_attr[k]?(e[c.id].a_attr.href=f[d].a_attr.href,m&&m.attr("href",f[d].a_attr.href)):"class"===k&&(e[c.id].a_attr["class"]=f[d].a_attr["class"]+" "+e[c.id].a_attr["class"],m&&m.addClass(f[d].a_attr[k]))}return!0}},a.jstree.defaults.unique={case_sensitive:!1,trim_whitespace:!1,duplicate:function(a,b){return a+" ("+b+")"}},a.jstree.plugins.unique=function(c,d){this.check=function(b,c,e,f,g){if(d.check.call(this,b,c,e,f,g)===!1)return!1;if(c=c&&c.id?c:this.get_node(c),e=e&&e.id?e:this.get_node(e),!e||!e.children)return!0;var h="rename_node"===b?f:c.text,i=[],j=this.settings.unique.case_sensitive,k=this.settings.unique.trim_whitespace,l=this._model.data,m,n,o;for(m=0,n=e.children.length;n>m;m++)o=l[e.children[m]].text,j||(o=o.toLowerCase()),k&&(o=o.replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g,"")),i.push(o);switch(j||(h=h.toLowerCase()),k&&(h=h.replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g,"")),b){case"delete_node":return!0;case"rename_node":return o=c.text||"",j||(o=o.toLowerCase()),k&&(o=o.replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g,"")),m=-1===a.inArray(h,i)||c.text&&o===h,m||(this._data.core.last_error={error:"check",plugin:"unique",id:"unique_01",reason:"Child with name "+h+" already exists. Preventing: "+b,data:JSON.stringify({chk:b,pos:f,obj:c&&c.id?c.id:!1,par:e&&e.id?e.id:!1})}),m;case"create_node":return m=-1===a.inArray(h,i),m||(this._data.core.last_error={error:"check",plugin:"unique",id:"unique_04",reason:"Child with name "+h+" already exists. Preventing: "+b,data:JSON.stringify({chk:b,pos:f,obj:c&&c.id?c.id:!1,par:e&&e.id?e.id:!1})}),m;case"copy_node":return m=-1===a.inArray(h,i),m||(this._data.core.last_error={error:"check",plugin:"unique",id:"unique_02",reason:"Child with name "+h+" already exists. Preventing: "+b,data:JSON.stringify({chk:b,pos:f,obj:c&&c.id?c.id:!1,par:e&&e.id?e.id:!1})}),m;case"move_node":return m=c.parent===e.id&&(!g||!g.is_multi)||-1===a.inArray(h,i),m||(this._data.core.last_error={error:"check",plugin:"unique",id:"unique_03",reason:"Child with name "+h+" already exists. Preventing: "+b,data:JSON.stringify({chk:b,pos:f,obj:c&&c.id?c.id:!1,par:e&&e.id?e.id:!1})}),m}return!0},this.create_node=function(c,e,f,g,h){if(!e||e.text===b){if(null===c&&(c=a.jstree.root),c=this.get_node(c),!c)return d.create_node.call(this,c,e,f,g,h);if(f=f===b?"last":f,!f.toString().match(/^(before|after)$/)&&!h&&!this.is_loaded(c))return d.create_node.call(this,c,e,f,g,h);e||(e={});var i,j,k,l,m,n=this._model.data,o=this.settings.unique.case_sensitive,p=this.settings.unique.trim_whitespace,q=this.settings.unique.duplicate,r;for(j=i=this.get_string("New node"),k=[],l=0,m=c.children.length;m>l;l++)r=n[c.children[l]].text,o||(r=r.toLowerCase()),p&&(r=r.replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g,"")),k.push(r);l=1,r=j,o||(r=r.toLowerCase()),p&&(r=r.replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g,""));while(-1!==a.inArray(r,k))j=q.call(this,i,++l).toString(),r=j,o||(r=r.toLowerCase()),p&&(r=r.replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g,""));e.text=j}return d.create_node.call(this,c,e,f,g,h)}};var p=i.createElement("DIV");if(p.setAttribute("unselectable","on"),p.setAttribute("role","presentation"),p.className="jstree-wholerow",p.innerHTML="&#160;",a.jstree.plugins.wholerow=function(b,c){this.bind=function(){c.bind.call(this),this.element.on("ready.jstree set_state.jstree",function(){this.hide_dots()}.bind(this)).on("init.jstree loading.jstree ready.jstree",function(){this.get_container_ul().addClass("jstree-wholerow-ul")}.bind(this)).on("deselect_all.jstree",function(a,b){this.element.find(".jstree-wholerow-clicked").removeClass("jstree-wholerow-clicked")}.bind(this)).on("changed.jstree",function(a,b){this.element.find(".jstree-wholerow-clicked").removeClass("jstree-wholerow-clicked");var c=!1,d,e;for(d=0,e=b.selected.length;e>d;d++)c=this.get_node(b.selected[d],!0),c&&c.length&&c.children(".jstree-wholerow").addClass("jstree-wholerow-clicked")}.bind(this)).on("open_node.jstree",function(a,b){this.get_node(b.node,!0).find(".jstree-clicked").parent().children(".jstree-wholerow").addClass("jstree-wholerow-clicked")}.bind(this)).on("hover_node.jstree dehover_node.jstree",function(a,b){"hover_node"===a.type&&this.is_disabled(b.node)||this.get_node(b.node,!0).children(".jstree-wholerow")["hover_node"===a.type?"addClass":"removeClass"]("jstree-wholerow-hovered")}.bind(this)).on("contextmenu.jstree",".jstree-wholerow",function(b){if(this._data.contextmenu){b.preventDefault();var c=a.Event("contextmenu",{metaKey:b.metaKey,ctrlKey:b.ctrlKey,altKey:b.altKey,shiftKey:b.shiftKey,pageX:b.pageX,pageY:b.pageY});a(b.currentTarget).closest(".jstree-node").children(".jstree-anchor").first().trigger(c)}}.bind(this)).on("click.jstree",".jstree-wholerow",function(b){b.stopImmediatePropagation();var c=a.Event("click",{metaKey:b.metaKey,ctrlKey:b.ctrlKey,altKey:b.altKey,shiftKey:b.shiftKey});a(b.currentTarget).closest(".jstree-node").children(".jstree-anchor").first().trigger(c).trigger("focus")}).on("dblclick.jstree",".jstree-wholerow",function(b){b.stopImmediatePropagation();var c=a.Event("dblclick",{metaKey:b.metaKey,ctrlKey:b.ctrlKey,altKey:b.altKey,shiftKey:b.shiftKey});a(b.currentTarget).closest(".jstree-node").children(".jstree-anchor").first().trigger(c).trigger("focus")}).on("click.jstree",".jstree-leaf > .jstree-ocl",function(b){b.stopImmediatePropagation();var c=a.Event("click",{metaKey:b.metaKey,ctrlKey:b.ctrlKey,altKey:b.altKey,shiftKey:b.shiftKey});a(b.currentTarget).closest(".jstree-node").children(".jstree-anchor").first().trigger(c).trigger("focus")}.bind(this)).on("mouseover.jstree",".jstree-wholerow, .jstree-icon",function(a){return a.stopImmediatePropagation(),this.is_disabled(a.currentTarget)||this.hover_node(a.currentTarget),!1}.bind(this)).on("mouseleave.jstree",".jstree-node",function(a){this.dehover_node(a.currentTarget)}.bind(this))},this.teardown=function(){this.settings.wholerow&&this.element.find(".jstree-wholerow").remove(),c.teardown.call(this)},this.redraw_node=function(b,d,e,f){if(b=c.redraw_node.apply(this,arguments)){var g=p.cloneNode(!0);-1!==a.inArray(b.id,this._data.core.selected)&&(g.className+=" jstree-wholerow-clicked"),this._data.core.focused&&this._data.core.focused===b.id&&(g.className+=" jstree-wholerow-hovered"),b.insertBefore(g,b.childNodes[0])}return b}},window.customElements&&Object&&Object.create){var q=Object.create(HTMLElement.prototype);q.createdCallback=function(){var b={core:{},plugins:[]},c;for(c in a.jstree.plugins)a.jstree.plugins.hasOwnProperty(c)&&this.attributes[c]&&(b.plugins.push(c),this.getAttribute(c)&&JSON.parse(this.getAttribute(c))&&(b[c]=JSON.parse(this.getAttribute(c))));for(c in a.jstree.defaults.core)a.jstree.defaults.core.hasOwnProperty(c)&&this.attributes[c]&&(b.core[c]=JSON.parse(this.getAttribute(c))||this.getAttribute(c));a(this).jstree(b)};try{window.customElements.define("vakata-jstree",function(){},{prototype:q})}catch(r){}}}});
/*
 * A JavaScript implementation of the RSA Data Security, Inc. MD5 Message
 * Digest Algorithm, as defined in RFC 1321.
 * Version 2.2 Copyright (C) Paul Johnston 1999 - 2009
 * Other contributors: Greg Holt, Andrew Kepert, Ydnar, Lostinet
 * Distributed under the BSD License
 * See http://pajhome.org.uk/crypt/md5 for more info.
 */

/*
 * Configurable variables. You may need to tweak these to be compatible with
 * the server-side, but the defaults work in most cases.
 */
var hexcase = 0;   /* hex output format. 0 - lowercase; 1 - uppercase        */
var b64pad = "";  /* base-64 pad character. "=" for strict RFC compliance   */

/*
 * These are the functions you'll usually want to call
 * They take string arguments and return either hex or base-64 encoded strings
 */
function hex_md5(s) { return rstr2hex(rstr_md5(str2rstr_utf8(s))); }
function b64_md5(s) { return rstr2b64(rstr_md5(str2rstr_utf8(s))); }
function any_md5(s, e) { return rstr2any(rstr_md5(str2rstr_utf8(s)), e); }
function hex_hmac_md5(k, d) { return rstr2hex(rstr_hmac_md5(str2rstr_utf8(k), str2rstr_utf8(d))); }
function b64_hmac_md5(k, d) { return rstr2b64(rstr_hmac_md5(str2rstr_utf8(k), str2rstr_utf8(d))); }
function any_hmac_md5(k, d, e) { return rstr2any(rstr_hmac_md5(str2rstr_utf8(k), str2rstr_utf8(d)), e); }

/*
 * Perform a simple self-test to see if the VM is working
 */
function md5_vm_test() {
    return hex_md5("abc").toLowerCase() == "900150983cd24fb0d6963f7d28e17f72";
}

/*
 * Calculate the MD5 of a raw string
 */
function rstr_md5(s) {
    return binl2rstr(binl_md5(rstr2binl(s), s.length * 8));
}

/*
 * Calculate the HMAC-MD5, of a key and some data (raw strings)
 */
function rstr_hmac_md5(key, data) {
    var bkey = rstr2binl(key);
    if (bkey.length > 16) bkey = binl_md5(bkey, key.length * 8);

    var ipad = Array(16), opad = Array(16);
    for (var i = 0; i < 16; i++) {
        ipad[i] = bkey[i] ^ 0x36363636;
        opad[i] = bkey[i] ^ 0x5C5C5C5C;
    }

    var hash = binl_md5(ipad.concat(rstr2binl(data)), 512 + data.length * 8);
    return binl2rstr(binl_md5(opad.concat(hash), 512 + 128));
}

/*
 * Convert a raw string to a hex string
 */
function rstr2hex(input) {
    try { hexcase } catch (e) { hexcase = 0; }
    var hex_tab = hexcase ? "0123456789ABCDEF" : "0123456789abcdef";
    var output = "";
    var x;
    for (var i = 0; i < input.length; i++) {
        x = input.charCodeAt(i);
        output += hex_tab.charAt((x >>> 4) & 0x0F)
            + hex_tab.charAt(x & 0x0F);
    }
    return output;
}

/*
 * Convert a raw string to a base-64 string
 */
function rstr2b64(input) {
    try { b64pad } catch (e) { b64pad = ''; }
    var tab = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
    var output = "";
    var len = input.length;
    for (var i = 0; i < len; i += 3) {
        var triplet = (input.charCodeAt(i) << 16)
            | (i + 1 < len ? input.charCodeAt(i + 1) << 8 : 0)
            | (i + 2 < len ? input.charCodeAt(i + 2) : 0);
        for (var j = 0; j < 4; j++) {
            if (i * 8 + j * 6 > input.length * 8) output += b64pad;
            else output += tab.charAt((triplet >>> 6 * (3 - j)) & 0x3F);
        }
    }
    return output;
}

/*
 * Convert a raw string to an arbitrary string encoding
 */
function rstr2any(input, encoding) {
    var divisor = encoding.length;
    var i, j, q, x, quotient;

    /* Convert to an array of 16-bit big-endian values, forming the dividend */
    var dividend = Array(Math.ceil(input.length / 2));
    for (i = 0; i < dividend.length; i++) {
        dividend[i] = (input.charCodeAt(i * 2) << 8) | input.charCodeAt(i * 2 + 1);
    }

    /*
     * Repeatedly perform a long division. The binary array forms the dividend,
     * the length of the encoding is the divisor. Once computed, the quotient
     * forms the dividend for the next step. All remainders are stored for later
     * use.
     */
    var full_length = Math.ceil(input.length * 8 /
        (Math.log(encoding.length) / Math.log(2)));
    var remainders = Array(full_length);
    for (j = 0; j < full_length; j++) {
        quotient = Array();
        x = 0;
        for (i = 0; i < dividend.length; i++) {
            x = (x << 16) + dividend[i];
            q = Math.floor(x / divisor);
            x -= q * divisor;
            if (quotient.length > 0 || q > 0)
                quotient[quotient.length] = q;
        }
        remainders[j] = x;
        dividend = quotient;
    }

    /* Convert the remainders to the output string */
    var output = "";
    for (i = remainders.length - 1; i >= 0; i--)
        output += encoding.charAt(remainders[i]);

    return output;
}

/*
 * Encode a string as utf-8.
 * For efficiency, this assumes the input is valid utf-16.
 */
function str2rstr_utf8(input) {
    var output = "";
    var i = -1;
    var x, y;

    while (++i < input.length) {
        /* Decode utf-16 surrogate pairs */
        x = input.charCodeAt(i);
        y = i + 1 < input.length ? input.charCodeAt(i + 1) : 0;
        if (0xD800 <= x && x <= 0xDBFF && 0xDC00 <= y && y <= 0xDFFF) {
            x = 0x10000 + ((x & 0x03FF) << 10) + (y & 0x03FF);
            i++;
        }

        /* Encode output as utf-8 */
        if (x <= 0x7F)
            output += String.fromCharCode(x);
        else if (x <= 0x7FF)
            output += String.fromCharCode(0xC0 | ((x >>> 6) & 0x1F),
                0x80 | (x & 0x3F));
        else if (x <= 0xFFFF)
            output += String.fromCharCode(0xE0 | ((x >>> 12) & 0x0F),
                0x80 | ((x >>> 6) & 0x3F),
                0x80 | (x & 0x3F));
        else if (x <= 0x1FFFFF)
            output += String.fromCharCode(0xF0 | ((x >>> 18) & 0x07),
                0x80 | ((x >>> 12) & 0x3F),
                0x80 | ((x >>> 6) & 0x3F),
                0x80 | (x & 0x3F));
    }
    return output;
}

/*
 * Encode a string as utf-16
 */
function str2rstr_utf16le(input) {
    var output = "";
    for (var i = 0; i < input.length; i++)
        output += String.fromCharCode(input.charCodeAt(i) & 0xFF,
            (input.charCodeAt(i) >>> 8) & 0xFF);
    return output;
}

function str2rstr_utf16be(input) {
    var output = "";
    for (var i = 0; i < input.length; i++)
        output += String.fromCharCode((input.charCodeAt(i) >>> 8) & 0xFF,
            input.charCodeAt(i) & 0xFF);
    return output;
}

/*
 * Convert a raw string to an array of little-endian words
 * Characters >255 have their high-byte silently ignored.
 */
function rstr2binl(input) {
    var output = Array(input.length >> 2);
    for (var i = 0; i < output.length; i++)
        output[i] = 0;
    for (var i = 0; i < input.length * 8; i += 8)
        output[i >> 5] |= (input.charCodeAt(i / 8) & 0xFF) << (i % 32);
    return output;
}

/*
 * Convert an array of little-endian words to a string
 */
function binl2rstr(input) {
    var output = "";
    for (var i = 0; i < input.length * 32; i += 8)
        output += String.fromCharCode((input[i >> 5] >>> (i % 32)) & 0xFF);
    return output;
}

/*
 * Calculate the MD5 of an array of little-endian words, and a bit length.
 */
function binl_md5(x, len) {
    /* append padding */
    x[len >> 5] |= 0x80 << ((len) % 32);
    x[(((len + 64) >>> 9) << 4) + 14] = len;

    var a = 1732584193;
    var b = -271733879;
    var c = -1732584194;
    var d = 271733878;

    for (var i = 0; i < x.length; i += 16) {
        var olda = a;
        var oldb = b;
        var oldc = c;
        var oldd = d;

        a = md5_ff(a, b, c, d, x[i + 0], 7, -680876936);
        d = md5_ff(d, a, b, c, x[i + 1], 12, -389564586);
        c = md5_ff(c, d, a, b, x[i + 2], 17, 606105819);
        b = md5_ff(b, c, d, a, x[i + 3], 22, -1044525330);
        a = md5_ff(a, b, c, d, x[i + 4], 7, -176418897);
        d = md5_ff(d, a, b, c, x[i + 5], 12, 1200080426);
        c = md5_ff(c, d, a, b, x[i + 6], 17, -1473231341);
        b = md5_ff(b, c, d, a, x[i + 7], 22, -45705983);
        a = md5_ff(a, b, c, d, x[i + 8], 7, 1770035416);
        d = md5_ff(d, a, b, c, x[i + 9], 12, -1958414417);
        c = md5_ff(c, d, a, b, x[i + 10], 17, -42063);
        b = md5_ff(b, c, d, a, x[i + 11], 22, -1990404162);
        a = md5_ff(a, b, c, d, x[i + 12], 7, 1804603682);
        d = md5_ff(d, a, b, c, x[i + 13], 12, -40341101);
        c = md5_ff(c, d, a, b, x[i + 14], 17, -1502002290);
        b = md5_ff(b, c, d, a, x[i + 15], 22, 1236535329);

        a = md5_gg(a, b, c, d, x[i + 1], 5, -165796510);
        d = md5_gg(d, a, b, c, x[i + 6], 9, -1069501632);
        c = md5_gg(c, d, a, b, x[i + 11], 14, 643717713);
        b = md5_gg(b, c, d, a, x[i + 0], 20, -373897302);
        a = md5_gg(a, b, c, d, x[i + 5], 5, -701558691);
        d = md5_gg(d, a, b, c, x[i + 10], 9, 38016083);
        c = md5_gg(c, d, a, b, x[i + 15], 14, -660478335);
        b = md5_gg(b, c, d, a, x[i + 4], 20, -405537848);
        a = md5_gg(a, b, c, d, x[i + 9], 5, 568446438);
        d = md5_gg(d, a, b, c, x[i + 14], 9, -1019803690);
        c = md5_gg(c, d, a, b, x[i + 3], 14, -187363961);
        b = md5_gg(b, c, d, a, x[i + 8], 20, 1163531501);
        a = md5_gg(a, b, c, d, x[i + 13], 5, -1444681467);
        d = md5_gg(d, a, b, c, x[i + 2], 9, -51403784);
        c = md5_gg(c, d, a, b, x[i + 7], 14, 1735328473);
        b = md5_gg(b, c, d, a, x[i + 12], 20, -1926607734);

        a = md5_hh(a, b, c, d, x[i + 5], 4, -378558);
        d = md5_hh(d, a, b, c, x[i + 8], 11, -2022574463);
        c = md5_hh(c, d, a, b, x[i + 11], 16, 1839030562);
        b = md5_hh(b, c, d, a, x[i + 14], 23, -35309556);
        a = md5_hh(a, b, c, d, x[i + 1], 4, -1530992060);
        d = md5_hh(d, a, b, c, x[i + 4], 11, 1272893353);
        c = md5_hh(c, d, a, b, x[i + 7], 16, -155497632);
        b = md5_hh(b, c, d, a, x[i + 10], 23, -1094730640);
        a = md5_hh(a, b, c, d, x[i + 13], 4, 681279174);
        d = md5_hh(d, a, b, c, x[i + 0], 11, -358537222);
        c = md5_hh(c, d, a, b, x[i + 3], 16, -722521979);
        b = md5_hh(b, c, d, a, x[i + 6], 23, 76029189);
        a = md5_hh(a, b, c, d, x[i + 9], 4, -640364487);
        d = md5_hh(d, a, b, c, x[i + 12], 11, -421815835);
        c = md5_hh(c, d, a, b, x[i + 15], 16, 530742520);
        b = md5_hh(b, c, d, a, x[i + 2], 23, -995338651);

        a = md5_ii(a, b, c, d, x[i + 0], 6, -198630844);
        d = md5_ii(d, a, b, c, x[i + 7], 10, 1126891415);
        c = md5_ii(c, d, a, b, x[i + 14], 15, -1416354905);
        b = md5_ii(b, c, d, a, x[i + 5], 21, -57434055);
        a = md5_ii(a, b, c, d, x[i + 12], 6, 1700485571);
        d = md5_ii(d, a, b, c, x[i + 3], 10, -1894986606);
        c = md5_ii(c, d, a, b, x[i + 10], 15, -1051523);
        b = md5_ii(b, c, d, a, x[i + 1], 21, -2054922799);
        a = md5_ii(a, b, c, d, x[i + 8], 6, 1873313359);
        d = md5_ii(d, a, b, c, x[i + 15], 10, -30611744);
        c = md5_ii(c, d, a, b, x[i + 6], 15, -1560198380);
        b = md5_ii(b, c, d, a, x[i + 13], 21, 1309151649);
        a = md5_ii(a, b, c, d, x[i + 4], 6, -145523070);
        d = md5_ii(d, a, b, c, x[i + 11], 10, -1120210379);
        c = md5_ii(c, d, a, b, x[i + 2], 15, 718787259);
        b = md5_ii(b, c, d, a, x[i + 9], 21, -343485551);

        a = safe_add(a, olda);
        b = safe_add(b, oldb);
        c = safe_add(c, oldc);
        d = safe_add(d, oldd);
    }
    return Array(a, b, c, d);
}

/*
 * These functions implement the four basic operations the algorithm uses.
 */
function md5_cmn(q, a, b, x, s, t) {
    return safe_add(bit_rol(safe_add(safe_add(a, q), safe_add(x, t)), s), b);
}
function md5_ff(a, b, c, d, x, s, t) {
    return md5_cmn((b & c) | ((~b) & d), a, b, x, s, t);
}
function md5_gg(a, b, c, d, x, s, t) {
    return md5_cmn((b & d) | (c & (~d)), a, b, x, s, t);
}
function md5_hh(a, b, c, d, x, s, t) {
    return md5_cmn(b ^ c ^ d, a, b, x, s, t);
}
function md5_ii(a, b, c, d, x, s, t) {
    return md5_cmn(c ^ (b | (~d)), a, b, x, s, t);
}

/*
 * Add integers, wrapping at 2^32. This uses 16-bit operations internally
 * to work around bugs in some JS interpreters.
 */
function safe_add(x, y) {
    var lsw = (x & 0xFFFF) + (y & 0xFFFF);
    var msw = (x >> 16) + (y >> 16) + (lsw >> 16);
    return (msw << 16) | (lsw & 0xFFFF);
}

/*
 * Bitwise rotate a 32-bit number to the left.
 */
function bit_rol(num, cnt) {
    return (num << cnt) | (num >>> (32 - cnt));
}
/*
 RichText: WYSIWYG editor developed as jQuery plugin

 @name RichText
 @author https://github.com/webfashionist - Bob Schockweiler - richtext@webfashion.eu
 @license GNU AFFERO GENERAL PUBLIC LICENSE Version 3
 @preserve

 Copyright (C) 2020 Bob Schockweiler ( richtext@webfashion.eu )

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Affero General Public License as published
 by the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Affero General Public License for more details.

 You should have received a copy of the GNU Affero General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

(function ($) {

    $.fn.richText = function (options) {

        // set default options
        // and merge them with the parameter options
        var settings = $.extend({

            // text formatting
            bold: true,
            italic: true,
            underline: true,

            // text alignment
            leftAlign: true,
            centerAlign: true,
            rightAlign: true,
            justify: true,

            // lists
            ol: true,
            ul: true,

            // title
            heading: true,

            // fonts
            fonts: true,
            fontList: ["Arial",
                "Arial Black",
                "Comic Sans MS",
                "Courier New",
                "Geneva",
                "Georgia",
                "Helvetica",
                "Impact",
                "Lucida Console",
                "Tahoma",
                "Times New Roman",
                "Verdana"
            ],
            fontColor: true,
            fontSize: true,

            // uploads
            imageUpload: true,
            fileUpload: true,

            // media
            videoEmbed: true,

            // link
            urls: true,

            // tables
            table: true,

            // code
            removeStyles: true,
            code: true,

            // colors
            colors: [],

            // dropdowns
            fileHTML: '',
            imageHTML: '',

            // translations
            translations: {
                'title': 'Title',
                'white': 'White',
                'black': 'Black',
                'brown': 'Brown',
                'beige': 'Beige',
                'darkBlue': 'Dark Blue',
                'blue': 'Blue',
                'lightBlue': 'Light Blue',
                'darkRed': 'Dark Red',
                'red': 'Red',
                'darkGreen': 'Dark Green',
                'green': 'Green',
                'purple': 'Purple',
                'darkTurquois': 'Dark Turquois',
                'turquois': 'Turquois',
                'darkOrange': 'Dark Orange',
                'orange': 'Orange',
                'yellow': 'Yellow',
                'imageURL': 'Image URL',
                'fileURL': 'File URL',
                'linkText': 'Link text',
                'url': 'URL',
                'size': 'Size',
                'responsive': 'Responsive',
                'text': 'Text',
                'openIn': 'Open in',
                'sameTab': 'Same tab',
                'newTab': 'New tab',
                'align': 'Align',
                'left': 'Left',
                'justify': 'Justify',
                'center': 'Center',
                'right': 'Right',
                'rows': 'Rows',
                'columns': 'Columns',
                'add': 'Add',
                'pleaseEnterURL': 'Please enter an URL',
                'videoURLnotSupported': 'Video URL not supported',
                'pleaseSelectImage': 'Please select an image',
                'pleaseSelectFile': 'Please select a file',
                'bold': 'Bold',
                'italic': 'Italic',
                'underline': 'Underline',
                'alignLeft': 'Align left',
                'alignCenter': 'Align centered',
                'alignRight': 'Align right',
                'addOrderedList': 'Add ordered list',
                'addUnorderedList': 'Add unordered list',
                'addHeading': 'Add Heading/title',
                'addFont': 'Add font',
                'addFontColor': 'Add font color',
                'addFontSize': 'Add font size',
                'addImage': 'Add image',
                'addVideo': 'Add video',
                'addFile': 'Add file',
                'addURL': 'Add URL',
                'addTable': 'Add table',
                'removeStyles': 'Remove styles',
                'code': 'Show HTML code',
                'undo': 'Undo',
                'redo': 'Redo',
                'close': 'Close'
            },

            // privacy
            youtubeCookies: false,

            // dev settings
            useSingleQuotes: false,
            height: 0,
            heightPercentage: 0,
            id: "",
            class: "",
            useParagraph: false,
            maxlength: 0,
            callback: undefined,
            useTabForNext: false

        }, options);


        /* prepare toolbar */
        var $inputElement = $(this);
        $inputElement.addClass("richText-initial");
        var $editor,
            $toolbarList = $('<ul />'),
            $toolbarElement = $('<li />'),
            $btnBold = $('<a />', {
                class: "richText-btn",
                "data-command": "bold",
                "title": settings.translations.bold,
                html: '<span class="fa fa-bold"></span>'
            }), // bold
            $btnItalic = $('<a />', {
                class: "richText-btn",
                "data-command": "italic",
                "title": settings.translations.italic,
                html: '<span class="fa fa-italic"></span>'
            }), // italic
            $btnUnderline = $('<a />', {
                class: "richText-btn",
                "data-command": "underline",
                "title": settings.translations.underline,
                html: '<span class="fa fa-underline"></span>'
            }), // underline
            $btnJustify = $('<a />', {
                class: "richText-btn",
                "data-command": "justifyFull",
                "title": settings.translations.justify,
                html: '<span class="fa fa-align-justify"></span>'
            }), // left align
            $btnLeftAlign = $('<a />', {
                class: "richText-btn",
                "data-command": "justifyLeft",
                "title": settings.translations.alignLeft,
                html: '<span class="fa fa-align-left"></span>'
            }), // left align
            $btnCenterAlign = $('<a />', {
                class: "richText-btn",
                "data-command": "justifyCenter",
                "title": settings.translations.alignCenter,
                html: '<span class="fa fa-align-center"></span>'
            }), // centered
            $btnRightAlign = $('<a />', {
                class: "richText-btn",
                "data-command": "justifyRight",
                "title": settings.translations.alignRight,
                html: '<span class="fa fa-align-right"></span>'
            }), // right align
            $btnOL = $('<a />', {
                class: "richText-btn",
                "data-command": "insertOrderedList",
                "title": settings.translations.addOrderedList,
                html: '<span class="fa fa-list-ol"></span>'
            }), // ordered list
            $btnUL = $('<a />', {
                class: "richText-btn",
                "data-command": "insertUnorderedList",
                "title": settings.translations.addUnorderedList,
                html: '<span class="fa fa-list"></span>'
            }), // unordered list
            $btnHeading = $('<a />', {
                class: "richText-btn",
                "title": settings.translations.addHeading,
                html: '<span class="fa fa-header fa-heading"></span>'
            }), // title/header
            $btnFont = $('<a />', {
                class: "richText-btn",
                "title": settings.translations.addFont,
                html: '<span class="fa fa-font"></span>'
            }), // font color
            $btnFontColor = $('<a />', {
                class: "richText-btn",
                "title": settings.translations.addFontColor,
                html: '<span class="fa fa-paint-brush"></span>'
            }), // font color
            $btnFontSize = $('<a />', {
                class: "richText-btn",
                "title": settings.translations.addFontSize,
                html: '<span class="fa fa-text-height"></span>'
            }), // font color
            $btnImageUpload = $('<a />', {
                class: "richText-btn",
                "title": settings.translations.addImage,
                html: '<span class="fa fa-image"></span>'
            }), // image
            $btnVideoEmbed = $('<a />', {
                class: "richText-btn",
                "title": settings.translations.addVideo,
                html: '<span class="fa fa-video-camera fa-video"></span>'
            }), // video
            $btnFileUpload = $('<a />', {
                class: "richText-btn",
                "title": settings.translations.addFile,
                html: '<span class="fa fa-file-text-o far fa-file-alt"></span>'
            }), // file
            $btnURLs = $('<a />', {
                class: "richText-btn",
                "title": settings.translations.addURL,
                html: '<span class="fa fa-link"></span>'
            }), // urls/links
            $btnTable = $('<a />', {
                class: "richText-btn",
                "title": settings.translations.addTable,
                html: '<span class="fa fa-table"></span>'
            }), // table
            $btnRemoveStyles = $('<a />', {
                class: "richText-btn",
                "data-command": "removeFormat",
                "title": settings.translations.removeStyles,
                html: '<span class="fa fa-recycle"></span>'
            }), // clean up styles
            $btnCode = $('<a />', {
                class: "richText-btn",
                "data-command": "toggleCode",
                "title": settings.translations.code,
                html: '<span class="fa fa-code"></span>'
            }); // code


        /* prepare toolbar dropdowns */
        var $dropdownOuter = $('<div />', {class: "richText-dropdown-outer"});
        var $dropdownClose = $('<span />', {
            class: "richText-dropdown-close",
            html: '<span title="' + settings.translations.close + '"><span class="fa fa-times"></span></span>'
        });
        var $dropdownList = $('<ul />', {class: "richText-dropdown"}), // dropdown lists
            $dropdownBox = $('<div />', {class: "richText-dropdown"}), // dropdown boxes / custom dropdowns
            $form = $('<div />', {class: "richText-form"}), // symbolic form
            $formItem = $('<div />', {class: 'richText-form-item'}), // form item
            $formLabel = $('<label />'), // form label
            $formInput = $('<input />', {type: "text"}), //form input field
            $formInputFile = $('<input />', {type: "file"}), // form file input field
            $formInputSelect = $('<select />'),
            $formButton = $('<button />', {text: settings.translations.add, class: "btn"}); // button

        /* internal settings */
        var savedSelection; // caret position/selection
        var editorID = "richText-" + Math.random().toString(36).substring(7);
        var ignoreSave = false, $resizeImage = null;

        /* prepare editor history */
        var history = [];
        history[editorID] = [];
        var historyPosition = [];
        historyPosition[editorID] = 0;

        /* list dropdown for titles */
        var $titles = $dropdownList.clone();
        $titles.append($('<li />', {html: '<a data-command="formatBlock" data-option="h1">' + settings.translations.title + ' #1</a>'}));
        $titles.append($('<li />', {html: '<a data-command="formatBlock" data-option="h2">' + settings.translations.title + ' #2</a>'}));
        $titles.append($('<li />', {html: '<a data-command="formatBlock" data-option="h3">' + settings.translations.title + ' #3</a>'}));
        $titles.append($('<li />', {html: '<a data-command="formatBlock" data-option="h4">' + settings.translations.title + ' #4</a>'}));
        $btnHeading.append($dropdownOuter.clone().append($titles.prepend($dropdownClose.clone())));

        /* list dropdown for fonts */
        var fonts = settings.fontList;
        var $fonts = $dropdownList.clone();
        for (var i = 0; i < fonts.length; i++) {
            $fonts.append($('<li />', {html: '<a style="font-family:' + fonts[i] + ';" data-command="fontName" data-option="' + fonts[i] + '">' + fonts[i] + '</a>'}));
        }
        $btnFont.append($dropdownOuter.clone().append($fonts.prepend($dropdownClose.clone())));

        /* list dropdown for font sizes */
        var fontSizes = [24, 18, 16, 14, 12];
        var $fontSizes = $dropdownList.clone();
        for (var i = 0; i < fontSizes.length; i++) {
            $fontSizes.append($('<li />', {html: '<a style="font-size:' + fontSizes[i] + 'px;" data-command="fontSize" data-option="' + fontSizes[i] + '">' + settings.translations.text + ' ' + fontSizes[i] + 'px</a>'}));
        }
        $btnFontSize.append($dropdownOuter.clone().append($fontSizes.prepend($dropdownClose.clone())));

        /* font colors */
        var $fontColors = $dropdownList.clone();
        $fontColors.html(loadColors("forecolor"));
        $btnFontColor.append($dropdownOuter.clone().append($fontColors.prepend($dropdownClose.clone())));


        /* background colors */
        //var $bgColors = $dropdownList.clone();
        //$bgColors.html(loadColors("hiliteColor"));
        //$btnBGColor.append($dropdownOuter.clone().append($bgColors));

        /* box dropdown for links */
        var $linksDropdown = $dropdownBox.clone();
        var $linksForm = $form.clone().attr("id", "richText-URL").attr("data-editor", editorID);
        $linksForm.append(
            $formItem.clone()
                .append($formLabel.clone().text(settings.translations.url).attr("for", "url"))
                .append($formInput.clone().attr("id", "url"))
        );
        $linksForm.append(
            $formItem.clone()
                .append($formLabel.clone().text(settings.translations.text).attr("for", "urlText"))
                .append($formInput.clone().attr("id", "urlText"))
        );
        $linksForm.append(
            $formItem.clone()
                .append($formLabel.clone().text(settings.translations.openIn).attr("for", "openIn"))
                .append(
                    $formInputSelect
                        .clone().attr("id", "openIn")
                        .append($("<option />", {value: '_self', text: settings.translations.sameTab}))
                        .append($("<option />", {value: '_blank', text: settings.translations.newTab}))
                )
        );
        $linksForm.append($formItem.clone().append($formButton.clone()));
        $linksDropdown.append($linksForm);
        $btnURLs.append($dropdownOuter.clone().append($linksDropdown.prepend($dropdownClose.clone())));

        /* box dropdown for video embedding */
        var $videoDropdown = $dropdownBox.clone();
        var $videoForm = $form.clone().attr("id", "richText-Video").attr("data-editor", editorID);
        $videoForm.append(
            $formItem.clone()
                .append($formLabel.clone().text(settings.translations.url).attr("for", "videoURL"))
                .append($formInput.clone().attr("id", "videoURL"))
        );
        $videoForm.append(
            $formItem.clone()
                .append($formLabel.clone().text(settings.translations.size).attr("for", "size"))
                .append(
                    $formInputSelect
                        .clone().attr("id", "size")
                        .append($("<option />", {value: 'responsive', text: settings.translations.responsive}))
                        .append($("<option />", {value: '640x360', text: '640x360'}))
                        .append($("<option />", {value: '560x315', text: '560x315'}))
                        .append($("<option />", {value: '480x270', text: '480x270'}))
                        .append($("<option />", {value: '320x180', text: '320x180'}))
                )
        );
        $videoForm.append($formItem.clone().append($formButton.clone()));
        $videoDropdown.append($videoForm);
        $btnVideoEmbed.append($dropdownOuter.clone().append($videoDropdown.prepend($dropdownClose.clone())));

        /* box dropdown for image upload/image selection */
        var $imageDropdown = $dropdownBox.clone();
        var $imageForm = $form.clone().attr("id", "richText-Image").attr("data-editor", editorID);

        if (settings.imageHTML
            && ($(settings.imageHTML).find('#imageURL').length > 0 || $(settings.imageHTML).attr("id") === "imageURL")) {
            // custom image form
            $imageForm.html(settings.imageHTML);
        } else {
            // default image form
            $imageForm.append(
                $formItem.clone()
                    .append($formLabel.clone().text(settings.translations.imageURL).attr("for", "imageURL"))
                    .append($formInput.clone().attr("id", "imageURL"))
            );
            $imageForm.append(
                $formItem.clone()
                    .append($formLabel.clone().text(settings.translations.align).attr("for", "align"))
                    .append(
                        $formInputSelect
                            .clone().attr("id", "align")
                            .append($("<option />", {value: 'left', text: settings.translations.left}))
                            .append($("<option />", {value: 'center', text: settings.translations.center}))
                            .append($("<option />", {value: 'right', text: settings.translations.right}))
                    )
            );
        }
        $imageForm.append($formItem.clone().append($formButton.clone()));
        $imageDropdown.append($imageForm);
        $btnImageUpload.append($dropdownOuter.clone().append($imageDropdown.prepend($dropdownClose.clone())));

        /* box dropdown for file upload/file selection */
        var $fileDropdown = $dropdownBox.clone();
        var $fileForm = $form.clone().attr("id", "richText-File").attr("data-editor", editorID);

        if (settings.fileHTML
            && ($(settings.fileHTML).find('#fileURL').length > 0 || $(settings.fileHTML).attr("id") === "fileURL")) {
            // custom file form
            $fileForm.html(settings.fileHTML);
        } else {
            // default file form
            $fileForm.append(
                $formItem.clone()
                    .append($formLabel.clone().text(settings.translations.fileURL).attr("for", "fileURL"))
                    .append($formInput.clone().attr("id", "fileURL"))
            );
            $fileForm.append(
                $formItem.clone()
                    .append($formLabel.clone().text(settings.translations.linkText).attr("for", "fileText"))
                    .append($formInput.clone().attr("id", "fileText"))
            );
        }
        $fileForm.append($formItem.clone().append($formButton.clone()));
        $fileDropdown.append($fileForm);
        $btnFileUpload.append($dropdownOuter.clone().append($fileDropdown.prepend($dropdownClose.clone())));

        /* box dropdown for tables */
        var $tableDropdown = $dropdownBox.clone();
        var $tableForm = $form.clone().attr("id", "richText-Table").attr("data-editor", editorID);
        $tableForm.append(
            $formItem.clone()
                .append($formLabel.clone().text(settings.translations.rows).attr("for", "tableRows"))
                .append($formInput.clone().attr("id", "tableRows").attr("type", "number"))
        );
        $tableForm.append(
            $formItem.clone()
                .append($formLabel.clone().text(settings.translations.columns).attr("for", "tableColumns"))
                .append($formInput.clone().attr("id", "tableColumns").attr("type", "number"))
        );
        $tableForm.append($formItem.clone().append($formButton.clone()));
        $tableDropdown.append($tableForm);
        $btnTable.append($dropdownOuter.clone().append($tableDropdown.prepend($dropdownClose.clone())));


        /* initizalize editor */
        function init() {
            var value, attributes, attributes_html = '';

            if (settings.useParagraph !== false) {
                // set default tag when pressing ENTER to <p> instead of <div>
                document.execCommand("DefaultParagraphSeparator", false, 'p');
            }


            // reformat $inputElement to textarea
            if ($inputElement.prop("tagName") === "TEXTAREA") {
                // everything perfect
            } else if ($inputElement.val()) {
                value = $inputElement.val();
                attributes = $inputElement.prop("attributes");
                // loop through <select> attributes and apply them on <div>
                $.each(attributes, function () {
                    if (this.name) {
                        attributes_html += ' ' + this.name + '="' + this.value + '"';
                    }
                });
                $inputElement.replaceWith($('<textarea' + attributes_html + ' data-richtext="init">' + value + '</textarea>'));
                $inputElement = $('[data-richtext="init"]');
                $inputElement.removeAttr("data-richtext");
            } else if ($inputElement.html()) {
                value = $inputElement.html();
                attributes = $inputElement.prop("attributes");
                // loop through <select> attributes and apply them on <div>
                $.each(attributes, function () {
                    if (this.name) {
                        attributes_html += ' ' + this.name + '="' + this.value + '"';
                    }
                });
                $inputElement.replaceWith($('<textarea' + attributes_html + ' data-richtext="init">' + value + '</textarea>'));
                $inputElement = $('[data-richtext="init"]');
                $inputElement.removeAttr("data-richtext");
            } else {
                attributes = $inputElement.prop("attributes");
                // loop through <select> attributes and apply them on <div>
                $.each(attributes, function () {
                    if (this.name) {
                        attributes_html += ' ' + this.name + '="' + this.value + '"';
                    }
                });
                $inputElement.replaceWith($('<textarea' + attributes_html + ' data-richtext="init"></textarea>'));
                $inputElement = $('[data-richtext="init"]');
                $inputElement.removeAttr("data-richtext");
            }

            $editor = $('<div />', {class: "richText"});
            var $toolbar = $('<div />', {class: "richText-toolbar"});
            var $editorView = $('<div />', {class: "richText-editor", id: editorID, contenteditable: true});
            var tabindex = $inputElement.prop('tabindex');
            if (tabindex >= 0 && settings.useTabForNext === true) {
                $editorView.attr('tabindex', tabindex);
            }
            $toolbar.append($toolbarList);
            settings.$editor = $editor;

            /* text formatting */
            if (settings.bold === true) {
                $toolbarList.append($toolbarElement.clone().append($btnBold));
            }
            if (settings.italic === true) {
                $toolbarList.append($toolbarElement.clone().append($btnItalic));
            }
            if (settings.underline === true) {
                $toolbarList.append($toolbarElement.clone().append($btnUnderline));
            }

            /* align */
            if (settings.leftAlign === true) {
                $toolbarList.append($toolbarElement.clone().append($btnLeftAlign));
            }
            if (settings.centerAlign === true) {
                $toolbarList.append($toolbarElement.clone().append($btnCenterAlign));
            }
            if (settings.rightAlign === true) {
                $toolbarList.append($toolbarElement.clone().append($btnRightAlign));
            }
            if (settings.justify === true) {
                $toolbarList.append($toolbarElement.clone().append($btnJustify));
            }

            /* lists */
            if (settings.ol === true) {
                $toolbarList.append($toolbarElement.clone().append($btnOL));
            }
            if (settings.ul === true) {
                $toolbarList.append($toolbarElement.clone().append($btnUL));
            }

            /* fonts */
            if (settings.fonts === true && settings.fontList.length > 0) {
                $toolbarList.append($toolbarElement.clone().append($btnFont));
            }
            if (settings.fontSize === true) {
                $toolbarList.append($toolbarElement.clone().append($btnFontSize));
            }

            /* heading */
            if (settings.heading === true) {
                $toolbarList.append($toolbarElement.clone().append($btnHeading));
            }

            /* colors */
            if (settings.fontColor === true) {
                $toolbarList.append($toolbarElement.clone().append($btnFontColor));
            }

            /* uploads */
            if (settings.imageUpload === true) {
                $toolbarList.append($toolbarElement.clone().append($btnImageUpload));
            }
            if (settings.fileUpload === true) {
                $toolbarList.append($toolbarElement.clone().append($btnFileUpload));
            }

            /* media */
            if (settings.videoEmbed === true) {
                $toolbarList.append($toolbarElement.clone().append($btnVideoEmbed));
            }

            /* urls */
            if (settings.urls === true) {
                $toolbarList.append($toolbarElement.clone().append($btnURLs));
            }

            if (settings.table === true) {
                $toolbarList.append($toolbarElement.clone().append($btnTable));
            }

            /* code */
            if (settings.removeStyles === true) {
                $toolbarList.append($toolbarElement.clone().append($btnRemoveStyles));
            }
            if (settings.code === true) {
                $toolbarList.append($toolbarElement.clone().append($btnCode));
            }

            // set current textarea value to editor
            $editorView.html($inputElement.val());

            $editor.append($toolbar);
            $editor.append($editorView);
            $editor.append($inputElement.clone().hide());
            $inputElement.replaceWith($editor);

            // append bottom toolbar
            $editor.append(
                $('<div />', {class: 'richText-toolbar'})
                    .append($('<a />', {
                        class: 'richText-undo is-disabled',
                        html: '<span class="fa fa-undo"></span>',
                        'title': settings.translations.undo
                    }))
                    .append($('<a />', {
                        class: 'richText-redo is-disabled',
                        html: '<span class="fa fa-repeat fa-redo"></span>',
                        'title': settings.translations.redo
                    }))
                    .append($('<a />', {class: 'richText-help', html: '<span class="fa fa-question-circle"></span>'}))
            );

            if (settings.maxlength > 0) {
                // display max length in editor toolbar
                $editor.data('maxlength', settings.maxlength);
                $editor.children('.richText-toolbar').children('.richText-help').before($('<a />', {
                    class: 'richText-length',
                    text: '0/' + settings.maxlength
                }));
            }

            if (settings.height && settings.height > 0) {
                // set custom editor height
                $editor.children(".richText-editor, .richText-initial").css({
                    'min-height': settings.height + 'px',
                    'height': settings.height + 'px'
                });
            } else if (settings.heightPercentage && settings.heightPercentage > 0) {
                // set custom editor height in percentage
                var parentHeight = $editor.parent().innerHeight(); // get editor parent height
                var height = (settings.heightPercentage / 100) * parentHeight; // calculate pixel value from percentage
                height -= $toolbar.outerHeight() * 2; // remove toolbar size
                height -= parseInt($editor.css("margin-top")); // remove margins
                height -= parseInt($editor.css("margin-bottom")); // remove margins
                height -= parseInt($editor.find(".richText-editor").css("padding-top")); // remove paddings
                height -= parseInt($editor.find(".richText-editor").css("padding-bottom")); // remove paddings
                $editor.children(".richText-editor, .richText-initial").css({
                    'min-height': height + 'px',
                    'height': height + 'px'
                });
            }

            // add custom class
            if (settings.class) {
                $editor.addClass(settings.class);
            }
            if (settings.id) {
                $editor.attr("id", settings.id);
            }

            // fix the first line
            fixFirstLine();

            // save history
            history[editorID].push($editor.find("textarea").val());

            if (settings.callback && typeof settings.callback === 'function') {
                settings.callback($editor);
            }
        }

        // initialize editor
        init();


        /** EVENT HANDLERS */

        // Help popup
        settings.$editor.find('.richText-help').on('click', function () {
            var $editor = $(this).parents(".richText");
            if ($editor) {
                var $outer = $('<div />', {
                    class: 'richText-help-popup',
                    style: 'position:absolute;top:0;right:0;bottom:0;left:0;background-color: rgba(0,0,0,0.3);'
                });
                var $inner = $('<div />', {style: 'position:relative;margin:60px auto;padding:20px;background-color:#FAFAFA;width:70%;font-family:Calibri,Verdana,Helvetica,sans-serif;font-size:small;'});
                var $content = $('<div />', {html: '<span id="closeHelp" style="display:block;position:absolute;top:0;right:0;padding:10px;cursor:pointer;" title="' + settings.translations.close + '"><span class="fa fa-times"></span></span>'});
                $content.append('<h3 style="margin:0;">RichText</h3>');
                $content.append('<hr><br>Powered by <a href="https://github.com/webfashionist/RichText" target="_blank">webfashionist/RichText</a> (Github) <br>License: <a href="https://github.com/webfashionist/RichText/blob/master/LICENSE" target="_blank">AGPL-3.0</a>');

                $outer.append($inner.append($content));
                $editor.append($outer);

                $outer.on("click", "#closeHelp", function () {
                    $(this).parents('.richText-help-popup').remove();
                });
            }
        });

        // undo / redo
        settings.$editor.find('.richText-undo, .richText-redo').on('click', function () {
            var $this = $(this);
            if ($this.hasClass("richText-undo") && !$this.hasClass("is-disabled")) {
                undo(settings.$editor);
            } else if ($this.hasClass("richText-redo") && !$this.hasClass("is-disabled")) {
                redo(settings.$editor);
            }
        });


        // Saving changes from editor to textarea
        settings.$editor.find('.richText-editor').on('input change blur keydown keyup', function (e) {
            if ((e.keyCode === 9 || e.keyCode === "9") && e.type === "keydown") {
                // tab through table cells or focus next element
                if (settings.useTabForNext === true) {
                    focusNextElement();
                    return false;
                }
                e.preventDefault();
                tabifyEditableTable(window, e);
                return false;
            }
            fixFirstLine();
            updateTextarea();
            doSave($(this).attr("id"));
            updateMaxLength($(this).attr('id'));
        });


        // add context menu to several Node elements
        settings.$editor.find('.richText-editor').on('contextmenu', '.richText-editor', function (e) {

            var $list = $('<ul />', {'class': 'list-rightclick richText-list'});
            var $li = $('<li />');
            // remove Node selection
            $('.richText-editor').find('.richText-editNode').removeClass('richText-editNode');

            var $target = $(e.target);
            var $richText = $target.parents('.richText');
            var $toolbar = $richText.find('.richText-toolbar');

            var positionX = e.pageX - $richText.offset().left;
            var positionY = e.pageY - $richText.offset().top;

            $list.css({
                'top': positionY,
                'left': positionX
            });


            if ($target.prop("tagName") === "A") {
                // edit URL
                e.preventDefault();

                $list.append($li.clone().html('<span class="fa fa-link"></span>'));
                $target.parents('.richText').append($list);
                $list.find('.fa-link').on('click', function () {
                    $('.list-rightclick.richText-list').remove();
                    $target.addClass('richText-editNode');
                    var $popup = $toolbar.find('#richText-URL');
                    $popup.find('input#url').val($target.attr('href'));
                    $popup.find('input#urlText').val($target.text());
                    $popup.find('select#openIn').val($target.attr('target'));
                    $toolbar.find('.richText-btn').children('.fa-link').parents('li').addClass('is-selected');
                });

                return false;
            } else if ($target.prop("tagName") === "IMG") {
                // edit image
                e.preventDefault();

                $list.append($li.clone().html('<span class="fa fa-image"></span>'));
                $target.parents('.richText').append($list);
                $list.find('.fa-image').on('click', function () {
                    var align;
                    if ($target.parent('div').length > 0 && $target.parent('div').attr('style') === 'text-align:center;') {
                        align = 'center';
                    } else {
                        align = $target.attr('align');
                    }
                    $('.list-rightclick.richText-list').remove();
                    $target.addClass('richText-editNode');
                    var $popup = $toolbar.find('#richText-Image');
                    $popup.find('input#imageURL').val($target.attr('src'));
                    $popup.find('select#align').val(align);
                    $toolbar.find('.richText-btn').children('.fa-image').parents('li').addClass('is-selected');
                });

                return false;
            }

        });

        // Saving changes from textarea to editor
        settings.$editor.find('.richText-initial').on('input change blur', function () {
            if (settings.useSingleQuotes === true) {
                $(this).val(changeAttributeQuotes($(this).val()));
            }
            var editorID = $(this).siblings('.richText-editor').attr("id");
            updateEditor(editorID);
            doSave(editorID);
            updateMaxLength(editorID);
        });

        // Save selection seperately (mainly needed for Safari)
        settings.$editor.find('.richText-editor').on('dblclick mouseup', function () {
            doSave($(this).attr('id'));
        });

        // embedding video
        settings.$editor.find('#richText-Video button.btn').on('click', function (event) {
            event.preventDefault();
            var $button = $(this);
            var $form = $button.parent('.richText-form-item').parent('.richText-form');
            if ($form.attr("data-editor") === editorID) {
                // only for the currently selected editor
                var url = $form.find('input#videoURL').val();
                var size = $form.find('select#size').val();

                if (!url) {
                    // no url set
                    $form.prepend($('<div />', {
                        style: 'color:red;display:none;',
                        class: 'form-item is-error',
                        text: settings.translations.pleaseEnterURL
                    }));
                    $form.children('.form-item.is-error').slideDown();
                    setTimeout(function () {
                        $form.children('.form-item.is-error').slideUp(function () {
                            $(this).remove();
                        });
                    }, 5000);
                } else {
                    // write html in editor
                    var html = '';
                    html = getVideoCode(url, size);
                    if (!html) {
                        $form.prepend($('<div />', {
                            style: 'color:red;display:none;',
                            class: 'form-item is-error',
                            text: settings.translations.videoURLnotSupported
                        }));
                        $form.children('.form-item.is-error').slideDown();
                        setTimeout(function () {
                            $form.children('.form-item.is-error').slideUp(function () {
                                $(this).remove();
                            });
                        }, 5000);
                    } else {
                        if (settings.useSingleQuotes === true) {

                        } else {

                        }
                        restoreSelection(editorID, true);
                        pasteHTMLAtCaret(html);
                        updateTextarea();
                        // reset input values
                        $form.find('input#videoURL').val('');
                        $('.richText-toolbar li.is-selected').removeClass("is-selected");
                    }
                }
            }
        });

        // Resize images
        $(document).on('mousedown', function (e) {
            var $target = $(e.target);
            if (!$target.hasClass('richText-list') && $target.parents('.richText-list').length === 0) {
                // remove context menu
                $('.richText-list.list-rightclick').remove();
                if (!$target.hasClass('richText-form') && $target.parents('.richText-form').length === 0) {
                    $('.richText-editNode').each(function () {
                        var $this = $(this);
                        $this.removeClass('richText-editNode');
                        if ($this.attr('class') === '') {
                            $this.removeAttr('class');
                        }
                    });
                }
            }
            if ($target.prop("tagName") === "IMG" && $target.parents("#" + editorID)) {
                startX = e.pageX;
                startY = e.pageY;
                startW = $target.innerWidth();
                startH = $target.innerHeight();

                var left = $target.offset().left;
                var right = $target.offset().left + $target.innerWidth();
                var bottom = $target.offset().top + $target.innerHeight();
                var top = $target.offset().top;
                var resize = false;
                $target.css({'cursor': 'default'});

                if (startY <= bottom && startY >= bottom - 20 && startX >= right - 20 && startX <= right) {
                    // bottom right corner
                    $resizeImage = $target;
                    $resizeImage.css({'cursor': 'nwse-resize'});
                    resize = true;
                }

                if ((resize === true || $resizeImage) && !$resizeImage.data("width")) {
                    // set initial image size and prevent dragging image while resizing
                    $resizeImage.data("width", $target.parents("#" + editorID).innerWidth());
                    $resizeImage.data("height", $target.parents("#" + editorID).innerHeight() * 3);
                    e.preventDefault();
                } else if (resize === true || $resizeImage) {
                    // resizing active, prevent other events
                    e.preventDefault();
                } else {
                    // resizing disabled, allow dragging image
                    $resizeImage = null;
                }

            }
        });
        $(document)
            .mouseup(function () {
                if ($resizeImage) {
                    $resizeImage.css({'cursor': 'default'});
                }
                $resizeImage = null;
            })
            .mousemove(function (e) {
                if ($resizeImage !== null) {
                    var maxWidth = $resizeImage.data('width');
                    var currentWidth = $resizeImage.width();
                    var maxHeight = $resizeImage.data('height');
                    var currentHeight = $resizeImage.height();
                    if ((startW + e.pageX - startX) <= maxWidth && (startH + e.pageY - startY) <= maxHeight) {
                        // only resize if new size is smaller than the original image size
                        $resizeImage.innerWidth(startW + e.pageX - startX); // only resize width to adapt height proportionally
                        // $box.innerHeight(startH + e.pageY-startY);
                        updateTextarea();
                    } else if ((startW + e.pageX - startX) <= currentWidth && (startH + e.pageY - startY) <= currentHeight) {
                        // only resize if new size is smaller than the previous size
                        $resizeImage.innerWidth(startW + e.pageX - startX); // only resize width to adapt height proportionally
                        updateTextarea();
                    }
                }
            });

        // adding URL
        settings.$editor.find('#richText-URL button.btn').on('click', function (event) {
            event.preventDefault();
            var $button = $(this);
            var $form = $button.parent('.richText-form-item').parent('.richText-form');
            if ($form.attr("data-editor") === editorID) {
                // only for currently selected editor
                var url = $form.find('input#url').val();
                var text = $form.find('input#urlText').val();
                var target = $form.find('#openIn').val();

                // set default values
                if (!target) {
                    target = '_self';
                }
                if (!text) {
                    text = url;
                }
                if (!url) {
                    // no url set
                    $form.prepend($('<div />', {
                        style: 'color:red;display:none;',
                        class: 'form-item is-error',
                        text: settings.translations.pleaseEnterURL
                    }));
                    $form.children('.form-item.is-error').slideDown();
                    setTimeout(function () {
                        $form.children('.form-item.is-error').slideUp(function () {
                            $(this).remove();
                        });
                    }, 5000);
                } else {
                    // write html in editor
                    var html = '';
                    if (settings.useSingleQuotes === true) {
                        html = "<a href='" + url + "' target='" + target + "'>" + text + "</a>";
                    } else {
                        html = '<a href="' + url + '" target="' + target + '">' + text + '</a>';
                    }
                    restoreSelection(editorID, false, true);

                    var $editNode = $('.richText-editNode');
                    if ($editNode.length > 0 && $editNode.prop("tagName") === "A") {
                        $editNode.attr("href", url);
                        $editNode.attr("target", target);
                        $editNode.text(text);
                        $editNode.removeClass('richText-editNode');
                        if ($editNode.attr('class') === '') {
                            $editNode.removeAttr('class');
                        }
                    } else {
                        pasteHTMLAtCaret(html);
                    }
                    // reset input values
                    $form.find('input#url').val('');
                    $form.find('input#urlText').val('');
                    $('.richText-toolbar li.is-selected').removeClass("is-selected");
                }
            }
        });

        // adding image
        settings.$editor.find('#richText-Image button.btn').on('click', function (event) {
            event.preventDefault();
            var $button = $(this);
            var $form = $button.parent('.richText-form-item').parent('.richText-form');
            if ($form.attr("data-editor") === editorID) {
                // only for currently selected editor
                var url = $form.find('#imageURL').val();
                var align = $form.find('select#align').val();

                // set default values
                if (!align) {
                    align = 'center';
                }
                if (!url) {
                    // no url set
                    $form.prepend($('<div />', {
                        style: 'color:red;display:none;',
                        class: 'form-item is-error',
                        text: settings.translations.pleaseSelectImage
                    }));
                    $form.children('.form-item.is-error').slideDown();
                    setTimeout(function () {
                        $form.children('.form-item.is-error').slideUp(function () {
                            $(this).remove();
                        });
                    }, 5000);
                } else {
                    // write html in editor
                    var html = '';
                    if (settings.useSingleQuotes === true) {
                        if (align === "center") {
                            html = "<div style='text-align:center;'><img src='" + url + "'></div>";
                        } else {
                            html = "<img src='" + url + "' align='" + align + "'>";
                        }
                    } else {
                        if (align === "center") {
                            html = '<div style="text-align:center;"><img src="' + url + '"></div>';
                        } else {
                            html = '<img src="' + url + '" align="' + align + '">';
                        }
                    }
                    restoreSelection(editorID, true);
                    var $editNode = $('.richText-editNode');
                    if ($editNode.length > 0 && $editNode.prop("tagName") === "IMG") {
                        $editNode.attr("src", url);
                        if ($editNode.parent('div').length > 0 && $editNode.parent('div').attr('style') === 'text-align:center;' && align !== 'center') {
                            $editNode.unwrap('div');
                            $editNode.attr('align', align);
                        } else if (($editNode.parent('div').length === 0 || $editNode.parent('div').attr('style') !== 'text-align:center;') && align === 'center') {
                            $editNode.wrap('<div style="text-align:center;"></div>');
                            $editNode.removeAttr('align');
                        } else {
                            $editNode.attr('align', align);
                        }
                        $editNode.removeClass('richText-editNode');
                        if ($editNode.attr('class') === '') {
                            $editNode.removeAttr('class');
                        }
                    } else {
                        pasteHTMLAtCaret(html);
                    }
                    // reset input values
                    $form.find('input#imageURL').val('');
                    $('.richText-toolbar li.is-selected').removeClass("is-selected");
                }
            }
        });

        // adding file
        settings.$editor.find('#richText-File button.btn').on('click', function (event) {
            event.preventDefault();
            var $button = $(this);
            var $form = $button.parent('.richText-form-item').parent('.richText-form');
            if ($form.attr("data-editor") === editorID) {
                // only for currently selected editor
                var url = $form.find('#fileURL').val();
                var text = $form.find('#fileText').val();

                // set default values
                if (!text) {
                    text = url;
                }
                if (!url) {
                    // no url set
                    $form.prepend($('<div />', {
                        style: 'color:red;display:none;',
                        class: 'form-item is-error',
                        text: settings.translations.pleaseSelectFile
                    }));
                    $form.children('.form-item.is-error').slideDown();
                    setTimeout(function () {
                        $form.children('.form-item.is-error').slideUp(function () {
                            $(this).remove();
                        });
                    }, 5000);
                } else {
                    // write html in editor
                    var html = '';
                    if (settings.useSingleQuotes === true) {
                        html = "<a href='" + url + "' target='_blank'>" + text + "</a>";
                    } else {
                        html = '<a href="' + url + '" target="_blank">' + text + '</a>';
                    }
                    restoreSelection(editorID, true);
                    pasteHTMLAtCaret(html);
                    // reset input values
                    $form.find('input#fileURL').val('');
                    $form.find('input#fileText').val('');
                    $('.richText-toolbar li.is-selected').removeClass("is-selected");
                }
            }
        });


        // adding table
        settings.$editor.find('#richText-Table button.btn').on('click', function (event) {
            event.preventDefault();
            var $button = $(this);
            var $form = $button.parent('.richText-form-item').parent('.richText-form');
            if ($form.attr("data-editor") === editorID) {
                // only for currently selected editor
                var rows = $form.find('input#tableRows').val();
                var columns = $form.find('input#tableColumns').val();

                // set default values
                if (!rows || rows <= 0) {
                    rows = 2;
                }
                if (!columns || columns <= 0) {
                    columns = 2;
                }

                // generate table
                var html = '';
                if (settings.useSingleQuotes === true) {
                    html = "<table class='table-1'><tbody>";
                } else {
                    html = '<table class="table-1"><tbody>';
                }
                for (var i = 1; i <= rows; i++) {
                    // start new row
                    html += '<tr>';
                    for (var n = 1; n <= columns; n++) {
                        // start new column in row
                        html += '<td> </td>';
                    }
                    html += '</tr>';
                }
                html += '</tbody></table>';

                // write html in editor
                restoreSelection(editorID, true);
                pasteHTMLAtCaret(html);
                // reset input values
                $form.find('input#tableColumns').val('');
                $form.find('input#tableRows').val('');
                $('.richText-toolbar li.is-selected').removeClass("is-selected");
            }
        });

        // opening / closing toolbar dropdown
        $(document).on("click", function (event) {
            var $clickedElement = $(event.target);

            if ($clickedElement.parents('.richText-toolbar').length === 0) {
                // element not in toolbar
                // ignore
            } else if ($clickedElement.hasClass("richText-dropdown-outer")) {
                // closing dropdown by clicking inside the editor
                $clickedElement.parent('a').parent('li').removeClass("is-selected");
            } else if ($clickedElement.find(".richText").length > 0) {
                // closing dropdown by clicking outside of the editor
                $('.richText-toolbar li').removeClass("is-selected");
            } else if ($clickedElement.parent().hasClass("richText-dropdown-close")) {
                // closing dropdown by clicking on the close button
                $('.richText-toolbar li').removeClass("is-selected");
            } else if ($clickedElement.hasClass("richText-btn") && $(event.target).children('.richText-dropdown-outer').length > 0) {
                // opening dropdown by clicking on toolbar button
                $clickedElement.parent('li').addClass("is-selected");

                if ($clickedElement.children('.fa,svg').hasClass("fa-link")) {
                    // put currently selected text in URL form to replace it
                    restoreSelection(editorID, false, true);
                    var selectedText = getSelectedText();
                    $clickedElement.find("input#urlText").val('');
                    $clickedElement.find("input#url").val('');
                    if (selectedText) {
                        $clickedElement.find("input#urlText").val(selectedText);
                    }
                } else if ($clickedElement.hasClass("fa-image")) {
                    // image
                }
            }
        });

        // Executing editor commands
        settings.$editor.find('.richText-toolbar a[data-command]').on('click', function (event) {
            var $button = $(this);
            var $toolbar = $button.closest('.richText-toolbar');
            var $editor = $toolbar.siblings('.richText-editor');
            var id = $editor.attr("id");
            if ($editor.length > 0 && id === editorID && (!$button.parent("li").attr('data-disable') || $button.parent("li").attr('data-disable') === "false")) {
                event.preventDefault();
                var command = $(this).data("command");

                if (command === "toggleCode") {
                    toggleCode($editor.attr("id"));
                } else {
                    var option = null;
                    if ($(this).data('option')) {
                        option = $(this).data('option').toString();
                        if (option.match(/^h[1-6]$/)) {
                            command = "heading";
                        }
                    }

                    formatText(command, option, id);
                    if (command === "removeFormat") {
                        // remove HTML/CSS formatting
                        $editor.find('*').each(function () {
                            // remove all, but very few, attributes from the nodes
                            var keepAttributes = [
                                "id", "class",
                                "name", "action", "method",
                                "src", "align", "alt", "title",
                                "style", "webkitallowfullscreen", "mozallowfullscreen", "allowfullscreen",
                                "width", "height", "frameborder"
                            ];
                            var element = $(this);
                            var attributes = $.map(this.attributes, function (item) {
                                return item.name;
                            });
                            $.each(attributes, function (i, item) {
                                if (keepAttributes.indexOf(item) < 0 && item.substr(0, 5) !== 'data-') {
                                    element.removeAttr(item);
                                }
                            });
                            if (element.prop('tagName') === "A") {
                                // remove empty URL tags
                                element.replaceWith(function () {
                                    return $('<span />', {html: $(this).html()});
                                });
                            }
                        });
                        formatText('formatBlock', 'div', id);
                    }
                    // clean up empty tags, which can be created while replacing formatting or when copy-pasting from other tools
                    $editor.find('div:empty,p:empty,li:empty,h1:empty,h2:empty,h3:empty,h4:empty,h5:empty,h6:empty').remove();
                    $editor.find('h1,h2,h3,h4,h5,h6').unwrap('h1,h2,h3,h4,h5,h6');
                }
            }
            // close dropdown after click
            $button.parents('li.is-selected').removeClass('is-selected');
        });


        /** INTERNAL METHODS **/

        function focusNextElement () {
            // add all elements we want to include in our selection
            var focussableElements = 'a:not([disabled]):not(.richText-btn,.richText-undo,.richText-redo,.richText-help), button:not([disabled]), input:not([disabled]), select:not([disabled]), textarea:not([disabled]), [tabindex]:not([disabled]):not([tabindex="-1"])';
            if (document.activeElement) {
                var focussable = Array.prototype.filter.call(document.querySelectorAll(focussableElements),
                    function (element) {
                        // check for visibility while always include the current activeElement
                        return element.offsetWidth > 0 || element.offsetHeight > 0 || element === document.activeElement
                    });
                var index = focussable.indexOf(document.activeElement);
                if (index > -1) {
                    var nextElement = focussable[index + 1] || focussable[0];
                    nextElement.focus();
                }
            }
        }

        /**
         * Format text in editor
         * @param {string} command
         * @param {string|null} option
         * @param {string} editorID
         * @private
         */
        function formatText(command, option, editorID) {
            if (typeof option === "undefined") {
                option = null;
            }
            // restore selection from before clicking on any button
            doRestore(editorID);
            // Temporarily enable designMode so that
            // document.execCommand() will work
            // document.designMode = "ON";
            // Execute the command
            if (command === "heading" && getSelectedText()) {
                // IE workaround
                pasteHTMLAtCaret('<' + option + '>' + getSelectedText() + '</' + option + '>');
            } else if (command === "fontSize" && parseInt(option) > 0) {
                var selection = getSelectedText();
                selection = (selection + '').replace(/([^>\r\n]?)(\r\n|\n\r|\r|\n)/g, '$1' + '<br>' + '$2');
                var html = (settings.useSingleQuotes ? "<span style='font-size:" + option + "px;'>" + selection + "</span>" : '<span style="font-size:' + option + 'px;">' + selection + '</span>');
                pasteHTMLAtCaret(html);
            } else {
                document.execCommand(command, false, option);
            }
            // Disable designMode
            // document.designMode = "OFF";
        }


        /**
         * Update textarea when updating editor
         * @private
         */
        function updateTextarea() {
            var $editor = $('#' + editorID);
            var content = $editor.html();
            if (settings.useSingleQuotes === true) {
                content = changeAttributeQuotes(content);
            }
            $editor.siblings('.richText-initial').val(content);
        }


        /**
         * Update editor when updating textarea
         * @private
         */
        function updateEditor(editorID) {
            var $editor = $('#' + editorID);
            var content = $editor.siblings('.richText-initial').val();
            $editor.html(content);
        }


        /**
         * Save caret position and selection
         * @return object
         **/
        function saveSelection(editorID) {
            var containerEl = document.getElementById(editorID);
            var range, start, end, type;
            if (window.getSelection && document.createRange) {
                var sel = window.getSelection && window.getSelection();
                if (sel && sel.rangeCount > 0 && $(sel.anchorNode).parents('#' + editorID).length > 0) {
                    range = window.getSelection().getRangeAt(0);
                    var preSelectionRange = range.cloneRange();
                    preSelectionRange.selectNodeContents(containerEl);
                    preSelectionRange.setEnd(range.startContainer, range.startOffset);

                    start = preSelectionRange.toString().length;
                    end = (start + range.toString().length);

                    type = (start === end ? 'caret' : 'selection');
                    anchor = sel.anchorNode; //(type === "caret" && sel.anchorNode.tagName ? sel.anchorNode : false);
                    start = (type === 'caret' && anchor !== false ? start : preSelectionRange.toString().length);
                    end = (type === 'caret' && anchor !== false ? end : (start + range.toString().length));

                    return {
                        start: start,
                        end: end,
                        type: type,
                        anchor: anchor,
                        editorID: editorID
                    }
                }
            }
            return (savedSelection ? savedSelection : {
                start: 0,
                end: 0
            });
        }


        /**
         * Restore selection
         **/
        function restoreSelection(editorID, media, url) {
            var containerEl = document.getElementById(editorID);
            var savedSel = savedSelection;
            if (!savedSel) {
                // fix selection if editor has not been focused
                savedSel = {
                    'start': 0,
                    'end': 0,
                    'type': 'caret',
                    'editorID': editorID,
                    'anchor': $('#' + editorID).children('div')[0]
                };
            }

            if (savedSel.editorID !== editorID) {
                return false;
            } else if (media === true) {
                containerEl = (savedSel.anchor ? savedSel.anchor : containerEl); // fix selection issue
            } else if (url === true) {
                if (savedSel.start === 0 && savedSel.end === 0) {
                    containerEl = (savedSel.anchor ? savedSel.anchor : containerEl); // fix selection issue
                }
            }

            if (window.getSelection && document.createRange) {
                var charIndex = 0, range = document.createRange();
                if (!range || !containerEl) {
                    window.getSelection().removeAllRanges();
                    return true;
                }
                range.setStart(containerEl, 0);
                range.collapse(true);
                var nodeStack = [containerEl], node, foundStart = false, stop = false;

                while (!stop && (node = nodeStack.pop())) {
                    if (node.nodeType === 3) {
                        var nextCharIndex = charIndex + node.length;
                        if (!foundStart && savedSel.start >= charIndex && savedSel.start <= nextCharIndex) {
                            range.setStart(node, savedSel.start - charIndex);
                            foundStart = true;
                        }
                        if (foundStart && savedSel.end >= charIndex && savedSel.end <= nextCharIndex) {
                            range.setEnd(node, savedSel.end - charIndex);
                            stop = true;
                        }
                        charIndex = nextCharIndex;
                    } else {
                        var i = node.childNodes.length;
                        while (i--) {
                            nodeStack.push(node.childNodes[i]);
                        }
                    }
                }
                var sel = window.getSelection();
                sel.removeAllRanges();
                sel.addRange(range);
            }
        }


        /**
         * Save caret position and selection
         * @return object
         **/
        /*
       function saveSelection(editorID) {
           var containerEl = document.getElementById(editorID);
           var start;
           if (window.getSelection && document.createRange) {
               var sel = window.getSelection && window.getSelection();
               if (sel && sel.rangeCount > 0) {
                   var range = window.getSelection().getRangeAt(0);
                   var preSelectionRange = range.cloneRange();
                   preSelectionRange.selectNodeContents(containerEl);
                   preSelectionRange.setEnd(range.startContainer, range.startOffset);
                   start = preSelectionRange.toString().length;

                   return {
                       start: start,
                       end: start + range.toString().length,
                       editorID: editorID
                   }
               } else {
                   return (savedSelection ? savedSelection : {
                       start: 0,
                       end: 0
                   });
               }
           } else if (document.selection && document.body.createTextRange) {
               var selectedTextRange = document.selection.createRange();
               var preSelectionTextRange = document.body.createTextRange();
               preSelectionTextRange.moveToElementText(containerEl);
               preSelectionTextRange.setEndPoint("EndToStart", selectedTextRange);
               start = preSelectionTextRange.text.length;

               return {
                   start: start,
                   end: start + selectedTextRange.text.length,
                   editorID: editorID
               };
           }
       }
       */

        /**
         * Restore selection
         **/

        /*
       function restoreSelection(editorID) {
           var containerEl = document.getElementById(editorID);
           var savedSel = savedSelection;
           if(savedSel.editorID !== editorID) {
               return false;
           }
           if (window.getSelection && document.createRange) {
               var charIndex = 0, range = document.createRange();
               range.setStart(containerEl, 0);
               range.collapse(true);
               var nodeStack = [containerEl], node, foundStart = false, stop = false;

               while (!stop && (node = nodeStack.pop())) {
                   if (node.nodeType === 3) {
                       var nextCharIndex = charIndex + node.length;
                       if (!foundStart && savedSel.start >= charIndex && savedSel.start <= nextCharIndex) {
                           range.setStart(node, savedSel.start - charIndex);
                           foundStart = true;
                       }
                       if (foundStart && savedSel.end >= charIndex && savedSel.end <= nextCharIndex) {
                           range.setEnd(node, savedSel.end - charIndex);
                           stop = true;
                       }
                       charIndex = nextCharIndex;
                   } else {
                       var i = node.childNodes.length;
                       while (i--) {
                           nodeStack.push(node.childNodes[i]);
                       }
                   }
               }
               var sel = window.getSelection();
               sel.removeAllRanges();
               sel.addRange(range);
           } else if (document.selection && document.body.createTextRange) {
               var textRange = document.body.createTextRange();
               textRange.moveToElementText(containerEl);
               textRange.collapse(true);
               textRange.moveEnd("character", savedSel.end);
               textRange.moveStart("character", savedSel.start);
               textRange.select();
           }
       }
       */

        /**
         * Enables tabbing/shift-tabbing between contentEditable table cells
         * @param {Window} win - Active window context.
         * @param {Event} e - jQuery Event object for the keydown that fired.
         */
        function tabifyEditableTable(win, e) {

            if (e.keyCode !== 9) {
                return false;
            }

            var sel;
            if (win.getSelection) {
                sel = win.getSelection();
                if (sel.rangeCount > 0) {

                    var textNode = null,
                        direction = null;

                    if (!e.shiftKey) {
                        direction = "next";
                        textNode = (sel.focusNode.nodeName === "TD")
                            ? (sel.focusNode.nextSibling != null)
                                ? sel.focusNode.nextSibling
                                : (sel.focusNode.parentNode.nextSibling != null)
                                    ? sel.focusNode.parentNode.nextSibling.childNodes[0]
                                    : null
                            : (sel.focusNode.parentNode.nextSibling != null)
                                ? sel.focusNode.parentNode.nextSibling
                                : (sel.focusNode.parentNode.parentNode.nextSibling != null)
                                    ? sel.focusNode.parentNode.parentNode.nextSibling.childNodes[0]
                                    : null;
                    } else {
                        direction = "previous";
                        textNode = (sel.focusNode.nodeName === "TD")
                            ? (sel.focusNode.previousSibling != null)
                                ? sel.focusNode.previousSibling
                                : (sel.focusNode.parentNode.previousSibling != null)
                                    ? sel.focusNode.parentNode.previousSibling.childNodes[sel.focusNode.parentNode.previousSibling.childNodes.length - 1]
                                    : null
                            : (sel.focusNode.parentNode.previousSibling != null)
                                ? sel.focusNode.parentNode.previousSibling
                                : (sel.focusNode.parentNode.parentNode.previousSibling != null)
                                    ? sel.focusNode.parentNode.parentNode.previousSibling.childNodes[sel.focusNode.parentNode.parentNode.previousSibling.childNodes.length - 1]
                                    : null;
                    }

                    if (textNode != null) {
                        sel.collapse(textNode, Math.min(textNode.length, sel.focusOffset + 1));
                        if (textNode.textContent != null) {
                            sel.selectAllChildren(textNode);
                        }
                        e.preventDefault();
                        return true;
                    } else if (textNode === null && direction === "next" && sel.focusNode.nodeName === "TD") {
                        // add new row on TAB if arrived at the end of the row
                        var $table = $(sel.focusNode).parents("table");
                        var cellsPerLine = $table.find("tr").first().children("td").length;
                        var $tr = $("<tr />");
                        var $td = $("<td />");
                        for (var i = 1; i <= cellsPerLine; i++) {
                            $tr.append($td.clone());
                        }
                        $table.append($tr);
                        // simulate tabing through table
                        tabifyEditableTable(window, {
                            keyCode: 9, shiftKey: false, preventDefault: function () {
                            }
                        });
                    }
                }
            }
            return false;
        }

        /**
         * Returns the text from the current selection
         * @private
         * @return {string|boolean}
         */
        function getSelectedText() {
            var range;
            if (window.getSelection) {  // all browsers, except IE before version 9
                range = window.getSelection();
                return range.toString() ? range.toString() : range.focusNode.nodeValue;
            } else if (document.selection.createRange) { // Internet Explorer
                range = document.selection.createRange();
                return range.text;
            }
            return false;
        }

        /**
         * Save selection
         */
        function doSave(editorID) {
            var $textarea = $('.richText-editor#' + editorID).siblings('.richText-initial');
            addHistory($textarea.val(), editorID);
            savedSelection = saveSelection(editorID);
        }

        /**
         * @param editorID
         * @returns {boolean}
         */
        function updateMaxLength(editorID) {
            var $editorInner = $('.richText-editor#' + editorID);
            var $editor = $editorInner.parents('.richText');
            if (!$editor.data('maxlength')) {
                return true;
            }
            var color;
            var maxLength = parseInt($editor.data('maxlength'));
            var content = $editorInner.text();
            var percentage = (content.length / maxLength) * 100;
            if (percentage > 99) {
                color = 'red';
            } else if (percentage >= 90) {
                color = 'orange';
            } else {
                color = 'black';
            }

            $editor.find('.richText-length').html('<span class="' + color + '">' + content.length + '</span>/' + maxLength);

            if (content.length > maxLength) {
                // content too long
                undo($editor);
                return false;
            }
            return true;
        }

        /**
         * Add to history
         * @param val Editor content
         * @param id Editor ID
         */
        function addHistory(val, id) {
            if (!history[id]) {
                return false;
            }
            if (history[id].length - 1 > historyPosition[id]) {
                history[id].length = historyPosition[id] + 1;
            }

            if (history[id][history[id].length - 1] !== val) {
                history[id].push(val);
            }

            historyPosition[id] = history[id].length - 1;
            setHistoryButtons(id);
        }

        function setHistoryButtons(id) {
            if (historyPosition[id] <= 0) {
                $editor.find(".richText-undo").addClass("is-disabled");
            } else {
                $editor.find(".richText-undo").removeClass("is-disabled");
            }

            if (historyPosition[id] >= history[id].length - 1 || history[id].length === 0) {
                $editor.find(".richText-redo").addClass("is-disabled");
            } else {
                $editor.find(".richText-redo").removeClass("is-disabled");
            }
        }

        /**
         * Undo
         * @param $editor
         */
        function undo($editor) {
            var id = $editor.children('.richText-editor').attr('id');
            historyPosition[id]--;
            if (!historyPosition[id] && historyPosition[id] !== 0) {
                return false;
            }
            var value = history[id][historyPosition[id]];
            $editor.find('textarea').val(value);
            $editor.find('.richText-editor').html(value);
            setHistoryButtons(id);
        }

        /**
         * Undo
         * @param $editor
         */
        function redo($editor) {
            var id = $editor.children('.richText-editor').attr('id');
            historyPosition[id]++;
            if (!historyPosition[id] && historyPosition[id] !== 0) {
                return false;
            }
            var value = history[id][historyPosition[id]];
            $editor.find('textarea').val(value);
            $editor.find('.richText-editor').html(value);
            setHistoryButtons(id);
        }

        /**
         * Restore selection
         */
        function doRestore(id) {
            if (savedSelection) {
                restoreSelection((id ? id : savedSelection.editorID));
            }
        }

        /**
         * Paste HTML at caret position
         * @param {string} html HTML code
         * @private
         */
        function pasteHTMLAtCaret(html) {
            // add HTML code for Internet Explorer
            var sel, range;
            if (window.getSelection) {
                // IE9 and non-IE
                sel = window.getSelection();
                if (sel.getRangeAt && sel.rangeCount) {
                    range = sel.getRangeAt(0);
                    range.deleteContents();

                    // Range.createContextualFragment() would be useful here but is
                    // only relatively recently standardized and is not supported in
                    // some browsers (IE9, for one)
                    var el = document.createElement("div");
                    el.innerHTML = html;
                    var frag = document.createDocumentFragment(), node, lastNode;
                    while ((node = el.firstChild)) {
                        lastNode = frag.appendChild(node);
                    }
                    range.insertNode(frag);

                    // Preserve the selection
                    if (lastNode) {
                        range = range.cloneRange();
                        range.setStartAfter(lastNode);
                        range.collapse(true);
                        sel.removeAllRanges();
                        sel.addRange(range);
                    }
                }
            } else if (document.selection && document.selection.type !== "Control") {
                // IE < 9
                document.selection.createRange().pasteHTML(html);
            }
        }


        /**
         * Change quotes around HTML attributes
         * @param  {string} string
         * @return {string}
         */
        function changeAttributeQuotes(string) {
            if (!string) {
                return '';
            }

            var regex;
            var rstring;
            if (settings.useSingleQuotes === true) {
                regex = /\s+(\w+\s*=\s*(["][^"]*["])|(['][^']*[']))+/g;
                rstring = string.replace(regex, function ($0, $1, $2) {
                    if (!$2) {
                        return $0;
                    }
                    return $0.replace($2, $2.replace(/\"/g, "'"));
                });
            } else {
                regex = /\s+(\w+\s*=\s*(['][^']*['])|(["][^"]*["]))+/g;
                rstring = string.replace(regex, function ($0, $1, $2) {
                    if (!$2) {
                        return $0;
                    }
                    return $0.replace($2, $2.replace(/'/g, '"'));
                });
            }
            return rstring;
        }


        /**
         * Load colors for font or background
         * @param {string} command Command
         * @returns {string}
         * @private
         */
        function loadColors(command) {
            var colors = [];
            var result = '';

            colors["#FFFFFF"] = settings.translations.white;
            colors["#000000"] = settings.translations.black;
            colors["#7F6000"] = settings.translations.brown;
            colors["#938953"] = settings.translations.beige;
            colors["#1F497D"] = settings.translations.darkBlue;
            colors["blue"] = settings.translations.blue;
            colors["#4F81BD"] = settings.translations.lightBlue;
            colors["#953734"] = settings.translations.darkRed;
            colors["red"] = settings.translations.red;
            colors["#4F6128"] = settings.translations.darkGreen;
            colors["green"] = settings.translations.green;
            colors["#3F3151"] = settings.translations.purple;
            colors["#31859B"] = settings.translations.darkTurquois;
            colors["#4BACC6"] = settings.translations.turquois;
            colors["#E36C09"] = settings.translations.darkOrange;
            colors["#F79646"] = settings.translations.orange;
            colors["#FFFF00"] = settings.translations.yellow;

            if (settings.colors && settings.colors.length > 0) {
                colors = settings.colors;
            }

            for (var i in colors) {
                result += '<li class="inline"><a data-command="' + command + '" data-option="' + i + '" style="text-align:left;" title="' + colors[i] + '"><span class="box-color" style="background-color:' + i + '"></span></a></li>';
            }
            return result;
        }


        /**
         * Toggle (show/hide) code or editor
         * @private
         */
        function toggleCode(editorID) {
            doRestore(editorID);
            if ($editor.find('.richText-editor').is(":visible")) {
                // show code
                $editor.find('.richText-initial').show();
                $editor.find('.richText-editor').hide();
                // disable non working buttons
                $('.richText-toolbar').find('.richText-btn').each(function () {
                    if ($(this).children('.fa-code').length === 0) {
                        $(this).parent('li').attr("data-disable", "true");
                    }
                });
                convertCaretPosition(editorID, savedSelection);
            } else {
                // show editor
                $editor.find('.richText-initial').hide();
                $editor.find('.richText-editor').show();
                convertCaretPosition(editorID, savedSelection, true);
                // enable all buttons again
                $('.richText-toolbar').find('li').removeAttr("data-disable");
            }
        }


        /**
         * Convert caret position from editor to code view (or in reverse)
         * @param {string} editorID
         * @param {object} selection
         * @param {boolean} reverse
         **/
        function convertCaretPosition(editorID, selection, reverse) {
            var $editor = $('#' + editorID);
            var $textarea = $editor.siblings(".richText-initial");

            var code = $textarea.val();
            if (!selection || !code) {
                return {start: 0, end: 0};
            }

            if (reverse === true) {
                savedSelection = {start: $editor.text().length, end: $editor.text().length, editorID: editorID};
                restoreSelection(editorID);
                return true;
            }
            selection.node = $textarea[0];
            var states = {
                start: false,
                end: false,
                tag: false,
                isTag: false,
                tagsCount: 0,
                isHighlight: (selection.start !== selection.end)
            };
            for (var i = 0; i < code.length; i++) {
                if (code[i] === "<") {
                    // HTML tag starts
                    states.isTag = true;
                    states.tag = false;
                    states.tagsCount++;
                } else if (states.isTag === true && code[i] !== ">") {
                    states.tagsCount++;
                } else if (states.isTag === true && code[i] === ">") {
                    states.isTag = false;
                    states.tag = true;
                    states.tagsCount++;
                } else if (states.tag === true) {
                    states.tag = false;
                }

                if (!reverse) {
                    if ((selection.start + states.tagsCount) <= i && states.isHighlight && !states.isTag && !states.tag && !states.start) {
                        selection.start = i;
                        states.start = true;
                    } else if ((selection.start + states.tagsCount) <= i + 1 && !states.isHighlight && !states.isTag && !states.tag && !states.start) {
                        selection.start = i + 1;
                        states.start = true;
                    }
                    if ((selection.end + states.tagsCount) <= i + 1 && !states.isTag && !states.tag && !states.end) {
                        selection.end = i + 1;
                        states.end = true;
                    }
                }

            }
            createSelection(selection.node, selection.start, selection.end);
            return selection;
        }

        /**
         * Create selection on node element
         * @param {Node} field
         * @param {int} start
         * @param {int} end
         **/
        function createSelection(field, start, end) {
            if (field.createTextRange) {
                var selRange = field.createTextRange();
                selRange.collapse(true);
                selRange.moveStart('character', start);
                selRange.moveEnd('character', end);
                selRange.select();
                field.focus();
            } else if (field.setSelectionRange) {
                field.focus();
                field.setSelectionRange(start, end);
            } else if (typeof field.selectionStart != 'undefined') {
                field.selectionStart = start;
                field.selectionEnd = end;
                field.focus();
            }
        }


        /**
         * Get video embed code from URL
         * @param {string} url Video URL
         * @param {string} size Size in the form of widthxheight
         * @return {string|boolean}
         * @private
         **/
        function getVideoCode(url, size) {
            var video = getVideoID(url);
            var responsive = false, success = false;

            if (!video) {
                // video URL not supported
                return false;
            }

            if (!size) {
                size = "640x360";
                size = size.split("x");
            } else if (size !== "responsive") {
                size = size.split("x");
            } else {
                responsive = true;
                size = "640x360";
                size = size.split("x");
            }

            var html = '<br><br>';
            if (responsive === true) {
                html += '<div class="videoEmbed" style="position:relative;height:0;padding-bottom:56.25%">';
            }
            var allowfullscreen = 'webkitallowfullscreen mozallowfullscreen allowfullscreen';

            if (video.platform === "YouTube") {
                var youtubeDomain = (settings.youtubeCookies ? 'www.youtube.com' : 'www.youtube-nocookie.com');
                html += '<iframe src="https://' + youtubeDomain + '/embed/' + video.id + '?ecver=2" width="' + size[0] + '" height="' + size[1] + '" frameborder="0"' + (responsive === true ? ' style="position:absolute;width:100%;height:100%;left:0"' : '') + ' ' + allowfullscreen + '></iframe>';
                success = true;
            } else if (video.platform === "Vimeo") {
                html += '<iframe src="https://player.vimeo.com/video/' + video.id + '" width="' + size[0] + '" height="' + size[1] + '" frameborder="0"' + (responsive === true ? ' style="position:absolute;width:100%;height:100%;left:0"' : '') + ' ' + allowfullscreen + '></iframe>';
                success = true;
            } else if (video.platform === "Facebook") {
                html += '<iframe src="https://www.facebook.com/plugins/video.php?href=' + encodeURI(url) + '&show_text=0&width=' + size[0] + '" width="' + size[0] + '" height="' + size[1] + '" style="' + (responsive === true ? 'position:absolute;width:100%;height:100%;left:0;border:none;overflow:hidden"' : 'border:none;overflow:hidden') + '" scrolling="no" frameborder="0" allowTransparency="true" ' + allowfullscreen + '></iframe>';
                success = true;
            } else if (video.platform === "Dailymotion") {
                html += '<iframe frameborder="0" width="' + size[0] + '" height="' + size[1] + '" src="//www.dailymotion.com/embed/video/' + video.id + '"' + (responsive === true ? ' style="position:absolute;width:100%;height:100%;left:0"' : '') + ' ' + allowfullscreen + '></iframe>';
                success = true;
            }

            if (responsive === true) {
                html += '</div>';
            }
            html += '<br><br>';

            if (success) {
                return html;
            }
            return false;
        }

        /**
         * Returns the unique video ID
         * @param {string} url
         * return {object|boolean}
         **/
        function getVideoID(url) {
            var vimeoRegExp = /(?:http?s?:\/\/)?(?:www\.)?(?:vimeo\.com)\/?(.+)/;
            var youtubeRegExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*/;
            var facebookRegExp = /(?:http?s?:\/\/)?(?:www\.)?(?:facebook\.com)\/.*\/videos\/[0-9]+/;
            var dailymotionRegExp = /(?:http?s?:\/\/)?(?:www\.)?(?:dailymotion\.com)\/video\/([a-zA-Z0-9]+)/;
            var youtubeMatch = url.match(youtubeRegExp);
            var vimeoMatch = url.match(vimeoRegExp);
            var facebookMatch = url.match(facebookRegExp);
            var dailymotionMatch = url.match(dailymotionRegExp);

            if (youtubeMatch && youtubeMatch[2].length === 11) {
                return {
                    "platform": "YouTube",
                    "id": youtubeMatch[2]
                };
            } else if (vimeoMatch && vimeoMatch[1]) {
                return {
                    "platform": "Vimeo",
                    "id": vimeoMatch[1]
                };
            } else if (facebookMatch && facebookMatch[0]) {
                return {
                    "platform": "Facebook",
                    "id": facebookMatch[0]
                };
            } else if (dailymotionMatch && dailymotionMatch[1]) {
                return {
                    "platform": "Dailymotion",
                    "id": dailymotionMatch[1]
                };
            }

            return false;
        }


        /**
         * Fix the first line as by default the first line has no tag container
         */
        function fixFirstLine() {
            if ($editor && !$editor.find(".richText-editor").html()) {
                // set first line with the right tags
                if (settings.useParagraph !== false) {
                    $editor.find(".richText-editor").html('<p><br></p>');
                } else {
                    $editor.find(".richText-editor").html('<div><br></div>');
                }
            } else {
                // replace tags, to force <div> or <p> tags and fix issues
                if (settings.useParagraph !== false) {
                    $editor.find(".richText-editor").find('div:not(.videoEmbed)').replaceWith(function () {
                        return $('<p />', {html: $(this).html()});
                    });
                } else {
                    $editor.find(".richText-editor").find('p').replaceWith(function () {
                        return $('<div />', {html: $(this).html()});
                    });
                }
            }
            updateTextarea();
        }

        return $(this);
    };

    $.fn.unRichText = function (options) {

        // set default options
        // and merge them with the parameter options
        var settings = $.extend({
            delay: 0 // delay in ms
        }, options);

        var $editor, $textarea, $main;
        var $el = $(this);

        /**
         * Initialize undoing RichText and call remove() method
         */
        function init() {

            if ($el.hasClass('richText')) {
                $main = $el;
            } else if ($el.hasClass('richText-initial') || $el.hasClass('richText-editor')) {
                $main = $el.parents('.richText');
            }

            if (!$main) {
                // node element does not correspond to RichText elements
                return false;
            }

            $editor = $main.find('.richText-editor');
            $textarea = $main.find('.richText-initial');

            if (parseInt(settings.delay) > 0) {
                // a delay has been set
                setTimeout(remove, parseInt(settings.delay));
            } else {
                remove();
            }
        }

        init();

        /**
         * Remove RichText elements
         */
        function remove() {
            $main.find('.richText-toolbar').remove();
            $main.find('.richText-editor').remove();
            $textarea
                .unwrap('.richText')
                .data('editor', 'richText')
                .removeClass('richText-initial')
                .show();

            if (settings.callback && typeof settings.callback === 'function') {
                settings.callback();
            }
        }

    };

}(jQuery));

/*
 *  webui popover plugin  - v1.2.17
 *  A lightWeight popover plugin with jquery ,enchance the  popover plugin of bootstrap with some awesome new features. It works well with bootstrap ,but bootstrap is not necessary!
 *  https://github.com/sandywalker/webui-popover
 *
 *  Made by Sandy Duan
 *  Under MIT License
 */
!function(a,b,c){"use strict";!function(b){"function"==typeof define&&define.amd?define(["jquery"],b):"object"==typeof exports?module.exports=b(require("jquery")):b(a.jQuery)}(function(d){function e(a,b){return this.$element=d(a),b&&("string"===d.type(b.delay)||"number"===d.type(b.delay))&&(b.delay={show:b.delay,hide:b.delay}),this.options=d.extend({},i,b),this._defaults=i,this._name=f,this._targetclick=!1,this.init(),k.push(this.$element),this}var f="webuiPopover",g="webui-popover",h="webui.popover",i={placement:"auto",container:null,width:"auto",height:"auto",trigger:"click",style:"",selector:!1,delay:{show:null,hide:300},async:{type:"GET",before:null,success:null,error:null},cache:!0,multi:!1,arrow:!0,title:"",content:"",closeable:!1,padding:!0,url:"",type:"html",direction:"",animation:null,template:'<div class="webui-popover"><div class="webui-arrow"></div><div class="webui-popover-inner"><a href="#" class="close"></a><h3 class="webui-popover-title"></h3><div class="webui-popover-content"><i class="icon-refresh"></i> <p>&nbsp;</p></div></div></div>',backdrop:!1,dismissible:!0,onShow:null,onHide:null,abortXHR:!0,autoHide:!1,offsetTop:0,offsetLeft:0,iframeOptions:{frameborder:"0",allowtransparency:"true",id:"",name:"",scrolling:"",onload:"",height:"",width:""},hideEmpty:!1},j=g+"-rtl",k=[],l=d('<div class="webui-popover-backdrop"></div>'),m=0,n=!1,o=-2e3,p=d(b),q=function(a,b){return isNaN(a)?b||0:Number(a)},r=function(a){return a.data("plugin_"+f)},s=function(){for(var a=null,b=0;b<k.length;b++)a=r(k[b]),a&&a.hide(!0);p.trigger("hiddenAll."+h)},t=function(a){for(var b=null,c=0;c<k.length;c++)b=r(k[c]),b&&b.id!==a.id&&b.hide(!0);p.trigger("hiddenAll."+h)},u="ontouchstart"in b.documentElement&&/Mobi/.test(navigator.userAgent),v=function(a){var b={x:0,y:0};if("touchstart"===a.type||"touchmove"===a.type||"touchend"===a.type||"touchcancel"===a.type){var c=a.originalEvent.touches[0]||a.originalEvent.changedTouches[0];b.x=c.pageX,b.y=c.pageY}else("mousedown"===a.type||"mouseup"===a.type||"click"===a.type)&&(b.x=a.pageX,b.y=a.pageY);return b};e.prototype={init:function(){if(this.$element[0]instanceof b.constructor&&!this.options.selector)throw new Error("`selector` option must be specified when initializing "+this.type+" on the window.document object!");"manual"!==this.getTrigger()&&(u?this.$element.off("touchend",this.options.selector).on("touchend",this.options.selector,d.proxy(this.toggle,this)):"click"===this.getTrigger()?this.$element.off("click",this.options.selector).on("click",this.options.selector,d.proxy(this.toggle,this)):"hover"===this.getTrigger()&&this.$element.off("mouseenter mouseleave click",this.options.selector).on("mouseenter",this.options.selector,d.proxy(this.mouseenterHandler,this)).on("mouseleave",this.options.selector,d.proxy(this.mouseleaveHandler,this))),this._poped=!1,this._inited=!0,this._opened=!1,this._idSeed=m,this.id=f+this._idSeed,this.options.container=d(this.options.container||b.body).first(),this.options.backdrop&&l.appendTo(this.options.container).hide(),m++,"sticky"===this.getTrigger()&&this.show(),this.options.selector&&(this._options=d.extend({},this.options,{selector:""}))},destroy:function(){for(var a=-1,b=0;b<k.length;b++)if(k[b]===this.$element){a=b;break}k.splice(a,1),this.hide(),this.$element.data("plugin_"+f,null),"click"===this.getTrigger()?this.$element.off("click"):"hover"===this.getTrigger()&&this.$element.off("mouseenter mouseleave"),this.$target&&this.$target.remove()},getDelegateOptions:function(){var a={};return this._options&&d.each(this._options,function(b,c){i[b]!==c&&(a[b]=c)}),a},hide:function(a,b){if((a||"sticky"!==this.getTrigger())&&this._opened){b&&(b.preventDefault(),b.stopPropagation()),this.xhr&&this.options.abortXHR===!0&&(this.xhr.abort(),this.xhr=null);var c=d.Event("hide."+h);if(this.$element.trigger(c,[this.$target]),this.$target){this.$target.removeClass("in").addClass(this.getHideAnimation());var e=this;setTimeout(function(){e.$target.hide(),e.getCache()||e.$target.remove()},e.getHideDelay())}this.options.backdrop&&l.hide(),this._opened=!1,this.$element.trigger("hidden."+h,[this.$target]),this.options.onHide&&this.options.onHide(this.$target)}},resetAutoHide:function(){var a=this,b=a.getAutoHide();b&&(a.autoHideHandler&&clearTimeout(a.autoHideHandler),a.autoHideHandler=setTimeout(function(){a.hide()},b))},delegate:function(a){var b=d(a).data("plugin_"+f);return b||(b=new e(a,this.getDelegateOptions()),d(a).data("plugin_"+f,b)),b},toggle:function(a){var b=this;a&&(a.preventDefault(),a.stopPropagation(),this.options.selector&&(b=this.delegate(a.currentTarget))),b[b.getTarget().hasClass("in")?"hide":"show"]()},hideAll:function(){s()},hideOthers:function(){t(this)},show:function(){if(!this._opened){var a=this.getTarget().removeClass().addClass(g).addClass(this._customTargetClass);if(this.options.multi||this.hideOthers(),!this.getCache()||!this._poped||""===this.content){if(this.content="",this.setTitle(this.getTitle()),this.options.closeable||a.find(".close").off("click").remove(),this.isAsync()?this.setContentASync(this.options.content):this.setContent(this.getContent()),this.canEmptyHide()&&""===this.content)return;a.show()}this.displayContent(),this.options.onShow&&this.options.onShow(a),this.bindBodyEvents(),this.options.backdrop&&l.show(),this._opened=!0,this.resetAutoHide()}},displayContent:function(){var a=this.getElementPosition(),b=this.getTarget().removeClass().addClass(g).addClass(this._customTargetClass),c=this.getContentElement(),e=b[0].offsetWidth,f=b[0].offsetHeight,i="bottom",k=d.Event("show."+h);if(this.canEmptyHide()){var l=c.children().html();if(null!==l&&0===l.trim().length)return}this.$element.trigger(k,[b]);var m=this.$element.data("width")||this.options.width;""===m&&(m=this._defaults.width),"auto"!==m&&b.width(m);var n=this.$element.data("height")||this.options.height;""===n&&(n=this._defaults.height),"auto"!==n&&c.height(n),this.options.style&&this.$target.addClass(g+"-"+this.options.style),"rtl"!==this.options.direction||c.hasClass(j)||c.addClass(j),this.options.arrow||b.find(".webui-arrow").remove(),b.detach().css({top:o,left:o,display:"block"}),this.getAnimation()&&b.addClass(this.getAnimation()),b.appendTo(this.options.container),i=this.getPlacement(a),this.$element.trigger("added."+h),this.initTargetEvents(),this.options.padding||("auto"!==this.options.height&&c.css("height",c.outerHeight()),this.$target.addClass("webui-no-padding")),this.options.maxHeight&&c.css("maxHeight",this.options.maxHeight),this.options.maxWidth&&c.css("maxWidth",this.options.maxWidth),e=b[0].offsetWidth,f=b[0].offsetHeight;var p=this.getTargetPositin(a,i,e,f);if(this.$target.css(p.position).addClass(i).addClass("in"),"iframe"===this.options.type){var q=b.find("iframe"),r=b.width(),s=q.parent().height();""!==this.options.iframeOptions.width&&"auto"!==this.options.iframeOptions.width&&(r=this.options.iframeOptions.width),""!==this.options.iframeOptions.height&&"auto"!==this.options.iframeOptions.height&&(s=this.options.iframeOptions.height),q.width(r).height(s)}if(this.options.arrow||this.$target.css({margin:0}),this.options.arrow){var t=this.$target.find(".webui-arrow");t.removeAttr("style"),"left"===i||"right"===i?t.css({top:this.$target.height()/2}):("top"===i||"bottom"===i)&&t.css({left:this.$target.width()/2}),p.arrowOffset&&(-1===p.arrowOffset.left||-1===p.arrowOffset.top?t.hide():t.css(p.arrowOffset))}this._poped=!0,this.$element.trigger("shown."+h,[this.$target])},isTargetLoaded:function(){return 0===this.getTarget().find("i.glyphicon-refresh").length},getTriggerElement:function(){return this.$element},getTarget:function(){if(!this.$target){var a=f+this._idSeed;this.$target=d(this.options.template).attr("id",a),this._customTargetClass=this.$target.attr("class")!==g?this.$target.attr("class"):null,this.getTriggerElement().attr("data-target",a)}return this.$target.data("trigger-element")||this.$target.data("trigger-element",this.getTriggerElement()),this.$target},removeTarget:function(){this.$target.remove(),this.$target=null,this.$contentElement=null},getTitleElement:function(){return this.getTarget().find("."+g+"-title")},getContentElement:function(){return this.$contentElement||(this.$contentElement=this.getTarget().find("."+g+"-content")),this.$contentElement},getTitle:function(){return this.$element.attr("data-title")||this.options.title||this.$element.attr("title")},getUrl:function(){return this.$element.attr("data-url")||this.options.url},getAutoHide:function(){return this.$element.attr("data-auto-hide")||this.options.autoHide},getOffsetTop:function(){return q(this.$element.attr("data-offset-top"))||this.options.offsetTop},getOffsetLeft:function(){return q(this.$element.attr("data-offset-left"))||this.options.offsetLeft},getCache:function(){var a=this.$element.attr("data-cache");if("undefined"!=typeof a)switch(a.toLowerCase()){case"true":case"yes":case"1":return!0;case"false":case"no":case"0":return!1}return this.options.cache},getTrigger:function(){return this.$element.attr("data-trigger")||this.options.trigger},getDelayShow:function(){var a=this.$element.attr("data-delay-show");return"undefined"!=typeof a?a:0===this.options.delay.show?0:this.options.delay.show||100},getHideDelay:function(){var a=this.$element.attr("data-delay-hide");return"undefined"!=typeof a?a:0===this.options.delay.hide?0:this.options.delay.hide||100},getAnimation:function(){var a=this.$element.attr("data-animation");return a||this.options.animation},getHideAnimation:function(){var a=this.getAnimation();return a?a+"-out":"out"},setTitle:function(a){var b=this.getTitleElement();a?("rtl"!==this.options.direction||b.hasClass(j)||b.addClass(j),b.html(a)):b.remove()},hasContent:function(){return this.getContent()},canEmptyHide:function(){return this.options.hideEmpty&&"html"===this.options.type},getIframe:function(){var a=d("<iframe></iframe>").attr("src",this.getUrl()),b=this;return d.each(this._defaults.iframeOptions,function(c){"undefined"!=typeof b.options.iframeOptions[c]&&a.attr(c,b.options.iframeOptions[c])}),a},getContent:function(){if(this.getUrl())switch(this.options.type){case"iframe":this.content=this.getIframe();break;case"html":try{this.content=d(this.getUrl()),this.content.is(":visible")||this.content.show()}catch(a){throw new Error("Unable to get popover content. Invalid selector specified.")}}else if(!this.content){var b="";if(b=d.isFunction(this.options.content)?this.options.content.apply(this.$element[0],[this]):this.options.content,this.content=this.$element.attr("data-content")||b,!this.content){var c=this.$element.next();c&&c.hasClass(g+"-content")&&(this.content=c)}}return this.content},setContent:function(a){var b=this.getTarget(),c=this.getContentElement();"string"==typeof a?c.html(a):a instanceof d&&(c.html(""),this.options.cache?a.removeClass(g+"-content").appendTo(c):a.clone(!0,!0).removeClass(g+"-content").appendTo(c)),this.$target=b},isAsync:function(){return"async"===this.options.type},setContentASync:function(a){var b=this;this.xhr||(this.xhr=d.ajax({url:this.getUrl(),type:this.options.async.type,cache:this.getCache(),beforeSend:function(a,c){b.options.async.before&&b.options.async.before(b,a,c)},success:function(c){b.bindBodyEvents(),a&&d.isFunction(a)?b.content=a.apply(b.$element[0],[c]):b.content=c,b.setContent(b.content);var e=b.getContentElement();e.removeAttr("style"),b.displayContent(),b.options.async.success&&b.options.async.success(b,c)},complete:function(){b.xhr=null},error:function(a,c){b.options.async.error&&b.options.async.error(b,a,c)}}))},bindBodyEvents:function(){n||(this.options.dismissible&&"click"===this.getTrigger()?u?p.off("touchstart.webui-popover").on("touchstart.webui-popover",d.proxy(this.bodyTouchStartHandler,this)):(p.off("keyup.webui-popover").on("keyup.webui-popover",d.proxy(this.escapeHandler,this)),p.off("click.webui-popover").on("click.webui-popover",d.proxy(this.bodyClickHandler,this))):"hover"===this.getTrigger()&&p.off("touchend.webui-popover").on("touchend.webui-popover",d.proxy(this.bodyClickHandler,this)))},mouseenterHandler:function(a){var b=this;a&&this.options.selector&&(b=this.delegate(a.currentTarget)),b._timeout&&clearTimeout(b._timeout),b._enterTimeout=setTimeout(function(){b.getTarget().is(":visible")||b.show()},this.getDelayShow())},mouseleaveHandler:function(){var a=this;clearTimeout(a._enterTimeout),a._timeout=setTimeout(function(){a.hide()},this.getHideDelay())},escapeHandler:function(a){27===a.keyCode&&this.hideAll()},bodyTouchStartHandler:function(a){var b=this,c=d(a.currentTarget);c.on("touchend",function(a){b.bodyClickHandler(a),c.off("touchend")}),c.on("touchmove",function(){c.off("touchend")})},bodyClickHandler:function(a){n=!0;for(var b=!0,c=0;c<k.length;c++){var d=r(k[c]);if(d&&d._opened){var e=d.getTarget().offset(),f=e.left,g=e.top,h=e.left+d.getTarget().width(),i=e.top+d.getTarget().height(),j=v(a),l=j.x>=f&&j.x<=h&&j.y>=g&&j.y<=i;if(l){b=!1;break}}}b&&s()},initTargetEvents:function(){"hover"===this.getTrigger()&&this.$target.off("mouseenter mouseleave").on("mouseenter",d.proxy(this.mouseenterHandler,this)).on("mouseleave",d.proxy(this.mouseleaveHandler,this)),this.$target.find(".close").off("click").on("click",d.proxy(this.hide,this,!0))},getPlacement:function(a){var b,c=this.options.container,d=c.innerWidth(),e=c.innerHeight(),f=c.scrollTop(),g=c.scrollLeft(),h=Math.max(0,a.left-g),i=Math.max(0,a.top-f);b="function"==typeof this.options.placement?this.options.placement.call(this,this.getTarget()[0],this.$element[0]):this.$element.data("placement")||this.options.placement;var j="horizontal"===b,k="vertical"===b,l="auto"===b||j||k;return l?b=d/3>h?e/3>i?j?"right-bottom":"bottom-right":2*e/3>i?k?e/2>=i?"bottom-right":"top-right":"right":j?"right-top":"top-right":2*d/3>h?e/3>i?j?d/2>=h?"right-bottom":"left-bottom":"bottom":2*e/3>i?j?d/2>=h?"right":"left":e/2>=i?"bottom":"top":j?d/2>=h?"right-top":"left-top":"top":e/3>i?j?"left-bottom":"bottom-left":2*e/3>i?k?e/2>=i?"bottom-left":"top-left":"left":j?"left-top":"top-left":"auto-top"===b?b=d/3>h?"top-right":2*d/3>h?"top":"top-left":"auto-bottom"===b?b=d/3>h?"bottom-right":2*d/3>h?"bottom":"bottom-left":"auto-left"===b?b=e/3>i?"left-top":2*e/3>i?"left":"left-bottom":"auto-right"===b&&(b=e/3>i?"right-bottom":2*e/3>i?"right":"right-top"),b},getElementPosition:function(){var a=this.$element[0].getBoundingClientRect(),c=this.options.container,e=c.css("position");if(c.is(b.body)||"static"===e)return d.extend({},this.$element.offset(),{width:this.$element[0].offsetWidth||a.width,height:this.$element[0].offsetHeight||a.height});if("fixed"===e){var f=c[0].getBoundingClientRect();return{top:a.top-f.top+c.scrollTop(),left:a.left-f.left+c.scrollLeft(),width:a.width,height:a.height}}return"relative"===e?{top:this.$element.offset().top-c.offset().top,left:this.$element.offset().left-c.offset().left,width:this.$element[0].offsetWidth||a.width,height:this.$element[0].offsetHeight||a.height}:void 0},getTargetPositin:function(a,c,d,e){var f=a,g=this.options.container,h=this.$element.outerWidth(),i=this.$element.outerHeight(),j=b.documentElement.scrollTop+g.scrollTop(),k=b.documentElement.scrollLeft+g.scrollLeft(),l={},m=null,n=this.options.arrow?20:0,p=10,q=n+p>h?n:0,r=n+p>i?n:0,s=0,t=b.documentElement.clientHeight+j,u=b.documentElement.clientWidth+k,v=f.left+f.width/2-q>0,w=f.left+f.width/2+q<u,x=f.top+f.height/2-r>0,y=f.top+f.height/2+r<t;switch(c){case"bottom":l={top:f.top+f.height,left:f.left+f.width/2-d/2};break;case"top":l={top:f.top-e,left:f.left+f.width/2-d/2};break;case"left":l={top:f.top+f.height/2-e/2,left:f.left-d};break;case"right":l={top:f.top+f.height/2-e/2,left:f.left+f.width};break;case"top-right":l={top:f.top-e,left:v?f.left-q:p},m={left:v?Math.min(h,d)/2+q:o};break;case"top-left":s=w?q:-p,l={top:f.top-e,left:f.left-d+f.width+s},m={left:w?d-Math.min(h,d)/2-q:o};break;case"bottom-right":l={top:f.top+f.height,left:v?f.left-q:p},m={left:v?Math.min(h,d)/2+q:o};break;case"bottom-left":s=w?q:-p,l={top:f.top+f.height,left:f.left-d+f.width+s},m={left:w?d-Math.min(h,d)/2-q:o};break;case"right-top":s=y?r:-p,l={top:f.top-e+f.height+s,left:f.left+f.width},m={top:y?e-Math.min(i,e)/2-r:o};break;case"right-bottom":l={top:x?f.top-r:p,left:f.left+f.width},m={top:x?Math.min(i,e)/2+r:o};break;case"left-top":s=y?r:-p,l={top:f.top-e+f.height+s,left:f.left-d},m={top:y?e-Math.min(i,e)/2-r:o};break;case"left-bottom":l={top:x?f.top-r:p,left:f.left-d},m={top:x?Math.min(i,e)/2+r:o}}return l.top+=this.getOffsetTop(),l.left+=this.getOffsetLeft(),{position:l,arrowOffset:m}}},d.fn[f]=function(a,b){var c=[],g=this.each(function(){var g=d.data(this,"plugin_"+f);g?"destroy"===a?g.destroy():"string"==typeof a&&c.push(g[a]()):(a?"string"==typeof a?"destroy"!==a&&(b||(g=new e(this,null),c.push(g[a]()))):"object"==typeof a&&(g=new e(this,a)):g=new e(this,null),d.data(this,"plugin_"+f,g))});return c.length?c:g};var w=function(){var a=function(){s()},b=function(a,b){b=b||{},d(a).webuiPopover(b)},e=function(a){var b=!0;return d(a).each(function(a,e){b=b&&d(e).data("plugin_"+f)!==c}),b},g=function(a,b){b?d(a).webuiPopover(b).webuiPopover("show"):d(a).webuiPopover("show")},h=function(a){d(a).webuiPopover("hide")},j=function(a){i=d.extend({},i,a)},k=function(a,b){var c=d(a).data("plugin_"+f);if(c){var e=c.getCache();c.options.cache=!1,c.options.content=b,c._opened?(c._opened=!1,c.show()):c.isAsync()?c.setContentASync(b):c.setContent(b),c.options.cache=e}},l=function(a,b){var c=d(a).data("plugin_"+f);if(c){var e=c.getCache(),g=c.options.type;c.options.cache=!1,c.options.url=b,c._opened?(c._opened=!1,c.show()):(c.options.type="async",c.setContentASync(c.content)),c.options.cache=e,c.options.type=g}};return{show:g,hide:h,create:b,isCreated:e,hideAll:a,updateContent:k,updateContentAsync:l,setDefaultOptions:j}}();a.WebuiPopovers=w})}(window,document);
var EncryptionKey = "SynergyKey";
function GetStack() {
    return new Stack();
}
function TrimText(text, size) {
    if (text.length > size - 3) {
        return text.substring(0, size - 4) + '...';
    }
    else {
        return text;
    }
}
window.OnPhotoError = (obj) => {
    obj.onerror = null;
    obj.src = '/images/profile.jpg';

};
window.OnDocumentError = (obj) => {
    obj.onerror = null;
    obj.src = '/images/document.png';

};
window.OnLogoError = (obj) => {
    console.log($(obj).parent());

    // obj.src = '/images/logo.png';
    $(obj).parent.css('padding-top', '15px');
    obj.onerror = null;
    $(obj).hide();


};
function InitializeContePlaceHolder() {

    $('.content-editable').each(function (i, obj) {
        var t = $(this);
        if (t.html().trim() === '') {
            t.html(t.data('placeholder'));
        }
    });



}
function ContentEditableBlur(e) {
    var t = $(e);
    var value = t.html().trim();
    if (value === '') {
        t.html(t.data('placeholder'));
        t.addClass('placeholder');
    }
    else {
        t.removeClass('placeholder');
    }
}
function ContentEditableFocus(e) {
    var t = $(e);
    var value = t.html().trim();
    if (value == t.data('placeholder')) {
        t.html('');
        t.removeClass('placeholder');
    }
}

function GenerateGuid() {
    //return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
    //    var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
    //    return v.toString(16);
    //});

    //var timestamp = (new Date().getTime() / 1000 | 0).toString(16);
    //return timestamp + 'xxxxxxxxxxxxxxxx'.replace(/[x]/g, function () {
    //    return (Math.random() * 16 | 0).toString(16);
    //}).toLowerCase();
    function _p8(s) {
        var p = (Math.random().toString(16) + "000000000").substr(2, 8);
        return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
    }
    return _p8() + _p8(true) + _p8(true) + _p8();
}

function UtilityAjax(action, query) {
    var url = "../../utility/" + action + "?" + query;
    var ret = "";
    $.ajax({
        url: url,
        type: 'GET',
        cache: false,
        async: false,
        success: function (data) {
            ret = data;
        },
        error: function (errData) {
            OnError(errData);

        }
    });
    return ret;
}
class Stack {

    constructor() {
        this.items = [];
    }

    push(element) {
        this.items.push(element);
    }
    pop() {
        if (this.items.length == 0)
            return null;
        return this.items.pop();
    }
    peek(index) {
        if (index === null || index === undefined) {
            index = 0;
        }
        if (this.items.length <= index)
            return null;
        return this.items[this.items.length - index - 1];
    }
    isEmpty() {
        return this.items.length == 0;
    }
}
