'use strict';

Object.defineProperty(exports, '__esModule', { value: true });

var core = require('@cubejs-client/core');
require('core-js/modules/es.reflect.construct.js');
require('core-js/modules/es.object.keys.js');
require('core-js/modules/es.symbol.js');
require('core-js/modules/es.array.filter.js');
require('core-js/modules/es.object.get-own-property-descriptor.js');
require('core-js/modules/es.array.for-each.js');
require('core-js/modules/web.dom-collections.for-each.js');
require('core-js/modules/es.object.get-own-property-descriptors.js');
require('core-js/modules/es.object.define-properties.js');
require('core-js/modules/es.object.define-property.js');
var _slicedToArray = require('@babel/runtime/helpers/slicedToArray');
var _defineProperty = require('@babel/runtime/helpers/defineProperty');
var _classCallCheck = require('@babel/runtime/helpers/classCallCheck');
var _createClass = require('@babel/runtime/helpers/createClass');
var _inherits = require('@babel/runtime/helpers/inherits');
var _possibleConstructorReturn = require('@babel/runtime/helpers/possibleConstructorReturn');
var _getPrototypeOf = require('@babel/runtime/helpers/getPrototypeOf');
require('core-js/modules/es.array.iterator.js');
require('core-js/modules/es.object.to-string.js');
require('core-js/modules/es.promise.js');
require('core-js/modules/es.string.iterator.js');
require('core-js/modules/web.dom-collections.iterator.js');
require('core-js/modules/es.array.map.js');
var React = require('react');
var ramda = require('ramda');
var _extends = require('@babel/runtime/helpers/extends');
var _objectWithoutProperties = require('@babel/runtime/helpers/objectWithoutProperties');
var _toConsumableArray = require('@babel/runtime/helpers/toConsumableArray');
var _asyncToGenerator = require('@babel/runtime/helpers/asyncToGenerator');
var _assertThisInitialized = require('@babel/runtime/helpers/assertThisInitialized');
require('core-js/modules/es.function.name.js');
require('core-js/modules/es.array.concat.js');
require('core-js/modules/es.array.splice.js');
require('core-js/modules/es.array.is-array.js');
require('core-js/modules/es.object.entries.js');
require('core-js/modules/es.array.sort.js');
require('core-js/modules/es.array.find.js');
require('core-js/modules/es.array.reduce.js');
var _regeneratorRuntime = require('@babel/runtime/regenerator');

function _interopDefaultLegacy (e) { return e && typeof e === 'object' && 'default' in e ? e : { 'default': e }; }

var _slicedToArray__default = /*#__PURE__*/_interopDefaultLegacy(_slicedToArray);
var _defineProperty__default = /*#__PURE__*/_interopDefaultLegacy(_defineProperty);
var _classCallCheck__default = /*#__PURE__*/_interopDefaultLegacy(_classCallCheck);
var _createClass__default = /*#__PURE__*/_interopDefaultLegacy(_createClass);
var _inherits__default = /*#__PURE__*/_interopDefaultLegacy(_inherits);
var _possibleConstructorReturn__default = /*#__PURE__*/_interopDefaultLegacy(_possibleConstructorReturn);
var _getPrototypeOf__default = /*#__PURE__*/_interopDefaultLegacy(_getPrototypeOf);
var React__default = /*#__PURE__*/_interopDefaultLegacy(React);
var _extends__default = /*#__PURE__*/_interopDefaultLegacy(_extends);
var _objectWithoutProperties__default = /*#__PURE__*/_interopDefaultLegacy(_objectWithoutProperties);
var _toConsumableArray__default = /*#__PURE__*/_interopDefaultLegacy(_toConsumableArray);
var _asyncToGenerator__default = /*#__PURE__*/_interopDefaultLegacy(_asyncToGenerator);
var _assertThisInitialized__default = /*#__PURE__*/_interopDefaultLegacy(_assertThisInitialized);
var _regeneratorRuntime__default = /*#__PURE__*/_interopDefaultLegacy(_regeneratorRuntime);

var CubeContext = /*#__PURE__*/React.createContext(null);

function ownKeys$2(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$2(Object(source), true).forEach(function (key) { _defineProperty__default['default'](target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$2(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _createSuper$1(Derived) { var hasNativeReflectConstruct = _isNativeReflectConstruct$1(); return function _createSuperInternal() { var Super = _getPrototypeOf__default['default'](Derived), result; if (hasNativeReflectConstruct) { var NewTarget = _getPrototypeOf__default['default'](this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn__default['default'](this, result); }; }

function _isNativeReflectConstruct$1() { if (typeof Reflect === "undefined" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === "function") return true; try { Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {})); return true; } catch (e) { return false; } }

var QueryRenderer = /*#__PURE__*/function (_React$Component) {
  _inherits__default['default'](QueryRenderer, _React$Component);

  var _super = _createSuper$1(QueryRenderer);

  function QueryRenderer(props) {
    var _this;

    _classCallCheck__default['default'](this, QueryRenderer);

    _this = _super.call(this, props);
    _this.state = {};
    _this.mutexObj = {};
    return _this;
  }

  _createClass__default['default'](QueryRenderer, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var _this$props = this.props,
          query = _this$props.query,
          queries = _this$props.queries;

      if (query) {
        this.load(query);
      }

      if (queries) {
        this.loadQueries(queries);
      }
    }
  }, {
    key: "shouldComponentUpdate",
    value: function shouldComponentUpdate(nextProps, nextState) {
      var _this$props2 = this.props,
          query = _this$props2.query,
          queries = _this$props2.queries,
          render = _this$props2.render,
          cubejsApi = _this$props2.cubejsApi,
          loadSql = _this$props2.loadSql,
          updateOnlyOnStateChange = _this$props2.updateOnlyOnStateChange;

      if (!updateOnlyOnStateChange) {
        return true;
      }

      return !ramda.equals(nextProps.query, query) || !ramda.equals(nextProps.queries, queries) || (nextProps.render == null || render == null) && nextProps.render !== render || nextProps.cubejsApi !== cubejsApi || nextProps.loadSql !== loadSql || !ramda.equals(nextState, this.state) || nextProps.updateOnlyOnStateChange !== updateOnlyOnStateChange;
    }
  }, {
    key: "componentDidUpdate",
    value: function componentDidUpdate(prevProps) {
      var _this$props3 = this.props,
          query = _this$props3.query,
          queries = _this$props3.queries;

      if (!ramda.equals(prevProps.query, query)) {
        this.load(query);
      }

      if (!ramda.equals(prevProps.queries, queries)) {
        this.loadQueries(queries);
      }
    }
  }, {
    key: "cubejsApi",
    value: function cubejsApi() {
      // eslint-disable-next-line react/destructuring-assignment
      return this.props.cubejsApi || this.context && this.context.cubejsApi;
    }
  }, {
    key: "load",
    value: function load(query) {
      var _this2 = this;

      var resetResultSetOnChange = this.props.resetResultSetOnChange;
      this.setState(_objectSpread$2({
        isLoading: true,
        error: null,
        sqlQuery: null
      }, resetResultSetOnChange ? {
        resultSet: null
      } : {}));
      var loadSql = this.props.loadSql;
      var cubejsApi = this.cubejsApi();

      if (query && QueryRenderer.isQueryPresent(query)) {
        if (loadSql === 'only') {
          cubejsApi.sql(query, {
            mutexObj: this.mutexObj,
            mutexKey: 'sql'
          }).then(function (sqlQuery) {
            return _this2.setState({
              sqlQuery: sqlQuery,
              error: null,
              isLoading: false
            });
          })["catch"](function (error) {
            return _this2.setState(_objectSpread$2(_objectSpread$2({}, resetResultSetOnChange ? {
              resultSet: null
            } : {}), {}, {
              error: error,
              isLoading: false
            }));
          });
        } else if (loadSql) {
          Promise.all([cubejsApi.sql(query, {
            mutexObj: this.mutexObj,
            mutexKey: 'sql'
          }), cubejsApi.load(query, {
            mutexObj: this.mutexObj,
            mutexKey: 'query'
          })]).then(function (_ref) {
            var _ref2 = _slicedToArray__default['default'](_ref, 2),
                sqlQuery = _ref2[0],
                resultSet = _ref2[1];

            return _this2.setState({
              sqlQuery: sqlQuery,
              resultSet: resultSet,
              error: null,
              isLoading: false
            });
          })["catch"](function (error) {
            return _this2.setState(_objectSpread$2(_objectSpread$2({}, resetResultSetOnChange ? {
              resultSet: null
            } : {}), {}, {
              error: error,
              isLoading: false
            }));
          });
        } else {
          cubejsApi.load(query, {
            mutexObj: this.mutexObj,
            mutexKey: 'query'
          }).then(function (resultSet) {
            return _this2.setState({
              resultSet: resultSet,
              error: null,
              isLoading: false
            });
          })["catch"](function (error) {
            return _this2.setState(_objectSpread$2(_objectSpread$2({}, resetResultSetOnChange ? {
              resultSet: null
            } : {}), {}, {
              error: error,
              isLoading: false
            }));
          });
        }
      }
    }
  }, {
    key: "loadQueries",
    value: function loadQueries(queries) {
      var _this3 = this;

      var cubejsApi = this.cubejsApi();
      var resetResultSetOnChange = this.props.resetResultSetOnChange;
      this.setState(_objectSpread$2(_objectSpread$2({
        isLoading: true
      }, resetResultSetOnChange ? {
        resultSet: null
      } : {}), {}, {
        error: null
      }));
      var resultPromises = Promise.all(ramda.toPairs(queries).map(function (_ref3) {
        var _ref4 = _slicedToArray__default['default'](_ref3, 2),
            name = _ref4[0],
            query = _ref4[1];

        return cubejsApi.load(query, {
          mutexObj: _this3.mutexObj,
          mutexKey: name
        }).then(function (r) {
          return [name, r];
        });
      }));
      resultPromises.then(function (resultSet) {
        return _this3.setState({
          resultSet: ramda.fromPairs(resultSet),
          error: null,
          isLoading: false
        });
      })["catch"](function (error) {
        return _this3.setState(_objectSpread$2(_objectSpread$2({}, resetResultSetOnChange ? {
          resultSet: null
        } : {}), {}, {
          error: error,
          isLoading: false
        }));
      });
    }
  }, {
    key: "render",
    value: function render() {
      var _this$state = this.state,
          error = _this$state.error,
          queries = _this$state.queries,
          resultSet = _this$state.resultSet,
          isLoading = _this$state.isLoading,
          sqlQuery = _this$state.sqlQuery;
      var render = this.props.render;
      var loadState = {
        error: error,
        resultSet: queries ? resultSet || {} : resultSet,
        loadingState: {
          isLoading: isLoading
        },
        sqlQuery: sqlQuery
      };

      if (render) {
        return render(loadState);
      }

      return null;
    }
  }], [{
    key: "isQueryPresent",
    value: function isQueryPresent(query) {
      return core.isQueryPresent(query);
    }
  }]);

  return QueryRenderer;
}(React__default['default'].Component);
QueryRenderer.contextType = CubeContext;
QueryRenderer.defaultProps = {
  cubejsApi: null,
  query: null,
  render: null,
  queries: null,
  loadSql: null,
  updateOnlyOnStateChange: false,
  resetResultSetOnChange: true
};

function ownKeys$1(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$1(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$1(Object(source), true).forEach(function (key) { _defineProperty__default['default'](target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$1(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

var QueryRendererWithTotals = function QueryRendererWithTotals(_ref) {
  var query = _ref.query,
      restProps = _objectWithoutProperties__default['default'](_ref, ["query"]);

  return /*#__PURE__*/React__default['default'].createElement(QueryRenderer, _extends__default['default']({
    queries: {
      totals: _objectSpread$1(_objectSpread$1({}, query), {}, {
        dimensions: [],
        timeDimensions: query.timeDimensions ? query.timeDimensions.map(function (td) {
          return _objectSpread$1(_objectSpread$1({}, td), {}, {
            granularity: null
          });
        }) : undefined
      }),
      main: query
    }
  }, restProps));
};

QueryRendererWithTotals.defaultProps = {
  query: null,
  render: null,
  queries: null,
  loadSql: null
};

function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty__default['default'](target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _createSuper(Derived) { var hasNativeReflectConstruct = _isNativeReflectConstruct(); return function _createSuperInternal() { var Super = _getPrototypeOf__default['default'](Derived), result; if (hasNativeReflectConstruct) { var NewTarget = _getPrototypeOf__default['default'](this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn__default['default'](this, result); }; }

function _isNativeReflectConstruct() { if (typeof Reflect === "undefined" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === "function") return true; try { Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {})); return true; } catch (e) { return false; } }
var granularities = [{
  name: undefined,
  title: 'w/o grouping'
}, {
  name: 'second',
  title: 'Second'
}, {
  name: 'minute',
  title: 'Minute'
}, {
  name: 'hour',
  title: 'Hour'
}, {
  name: 'day',
  title: 'Day'
}, {
  name: 'week',
  title: 'Week'
}, {
  name: 'month',
  title: 'Month'
}, {
  name: 'year',
  title: 'Year'
}];

var QueryBuilder = /*#__PURE__*/function (_React$Component) {
  _inherits__default['default'](QueryBuilder, _React$Component);

  var _super = _createSuper(QueryBuilder);

  function QueryBuilder(props) {
    var _this;

    _classCallCheck__default['default'](this, QueryBuilder);

    _this = _super.call(this, props);

    _defineProperty__default['default'](_assertThisInitialized__default['default'](_this), "fetchMeta", /*#__PURE__*/_asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee() {
      var meta, metaError;
      return _regeneratorRuntime__default['default'].wrap(function _callee$(_context) {
        while (1) {
          switch (_context.prev = _context.next) {
            case 0:
              metaError = null;
              _context.prev = 1;

              _this.setState({
                isFetchingMeta: true
              });

              _context.next = 5;
              return _this.cubejsApi().meta();

            case 5:
              meta = _context.sent;
              _context.next = 11;
              break;

            case 8:
              _context.prev = 8;
              _context.t0 = _context["catch"](1);
              metaError = _context.t0;

            case 11:
              _this.setState({
                meta: meta,
                metaError: metaError,
                isFetchingMeta: false
              }, function () {
                // Run update query to force viz state update
                // This will catch any new missing members, and also validate the query against the new meta
                _this.updateQuery({});
              });

            case 12:
            case "end":
              return _context.stop();
          }
        }
      }, _callee, null, [[1, 8]]);
    })));

    _this.state = _objectSpread(_objectSpread({
      query: props.defaultQuery || props.query,
      chartType: props.defaultChartType,
      validatedQuery: props.query,
      // deprecated, validatedQuery should not be set until after dry-run for safety
      missingMembers: [],
      isFetchingMeta: false,
      dryRunResponse: null
    }, props.vizState), props.initialVizState);
    _this.mutexObj = {};
    return _this;
  }

  _createClass__default['default'](QueryBuilder, [{
    key: "componentDidMount",
    value: function () {
      var _componentDidMount = _asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee2() {
        return _regeneratorRuntime__default['default'].wrap(function _callee2$(_context2) {
          while (1) {
            switch (_context2.prev = _context2.next) {
              case 0:
                _context2.next = 2;
                return this.fetchMeta();

              case 2:
              case "end":
                return _context2.stop();
            }
          }
        }, _callee2, this);
      }));

      function componentDidMount() {
        return _componentDidMount.apply(this, arguments);
      }

      return componentDidMount;
    }()
  }, {
    key: "componentDidUpdate",
    value: function () {
      var _componentDidUpdate = _asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee4(prevProps) {
        var _this2 = this;

        var _this$props, schemaVersion, onSchemaChange, meta, newMeta;

        return _regeneratorRuntime__default['default'].wrap(function _callee4$(_context4) {
          while (1) {
            switch (_context4.prev = _context4.next) {
              case 0:
                _this$props = this.props, schemaVersion = _this$props.schemaVersion, onSchemaChange = _this$props.onSchemaChange;
                meta = this.state.meta;

                if (!(prevProps.schemaVersion !== schemaVersion)) {
                  _context4.next = 13;
                  break;
                }

                _context4.prev = 3;
                _context4.next = 6;
                return this.cubejsApi().meta();

              case 6:
                newMeta = _context4.sent;

                if (!ramda.equals(newMeta, meta) && typeof onSchemaChange === 'function') {
                  onSchemaChange({
                    schemaVersion: schemaVersion,
                    refresh: function () {
                      var _refresh = _asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee3() {
                        return _regeneratorRuntime__default['default'].wrap(function _callee3$(_context3) {
                          while (1) {
                            switch (_context3.prev = _context3.next) {
                              case 0:
                                _context3.next = 2;
                                return _this2.fetchMeta();

                              case 2:
                              case "end":
                                return _context3.stop();
                            }
                          }
                        }, _callee3);
                      }));

                      function refresh() {
                        return _refresh.apply(this, arguments);
                      }

                      return refresh;
                    }()
                  });
                }

                _context4.next = 13;
                break;

              case 10:
                _context4.prev = 10;
                _context4.t0 = _context4["catch"](3);
                // eslint-disable-next-line
                this.setState({
                  metaError: _context4.t0
                });

              case 13:
              case "end":
                return _context4.stop();
            }
          }
        }, _callee4, this, [[3, 10]]);
      }));

      function componentDidUpdate(_x) {
        return _componentDidUpdate.apply(this, arguments);
      }

      return componentDidUpdate;
    }()
  }, {
    key: "cubejsApi",
    value: function cubejsApi() {
      var cubejsApi = this.props.cubejsApi; // eslint-disable-next-line react/destructuring-assignment

      return cubejsApi || this.context && this.context.cubejsApi;
    }
  }, {
    key: "getMissingMembers",
    value: function getMissingMembers(query, meta) {
      if (!meta) {
        return [];
      }

      return core.getQueryMembers(query).map(function (member) {
        var resolvedMember = meta.resolveMember(member, ['measures', 'dimensions', 'segments']);

        if (resolvedMember.error) {
          return member;
        }

        return false;
      }).filter(Boolean);
    }
  }, {
    key: "isQueryPresent",
    value: function isQueryPresent() {
      var query = this.state.query;
      return QueryRenderer.isQueryPresent(query);
    }
  }, {
    key: "prepareRenderProps",
    value: function prepareRenderProps(queryRendererProps) {
      var _this3 = this;

      var getName = function getName(member) {
        return member.name;
      };

      var toTimeDimension = function toTimeDimension(member) {
        var rangeSelection = member.compareDateRange ? {
          compareDateRange: member.compareDateRange
        } : {
          dateRange: member.dateRange
        };
        return _objectSpread({
          dimension: member.dimension.name,
          granularity: member.granularity
        }, rangeSelection);
      };

      var toFilter = function toFilter(member) {
        var _member$member, _member$dimension;

        return {
          member: ((_member$member = member.member) === null || _member$member === void 0 ? void 0 : _member$member.name) || ((_member$dimension = member.dimension) === null || _member$dimension === void 0 ? void 0 : _member$dimension.name),
          operator: member.operator,
          values: member.values
        };
      };

      var updateMethods = function updateMethods(memberType) {
        var toQuery = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : getName;
        return {
          add: function add(member) {
            var query = _this3.state.query;

            _this3.updateQuery(_defineProperty__default['default']({}, memberType, (query[memberType] || []).concat(toQuery(member))));
          },
          remove: function remove(member) {
            var query = _this3.state.query;
            var members = (query[memberType] || []).concat([]);
            members.splice(member.index, 1);
            return _this3.updateQuery(_defineProperty__default['default']({}, memberType, members));
          },
          update: function update(member, updateWith) {
            var query = _this3.state.query;
            var members = (query[memberType] || []).concat([]);
            members.splice(member.index, 1, toQuery(updateWith));
            return _this3.updateQuery(_defineProperty__default['default']({}, memberType, members));
          }
        };
      };

      var _this$state = this.state,
          meta = _this$state.meta,
          metaError = _this$state.metaError,
          query = _this$state.query,
          queryError = _this$state.queryError,
          chartType = _this$state.chartType,
          pivotConfig = _this$state.pivotConfig,
          validatedQuery = _this$state.validatedQuery,
          missingMembers = _this$state.missingMembers,
          isFetchingMeta = _this$state.isFetchingMeta,
          dryRunResponse = _this$state.dryRunResponse;
      var flatFilters = ramda.uniqBy(ramda.prop('member'), core.flattenFilters(meta && query.filters || []).map(function (filter) {
        return _objectSpread(_objectSpread({}, filter), {}, {
          member: filter.member || filter.dimension
        });
      }));
      var filters = flatFilters.map(function (m, i) {
        return _objectSpread(_objectSpread({}, m), {}, {
          dimension: meta.resolveMember(m.member || m.dimension, ['dimensions', 'measures']),
          operators: meta.filterOperatorsForMember(m.member || m.dimension, ['dimensions', 'measures']),
          index: i
        });
      });
      var measures = QueryBuilder.resolveMember('measures', this.state);
      var dimensions = QueryBuilder.resolveMember('dimensions', this.state);
      var timeDimensions = QueryBuilder.resolveMember('timeDimensions', this.state);
      var segments = (meta && query.segments || []).map(function (m, i) {
        return _objectSpread({
          index: i
        }, meta.resolveMember(m, 'segments'));
      });
      var availableMeasures = meta ? meta.membersForQuery(query, 'measures') : [];
      var availableDimensions = meta ? meta.membersForQuery(query, 'dimensions') : [];
      var availableSegments = meta ? meta.membersForQuery(query, 'segments') : [];
      var orderMembers = ramda.uniqBy(ramda.prop('id'), [].concat(_toConsumableArray__default['default']((Array.isArray(query.order) ? query.order : Object.entries(query.order || {})).map(function (_ref2) {
        var _ref3 = _slicedToArray__default['default'](_ref2, 2),
            id = _ref3[0],
            order = _ref3[1];

        return {
          id: id,
          order: order,
          title: meta ? meta.resolveMember(id, ['measures', 'dimensions']).title : ''
        };
      })), _toConsumableArray__default['default']([].concat(_toConsumableArray__default['default'](measures), _toConsumableArray__default['default'](dimensions)).map(function (_ref4) {
        var name = _ref4.name,
            title = _ref4.title;
        return {
          id: name,
          title: title,
          order: 'none'
        };
      })))); // Preserve order until the members change or manually re-ordered
      // This is needed so that when an order member becomes active, it doesn't jump to the top of the list

      var orderMemberOrderKey = JSON.stringify(orderMembers.map(function (_ref5) {
        var id = _ref5.id;
        return id;
      }).sort());

      if (this.orderMemberOrderKey && this.orderMemberOrder && orderMemberOrderKey === this.orderMemberOrderKey) {
        orderMembers = this.orderMemberOrder.map(function (id) {
          return orderMembers.find(function (member) {
            return member.id === id;
          });
        });
      } else {
        this.orderMemberOrderKey = orderMemberOrderKey;
        this.orderMemberOrder = orderMembers.map(function (_ref6) {
          var id = _ref6.id;
          return id;
        });
      }

      return _objectSpread({
        meta: meta,
        metaError: metaError,
        query: query,
        error: queryError,
        // Match same name as QueryRenderer prop
        validatedQuery: validatedQuery,
        isQueryPresent: this.isQueryPresent(),
        chartType: chartType,
        measures: measures,
        dimensions: dimensions,
        timeDimensions: timeDimensions,
        segments: segments,
        filters: filters,
        orderMembers: orderMembers,
        availableMeasures: availableMeasures,
        availableDimensions: availableDimensions,
        availableTimeDimensions: availableDimensions.filter(function (m) {
          return m.type === 'time';
        }),
        availableSegments: availableSegments,
        updateQuery: function updateQuery(queryUpdate) {
          return _this3.updateQuery(queryUpdate);
        },
        updateMeasures: updateMethods('measures'),
        updateDimensions: updateMethods('dimensions'),
        updateSegments: updateMethods('segments'),
        updateTimeDimensions: updateMethods('timeDimensions', toTimeDimension),
        updateFilters: updateMethods('filters', toFilter),
        updateChartType: function updateChartType(newChartType) {
          return _this3.updateVizState({
            chartType: newChartType
          });
        },
        updateOrder: {
          set: function set(memberId) {
            var newOrder = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : 'asc';

            _this3.updateQuery({
              order: orderMembers.map(function (orderMember) {
                return _objectSpread(_objectSpread({}, orderMember), {}, {
                  order: orderMember.id === memberId ? newOrder : orderMember.order
                });
              }).reduce(function (acc, _ref7) {
                var id = _ref7.id,
                    order = _ref7.order;
                return order !== 'none' ? [].concat(_toConsumableArray__default['default'](acc), [[id, order]]) : acc;
              }, [])
            });
          },
          update: function update(order) {
            _this3.updateQuery({
              order: order
            });
          },
          reorder: function reorder(sourceIndex, destinationIndex) {
            if (sourceIndex == null || destinationIndex == null) {
              return;
            }

            _this3.updateQuery({
              order: core.moveItemInArray(orderMembers, sourceIndex, destinationIndex).reduce(function (acc, _ref8) {
                var id = _ref8.id,
                    order = _ref8.order;
                return order !== 'none' ? [].concat(_toConsumableArray__default['default'](acc), [[id, order]]) : acc;
              }, [])
            });
          }
        },
        pivotConfig: pivotConfig,
        updatePivotConfig: {
          moveItem: function moveItem(_ref9) {
            var sourceIndex = _ref9.sourceIndex,
                destinationIndex = _ref9.destinationIndex,
                sourceAxis = _ref9.sourceAxis,
                destinationAxis = _ref9.destinationAxis;

            _this3.updateVizState({
              pivotConfig: core.movePivotItem(pivotConfig, sourceIndex, destinationIndex, sourceAxis, destinationAxis)
            });
          },
          update: function update(config) {
            var limit = config.limit;

            _this3.updateVizState(_objectSpread({
              pivotConfig: _objectSpread(_objectSpread({}, pivotConfig), config)
            }, limit ? {
              query: _objectSpread(_objectSpread({}, query), {}, {
                limit: limit
              })
            } : null));
          }
        },
        missingMembers: missingMembers,
        refresh: this.fetchMeta,
        isFetchingMeta: isFetchingMeta,
        dryRunResponse: dryRunResponse
      }, queryRendererProps);
    }
  }, {
    key: "updateQuery",
    value: function updateQuery(queryUpdate) {
      var query = this.state.query;
      this.updateVizState({
        query: _objectSpread(_objectSpread({}, query), queryUpdate)
      });
    }
  }, {
    key: "updateVizState",
    value: function () {
      var _updateVizState = _asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee5(state) {
        var _this4 = this;

        var _this$props2, setQuery, setVizState, _this$state2, stateQuery, statePivotConfig, chartType, meta, finalState, vizStateSent, handleVizStateChange, runSetters, response;

        return _regeneratorRuntime__default['default'].wrap(function _callee5$(_context5) {
          while (1) {
            switch (_context5.prev = _context5.next) {
              case 0:
                _this$props2 = this.props, setQuery = _this$props2.setQuery, setVizState = _this$props2.setVizState;
                _this$state2 = this.state, stateQuery = _this$state2.query, statePivotConfig = _this$state2.pivotConfig, chartType = _this$state2.chartType, meta = _this$state2.meta;
                finalState = this.applyStateChangeHeuristics(state);

                if (!finalState.query) {
                  finalState.query = _objectSpread({}, stateQuery);
                }

                vizStateSent = null;

                handleVizStateChange = function handleVizStateChange(currentState) {
                  var onVizStateChanged = _this4.props.onVizStateChanged;

                  if (onVizStateChanged) {
                    var newVizState = ramda.pick(['chartType', 'pivotConfig', 'query'], currentState); // Don't run callbacks more than once unless the viz state has changed since last time

                    if (!vizStateSent || !ramda.equals(vizStateSent, newVizState)) {
                      onVizStateChanged(newVizState);
                      vizStateSent = ramda.clone(newVizState); // use clone to make sure we don't save object references
                    }
                  }
                }; // deprecated, setters replaced by onVizStateChanged


                runSetters = function runSetters(currentState) {
                  if (setVizState) {
                    setVizState(ramda.pick(['chartType', 'pivotConfig', 'query'], currentState));
                  }

                  if (currentState.query && setQuery) {
                    setQuery(currentState.query);
                  }
                };

                if (finalState.shouldApplyHeuristicOrder) {
                  finalState.query.order = core.defaultOrder(finalState.query);
                }

                finalState.pivotConfig = core.ResultSet.getNormalizedPivotConfig(finalState.query, finalState.pivotConfig !== undefined ? finalState.pivotConfig : statePivotConfig);
                finalState.missingMembers = this.getMissingMembers(finalState.query, meta);
                finalState.chartType = finalState.chartType || state.chartType || chartType; // deprecated

                runSetters(_objectSpread(_objectSpread({}, state), {}, {
                  query: finalState.query
                })); // Update optimistically so that UI does not stutter

                this.setState(_objectSpread(_objectSpread({}, finalState), {}, {
                  queryError: null
                }));
                handleVizStateChange(finalState);

                if (!(QueryRenderer.isQueryPresent(finalState.query) && finalState.missingMembers.length === 0)) {
                  _context5.next = 30;
                  break;
                }

                _context5.prev = 15;
                _context5.next = 18;
                return this.cubejsApi().dryRun(finalState.query, {
                  mutexObj: this.mutexObj
                });

              case 18:
                response = _context5.sent;

                if (finalState.shouldApplyHeuristicOrder) {
                  finalState.query.order = (response.queryOrder || []).reduce(function (memo, current) {
                    return _objectSpread(_objectSpread({}, memo), current);
                  }, {});
                }

                finalState.pivotConfig = core.ResultSet.getNormalizedPivotConfig(response.pivotQuery, finalState.pivotConfig);
                finalState.validatedQuery = this.validatedQuery(finalState);
                finalState.dryRunResponse = response; // deprecated

                if (QueryRenderer.isQueryPresent(stateQuery)) {
                  runSetters(_objectSpread(_objectSpread({}, this.state), finalState));
                }

                _context5.next = 30;
                break;

              case 26:
                _context5.prev = 26;
                _context5.t0 = _context5["catch"](15);
                console.error(_context5.t0);
                this.setState({
                  queryError: _context5.t0
                });

              case 30:
                this.setState(finalState, function () {
                  return handleVizStateChange(_this4.state);
                });

              case 31:
              case "end":
                return _context5.stop();
            }
          }
        }, _callee5, this, [[15, 26]]);
      }));

      function updateVizState(_x2) {
        return _updateVizState.apply(this, arguments);
      }

      return updateVizState;
    }()
  }, {
    key: "validatedQuery",
    value: function validatedQuery(state) {
      var _ref10 = state || this.state,
          query = _ref10.query;

      return _objectSpread(_objectSpread({}, query), {}, {
        filters: (query.filters || []).filter(function (f) {
          return f.operator;
        })
      });
    }
  }, {
    key: "defaultHeuristics",
    value: function defaultHeuristics(newState) {
      var _this$state3 = this.state,
          query = _this$state3.query,
          sessionGranularity = _this$state3.sessionGranularity,
          meta = _this$state3.meta;
      return core.defaultHeuristics(newState, query, {
        meta: meta,
        sessionGranularity: sessionGranularity || 'day'
      });
    }
  }, {
    key: "applyStateChangeHeuristics",
    value: function applyStateChangeHeuristics(newState) {
      var _this$props3 = this.props,
          stateChangeHeuristics = _this$props3.stateChangeHeuristics,
          disableHeuristics = _this$props3.disableHeuristics;

      if (disableHeuristics) {
        return newState;
      }

      return stateChangeHeuristics && stateChangeHeuristics(this.state, newState) || this.defaultHeuristics(newState);
    }
  }, {
    key: "render",
    value: function render() {
      var _this5 = this;

      var query = this.state.query;
      var _this$props4 = this.props,
          cubejsApi = _this$props4.cubejsApi,
          _render = _this$props4.render,
          wrapWithQueryRenderer = _this$props4.wrapWithQueryRenderer;

      if (wrapWithQueryRenderer) {
        return /*#__PURE__*/React__default['default'].createElement(QueryRenderer, {
          query: query,
          cubejsApi: cubejsApi,
          resetResultSetOnChange: false,
          render: function render(queryRendererProps) {
            if (_render) {
              return _render(_this5.prepareRenderProps(queryRendererProps));
            }

            return null;
          }
        });
      } else {
        if (_render) {
          return _render(this.prepareRenderProps());
        }

        return null;
      }
    }
  }], [{
    key: "getDerivedStateFromProps",
    value: // This is an anti-pattern, only kept for backward compatibility
    // https://reactjs.org/blog/2018/06/07/you-probably-dont-need-derived-state.html#anti-pattern-unconditionally-copying-props-to-state
    function getDerivedStateFromProps(props, state) {
      if (props.query || props.vizState) {
        var nextState = _objectSpread(_objectSpread({}, state), props.vizState || {});

        if (Array.isArray(props.query)) {
          throw new Error('Array of queries is not supported.');
        }

        return _objectSpread(_objectSpread({}, nextState), {}, {
          query: _objectSpread(_objectSpread({}, nextState.query), props.query || {})
        });
      }

      return null;
    }
  }, {
    key: "resolveMember",
    value: function resolveMember(type, _ref11) {
      var meta = _ref11.meta,
          query = _ref11.query;

      if (!meta) {
        return [];
      }

      if (Array.isArray(query)) {
        return query.reduce(function (memo, currentQuery) {
          return memo.concat(QueryBuilder.resolveMember(type, {
            meta: meta,
            query: currentQuery
          }));
        }, []);
      }

      if (type === 'timeDimensions') {
        return (query.timeDimensions || []).map(function (m, index) {
          return _objectSpread(_objectSpread({}, m), {}, {
            dimension: _objectSpread(_objectSpread({}, meta.resolveMember(m.dimension, 'dimensions')), {}, {
              granularities: granularities
            }),
            index: index
          });
        });
      }

      return (query[type] || []).map(function (m, index) {
        return _objectSpread({
          index: index
        }, meta.resolveMember(m, type));
      });
    }
  }]);

  return QueryBuilder;
}(React__default['default'].Component);
QueryBuilder.contextType = CubeContext;
QueryBuilder.defaultProps = {
  cubejsApi: null,
  stateChangeHeuristics: null,
  disableHeuristics: false,
  render: null,
  wrapWithQueryRenderer: true,
  defaultChartType: 'line',
  defaultQuery: {},
  initialVizState: null,
  onVizStateChanged: null,
  // deprecated
  query: null,
  setQuery: null,
  vizState: null,
  setVizState: null
};

var CubeProvider = function CubeProvider(_ref) {
  var cubejsApi = _ref.cubejsApi,
      children = _ref.children;
  return /*#__PURE__*/React__default['default'].createElement(CubeContext.Provider, {
    value: {
      cubejsApi: cubejsApi
    }
  }, children);
};

function useDeepCompareMemoize(value) {
  var ref = React.useRef([]);

  if (!ramda.equals(value, ref.current)) {
    ref.current = value;
  }

  return ref.current;
}

function useCubeQuery(query) {
  var options = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : {};
  var mutexRef = React.useRef({});
  var isMounted = React.useRef(true);

  var _useState = React.useState(null),
      _useState2 = _slicedToArray__default['default'](_useState, 2),
      currentQuery = _useState2[0],
      setCurrentQuery = _useState2[1];

  var _useState3 = React.useState(false),
      _useState4 = _slicedToArray__default['default'](_useState3, 2),
      isLoading = _useState4[0],
      setLoading = _useState4[1];

  var _useState5 = React.useState(null),
      _useState6 = _slicedToArray__default['default'](_useState5, 2),
      resultSet = _useState6[0],
      setResultSet = _useState6[1];

  var _useState7 = React.useState(null),
      _useState8 = _slicedToArray__default['default'](_useState7, 2),
      progress = _useState8[0],
      setProgress = _useState8[1];

  var _useState9 = React.useState(null),
      _useState10 = _slicedToArray__default['default'](_useState9, 2),
      error = _useState10[0],
      setError = _useState10[1];

  var context = React.useContext(CubeContext);
  var subscribeRequest = null;

  var progressCallback = function progressCallback(_ref) {
    var progressResponse = _ref.progressResponse;
    return setProgress(progressResponse);
  };

  function fetch() {
    return _fetch.apply(this, arguments);
  }

  function _fetch() {
    _fetch = _asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee2() {
      var resetResultSetOnChange, cubejsApi, response;
      return _regeneratorRuntime__default['default'].wrap(function _callee2$(_context2) {
        while (1) {
          switch (_context2.prev = _context2.next) {
            case 0:
              resetResultSetOnChange = options.resetResultSetOnChange;
              cubejsApi = options.cubejsApi || (context === null || context === void 0 ? void 0 : context.cubejsApi);

              if (cubejsApi) {
                _context2.next = 4;
                break;
              }

              throw new Error('Cube.js API client is not provided');

            case 4:
              if (resetResultSetOnChange) {
                setResultSet(null);
              }

              setError(null);
              setLoading(true);
              _context2.prev = 7;
              _context2.next = 10;
              return cubejsApi.load(query, {
                mutexObj: mutexRef.current,
                mutexKey: 'query',
                progressCallback: progressCallback
              });

            case 10:
              response = _context2.sent;

              if (isMounted.current) {
                setResultSet(response);
                setProgress(null);
              }

              _context2.next = 17;
              break;

            case 14:
              _context2.prev = 14;
              _context2.t0 = _context2["catch"](7);

              if (isMounted.current) {
                setError(_context2.t0);
                setResultSet(null);
                setProgress(null);
              }

            case 17:
              if (isMounted.current) {
                setLoading(false);
              }

            case 18:
            case "end":
              return _context2.stop();
          }
        }
      }, _callee2, null, [[7, 14]]);
    }));
    return _fetch.apply(this, arguments);
  }

  React.useEffect(function () {
    return function () {
      isMounted.current = false;
    };
  }, []);
  React.useEffect(function () {
    var _options$skip = options.skip,
        skip = _options$skip === void 0 ? false : _options$skip,
        resetResultSetOnChange = options.resetResultSetOnChange;
    var cubejsApi = options.cubejsApi || (context === null || context === void 0 ? void 0 : context.cubejsApi);

    if (!cubejsApi) {
      throw new Error('Cube.js API client is not provided');
    }

    function loadQuery() {
      return _loadQuery.apply(this, arguments);
    }

    function _loadQuery() {
      _loadQuery = _asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee() {
        return _regeneratorRuntime__default['default'].wrap(function _callee$(_context) {
          while (1) {
            switch (_context.prev = _context.next) {
              case 0:
                if (!(!skip && core.isQueryPresent(query))) {
                  _context.next = 20;
                  break;
                }

                if (!core.areQueriesEqual(currentQuery, query)) {
                  if (resetResultSetOnChange == null || resetResultSetOnChange) {
                    setResultSet(null);
                  }

                  setCurrentQuery(query);
                }

                setError(null);
                setLoading(true);
                _context.prev = 4;

                if (!subscribeRequest) {
                  _context.next = 9;
                  break;
                }

                _context.next = 8;
                return subscribeRequest.unsubscribe();

              case 8:
                subscribeRequest = null;

              case 9:
                if (!options.subscribe) {
                  _context.next = 13;
                  break;
                }

                subscribeRequest = cubejsApi.subscribe(query, {
                  mutexObj: mutexRef.current,
                  mutexKey: 'query',
                  progressCallback: progressCallback
                }, function (e, result) {
                  if (isMounted.current) {
                    if (e) {
                      setError(e);
                    } else {
                      setResultSet(result);
                    }

                    setLoading(false);
                    setProgress(null);
                  }
                });
                _context.next = 15;
                break;

              case 13:
                _context.next = 15;
                return fetch();

              case 15:
                _context.next = 20;
                break;

              case 17:
                _context.prev = 17;
                _context.t0 = _context["catch"](4);

                if (isMounted.current) {
                  setError(_context.t0);
                  setResultSet(null);
                  setLoading(false);
                  setProgress(null);
                }

              case 20:
              case "end":
                return _context.stop();
            }
          }
        }, _callee, null, [[4, 17]]);
      }));
      return _loadQuery.apply(this, arguments);
    }

    loadQuery();
    return function () {
      if (subscribeRequest) {
        subscribeRequest.unsubscribe();
        subscribeRequest = null;
      }
    };
  }, useDeepCompareMemoize([query, Object.keys(query && query.order || {}), options, context]));
  return {
    isLoading: isLoading,
    resultSet: resultSet,
    error: error,
    progress: progress,
    previousQuery: currentQuery,
    refetch: fetch
  };
}

function useDryRun(query) {
  var options = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : {};
  var context = React.useContext(CubeContext);
  var mutexRef = React.useRef({});

  var _useState = React.useState(null),
      _useState2 = _slicedToArray__default['default'](_useState, 2),
      response = _useState2[0],
      setResponse = _useState2[1];

  var _useState3 = React.useState(false),
      _useState4 = _slicedToArray__default['default'](_useState3, 2),
      isLoading = _useState4[0],
      setLoading = _useState4[1];

  var _useState5 = React.useState(null),
      _useState6 = _slicedToArray__default['default'](_useState5, 2),
      error = _useState6[0],
      setError = _useState6[1];

  React.useEffect(function () {
    var isMounted = true;
    var _options$skip = options.skip,
        skip = _options$skip === void 0 ? false : _options$skip;
    var cubejsApi = options.cubejsApi || context && context.cubejsApi;

    if (!cubejsApi) {
      throw new Error('Cube.js API client is not provided');
    }

    function loadQuery() {
      return _loadQuery.apply(this, arguments);
    }

    function _loadQuery() {
      _loadQuery = _asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee() {
        var dryRunResponse;
        return _regeneratorRuntime__default['default'].wrap(function _callee$(_context) {
          while (1) {
            switch (_context.prev = _context.next) {
              case 0:
                if (!(!skip && query && core.isQueryPresent(query))) {
                  _context.next = 13;
                  break;
                }

                setError(null);
                setLoading(true);
                _context.prev = 3;
                _context.next = 6;
                return cubejsApi.dryRun(query, {
                  mutexObj: mutexRef.current,
                  mutexKey: 'dry-run'
                });

              case 6:
                dryRunResponse = _context.sent;

                if (isMounted) {
                  setResponse(dryRunResponse);
                  setLoading(false);
                }

                _context.next = 13;
                break;

              case 10:
                _context.prev = 10;
                _context.t0 = _context["catch"](3);

                if (isMounted) {
                  setError(_context.t0);
                  setLoading(false);
                }

              case 13:
              case "end":
                return _context.stop();
            }
          }
        }, _callee, null, [[3, 10]]);
      }));
      return _loadQuery.apply(this, arguments);
    }

    loadQuery();
    return function () {
      isMounted = false;
    };
  }, useDeepCompareMemoize([query, Object.keys(query && query.order || {}), options, context]));
  return {
    isLoading: isLoading,
    error: error,
    response: response
  };
}

Object.defineProperty(exports, 'isQueryPresent', {
  enumerable: true,
  get: function () {
    return core.isQueryPresent;
  }
});
exports.CubeContext = CubeContext;
exports.CubeProvider = CubeProvider;
exports.QueryBuilder = QueryBuilder;
exports.QueryRenderer = QueryRenderer;
exports.QueryRendererWithTotals = QueryRendererWithTotals;
exports.useCubeQuery = useCubeQuery;
exports.useDryRun = useDryRun;
//# sourceMappingURL=cubejs-client-react.js.map
