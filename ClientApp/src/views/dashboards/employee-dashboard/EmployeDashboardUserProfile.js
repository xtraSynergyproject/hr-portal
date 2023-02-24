// ** MUI Import
import Box from '@mui/material/Box'
import Avatar from '@mui/material/Avatar'
import Divider from '@mui/material/Divider'
import { styled } from '@mui/material/styles'
import TimelineDot from '@mui/lab/TimelineDot'
import TimelineItem from '@mui/lab/TimelineItem'
import Typography from '@mui/material/Typography'
import IconButton from '@mui/material/IconButton'
import TimelineContent from '@mui/lab/TimelineContent'
import TimelineSeparator from '@mui/lab/TimelineSeparator'
import TimelineConnector from '@mui/lab/TimelineConnector'
import MuiTimeline from '@mui/lab/Timeline'
import Card from '@mui/material/Card'
import Grid from '@mui/material/Grid'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

import EmployeeDashboardEmployeeProfile from 'src/views/dashboards/employee-dashboard/EmployeeDashboardEmployeeProfile'
// Styled Timeline component
const Timeline = styled(MuiTimeline)({
    paddingLeft: 0,
    paddingRight: 0,
    '& .MuiTimelineItem-root': {
        width: '100%',
        '&:before': {
            display: 'none'
        }
    }
})

// Styled component for the image of a shoe
const ImgShoe = styled('img')(({ theme }) => ({
    borderRadius: theme.shape.borderRadius
}))

const TimelineLeft = () => {
    return (
        <Card sx={{ py: "10px", display: "flex" }} className="user_profile_grid">
            <Timeline>
                <TimelineItem sx={{ width: '100%' }}>
                    <TimelineSeparator>
                        <TimelineDot color='primary' />
                        <TimelineConnector />
                    </TimelineSeparator>
                    <TimelineContent>
                        <Box sx={{ mb: 2, display: 'flex', flexWrap: 'wrap', alignItems: 'center', justifyContent: 'space-between' }}>
                            <Typography variant='p6' sx={{ mr: 2, fontWeight: 600, color: 'text.primary' }}>
                                User Profile
                            </Typography>

                        </Box>

                        <Divider />
                        <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                            <Box sx={{ display: 'flex' }}>
                                <Avatar src='/images/avatars/1.png' sx={{ width: '7rem', height: '7rem', mr: 2 }} />
                            </Box>

                            <Grid item xs={12} sm={6} md={2}>
                                <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                                    <Typography variant='p6' sx={{ fontWeight: 600 }}>
                                        System Administrators
                                    </Typography>
                                    <Typography variant='caption'>CEO</Typography>
                                </Box>
                            </Grid>
                            <Grid item xs={12} sm={6} md={10}>
                                <EmployeeDashboardEmployeeProfile />
                            </Grid>
                        </Box>

                    </TimelineContent>
                </TimelineItem>


            </Timeline>
        </Card>
    )
}

export default TimelineLeft
