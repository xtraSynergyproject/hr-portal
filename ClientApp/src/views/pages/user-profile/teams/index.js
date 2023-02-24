// ** MUI Components
import Box from '@mui/material/Box'
import Grid from '@mui/material/Grid'
import Card from '@mui/material/Card'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'

// ** Icon Imports
import Icon from 'src/@core/components/icon'
import GroupsIcon from '@mui/icons-material/Groups'
import MenuIcon from '@mui/icons-material/Menu'
import FlagIcon from '@mui/icons-material/Flag'
import { Divider } from '@mui/material'

import Avatar from '@mui/material/Avatar'
import Stack from '@mui/material/Stack'

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
            '&:not(:last-of-type)': { mb: 4 },
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
    // <Card>
    //   <CardContent>
        <Grid container spacing={12}>
          <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'flex-start' }}>
            {/* Card 1 */}
            <Card
              sx={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                m: 5,
                height: '200px',
                width: '400px'
              }}
            >
              <CardContent className='UDDcard1'>
                <Box sx={{ display: 'flex', justifyContent: 'center', flexDirection: 'row', gap: 2 }}>
                  <Box>
                    <GroupsIcon fontSize='large' />
                  </Box>
                  <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                    <Typography fontSize='12px'>
                      <b> Test BP 01 </b>
                    </Typography>
                    <Typography fontSize='12px'>Administrator</Typography>
                  </Box>
                </Box>
                <Divider />

                <Box sx={{ display: 'flex', justifyContent: 'center', gap: 2, marginLeft: '12px' }}>
                  <FlagIcon />
                  <Typography fontSize='12px'>Since Feb06, 2023</Typography>
                </Box>
                <Stack direction='row' spacing={2} sx={{ marginLeft: '30px' }}>
                  <Avatar
                    alt='Remy Sharp'
                    src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e'
                  />
                </Stack>
              </CardContent>
            </Card>

            {/* Card 2 */}
            <Card
              sx={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                m: 5,
                height: '200px',
                width: '400px'
              }}
            >
              <CardContent>
                <Box sx={{ display: 'flex', justifyContent: 'center', flexDirection: 'row', gap: 2 }}>
                  <Box>
                    <MenuIcon fontSize='large' />
                  </Box>
                  <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                    <Typography fontSize='12px'>
                      <b> Test BP 02 </b>
                    </Typography>
                    <Typography fontSize='12px'>Administrator</Typography>
                  </Box>
                </Box>
                <Divider />
                <Box sx={{ display: 'flex', justifyContent: 'center', gap: 2, marginLeft: '18px' }}>
                  <FlagIcon />
                  <Typography fontSize='12px'>Since Feb06, 2023</Typography>
                </Box>
                <Stack direction='row' spacing={2} sx={{ marginLeft: '30px' }}>
                  <Avatar
                    alt='Remy Sharp'
                    src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e'
                  />
                </Stack>
              </CardContent>
            </Card>
          </Grid>
        </Grid>
    //   </CardContent>
    // </Card>
  )
}

export default AboutOverview
