// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Menu from '@mui/material/Menu'
import Button from '@mui/material/Button'
import MenuItem from '@mui/material/MenuItem'
import { styled } from '@mui/material/styles'
import Typography from '@mui/material/Typography'
import IconButton from '@mui/material/IconButton'
import CardContent from '@mui/material/CardContent'
import CardActions from '@mui/material/CardActions'
import Grid from '@mui/material/Grid'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

// Styled Grid component
const StyledGrid = styled(Grid)(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  [theme.breakpoints.down('md')]: {
    borderBottom: `1px solid ${theme.palette.divider}`
  },
  [theme.breakpoints.up('md')]: {
    borderRight: `1px solid ${theme.palette.divider}`
  }
}))

const AnnouncementCard = () => {
  // ** State
  const [anchorEl, setAnchorEl] = useState(null)
  const open = Boolean(anchorEl)

  const handleClick = event => {
    setAnchorEl(event.currentTarget)
  }

  const handleClose = () => {
    setAnchorEl(null)
  }

  return (
    <Card sx={{width:"80%", margin:"auto"}}>
      <Grid container spacing={6}>
        <StyledGrid item md={5} xs={12}>
          <CardContent sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', boxShadow:5 }}>
            <img width={220} height={140} alt='Announcement' src='/images/cards/announcements1.jpg' />
          </CardContent>
        </StyledGrid>
        <Grid
          item
          xs={12}
          md={7}
          sx={{
            pt: ['0 !important', '0 !important', '1.5rem !important'],
            pl: ['1.5rem !important', '1.5rem !important', '0 !important']
          }}
        >
          <CardContent>
            <Typography variant='h6' sx={{ mb: 2 }}>
            Announcement
            </Typography>
            <Typography variant='body2' sx={{ mb: 3.5 }}>
            The Indeed Editorial Team comprises a diverse and talented team of writers, researchers and subject matter experts equipped with Indeed's data and insights to deliver useful tips to help guide your career journey.
            </Typography>
            <Typography sx={{ fontWeight: 500, mb: 3 }}>
              Date:{' '}
              <Box component='span' sx={{ fontWeight: 'bold' }}>
               12-1-2023
              </Box>
            </Typography>
            <Typography sx={{ fontWeight: 500, mb: 3 }}>
              Time:{' '}
              <Box component='span' sx={{ fontWeight: 'bold' }}>
               2:30PM
              </Box>
            </Typography>
          </CardContent>
          <CardActions className='card-action-dense'>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', width: '100%' }}>
              <Button sx={{ '& svg': { mr: 2 } }}>
                <Icon icon='mdi:cart-plus' fontSize={20} />
                Update
              </Button>
              <IconButton
                id='long-button'
                aria-label='share'
                aria-haspopup='true'
                onClick={handleClick}
                aria-controls='long-menu'
                aria-expanded={open ? 'true' : undefined}
              >
                <Icon icon='mdi:share-variant' fontSize={20} />
              </IconButton>
              <Menu
                open={open}
                id='long-menu'
                anchorEl={anchorEl}
                onClose={handleClose}
                MenuListProps={{
                  'aria-labelledby': 'long-button'
                }}
              >
                <MenuItem onClick={handleClose}>
                  <Icon icon='mdi:facebook' />
                </MenuItem>
                <MenuItem onClick={handleClose}>
                  <Icon icon='mdi:twitter' />
                </MenuItem>
                <MenuItem onClick={handleClose}>
                  <Icon icon='mdi:linkedin' />
                </MenuItem>
                <MenuItem onClick={handleClose}>
                  <Icon icon='mdi:google-plus' />
                </MenuItem>
              </Menu>
            </Box>
          </CardActions>
        </Grid>
      </Grid>
    </Card>
  )
}

export default AnnouncementCard
