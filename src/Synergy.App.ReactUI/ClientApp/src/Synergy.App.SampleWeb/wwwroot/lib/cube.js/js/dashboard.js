import ReactDOM from 'react-dom';
import cubejs from '@cubejs-client/core';
import { QueryRenderer } from '@cubejs-client/react';
import { Spin } from 'antd';
import '~/lib/cube.js/css/antd.css';
import React from 'react';
import { Line, Bar, Pie } from 'react-chartjs-2';
import { Row, Col, Statistic, Table } from 'antd';

const COLORS_SERIES = ['#FF6492', '#141446', '#7A77FF'];
const commonOptions = {
    maintainAspectRatio: false,
};


const cubejsApi = cubejs(
    'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpYXQiOjE2MTg2MzcxNzYsImV4cCI6MTYxODcyMzU3Nn0.4gjfNUobTJbCWDe_qll07cZk6LNNKYE9lMFKZHeWFYM',
    { apiUrl: 'http://95.111.235.64:4000/cubejs-api/v1' }
);

const renderChart = ({ resultSet, error, pivotConfig }) => {
    if (error) {
        return <div>{error.toString()}</div>;
    }

    if (!resultSet) {
        return <Spin />;
    }

    const data = {
        labels: resultSet.categories().map((c) => c.category),
        datasets: resultSet.series().map((s, index) => ({
            label: s.title,
            data: s.series.map((r) => r.value),
            borderColor: COLORS_SERIES[index],
            fill: false,
        })),
    };
    const options = { ...commonOptions };
    return <Line data={data} options={options} />;

};

const ChartRenderer = () => {
    return (
        <QueryRenderer
            query={{
                "measures": [
                    "Application.count"
                ],
                "timeDimensions": [
                    {
                        "dimension": "Application.createddate",
                        "granularity": "day"
                    }
                ],
                "order": {
                    "Application.createddate": "asc"
                }
            }}
            cubejsApi={cubejsApi}
            resetResultSetOnChange={false}
            render={(props) => renderChart({
                ...props,
                chartType: 'line',
                pivotConfig: {
                    "x": [
                        "Application.createddate.day"
                    ],
                    "y": [
                        "measures"
                    ],
                    "fillMissingDates": true,
                    "joinDateRange": false
                }
            })}
        />
    );
};

const rootElement = document.getElementById('root');
ReactDOM.render(<ChartRenderer />, rootElement);
