// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import { styled } from '@mui/material/styles'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import MuiAvatar from '@mui/material/Avatar'
import { Fragment, useState, useEffect } from 'react'
import Icon from 'src/@core/components/icon'
import CardRate from './CardRate'
import SalesChard from './SalesCard'
import CardStatus from './CardStatus'
import CardStatsChart from './CardStatsChart'
import CardStatusLineChart from './CardStatusLineChart'
//** Axios */
import axios from 'axios'

function DashBoard(OpenRequestCount, SLAViolated, ServiceApproachingViolationInaMin, ServiceApproachingViolation) {
    return {
        OpenRequestCount, SLAViolated, ServiceApproachingViolationInaMin, ServiceApproachingViolation

    }
}

// ** Styled Avatar component
const Avatar = styled(MuiAvatar)(({ theme }) => ({
    width: 44,
    height: 44,
    boxShadow: theme.shadows[3],
    marginRight: theme.spacing(2.75),
    backgroundColor: theme.palette.background.paper,
    '& svg': {
        fontSize: '1.75rem'
    }
}))
const CardsDashBoard = props => {
    // ** Props
    const { title, icon, stats, trendNumber, color = 'primary', trend = 'positive' } = props

    const [data, setData] = useState([])
    useEffect(() => {
        axios.get('https://webapidev.aitalkx.com/tms/query/HelpdeskDashboard').then(response => {
            setData(response.data)
            console.log(response.data, 'OpenRequestCount data');
        })
    }, [])
    return (

        <Box>

            <Box sx={{ display: 'flex', justifyContent: "space-evenly" }}>
                <Card
                    sx={{
                        backgroundColor: 'transparent !important',
                        boxShadow: theme => `${theme.shadows[0]} !important`,
                        border: theme => `1px solid ${theme.palette.divider}`,
                        width: 225
                    }}
                >
                    <CardContent>
                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                            <Avatar variant='rounded' sx={{ color: `${color}.main` }}>
                                {icon}
                            </Avatar>
                            <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                                <Typography variant='caption'>{title}Person</Typography>
                                <Box sx={{ mt: 0.5, display: 'flex', flexWrap: 'wrap', alignItems: 'center' }}>
                                    <Typography variant='h6' sx={{ mr: 1, fontWeight: 600, lineHeight: 1.05 }}>
                                        {stats}{data.OpenRequestCount}
                                    </Typography>
                                    <Box
                                        sx={{
                                            display: 'flex',
                                            alignItems: 'center'
                                        }}
                                    >
                                        <Box
                                            component='span'
                                            sx={{ display: 'inline-flex', color: trend === 'positive' ? 'success.main' : 'error.main' }}
                                        >
                                            {/* <MovingIcon /> */}
                                            <Icon icon={trend === 'positive' ? 'mdi:chevron-up' : 'mdi:chevron-down'} />

                                        </Box>

                                        <Typography
                                            variant='caption'
                                            sx={{ fontWeight: 600, color: trend === 'positive' ? 'success.main' : 'error.main' }}
                                        >
                                            {trendNumber}
                                        </Typography>
                                    </Box>
                                </Box>
                            </Box>
                        </Box>
                    </CardContent>
                </Card>

                <Card
                    sx={{
                        backgroundColor: 'transparent !important',
                        boxShadow: theme => `${theme.shadows[0]} !important`,
                        border: theme => `1px solid ${theme.palette.divider}`,
                        width: 225
                    }}
                >
                    <CardContent>
                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                            <Avatar variant='rounded' sx={{ color: `${color}.main` }}>
                                {icon}
                            </Avatar>
                            <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                                <Typography variant='caption'>{title}Employee</Typography>
                                <Box sx={{ mt: 0.5, display: 'flex', flexWrap: 'wrap', alignItems: 'center' }}>
                                    <Typography variant='h6' sx={{ mr: 1, fontWeight: 600, lineHeight: 1.05 }}>
                                        {stats} {data.SLAViolated}
                                    </Typography>
                                    <Box
                                        sx={{
                                            display: 'flex',
                                            alignItems: 'center'
                                        }}
                                    >
                                        <Box
                                            component='span'
                                            sx={{ display: 'inline-flex', color: trend === 'positive' ? 'success.main' : 'error.main' }}
                                        >
                                            <Icon icon={trend === 'positive' ? 'mdi:chevron-up' : 'mdi:chevron-down'} />
                                        </Box>
                                        <Typography
                                            variant='caption'
                                            sx={{ fontWeight: 600, color: trend === 'positive' ? 'success.main' : 'error.main' }}
                                        >
                                            {trendNumber}
                                        </Typography>
                                    </Box>
                                </Box>
                            </Box>
                        </Box>
                    </CardContent>
                </Card>

                <Card
                    sx={{
                        backgroundColor: 'transparent !important',
                        boxShadow: theme => `${theme.shadows[0]} !important`,
                        border: theme => `1px solid ${theme.palette.divider}`,
                        width: 225
                    }}
                >
                    <CardContent>
                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                            <Avatar variant='rounded' sx={{ color: `${color}.main` }}>
                                {icon}
                            </Avatar>
                            <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                                <Typography variant='caption'>{title}Salary</Typography>
                                <Box sx={{ mt: 0.5, display: 'flex', flexWrap: 'wrap', alignItems: 'center' }}>
                                    <Typography variant='h6' sx={{ mr: 1, fontWeight: 600, lineHeight: 1.05 }}>
                                        {stats} {data.ServiceApproachingViolationInaMin}
                                    </Typography>
                                    <Box
                                        sx={{
                                            display: 'flex',
                                            alignItems: 'center'
                                        }}
                                    >
                                        <Box
                                            component='span'
                                            sx={{ display: 'inline-flex', color: trend === 'positive' ? 'success.main' : 'error.main' }}
                                        >
                                            <Icon icon={trend === 'positive' ? 'mdi:chevron-up' : 'mdi:chevron-down'} />
                                        </Box>
                                        <Typography
                                            variant='caption'
                                            sx={{ fontWeight: 600, color: trend === 'positive' ? 'success.main' : 'error.main' }}
                                        >
                                            {trendNumber}
                                        </Typography>
                                    </Box>
                                </Box>
                            </Box>
                        </Box>
                    </CardContent>
                </Card>

                <Card
                    sx={{
                        backgroundColor: 'transparent !important',
                        boxShadow: theme => `${theme.shadows[0]} !important`,
                        border: theme => `1px solid ${theme.palette.divider}`,
                        width: 225
                    }}
                >
                    <CardContent>
                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                            <Avatar variant='rounded' sx={{ color: `${color}.main` }}>
                                {icon}
                            </Avatar>
                            <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                                <Typography variant='caption'>{title}Tax</Typography>
                                <Box sx={{ mt: 0.5, display: 'flex', flexWrap: 'wrap', alignItems: 'center' }}>
                                    <Typography variant='h6' sx={{ mr: 1, fontWeight: 600, lineHeight: 1.05 }}>
                                        {stats} {data.ServiceApproachingViolation}
                                    </Typography>
                                    <Box
                                        sx={{
                                            display: 'flex',
                                            alignItems: 'center'
                                        }}
                                    >
                                        <Box
                                            component='span'
                                            sx={{ display: 'inline-flex', color: trend === 'positive' ? 'success.main' : 'error.main' }}
                                        >
                                            <Icon icon={trend === 'positive' ? 'mdi:chevron-up' : 'mdi:chevron-down'} />
                                        </Box>
                                        <Typography
                                            variant='caption'
                                            sx={{ fontWeight: 600, color: trend === 'positive' ? 'success.main' : 'error.main' }}
                                        >
                                            {trendNumber}
                                        </Typography>
                                    </Box>
                                </Box>
                            </Box>
                        </Box>
                    </CardContent>
                </Card>
            </Box>


            <Box sx={{ display: "flex", justifyContent: "space-between" }}>
                <CardRate />
                <SalesChard />
            </Box>

            <Box sx={{ display: "flex", justifyContent: "space-between" }}>
                <CardStatus />
                <CardStatsChart />
                <CardStatusLineChart />

            </Box>

        </Box>
    )
}

export default CardsDashBoard
