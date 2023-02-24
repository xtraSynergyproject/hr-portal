// ** MUI Imports
import Card from '@mui/material/Card'
import Grid from '@mui/material/Grid'
import Divider from '@mui/material/Divider'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'

// ** Icon Imports
import MessageIcon from '@mui/icons-material/Message'
import VisibilityIcon from '@mui/icons-material/Visibility'

import Avatar from '@mui/material/Avatar'
import Stack from '@mui/material/Stack'

import { styled } from '@mui/material/styles'
import Box from '@mui/material/Box'
import LinearProgress, { linearProgressClasses } from '@mui/material/LinearProgress'

import AccessTimeIcon from '@mui/icons-material/AccessTime'

const BorderLinearProgress = styled(LinearProgress)(({ theme }) => ({
  height: 10,
  borderRadius: 5,
  [`&.${linearProgressClasses.colorsuccess}`]: {
    backgroundColor: theme.palette.grey[theme.palette.mode === 'light' ? 200 : 800]
  },
  [`& .${linearProgressClasses.bar}`]: {
    borderRadius: 5,
    backgroundColor: theme.palette.mode === 'light' ? '#1a90ff' : '#308fe8'
  }
}))

// Styled Box component
const StyledBox = styled(Box)(({ theme }) => ({
  [theme.breakpoints.up('sm')]: {
    borderRight: `1px solid ${theme.palette.divider}`
  }
}))

const CardMembership = () => {
  return (
    <Card sx={{ height: '32vh', maxWidth: '15rem' }}>
      <Grid container sx={{ padding: '1px', gap: '2px', mt: '10px' }}>
        <Grid item xs={6} sm={12}>
          {/* For progress line  */}
          <Box sx={{ flexGrow: 1 }}>
            <BorderLinearProgress color='secondary' variant='determinate' value={100} />
          </Box>
          <CardContent>
            <Typography fontSize='12px'>
              <b> Business Diagram </b>
            </Typography>
            <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
              <Typography fontSize='10px' variant='body2'>
                00:00 / 00:00·{' '}
              </Typography>
              <Box sx={{ display: 'flex', alignItems: 'center' }}>
                <AccessTimeIcon />
                <Typography fontSize='10px' variant='body2'>
                  {' '}
                  10 months ago
                </Typography>
              </Box>
            </Box>
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', mt: 5 }}>
              <Stack direction='row' spacing={6}>
                <Avatar
                  alt='Remy Sharp'
                  src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e'
                />
              </Stack>
              <Typography fontSize='12px'>
                <p>Administrator</p>
              </Typography>
            </Box>

            <Divider />
            <Grid container spacing={6} sx={{ display: 'flex', justifyContent: 'center' }}>
              <Grid item xs={12} sm={6}>
                <StyledBox>
                  <Box
                    sx={{
                      mb: 2.75,
                      display: 'flex',
                      alignItems: 'center',
                      '& svg': { color: 'primary.main', mr: 1.75 }
                    }}
                  >
                    <MessageIcon />
                    <Typography variant='body2'>0</Typography>
                  </Box>
                </StyledBox>
              </Grid>

              <Grid item xs={6} sm={3}>
                <Box
                  sx={{ mb: 6.75, display: 'flex', alignItems: 'center', '& svg': { color: 'primary.main', mr: 2.75 } }}
                >
                  <VisibilityIcon />
                </Box>
              </Grid>
            </Grid>
          </CardContent>
        </Grid>
      </Grid>
    </Card>
  )
}

export default CardMembership
