// ** MUI Import
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Avatar from '@mui/material/Avatar'
import { styled } from '@mui/material/styles'
import TimelineDot from '@mui/lab/TimelineDot'
import TimelineItem from '@mui/lab/TimelineItem'
import CardHeader from '@mui/material/CardHeader'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import TimelineContent from '@mui/lab/TimelineContent'
import TimelineSeparator from '@mui/lab/TimelineSeparator'
import TimelineConnector from '@mui/lab/TimelineConnector'
import MuiTimeline from '@mui/lab/Timeline'

//Avater

import Stack from '@mui/material/Stack';

//Administrator
// import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
// import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Button from '@mui/material/Button';
// import Typography from '@mui/material/Typography';

//===

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

const CrmActivityTimeline = () => {
  return (
    <Card>
      <CardHeader
        title='You Report To'
        titleTypographyProps={{ sx: { lineHeight: '2rem !important', letterSpacing: '0.15px !important' } }}
      />
      <CardContent>
        <Timeline sx={{ my: 0, py: 0 }}>


          <Card sx={{ maxWidth: 345 }}>
            {/* <CardMedia
              sx={{ height: 140, borderRadius: 80 }}
              image=""
              title="green iguana"
            /> */}

            <Stack direction="row" spacing={2}>

              <Avatar
                alt="Administrator"
                src="/static/images/avatar/1.jpg"
                sx={{ width: 80, height: 80, marginLeft: 2, marginTop:2}}
              />
            </Stack>
            <CardContent>
              <Typography gutterBottom variant="h5" component="div">
                Administrator
              </Typography>
              <Typography variant="body2" color="text.secondary">
                System Admimistrator
              </Typography>
            </CardContent>
            <CardActions>
              <Button size="small">Contact</Button>
              <Button size="small">Learn More</Button>
            </CardActions>
          </Card>
        </Timeline>
      </CardContent>
    </Card>
  )
}

export default CrmActivityTimeline
