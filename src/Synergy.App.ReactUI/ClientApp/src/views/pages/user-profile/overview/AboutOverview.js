// ** MUI Components
import Box from '@mui/material/Box'
import Grid from '@mui/material/Grid'
import Card from '@mui/material/Card'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'

// ** Icon Imports
import Icon from 'src/@core/components/icon'
import { flexbox } from '@mui/system'
import GroupsIcon from '@mui/icons-material/Groups'; 
import MenuIcon from '@mui/icons-material/Menu';

const renderList = arr => {
  if (arr && arr.length) {
    return arr.map((item, index) => {
      return (
        <Box
          key={index}
          sx={{
            display: 'flex',
            alignItems: 'center',
            '&:not(:last-of-type)': { mb: 4 },
            '& svg': { color: 'text.secondary' }
          }}
        >
          <Icon icon={item.icon} />

          <Typography sx={{ mx: 2, fontWeight: 600, color: 'text.secondary' }}>
            {`${item.property.charAt(0).toUpperCase() + item.property.slice(1)}:`}
          </Typography>
          <Typography sx={{ color: 'text.secondary' }}>
            {item.value.charAt(0).toUpperCase() + item.value.slice(1)}
          </Typography>
        </Box>
      )
    })
  } else {
    return null
  }
}

const renderTeams = arr => {
  if (arr && arr.length) {
    return arr.map((item, index) => {
      return (
        <Box
          key={index}
          sx={{
            display: 'flex',
            alignItems: 'center',
            // '&:not(:last-of-type)': { mb: 4 },
            '& svg': { color: `${item.color}.main` }
          }}
        >
          <Icon icon='item.icon' />

          <Typography sx={{ mx: 2, fontWeight: 600, color: 'text.secondary' }}>
            {item.property.charAt(0).toUpperCase() + item.property.slice(1)}
          </Typography>
          <Typography sx={{ color: 'text.secondary' }}>
            {item.value.charAt(0).toUpperCase() + item.value.slice(1)}
          </Typography>
        </Box>
      )
    })
  } else {
    return null
  }
}

const AboutOverview = props => {
  const { teams, about, contacts, overview } = props

  return (
    <Grid container spacing={12}>
      <Grid item xs={16} sx={{display:'flex', justifyContent:'flex-start'}}>
        <Card sx={{display:'flex', justifyContent:'center',alignItems:'center' ,m: 5,height:'200px',width:'500px'}}>
          <CardContent>
            <div>
              <Box sx={{display:'flex',justifyContent:'center',alignItems:"end", gap:2}}>
                
                <GroupsIcon fontSize='large'/>
                <Typography variant='h4' component="p">2</Typography>
                
              </Box>
                <Typography variant='p' component="p">Teams</Typography>
            </div>
          </CardContent>
        </Card>
        
        <Card sx={{display:'flex', justifyContent:'center',alignItems:'center' ,m: 5,height:'200px',width:'500px'}}>
          <CardContent>
            <div>
              <Box sx={{display:'flex',justifyContent:'center',alignItems:"end", gap:2}}>
                
                <MenuIcon fontSize='large'/>
                <Typography variant='h4' component="p">129</Typography>
                
              </Box>
                <Typography variant='p' component="p">Active Task</Typography>
            </div>
          </CardContent>
        </Card>
      </Grid>
    </Grid>
  )
}

export default AboutOverview
