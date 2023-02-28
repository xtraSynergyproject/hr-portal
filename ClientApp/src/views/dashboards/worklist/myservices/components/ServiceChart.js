// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Rating from '@mui/material/Rating'
import Button from '@mui/material/Button'
import { styled } from '@mui/material/styles'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import CardActions from '@mui/material/CardActions'
import Grid from '@mui/material/Grid'
import MyChart from './MyChart'
import MyChart1 from './MyChart1'
import MyChart3 from './MyChart3'

// Styled Grid component
const StyledGrid1 = styled(Grid)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  alignItems: 'flex-start',
  [theme.breakpoints.up('md')]: {
    paddingTop: '0 !important'
  },
  '& .MuiCardContent-root': {
    padding: theme.spacing(3, 4.75),
    [theme.breakpoints.down('md')]: {
      paddingTop: 0
    }
  }
}))

// Styled Grid component
const StyledGrid2 = styled(Grid)(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  [theme.breakpoints.down('md')]: {
    paddingLeft: '0 !important'
  },
  [theme.breakpoints.down('md')]: {
    order: -1
  }
}))
//style Grid component
const StyledGrid3 = styled(Grid)(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  [theme.breakpoints.up('md')]: {
    paddingLeft: '0 !important'
  },
  [theme.breakpoints.down('md')]: {
    order: -1
  }
}))

// Styled component for the image
const Img = styled('img')(({ theme }) => ({
  height: '11rem',
  borderRadius: theme.shape.borderRadius
}))

const ServiceChart = () => {
  return (
    <Card sx={{width:'85rem'}}>
      <Grid container >
        <StyledGrid1 item xs={6} md={4} lg={4}>
          <CardContent>
            <MyChart />
          </CardContent>
        </StyledGrid1>
        <StyledGrid2 item xs={6} md={4} lg={4}>
          <CardContent sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
            <MyChart1 />
          </CardContent>
        </StyledGrid2>
        <StyledGrid1 item xs={6} md={4} lg={3}>
          <CardContent>
            <MyChart3/>
          </CardContent>
        </StyledGrid1>
      </Grid>
    </Card>
  )
}

export default ServiceChart