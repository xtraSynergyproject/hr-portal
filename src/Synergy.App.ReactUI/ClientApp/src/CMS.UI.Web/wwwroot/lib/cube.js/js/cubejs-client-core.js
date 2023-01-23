'use strict';

Object.defineProperty(exports, '__esModule', { value: true });

require('core-js/modules/es.object.keys.js');
require('core-js/modules/es.symbol.js');
require('core-js/modules/es.array.filter.js');
require('core-js/modules/es.object.get-own-property-descriptor.js');
require('core-js/modules/es.array.for-each.js');
require('core-js/modules/web.dom-collections.for-each.js');
require('core-js/modules/es.object.get-own-property-descriptors.js');
require('core-js/modules/es.object.define-properties.js');
require('core-js/modules/es.object.define-property.js');
var _asyncToGenerator = require('@babel/runtime/helpers/asyncToGenerator');
var _defineProperty = require('@babel/runtime/helpers/defineProperty');
var _typeof = require('@babel/runtime/helpers/typeof');
var _classCallCheck = require('@babel/runtime/helpers/classCallCheck');
var _createClass = require('@babel/runtime/helpers/createClass');
var _regeneratorRuntime = require('@babel/runtime/regenerator');
require('core-js/modules/es.object.to-string.js');
require('core-js/modules/es.promise.js');
require('core-js/modules/web.timers.js');
require('core-js/modules/es.array.is-array.js');
require('core-js/modules/es.array.map.js');
var uuid = require('uuid/v4');
var _objectWithoutProperties = require('@babel/runtime/helpers/objectWithoutProperties');
var _toConsumableArray = require('@babel/runtime/helpers/toConsumableArray');
var _slicedToArray = require('@babel/runtime/helpers/slicedToArray');
require('core-js/modules/es.array.from.js');
require('core-js/modules/es.string.iterator.js');
require('core-js/modules/es.array.iterator.js');
require('core-js/modules/es.map.js');
require('core-js/modules/web.dom-collections.iterator.js');
require('core-js/modules/es.array.includes.js');
require('core-js/modules/es.string.includes.js');
require('core-js/modules/es.object.values.js');
require('core-js/modules/es.array.find.js');
require('core-js/modules/es.array.concat.js');
require('core-js/modules/es.regexp.exec.js');
require('core-js/modules/es.string.split.js');
require('core-js/modules/es.array.join.js');
require('core-js/modules/es.date.to-string.js');
require('core-js/modules/es.regexp.to-string.js');
require('core-js/modules/es.string.match.js');
require('core-js/modules/es.array.reduce.js');
require('core-js/modules/es.number.is-nan.js');
require('core-js/modules/es.number.constructor.js');
require('core-js/modules/es.number.parse-float.js');
require('core-js/modules/es.set.js');
require('core-js/modules/es.array.index-of.js');
require('core-js/modules/es.object.assign.js');
require('core-js/modules/es.string.trim.js');
var ramda = require('ramda');
var Moment = require('moment');
var momentRange = require('moment-range');
require('core-js/modules/es.function.name.js');
require('core-js/modules/web.url.js');
var fetch = require('cross-fetch');
require('url-search-params-polyfill');
require('core-js/modules/es.reflect.construct.js');
var _inherits = require('@babel/runtime/helpers/inherits');
var _possibleConstructorReturn = require('@babel/runtime/helpers/possibleConstructorReturn');
var _getPrototypeOf = require('@babel/runtime/helpers/getPrototypeOf');
var _wrapNativeSuper = require('@babel/runtime/helpers/wrapNativeSuper');
require('core-js/modules/es.object.entries.js');
require('core-js/modules/es.array.every.js');
require('core-js/modules/es.array.splice.js');

function _interopDefaultLegacy (e) { return e && typeof e === 'object' && 'default' in e ? e : { 'default': e }; }

var _asyncToGenerator__default = /*#__PURE__*/_interopDefaultLegacy(_asyncToGenerator);
var _defineProperty__default = /*#__PURE__*/_interopDefaultLegacy(_defineProperty);
var _typeof__default = /*#__PURE__*/_interopDefaultLegacy(_typeof);
var _classCallCheck__default = /*#__PURE__*/_interopDefaultLegacy(_classCallCheck);
var _createClass__default = /*#__PURE__*/_interopDefaultLegacy(_createClass);
var _regeneratorRuntime__default = /*#__PURE__*/_interopDefaultLegacy(_regeneratorRuntime);
var uuid__default = /*#__PURE__*/_interopDefaultLegacy(uuid);
var _objectWithoutProperties__default = /*#__PURE__*/_interopDefaultLegacy(_objectWithoutProperties);
var _toConsumableArray__default = /*#__PURE__*/_interopDefaultLegacy(_toConsumableArray);
var _slicedToArray__default = /*#__PURE__*/_interopDefaultLegacy(_slicedToArray);
var Moment__default = /*#__PURE__*/_interopDefaultLegacy(Moment);
var fetch__default = /*#__PURE__*/_interopDefaultLegacy(fetch);
var _inherits__default = /*#__PURE__*/_interopDefaultLegacy(_inherits);
var _possibleConstructorReturn__default = /*#__PURE__*/_interopDefaultLegacy(_possibleConstructorReturn);
var _getPrototypeOf__default = /*#__PURE__*/_interopDefaultLegacy(_getPrototypeOf);
var _wrapNativeSuper__default = /*#__PURE__*/_interopDefaultLegacy(_wrapNativeSuper);

function ownKeys$3(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$3(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$3(Object(source), true).forEach(function (key) { _defineProperty__default['default'](target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$3(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }
var moment = momentRange.extendMoment(Moment__default['default']);
var TIME_SERIES = {
  day: function day(range) {
    return Array.from(range.by('day')).map(function (d) {
      return d.format('YYYY-MM-DDT00:00:00.000');
    });
  },
  month: function month(range) {
    return Array.from(range.snapTo('month').by('month')).map(function (d) {
      return d.format('YYYY-MM-01T00:00:00.000');
    });
  },
  year: function year(range) {
    return Array.from(range.snapTo('year').by('year')).map(function (d) {
      return d.format('YYYY-01-01T00:00:00.000');
    });
  },
  hour: function hour(range) {
    return Array.from(range.by('hour')).map(function (d) {
      return d.format('YYYY-MM-DDTHH:00:00.000');
    });
  },
  minute: function minute(range) {
    return Array.from(range.by('minute')).map(function (d) {
      return d.format('YYYY-MM-DDTHH:mm:00.000');
    });
  },
  second: function second(range) {
    return Array.from(range.by('second')).map(function (d) {
      return d.format('YYYY-MM-DDTHH:mm:ss.000');
    });
  },
  week: function week(range) {
    return Array.from(range.snapTo('isoweek').by('week')).map(function (d) {
      return d.startOf('isoweek').format('YYYY-MM-DDT00:00:00.000');
    });
  }
};
var DateRegex = /^\d\d\d\d-\d\d-\d\d$/;
var LocalDateRegex = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}.\d{3}Z?$/;

var groupByToPairs = function groupByToPairs(keyFn) {
  var acc = new Map();
  return function (data) {
    data.forEach(function (row) {
      var key = keyFn(row);

      if (!acc.has(key)) {
        acc.set(key, []);
      }

      acc.get(key).push(row);
    });
    return Array.from(acc.entries());
  };
};

var QUERY_TYPE = {
  REGULAR_QUERY: 'regularQuery',
  COMPARE_DATE_RANGE_QUERY: 'compareDateRangeQuery',
  BLENDING_QUERY: 'blendingQuery'
};

var ResultSet = /*#__PURE__*/function () {
  function ResultSet(loadResponse) {
    var options = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : {};

    _classCallCheck__default['default'](this, ResultSet);

    this.loadResponse = loadResponse;

    if (this.loadResponse.queryType != null) {
      this.queryType = loadResponse.queryType;
      this.loadResponses = loadResponse.results;
    } else {
      this.queryType = QUERY_TYPE.REGULAR_QUERY;
      this.loadResponse.pivotQuery = _objectSpread$3(_objectSpread$3({}, loadResponse.query), {}, {
        queryType: this.queryType
      });
      this.loadResponses = [loadResponse];
    }

    if (!Object.values(QUERY_TYPE).includes(this.queryType)) {
      throw new Error('Unknown query type');
    }

    this.parseDateMeasures = options.parseDateMeasures;
    this.options = options;
    this.backwardCompatibleData = [];
  }

  _createClass__default['default'](ResultSet, [{
    key: "drillDown",
    value: function drillDown(drillDownLocator, pivotConfig) {
      if (this.queryType === QUERY_TYPE.COMPARE_DATE_RANGE_QUERY) {
        throw new Error('compareDateRange drillDown query is not currently supported');
      }

      if (this.queryType === QUERY_TYPE.BLENDING_QUERY) {
        throw new Error('Data blending drillDown query is not currently supported');
      }

      var _drillDownLocator$xVa = drillDownLocator.xValues,
          xValues = _drillDownLocator$xVa === void 0 ? [] : _drillDownLocator$xVa,
          _drillDownLocator$yVa = drillDownLocator.yValues,
          yValues = _drillDownLocator$yVa === void 0 ? [] : _drillDownLocator$yVa;
      var normalizedPivotConfig = this.normalizePivotConfig(pivotConfig);
      var values = [];
      normalizedPivotConfig.x.forEach(function (member, currentIndex) {
        return values.push([member, xValues[currentIndex]]);
      });
      normalizedPivotConfig.y.forEach(function (member, currentIndex) {
        return values.push([member, yValues[currentIndex]]);
      });

      var _this$query = this.query(),
          _this$query$filters = _this$query.filters,
          parentFilters = _this$query$filters === void 0 ? [] : _this$query$filters,
          _this$query$segments = _this$query.segments,
          segments = _this$query$segments === void 0 ? [] : _this$query$segments;

      var measures = this.loadResponses[0].annotation.measures;

      var _ref = values.find(function (_ref3) {
        var _ref4 = _slicedToArray__default['default'](_ref3, 1),
            member = _ref4[0];

        return member === 'measures';
      }) || [],
          _ref2 = _slicedToArray__default['default'](_ref, 2),
          measureName = _ref2[1];

      if (measureName === undefined) {
        var _Object$keys = Object.keys(measures);

        var _Object$keys2 = _slicedToArray__default['default'](_Object$keys, 1);

        measureName = _Object$keys2[0];
      }

      if (!(measures[measureName] && measures[measureName].drillMembers || []).length) {
        return null;
      }

      var filters = [{
        member: measureName,
        operator: 'measureFilter'
      }].concat(_toConsumableArray__default['default'](parentFilters));
      var timeDimensions = [];
      values.filter(function (_ref5) {
        var _ref6 = _slicedToArray__default['default'](_ref5, 1),
            member = _ref6[0];

        return member !== 'measures';
      }).forEach(function (_ref7) {
        var _ref8 = _slicedToArray__default['default'](_ref7, 2),
            member = _ref8[0],
            value = _ref8[1];

        var _member$split = member.split('.'),
            _member$split2 = _slicedToArray__default['default'](_member$split, 3),
            cubeName = _member$split2[0],
            dimension = _member$split2[1],
            granularity = _member$split2[2];

        if (granularity !== undefined) {
          var range = moment.range(value, value).snapTo(granularity);
          timeDimensions.push({
            dimension: [cubeName, dimension].join('.'),
            dateRange: [range.start, range.end].map(function (dt) {
              return dt.format(moment.HTML5_FMT.DATETIME_LOCAL_MS);
            })
          });
        } else if (value == null) {
          filters.push({
            member: member,
            operator: 'notSet'
          });
        } else {
          filters.push({
            member: member,
            operator: 'equals',
            values: [value.toString()]
          });
        }
      });
      var query = this.loadResponses[0].query;

      if (timeDimensions.length === 0 && query.timeDimensions.length > 0 && query.timeDimensions[0].granularity == null) {
        timeDimensions.push(query.timeDimensions[0]);
      }

      return _objectSpread$3(_objectSpread$3(_objectSpread$3({}, measures[measureName].drillMembersGrouped), {}, {
        filters: filters
      }, segments.length > 0 ? {
        segments: segments
      } : {}), {}, {
        timeDimensions: timeDimensions,
        segments: segments,
        timezone: query.timezone
      });
    }
  }, {
    key: "series",
    value: function series(pivotConfig) {
      var _this = this;

      return this.seriesNames(pivotConfig).map(function (_ref9) {
        var title = _ref9.title,
            key = _ref9.key;
        return {
          title: title,
          key: key,
          series: _this.chartPivot(pivotConfig).map(function (_ref10) {
            var category = _ref10.category,
                x = _ref10.x,
                obj = _objectWithoutProperties__default['default'](_ref10, ["category", "x"]);

            return {
              value: obj[key],
              category: category,
              x: x
            };
          })
        };
      });
    }
  }, {
    key: "axisValues",
    value: function axisValues(axis) {
      var resultIndex = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : 0;
      var query = this.loadResponses[resultIndex].query;
      return function (row) {
        var value = function value(measure) {
          return axis.filter(function (d) {
            return d !== 'measures';
          }).map(function (d) {
            return row[d] != null ? row[d] : null;
          }).concat(measure ? [measure] : []);
        };

        if (axis.find(function (d) {
          return d === 'measures';
        }) && (query.measures || []).length) {
          return query.measures.map(value);
        }

        return [value()];
      };
    }
  }, {
    key: "axisValuesString",
    value: function axisValuesString(axisValues, delimiter) {
      var formatValue = function formatValue(v) {
        if (v == null) {
          return '∅';
        } else if (v === '') {
          return '[Empty string]';
        } else {
          return v;
        }
      };

      return axisValues.map(formatValue).join(delimiter || ', ');
    }
  }, {
    key: "normalizePivotConfig",
    value: function normalizePivotConfig(pivotConfig) {
      return ResultSet.getNormalizedPivotConfig(this.loadResponse.pivotQuery, pivotConfig);
    }
  }, {
    key: "timeSeries",
    value: function timeSeries(timeDimension) {
      if (!timeDimension.granularity) {
        return null;
      }

      var dateRange = timeDimension.dateRange;

      if (!dateRange) {
        var dates = ramda.pipe(ramda.map(function (row) {
          return row[ResultSet.timeDimensionMember(timeDimension)] && moment(row[ResultSet.timeDimensionMember(timeDimension)]);
        }), ramda.filter(function (r) {
          return !!r;
        }))(this.timeDimensionBackwardCompatibleData());
        dateRange = dates.length && [ramda.reduce(ramda.minBy(function (d) {
          return d.toDate();
        }), dates[0], dates), ramda.reduce(ramda.maxBy(function (d) {
          return d.toDate();
        }), dates[0], dates)] || null;
      }

      if (!dateRange) {
        return null;
      }

      var padToDay = timeDimension.dateRange ? timeDimension.dateRange.find(function (d) {
        return d.match(DateRegex);
      }) : !['hour', 'minute', 'second'].includes(timeDimension.granularity);

      var _dateRange = dateRange,
          _dateRange2 = _slicedToArray__default['default'](_dateRange, 2),
          start = _dateRange2[0],
          end = _dateRange2[1];

      var range = moment.range(start, end);

      if (!TIME_SERIES[timeDimension.granularity]) {
        throw new Error("Unsupported time granularity: ".concat(timeDimension.granularity));
      }

      return TIME_SERIES[timeDimension.granularity](padToDay ? range.snapTo('day') : range);
    }
  }, {
    key: "pivot",
    value: function pivot(pivotConfig) {
      var _this2 = this;

      pivotConfig = this.normalizePivotConfig(pivotConfig);
      var query = this.loadResponse.pivotQuery;

      var pivotImpl = function pivotImpl() {
        var resultIndex = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : 0;
        var groupByXAxis = groupByToPairs(function (_ref11) {
          var xValues = _ref11.xValues;
          return _this2.axisValuesString(xValues);
        });

        var measureValue = function measureValue(row, measure) {
          return row[measure];
        };

        if (pivotConfig.fillMissingDates && pivotConfig.x.length === 1 && ramda.equals(pivotConfig.x, (query.timeDimensions || []).filter(function (td) {
          return !!td.granularity;
        }).map(function (td) {
          return ResultSet.timeDimensionMember(td);
        }))) {
          var series = _this2.loadResponses.map(function (loadResponse) {
            return _this2.timeSeries(loadResponse.query.timeDimensions[0]);
          });

          if (series[0]) {
            groupByXAxis = function groupByXAxis(rows) {
              var byXValues = ramda.groupBy(function (_ref12) {
                var xValues = _ref12.xValues;
                return moment(xValues[0]).format(moment.HTML5_FMT.DATETIME_LOCAL_MS);
              }, rows);
              return series[resultIndex].map(function (d) {
                return [d, byXValues[d] || [{
                  xValues: [d],
                  row: {}
                }]];
              });
            };

            measureValue = function measureValue(row, measure) {
              return row[measure] || 0;
            };
          }
        }

        var xGrouped = ramda.pipe(ramda.map(function (row) {
          return _this2.axisValues(pivotConfig.x, resultIndex)(row).map(function (xValues) {
            return {
              xValues: xValues,
              row: row
            };
          });
        }), ramda.unnest, groupByXAxis)(_this2.timeDimensionBackwardCompatibleData(resultIndex));
        var allYValues = ramda.pipe(ramda.map(function (_ref13) {
          var _ref14 = _slicedToArray__default['default'](_ref13, 2),
              rows = _ref14[1];

          return ramda.unnest( // collect Y values only from filled rows
          rows.filter(function (_ref15) {
            var row = _ref15.row;
            return Object.keys(row).length > 0;
          }).map(function (_ref16) {
            var row = _ref16.row;
            return _this2.axisValues(pivotConfig.y, resultIndex)(row);
          }));
        }), ramda.unnest, ramda.uniq)(xGrouped);
        return xGrouped.map(function (_ref17) {
          var _ref18 = _slicedToArray__default['default'](_ref17, 2),
              rows = _ref18[1];

          var xValues = rows[0].xValues;
          var yGrouped = ramda.pipe(ramda.map(function (_ref19) {
            var row = _ref19.row;
            return _this2.axisValues(pivotConfig.y, resultIndex)(row).map(function (yValues) {
              return {
                yValues: yValues,
                row: row
              };
            });
          }), ramda.unnest, ramda.groupBy(function (_ref20) {
            var yValues = _ref20.yValues;
            return _this2.axisValuesString(yValues);
          }))(rows);
          return {
            xValues: xValues,
            yValuesArray: ramda.unnest(allYValues.map(function (yValues) {
              var measure = pivotConfig.x.find(function (d) {
                return d === 'measures';
              }) ? ResultSet.measureFromAxis(xValues) : ResultSet.measureFromAxis(yValues);
              return (yGrouped[_this2.axisValuesString(yValues)] || [{
                row: {}
              }]).map(function (_ref21) {
                var row = _ref21.row;
                return [yValues, measureValue(row, measure)];
              });
            }))
          };
        });
      };

      var pivots = this.loadResponses.length > 1 ? this.loadResponses.map(function (_, index) {
        return pivotImpl(index);
      }) : [];
      return pivots.length ? this.mergePivots(pivots, pivotConfig.joinDateRange) : pivotImpl();
    }
  }, {
    key: "mergePivots",
    value: function mergePivots(pivots, joinDateRange) {
      var minLengthPivot = pivots.reduce(function (memo, current) {
        return memo != null && current.length >= memo.length ? memo : current;
      }, null);
      return minLengthPivot.map(function (_, index) {
        var xValues = joinDateRange ? [pivots.map(function (pivot) {
          return pivot[index] && pivot[index].xValues || [];
        }).join(', ')] : minLengthPivot[index].xValues;
        return {
          xValues: xValues,
          yValuesArray: ramda.unnest(pivots.map(function (pivot) {
            return pivot[index].yValuesArray;
          }))
        };
      });
    }
  }, {
    key: "pivotedRows",
    value: function pivotedRows(pivotConfig) {
      // TODO
      return this.chartPivot(pivotConfig);
    }
  }, {
    key: "chartPivot",
    value: function chartPivot(pivotConfig) {
      var _this3 = this;

      var validate = function validate(value) {
        if (_this3.parseDateMeasures && LocalDateRegex.test(value)) {
          return new Date(value);
        } else if (!Number.isNaN(Number.parseFloat(value))) {
          return Number.parseFloat(value);
        }

        return value;
      };

      var duplicateMeasures = new Set();

      if (this.queryType === QUERY_TYPE.BLENDING_QUERY) {
        var allMeasures = ramda.flatten(this.loadResponses.map(function (_ref22) {
          var query = _ref22.query;
          return query.measures;
        }));
        allMeasures.filter(function (e, i, a) {
          return a.indexOf(e) !== i;
        }).forEach(function (m) {
          return duplicateMeasures.add(m);
        });
      }

      var aliasSeries = function aliasSeries(yValues, i) {
        // manual alias
        if (pivotConfig && pivotConfig.aliasSeries && pivotConfig.aliasSeries[i]) {
          return [pivotConfig.aliasSeries[i]].concat(_toConsumableArray__default['default'](yValues));
        } else if (duplicateMeasures.has(yValues[0])) {
          return [i].concat(_toConsumableArray__default['default'](yValues));
        }

        return [yValues];
      };

      return this.pivot(pivotConfig).map(function (_ref23) {
        var xValues = _ref23.xValues,
            yValuesArray = _ref23.yValuesArray;
        return _objectSpread$3({
          category: _this3.axisValuesString(xValues, ','),
          // TODO deprecated
          x: _this3.axisValuesString(xValues, ','),
          xValues: xValues
        }, yValuesArray.map(function (_ref24, i) {
          var _ref25 = _slicedToArray__default['default'](_ref24, 2),
              yValues = _ref25[0],
              m = _ref25[1];

          return _defineProperty__default['default']({}, _this3.axisValuesString(aliasSeries(yValues, i), ','), m && validate(m));
        }).reduce(function (a, b) {
          return Object.assign(a, b);
        }, {}));
      });
    }
  }, {
    key: "tablePivot",
    value: function tablePivot(pivotConfig) {
      var normalizedPivotConfig = this.normalizePivotConfig(pivotConfig || {});
      var isMeasuresPresent = normalizedPivotConfig.x.concat(normalizedPivotConfig.y).includes('measures');
      return this.pivot(normalizedPivotConfig).map(function (_ref27) {
        var xValues = _ref27.xValues,
            yValuesArray = _ref27.yValuesArray;
        return ramda.fromPairs(normalizedPivotConfig.x.map(function (key, index) {
          return [key, xValues[index]];
        }).concat(isMeasuresPresent ? yValuesArray.map(function (_ref28) {
          var _ref29 = _slicedToArray__default['default'](_ref28, 2),
              yValues = _ref29[0],
              measure = _ref29[1];

          return [yValues.length ? yValues.join() : 'value', measure];
        }) : []));
      });
    }
  }, {
    key: "tableColumns",
    value: function tableColumns(pivotConfig) {
      var normalizedPivotConfig = this.normalizePivotConfig(pivotConfig || {});
      var annotations = ramda.pipe(ramda.pluck('annotation'), ramda.reduce(ramda.mergeDeepLeft(), {}))(this.loadResponses);
      var flatMeta = Object.values(annotations).reduce(function (a, b) {
        return _objectSpread$3(_objectSpread$3({}, a), b);
      }, {});
      var schema = {};

      var extractFields = function extractFields(key) {
        var _ref30 = flatMeta[key] || {},
            title = _ref30.title,
            shortTitle = _ref30.shortTitle,
            type = _ref30.type,
            format = _ref30.format,
            meta = _ref30.meta;

        return {
          key: key,
          title: title,
          shortTitle: shortTitle,
          type: type,
          format: format,
          meta: meta
        };
      };

      var pivot = this.pivot(normalizedPivotConfig);
      (pivot[0] && pivot[0].yValuesArray || []).forEach(function (_ref31) {
        var _ref32 = _slicedToArray__default['default'](_ref31, 1),
            yValues = _ref32[0];

        if (yValues.length > 0) {
          var currentItem = schema;
          yValues.forEach(function (value, index) {
            currentItem[value] = {
              key: value,
              memberId: normalizedPivotConfig.y[index] === 'measures' ? value : normalizedPivotConfig.y[index],
              children: currentItem[value] && currentItem[value].children || {}
            };
            currentItem = currentItem[value].children;
          });
        }
      });

      var toColumns = function toColumns() {
        var item = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};
        var path = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : [];

        if (Object.keys(item).length === 0) {
          return [];
        }

        return Object.values(item).map(function (_ref33) {
          var key = _ref33.key,
              currentItem = _objectWithoutProperties__default['default'](_ref33, ["key"]);

          var children = toColumns(currentItem.children, [].concat(_toConsumableArray__default['default'](path), [key]));

          var _extractFields = extractFields(currentItem.memberId),
              title = _extractFields.title,
              shortTitle = _extractFields.shortTitle,
              fields = _objectWithoutProperties__default['default'](_extractFields, ["title", "shortTitle"]);

          var dimensionValue = key !== currentItem.memberId || title == null ? key : '';

          if (!children.length) {
            return _objectSpread$3(_objectSpread$3({}, fields), {}, {
              key: key,
              dataIndex: [].concat(_toConsumableArray__default['default'](path), [key]).join(),
              title: [title, dimensionValue].join(' ').trim(),
              shortTitle: dimensionValue || shortTitle
            });
          }

          return _objectSpread$3(_objectSpread$3({}, fields), {}, {
            key: key,
            title: [title, dimensionValue].join(' ').trim(),
            shortTitle: dimensionValue || shortTitle,
            children: children
          });
        });
      };

      var otherColumns = [];

      if (!pivot.length && normalizedPivotConfig.y.includes('measures')) {
        otherColumns = (this.loadResponses[0].query.measures || []).map(function (key) {
          return _objectSpread$3(_objectSpread$3({}, extractFields(key)), {}, {
            dataIndex: key
          });
        });
      } // Syntatic column to display the measure value


      if (!normalizedPivotConfig.y.length && normalizedPivotConfig.x.includes('measures')) {
        otherColumns.push({
          key: 'value',
          dataIndex: 'value',
          title: 'Value',
          shortTitle: 'Value',
          type: 'string'
        });
      }

      return normalizedPivotConfig.x.map(function (key) {
        if (key === 'measures') {
          return {
            key: 'measures',
            dataIndex: 'measures',
            title: 'Measures',
            shortTitle: 'Measures',
            type: 'string'
          };
        }

        return _objectSpread$3(_objectSpread$3({}, extractFields(key)), {}, {
          dataIndex: key
        });
      }).concat(toColumns(schema)).concat(otherColumns);
    }
  }, {
    key: "totalRow",
    value: function totalRow(pivotConfig) {
      return this.chartPivot(pivotConfig)[0];
    }
  }, {
    key: "categories",
    value: function categories(pivotConfig) {
      // TODO
      return this.chartPivot(pivotConfig);
    }
  }, {
    key: "seriesNames",
    value: function seriesNames(pivotConfig) {
      var _this4 = this;

      pivotConfig = this.normalizePivotConfig(pivotConfig);
      var measures = ramda.pipe(ramda.pluck('annotation'), ramda.pluck('measures'), ramda.mergeAll)(this.loadResponses);
      var seriesNames = ramda.unnest(this.loadResponses.map(function (_, index) {
        return ramda.pipe(ramda.map(_this4.axisValues(pivotConfig.y, index)), ramda.unnest, ramda.uniq)(_this4.timeDimensionBackwardCompatibleData(index));
      }));
      var duplicateMeasures = new Set();

      if (this.queryType === QUERY_TYPE.BLENDING_QUERY) {
        var allMeasures = ramda.flatten(this.loadResponses.map(function (_ref34) {
          var query = _ref34.query;
          return query.measures;
        }));
        allMeasures.filter(function (e, i, a) {
          return a.indexOf(e) !== i;
        }).forEach(function (m) {
          return duplicateMeasures.add(m);
        });
      }

      var aliasSeries = function aliasSeries(yValues, i) {
        if (pivotConfig && pivotConfig.aliasSeries && pivotConfig.aliasSeries[i]) {
          return [pivotConfig.aliasSeries[i]].concat(_toConsumableArray__default['default'](yValues));
        } else if (duplicateMeasures.has(yValues[0])) {
          return [i].concat(_toConsumableArray__default['default'](yValues));
        }

        return yValues;
      };

      return seriesNames.map(function (axisValues, i) {
        var aliasedAxis = aliasSeries(axisValues, i);
        return {
          title: _this4.axisValuesString(pivotConfig.y.find(function (d) {
            return d === 'measures';
          }) ? ramda.dropLast(1, aliasedAxis).concat(measures[ResultSet.measureFromAxis(axisValues)].title) : aliasedAxis, ', '),
          key: _this4.axisValuesString(aliasedAxis, ','),
          yValues: axisValues
        };
      });
    }
  }, {
    key: "query",
    value: function query() {
      if (this.queryType !== QUERY_TYPE.REGULAR_QUERY) {
        throw new Error("Method is not supported for a '".concat(this.queryType, "' query type. Please use decompose"));
      }

      return this.loadResponses[0].query;
    }
  }, {
    key: "pivotQuery",
    value: function pivotQuery() {
      return this.loadResponse.pivotQuery || null;
    }
  }, {
    key: "rawData",
    value: function rawData() {
      if (this.queryType !== QUERY_TYPE.REGULAR_QUERY) {
        throw new Error("Method is not supported for a '".concat(this.queryType, "' query type. Please use decompose"));
      }

      return this.loadResponses[0].data;
    }
  }, {
    key: "annotation",
    value: function annotation() {
      if (this.queryType !== QUERY_TYPE.REGULAR_QUERY) {
        throw new Error("Method is not supported for a '".concat(this.queryType, "' query type. Please use decompose"));
      }

      return this.loadResponses[0].annotation;
    }
  }, {
    key: "timeDimensionBackwardCompatibleData",
    value: function timeDimensionBackwardCompatibleData() {
      var resultIndex = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : 0;

      if (!this.backwardCompatibleData[resultIndex]) {
        var _this$loadResponses$r = this.loadResponses[resultIndex],
            data = _this$loadResponses$r.data,
            query = _this$loadResponses$r.query;
        var timeDimensions = (query.timeDimensions || []).filter(function (td) {
          return !!td.granularity;
        });
        this.backwardCompatibleData[resultIndex] = data.map(function (row) {
          return _objectSpread$3(_objectSpread$3({}, row), ramda.fromPairs(Object.keys(row).filter(function (field) {
            return timeDimensions.find(function (d) {
              return d.dimension === field;
            }) && !row[ResultSet.timeDimensionMember(timeDimensions.find(function (d) {
              return d.dimension === field;
            }))];
          }).map(function (field) {
            return [ResultSet.timeDimensionMember(timeDimensions.find(function (d) {
              return d.dimension === field;
            })), row[field]];
          })));
        });
      }

      return this.backwardCompatibleData[resultIndex];
    }
  }, {
    key: "decompose",
    value: function decompose() {
      var _this5 = this;

      return this.loadResponses.map(function (result) {
        return new ResultSet({
          queryType: QUERY_TYPE.REGULAR_QUERY,
          pivotQuery: _objectSpread$3(_objectSpread$3({}, result.query), {}, {
            queryType: QUERY_TYPE.REGULAR_QUERY
          }),
          results: [result]
        }, _this5.options);
      });
    }
  }, {
    key: "serialize",
    value: function serialize() {
      return {
        loadResponse: ramda.clone(this.loadResponse)
      };
    }
  }], [{
    key: "measureFromAxis",
    value: function measureFromAxis(axisValues) {
      return axisValues[axisValues.length - 1];
    }
  }, {
    key: "timeDimensionMember",
    value: function timeDimensionMember(td) {
      return "".concat(td.dimension, ".").concat(td.granularity);
    }
  }, {
    key: "deserialize",
    value: function deserialize(data) {
      var options = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : {};
      return new ResultSet(data.loadResponse, options);
    }
  }, {
    key: "getNormalizedPivotConfig",
    value: function getNormalizedPivotConfig(query) {
      var pivotConfig = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : null;
      var defaultPivotConfig = {
        x: [],
        y: [],
        fillMissingDates: true,
        joinDateRange: false
      };
      var _query$measures = query.measures,
          measures = _query$measures === void 0 ? [] : _query$measures,
          _query$dimensions = query.dimensions,
          dimensions = _query$dimensions === void 0 ? [] : _query$dimensions;
      var timeDimensions = (query.timeDimensions || []).filter(function (td) {
        return !!td.granularity;
      });
      pivotConfig = pivotConfig || (timeDimensions.length ? {
        x: timeDimensions.map(function (td) {
          return ResultSet.timeDimensionMember(td);
        }),
        y: dimensions
      } : {
        x: dimensions,
        y: []
      });
      pivotConfig = ramda.mergeDeepLeft(pivotConfig, defaultPivotConfig);

      var substituteTimeDimensionMembers = function substituteTimeDimensionMembers(axis) {
        return axis.map(function (subDim) {
          return timeDimensions.find(function (td) {
            return td.dimension === subDim;
          }) && !dimensions.find(function (d) {
            return d === subDim;
          }) ? ResultSet.timeDimensionMember(query.timeDimensions.find(function (td) {
            return td.dimension === subDim;
          })) : subDim;
        });
      };

      pivotConfig.x = substituteTimeDimensionMembers(pivotConfig.x);
      pivotConfig.y = substituteTimeDimensionMembers(pivotConfig.y);
      var allIncludedDimensions = pivotConfig.x.concat(pivotConfig.y);
      var allDimensions = timeDimensions.map(function (td) {
        return ResultSet.timeDimensionMember(td);
      }).concat(dimensions);

      var dimensionFilter = function dimensionFilter(key) {
        return allDimensions.includes(key) || key === 'measures';
      };

      pivotConfig.x = pivotConfig.x.concat(allDimensions.filter(function (d) {
        return !allIncludedDimensions.includes(d);
      })).filter(dimensionFilter);
      pivotConfig.y = pivotConfig.y.filter(dimensionFilter);

      if (!pivotConfig.x.concat(pivotConfig.y).find(function (d) {
        return d === 'measures';
      })) {
        pivotConfig.y.push('measures');
      }

      if (!measures.length) {
        pivotConfig.x = pivotConfig.x.filter(function (d) {
          return d !== 'measures';
        });
        pivotConfig.y = pivotConfig.y.filter(function (d) {
          return d !== 'measures';
        });
      }

      return pivotConfig;
    }
  }]);

  return ResultSet;
}();

var SqlQuery = /*#__PURE__*/function () {
  function SqlQuery(sqlQuery) {
    _classCallCheck__default['default'](this, SqlQuery);

    this.sqlQuery = sqlQuery;
  }

  _createClass__default['default'](SqlQuery, [{
    key: "rawQuery",
    value: function rawQuery() {
      return this.sqlQuery.sql;
    }
  }, {
    key: "sql",
    value: function sql() {
      return this.rawQuery().sql[0];
    }
  }]);

  return SqlQuery;
}();

var memberMap = function memberMap(memberArray) {
  return ramda.fromPairs(memberArray.map(function (m) {
    return [m.name, m];
  }));
};

var operators = {
  string: [{
    name: 'contains',
    title: 'contains'
  }, {
    name: 'notContains',
    title: 'does not contain'
  }, {
    name: 'equals',
    title: 'equals'
  }, {
    name: 'notEquals',
    title: 'does not equal'
  }, {
    name: 'set',
    title: 'is set'
  }, {
    name: 'notSet',
    title: 'is not set'
  }],
  number: [{
    name: 'equals',
    title: 'equals'
  }, {
    name: 'notEquals',
    title: 'does not equal'
  }, {
    name: 'set',
    title: 'is set'
  }, {
    name: 'notSet',
    title: 'is not set'
  }, {
    name: 'gt',
    title: '>'
  }, {
    name: 'gte',
    title: '>='
  }, {
    name: 'lt',
    title: '<'
  }, {
    name: 'lte',
    title: '<='
  }]
};
/**
 * Contains information about available cubes and it's members.
 */

var Meta = /*#__PURE__*/function () {
  function Meta(metaResponse) {
    _classCallCheck__default['default'](this, Meta);

    this.meta = metaResponse;
    var cubes = this.meta.cubes;
    this.cubes = cubes;
    this.cubesMap = ramda.fromPairs(cubes.map(function (c) {
      return [c.name, {
        measures: memberMap(c.measures),
        dimensions: memberMap(c.dimensions),
        segments: memberMap(c.segments)
      }];
    }));
  }

  _createClass__default['default'](Meta, [{
    key: "membersForQuery",
    value: function membersForQuery(query, memberType) {
      return ramda.unnest(this.cubes.map(function (c) {
        return c[memberType];
      }));
    }
  }, {
    key: "resolveMember",
    value: function resolveMember(memberName, memberType) {
      var _this = this;

      var _memberName$split = memberName.split('.'),
          _memberName$split2 = _slicedToArray__default['default'](_memberName$split, 1),
          cube = _memberName$split2[0];

      if (!this.cubesMap[cube]) {
        return {
          title: memberName,
          error: "Cube not found ".concat(cube, " for path '").concat(memberName, "'")
        };
      }

      var memberTypes = Array.isArray(memberType) ? memberType : [memberType];
      var member = memberTypes.map(function (type) {
        return _this.cubesMap[cube][type] && _this.cubesMap[cube][type][memberName];
      }).find(function (m) {
        return m;
      });

      if (!member) {
        return {
          title: memberName,
          error: "Path not found '".concat(memberName, "'")
        };
      }

      return member;
    }
  }, {
    key: "defaultTimeDimensionNameFor",
    value: function defaultTimeDimensionNameFor(memberName) {
      var _this2 = this;

      var _memberName$split3 = memberName.split('.'),
          _memberName$split4 = _slicedToArray__default['default'](_memberName$split3, 1),
          cube = _memberName$split4[0];

      if (!this.cubesMap[cube]) {
        return null;
      }

      return Object.keys(this.cubesMap[cube].dimensions || {}).find(function (d) {
        return _this2.cubesMap[cube].dimensions[d].type === 'time';
      });
    }
  }, {
    key: "filterOperatorsForMember",
    value: function filterOperatorsForMember(memberName, memberType) {
      var member = this.resolveMember(memberName, memberType);
      return operators[member.type] || operators.string;
    }
  }]);

  return Meta;
}();

var ProgressResult = /*#__PURE__*/function () {
  function ProgressResult(progressResponse) {
    _classCallCheck__default['default'](this, ProgressResult);

    this.progressResponse = progressResponse;
  }

  _createClass__default['default'](ProgressResult, [{
    key: "stage",
    value: function stage() {
      return this.progressResponse.stage;
    }
  }, {
    key: "timeElapsed",
    value: function timeElapsed() {
      return this.progressResponse.timeElapsed;
    }
  }]);

  return ProgressResult;
}();

function ownKeys$2(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$2(Object(source), true).forEach(function (key) { _defineProperty__default['default'](target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$2(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

var HttpTransport = /*#__PURE__*/function () {
  function HttpTransport(_ref) {
    var authorization = _ref.authorization,
        apiUrl = _ref.apiUrl,
        method = _ref.method,
        _ref$headers = _ref.headers,
        headers = _ref$headers === void 0 ? {} : _ref$headers,
        credentials = _ref.credentials;

    _classCallCheck__default['default'](this, HttpTransport);

    this.authorization = authorization;
    this.apiUrl = apiUrl;
    this.method = method;
    this.headers = headers;
    this.credentials = credentials;
  }

  _createClass__default['default'](HttpTransport, [{
    key: "request",
    value: function request(method, _ref2) {
      var _this = this;

      var baseRequestId = _ref2.baseRequestId,
          params = _objectWithoutProperties__default['default'](_ref2, ["baseRequestId"]);

      var spanCounter = 1;
      var searchParams = new URLSearchParams(params && Object.keys(params).map(function (k) {
        return _defineProperty__default['default']({}, k, _typeof__default['default'](params[k]) === 'object' ? JSON.stringify(params[k]) : params[k]);
      }).reduce(function (a, b) {
        return _objectSpread$2(_objectSpread$2({}, a), b);
      }, {}));
      var url = "".concat(this.apiUrl, "/").concat(method).concat(searchParams.toString().length ? "?".concat(searchParams) : '');
      this.method = this.method || (url.length < 2000 ? 'GET' : 'POST');

      if (this.method === 'POST') {
        url = "".concat(this.apiUrl, "/").concat(method);
        this.headers['Content-Type'] = 'application/json';
      } // Currently, all methods make GET requests. If a method makes a request with a body payload,
      // remember to add {'Content-Type': 'application/json'} to the header.


      var runRequest = function runRequest() {
        return fetch__default['default'](url, {
          method: _this.method,
          headers: _objectSpread$2({
            Authorization: _this.authorization,
            'x-request-id': baseRequestId && "".concat(baseRequestId, "-span-").concat(spanCounter++)
          }, _this.headers),
          credentials: _this.credentials,
          body: _this.method === 'POST' ? JSON.stringify(params) : null
        });
      };

      return {
        subscribe: function subscribe(callback) {
          var _this2 = this;

          return _asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee() {
            var result;
            return _regeneratorRuntime__default['default'].wrap(function _callee$(_context) {
              while (1) {
                switch (_context.prev = _context.next) {
                  case 0:
                    _context.next = 2;
                    return runRequest();

                  case 2:
                    result = _context.sent;
                    return _context.abrupt("return", callback(result, function () {
                      return _this2.subscribe(callback);
                    }));

                  case 4:
                  case "end":
                    return _context.stop();
                }
              }
            }, _callee);
          }))();
        }
      };
    }
  }]);

  return HttpTransport;
}();

function _createSuper(Derived) { var hasNativeReflectConstruct = _isNativeReflectConstruct(); return function _createSuperInternal() { var Super = _getPrototypeOf__default['default'](Derived), result; if (hasNativeReflectConstruct) { var NewTarget = _getPrototypeOf__default['default'](this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn__default['default'](this, result); }; }

function _isNativeReflectConstruct() { if (typeof Reflect === "undefined" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === "function") return true; try { Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {})); return true; } catch (e) { return false; } }

var RequestError = /*#__PURE__*/function (_Error) {
  _inherits__default['default'](RequestError, _Error);

  var _super = _createSuper(RequestError);

  function RequestError(message, response) {
    var _this;

    _classCallCheck__default['default'](this, RequestError);

    _this = _super.call(this, message);
    _this.response = response;
    return _this;
  }

  return RequestError;
}( /*#__PURE__*/_wrapNativeSuper__default['default'](Error));

function ownKeys$1(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread$1(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys$1(Object(source), true).forEach(function (key) { _defineProperty__default['default'](target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys$1(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }
var DEFAULT_GRANULARITY = 'day';
var GRANULARITIES = [{
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
function areQueriesEqual() {
  var query1 = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};
  var query2 = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : {};
  return ramda.equals(Object.entries(query1 && query1.order || {}), Object.entries(query2 && query2.order || {})) && ramda.equals(query1, query2);
}
function defaultOrder(query) {
  var granularity = (query.timeDimensions || []).find(function (d) {
    return d.granularity;
  });

  if (granularity) {
    return _defineProperty__default['default']({}, granularity.dimension, 'asc');
  } else if ((query.measures || []).length > 0 && (query.dimensions || []).length > 0) {
    return _defineProperty__default['default']({}, query.measures[0], 'desc');
  } else if ((query.dimensions || []).length > 0) {
    return _defineProperty__default['default']({}, query.dimensions[0], 'asc');
  }

  return {};
}
function defaultHeuristics(newState) {
  var oldQuery = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : {};
  var options = arguments.length > 2 ? arguments[2] : undefined;

  var _clone = ramda.clone(newState),
      query = _clone.query,
      props = _objectWithoutProperties__default['default'](_clone, ["query"]);

  var meta = options.meta,
      sessionGranularity = options.sessionGranularity;
  var granularity = sessionGranularity || DEFAULT_GRANULARITY;

  var state = _objectSpread$1({
    query: query
  }, props);

  var newQuery = null;

  if (!areQueriesEqual(query, oldQuery)) {
    newQuery = query;
  }

  if (Array.isArray(newQuery) || Array.isArray(oldQuery)) {
    return newState;
  }

  if (newQuery) {
    if ((oldQuery.timeDimensions || []).length === 1 && (newQuery.timeDimensions || []).length === 1 && newQuery.timeDimensions[0].granularity && oldQuery.timeDimensions[0].granularity !== newQuery.timeDimensions[0].granularity) {
      state = _objectSpread$1(_objectSpread$1({}, state), {}, {
        sessionGranularity: newQuery.timeDimensions[0].granularity
      });
    }

    if ((oldQuery.measures || []).length === 0 && (newQuery.measures || []).length > 0 || (oldQuery.measures || []).length === 1 && (newQuery.measures || []).length === 1 && oldQuery.measures[0] !== newQuery.measures[0]) {
      var _ref4 = newQuery.timeDimensions || [],
          _ref5 = _slicedToArray__default['default'](_ref4, 1),
          td = _ref5[0];

      var defaultTimeDimension = meta.defaultTimeDimensionNameFor(newQuery.measures[0]);
      newQuery = _objectSpread$1(_objectSpread$1({}, newQuery), {}, {
        timeDimensions: defaultTimeDimension ? [{
          dimension: defaultTimeDimension,
          granularity: td && td.granularity || granularity,
          dateRange: td && td.dateRange
        }] : []
      });
      return _objectSpread$1(_objectSpread$1({}, state), {}, {
        pivotConfig: null,
        shouldApplyHeuristicOrder: true,
        query: newQuery,
        chartType: defaultTimeDimension ? 'line' : 'number'
      });
    }

    if ((oldQuery.dimensions || []).length === 0 && (newQuery.dimensions || []).length > 0) {
      newQuery = _objectSpread$1(_objectSpread$1({}, newQuery), {}, {
        timeDimensions: (newQuery.timeDimensions || []).map(function (td) {
          return _objectSpread$1(_objectSpread$1({}, td), {}, {
            granularity: undefined
          });
        })
      });
      return _objectSpread$1(_objectSpread$1({}, state), {}, {
        pivotConfig: null,
        shouldApplyHeuristicOrder: true,
        query: newQuery,
        chartType: 'table'
      });
    }

    if ((oldQuery.dimensions || []).length > 0 && (newQuery.dimensions || []).length === 0) {
      newQuery = _objectSpread$1(_objectSpread$1({}, newQuery), {}, {
        timeDimensions: (newQuery.timeDimensions || []).map(function (td) {
          return _objectSpread$1(_objectSpread$1({}, td), {}, {
            granularity: td.granularity || granularity
          });
        })
      });
      return _objectSpread$1(_objectSpread$1({}, state), {}, {
        pivotConfig: null,
        shouldApplyHeuristicOrder: true,
        query: newQuery,
        chartType: (newQuery.timeDimensions || []).length ? 'line' : 'number'
      });
    }

    if (((oldQuery.dimensions || []).length > 0 || (oldQuery.measures || []).length > 0) && (newQuery.dimensions || []).length === 0 && (newQuery.measures || []).length === 0) {
      newQuery = _objectSpread$1(_objectSpread$1({}, newQuery), {}, {
        timeDimensions: [],
        filters: []
      });
      return _objectSpread$1(_objectSpread$1({}, state), {}, {
        pivotConfig: null,
        shouldApplyHeuristicOrder: true,
        query: newQuery,
        sessionGranularity: null
      });
    }

    return state;
  }

  if (state.chartType) {
    var newChartType = state.chartType;

    if ((newChartType === 'line' || newChartType === 'area') && (oldQuery.timeDimensions || []).length === 1 && !oldQuery.timeDimensions[0].granularity) {
      var _oldQuery$timeDimensi = _slicedToArray__default['default'](oldQuery.timeDimensions, 1),
          _td = _oldQuery$timeDimensi[0];

      return _objectSpread$1(_objectSpread$1({}, state), {}, {
        pivotConfig: null,
        query: _objectSpread$1(_objectSpread$1({}, oldQuery), {}, {
          timeDimensions: [_objectSpread$1(_objectSpread$1({}, _td), {}, {
            granularity: granularity
          })]
        })
      });
    }

    if ((newChartType === 'pie' || newChartType === 'table' || newChartType === 'number') && (oldQuery.timeDimensions || []).length === 1 && oldQuery.timeDimensions[0].granularity) {
      var _oldQuery$timeDimensi2 = _slicedToArray__default['default'](oldQuery.timeDimensions, 1),
          _td2 = _oldQuery$timeDimensi2[0];

      return _objectSpread$1(_objectSpread$1({}, state), {}, {
        pivotConfig: null,
        shouldApplyHeuristicOrder: true,
        query: _objectSpread$1(_objectSpread$1({}, oldQuery), {}, {
          timeDimensions: [_objectSpread$1(_objectSpread$1({}, _td2), {}, {
            granularity: undefined
          })]
        })
      });
    }
  }

  return state;
}
function isQueryPresent(query) {
  if (!query) {
    return false;
  }

  return (Array.isArray(query) ? query : [query]).every(function (q) {
    return q.measures && q.measures.length || q.dimensions && q.dimensions.length || q.timeDimensions && q.timeDimensions.length;
  });
}
function movePivotItem(pivotConfig, sourceIndex, destinationIndex, sourceAxis, destinationAxis) {
  var nextPivotConfig = _objectSpread$1(_objectSpread$1({}, pivotConfig), {}, {
    x: _toConsumableArray__default['default'](pivotConfig.x),
    y: _toConsumableArray__default['default'](pivotConfig.y)
  });

  var id = pivotConfig[sourceAxis][sourceIndex];
  var lastIndex = nextPivotConfig[destinationAxis].length - 1;

  if (id === 'measures') {
    destinationIndex = lastIndex + 1;
  } else if (destinationIndex >= lastIndex && nextPivotConfig[destinationAxis][lastIndex] === 'measures') {
    destinationIndex = lastIndex - 1;
  }

  nextPivotConfig[sourceAxis].splice(sourceIndex, 1);
  nextPivotConfig[destinationAxis].splice(destinationIndex, 0, id);
  return nextPivotConfig;
}
function moveItemInArray(list, sourceIndex, destinationIndex) {
  var result = _toConsumableArray__default['default'](list);

  var _result$splice = result.splice(sourceIndex, 1),
      _result$splice2 = _slicedToArray__default['default'](_result$splice, 1),
      removed = _result$splice2[0];

  result.splice(destinationIndex, 0, removed);
  return result;
}
function flattenFilters() {
  var filters = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : [];
  return filters.reduce(function (memo, filter) {
    if (filter.or || filter.and) {
      return [].concat(_toConsumableArray__default['default'](memo), _toConsumableArray__default['default'](flattenFilters(filter.or || filter.and)));
    }

    return [].concat(_toConsumableArray__default['default'](memo), [filter]);
  }, []);
}
function getQueryMembers() {
  var query = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};
  var keys = ['measures', 'dimensions', 'segments'];
  var members = new Set();
  keys.forEach(function (key) {
    return (query[key] || []).forEach(function (member) {
      return members.add(member);
    });
  });
  (query.timeDimensions || []).forEach(function (td) {
    return members.add(td.dimension);
  });
  flattenFilters(query.filters).forEach(function (filter) {
    return members.add(filter.dimension || filter.member);
  });
  return _toConsumableArray__default['default'](members);
}
function getOrderMembersFromOrder(orderMembers, order) {
  var ids = new Set();
  var indexedOrderMembers = ramda.indexBy(ramda.prop('id'), orderMembers);
  var entries = Array.isArray(order) ? order : Object.entries(order || {});
  var nextOrderMembers = [];
  entries.forEach(function (_ref6) {
    var _ref7 = _slicedToArray__default['default'](_ref6, 2),
        memberId = _ref7[0],
        currentOrder = _ref7[1];

    if (currentOrder !== 'none' && indexedOrderMembers[memberId]) {
      ids.add(memberId);
      nextOrderMembers.push(_objectSpread$1(_objectSpread$1({}, indexedOrderMembers[memberId]), {}, {
        order: currentOrder
      }));
    }
  });
  orderMembers.forEach(function (member) {
    if (!ids.has(member.id)) {
      nextOrderMembers.push(_objectSpread$1(_objectSpread$1({}, member), {}, {
        order: member.order || 'none'
      }));
    }
  });
  return nextOrderMembers;
}

function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty__default['default'](target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }
var mutexCounter = 0;
var MUTEX_ERROR = 'Mutex has been changed';

var mutexPromise = function mutexPromise(promise) {
  return new Promise(function (resolve, reject) {
    promise.then(function (r) {
      return resolve(r);
    }, function (e) {
      return e !== MUTEX_ERROR && reject(e);
    });
  });
};

var CubejsApi = /*#__PURE__*/function () {
  function CubejsApi(apiToken, options) {
    _classCallCheck__default['default'](this, CubejsApi);

    if (_typeof__default['default'](apiToken) === 'object') {
      options = apiToken;
      apiToken = undefined;
    }

    options = options || {};

    if (!options.transport && !options.apiUrl) {
      throw new Error('The `apiUrl` option is required');
    }

    this.apiToken = apiToken;
    this.apiUrl = options.apiUrl;
    this.method = options.method;
    this.headers = options.headers || {};
    this.credentials = options.credentials;
    this.transport = options.transport || new HttpTransport({
      authorization: typeof apiToken === 'function' ? undefined : apiToken,
      apiUrl: this.apiUrl,
      method: this.method,
      headers: this.headers,
      credentials: this.credentials
    });
    this.pollInterval = options.pollInterval || 5;
    this.parseDateMeasures = options.parseDateMeasures;
  }

  _createClass__default['default'](CubejsApi, [{
    key: "request",
    value: function request(method, params) {
      return this.transport.request(method, _objectSpread({
        baseRequestId: uuid__default['default']()
      }, params));
    }
  }, {
    key: "loadMethod",
    value: function loadMethod(request, toResult, options, callback) {
      var _this = this;

      var mutexValue = ++mutexCounter;

      if (typeof options === 'function' && !callback) {
        callback = options;
        options = undefined;
      }

      options = options || {};
      var mutexKey = options.mutexKey || 'default';

      if (options.mutexObj) {
        options.mutexObj[mutexKey] = mutexValue;
      }

      var requestPromise = this.updateTransportAuthorization().then(function () {
        return request();
      });
      var unsubscribed = false;

      var checkMutex = /*#__PURE__*/function () {
        var _ref = _asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee() {
          var requestInstance;
          return _regeneratorRuntime__default['default'].wrap(function _callee$(_context) {
            while (1) {
              switch (_context.prev = _context.next) {
                case 0:
                  _context.next = 2;
                  return requestPromise;

                case 2:
                  requestInstance = _context.sent;

                  if (!(options.mutexObj && options.mutexObj[mutexKey] !== mutexValue)) {
                    _context.next = 9;
                    break;
                  }

                  unsubscribed = true;

                  if (!requestInstance.unsubscribe) {
                    _context.next = 8;
                    break;
                  }

                  _context.next = 8;
                  return requestInstance.unsubscribe();

                case 8:
                  throw MUTEX_ERROR;

                case 9:
                case "end":
                  return _context.stop();
              }
            }
          }, _callee);
        }));

        return function checkMutex() {
          return _ref.apply(this, arguments);
        };
      }();

      var loadImpl = /*#__PURE__*/function () {
        var _ref2 = _asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee4(response, next) {
          var requestInstance, subscribeNext, continueWait, body, error, result;
          return _regeneratorRuntime__default['default'].wrap(function _callee4$(_context4) {
            while (1) {
              switch (_context4.prev = _context4.next) {
                case 0:
                  _context4.next = 2;
                  return requestPromise;

                case 2:
                  requestInstance = _context4.sent;

                  subscribeNext = /*#__PURE__*/function () {
                    var _ref3 = _asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee2() {
                      return _regeneratorRuntime__default['default'].wrap(function _callee2$(_context2) {
                        while (1) {
                          switch (_context2.prev = _context2.next) {
                            case 0:
                              if (!(options.subscribe && !unsubscribed)) {
                                _context2.next = 8;
                                break;
                              }

                              if (!requestInstance.unsubscribe) {
                                _context2.next = 5;
                                break;
                              }

                              return _context2.abrupt("return", next());

                            case 5:
                              _context2.next = 7;
                              return new Promise(function (resolve) {
                                return setTimeout(function () {
                                  return resolve();
                                }, _this.pollInterval * 1000);
                              });

                            case 7:
                              return _context2.abrupt("return", next());

                            case 8:
                              return _context2.abrupt("return", null);

                            case 9:
                            case "end":
                              return _context2.stop();
                          }
                        }
                      }, _callee2);
                    }));

                    return function subscribeNext() {
                      return _ref3.apply(this, arguments);
                    };
                  }();

                  continueWait = /*#__PURE__*/function () {
                    var _ref4 = _asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee3(wait) {
                      return _regeneratorRuntime__default['default'].wrap(function _callee3$(_context3) {
                        while (1) {
                          switch (_context3.prev = _context3.next) {
                            case 0:
                              if (unsubscribed) {
                                _context3.next = 5;
                                break;
                              }

                              if (!wait) {
                                _context3.next = 4;
                                break;
                              }

                              _context3.next = 4;
                              return new Promise(function (resolve) {
                                return setTimeout(function () {
                                  return resolve();
                                }, _this.pollInterval * 1000);
                              });

                            case 4:
                              return _context3.abrupt("return", next());

                            case 5:
                              return _context3.abrupt("return", null);

                            case 6:
                            case "end":
                              return _context3.stop();
                          }
                        }
                      }, _callee3);
                    }));

                    return function continueWait(_x3) {
                      return _ref4.apply(this, arguments);
                    };
                  }();

                  _context4.next = 7;
                  return _this.updateTransportAuthorization();

                case 7:
                  if (!(response.status === 502)) {
                    _context4.next = 11;
                    break;
                  }

                  _context4.next = 10;
                  return checkMutex();

                case 10:
                  return _context4.abrupt("return", continueWait(true));

                case 11:
                  if (!(response.status < 200 || response.status > 299)) {
                    _context4.next = 13;
                    break;
                  }

                  throw new Error("Request error. Response status: ".concat(response.status));

                case 13:
                  _context4.next = 15;
                  return response.json();

                case 15:
                  body = _context4.sent;

                  if (!(body.error === 'Continue wait')) {
                    _context4.next = 21;
                    break;
                  }

                  _context4.next = 19;
                  return checkMutex();

                case 19:
                  if (options.progressCallback) {
                    options.progressCallback(new ProgressResult(body));
                  }

                  return _context4.abrupt("return", continueWait());

                case 21:
                  if (!(response.status !== 200)) {
                    _context4.next = 34;
                    break;
                  }

                  _context4.next = 24;
                  return checkMutex();

                case 24:
                  if (!(!options.subscribe && requestInstance.unsubscribe)) {
                    _context4.next = 27;
                    break;
                  }

                  _context4.next = 27;
                  return requestInstance.unsubscribe();

                case 27:
                  error = new RequestError(body.error, body); // TODO error class

                  if (!callback) {
                    _context4.next = 32;
                    break;
                  }

                  callback(error);
                  _context4.next = 33;
                  break;

                case 32:
                  throw error;

                case 33:
                  return _context4.abrupt("return", subscribeNext());

                case 34:
                  _context4.next = 36;
                  return checkMutex();

                case 36:
                  if (!(!options.subscribe && requestInstance.unsubscribe)) {
                    _context4.next = 39;
                    break;
                  }

                  _context4.next = 39;
                  return requestInstance.unsubscribe();

                case 39:
                  result = toResult(body);

                  if (!callback) {
                    _context4.next = 44;
                    break;
                  }

                  callback(null, result);
                  _context4.next = 45;
                  break;

                case 44:
                  return _context4.abrupt("return", result);

                case 45:
                  return _context4.abrupt("return", subscribeNext());

                case 46:
                case "end":
                  return _context4.stop();
              }
            }
          }, _callee4);
        }));

        return function loadImpl(_x, _x2) {
          return _ref2.apply(this, arguments);
        };
      }();

      var promise = requestPromise.then(function (requestInstance) {
        return mutexPromise(requestInstance.subscribe(loadImpl));
      });

      if (callback) {
        return {
          unsubscribe: function () {
            var _unsubscribe = _asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee5() {
              var requestInstance;
              return _regeneratorRuntime__default['default'].wrap(function _callee5$(_context5) {
                while (1) {
                  switch (_context5.prev = _context5.next) {
                    case 0:
                      _context5.next = 2;
                      return requestPromise;

                    case 2:
                      requestInstance = _context5.sent;
                      unsubscribed = true;

                      if (!requestInstance.unsubscribe) {
                        _context5.next = 6;
                        break;
                      }

                      return _context5.abrupt("return", requestInstance.unsubscribe());

                    case 6:
                      return _context5.abrupt("return", null);

                    case 7:
                    case "end":
                      return _context5.stop();
                  }
                }
              }, _callee5);
            }));

            function unsubscribe() {
              return _unsubscribe.apply(this, arguments);
            }

            return unsubscribe;
          }()
        };
      } else {
        return promise;
      }
    }
  }, {
    key: "updateTransportAuthorization",
    value: function () {
      var _updateTransportAuthorization = _asyncToGenerator__default['default']( /*#__PURE__*/_regeneratorRuntime__default['default'].mark(function _callee6() {
        var token;
        return _regeneratorRuntime__default['default'].wrap(function _callee6$(_context6) {
          while (1) {
            switch (_context6.prev = _context6.next) {
              case 0:
                if (!(typeof this.apiToken === 'function')) {
                  _context6.next = 5;
                  break;
                }

                _context6.next = 3;
                return this.apiToken();

              case 3:
                token = _context6.sent;

                if (this.transport.authorization !== token) {
                  this.transport.authorization = token;
                }

              case 5:
              case "end":
                return _context6.stop();
            }
          }
        }, _callee6, this);
      }));

      function updateTransportAuthorization() {
        return _updateTransportAuthorization.apply(this, arguments);
      }

      return updateTransportAuthorization;
    }()
  }, {
    key: "load",
    value: function load(query, options, callback) {
      var _this2 = this;

      return this.loadMethod(function () {
        return _this2.request('load', {
          query: query,
          queryType: 'multi'
        });
      }, function (response) {
        return new ResultSet(response, {
          parseDateMeasures: _this2.parseDateMeasures
        });
      }, options, callback);
    }
  }, {
    key: "sql",
    value: function sql(query, options, callback) {
      var _this3 = this;

      return this.loadMethod(function () {
        return _this3.request('sql', {
          query: query
        });
      }, function (response) {
        return Array.isArray(response) ? response.map(function (body) {
          return new SqlQuery(body);
        }) : new SqlQuery(response);
      }, options, callback);
    }
  }, {
    key: "meta",
    value: function meta(options, callback) {
      var _this4 = this;

      return this.loadMethod(function () {
        return _this4.request('meta');
      }, function (body) {
        return new Meta(body);
      }, options, callback);
    }
  }, {
    key: "dryRun",
    value: function dryRun(query, options, callback) {
      var _this5 = this;

      return this.loadMethod(function () {
        return _this5.request('dry-run', {
          query: query
        });
      }, function (response) {
        return response;
      }, options, callback);
    }
  }, {
    key: "subscribe",
    value: function subscribe(query, options, callback) {
      var _this6 = this;

      return this.loadMethod(function () {
        return _this6.request('subscribe', {
          query: query,
          queryType: 'multi'
        });
      }, function (body) {
        return new ResultSet(body, {
          parseDateMeasures: _this6.parseDateMeasures
        });
      }, _objectSpread(_objectSpread({}, options), {}, {
        subscribe: true
      }), callback);
    }
  }]);

  return CubejsApi;
}();

var index = (function (apiToken, options) {
  return new CubejsApi(apiToken, options);
});

exports.GRANULARITIES = GRANULARITIES;
exports.HttpTransport = HttpTransport;
exports.ResultSet = ResultSet;
exports.areQueriesEqual = areQueriesEqual;
exports.default = index;
exports.defaultHeuristics = defaultHeuristics;
exports.defaultOrder = defaultOrder;
exports.flattenFilters = flattenFilters;
exports.getOrderMembersFromOrder = getOrderMembersFromOrder;
exports.getQueryMembers = getQueryMembers;
exports.isQueryPresent = isQueryPresent;
exports.moveItemInArray = moveItemInArray;
exports.movePivotItem = movePivotItem;
//# sourceMappingURL=cubejs-client-core.js.map
