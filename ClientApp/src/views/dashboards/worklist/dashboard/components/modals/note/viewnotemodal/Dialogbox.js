// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import IconButton from '@mui/material/IconButton'
import Box from '@mui/material/Box'
import Icon from 'src/@core/components/icon'
import Button from '@mui/material/Button'
import Dialog from '@mui/material/Dialog'
import DialogTitle from '@mui/material/DialogTitle'
import DialogActions from '@mui/material/DialogActions'
import Typography from '@mui/material/Typography'
import Grid from '@mui/material/Grid'
import Item from '@mui/material/Grid'
import CardMedia from '@mui/material/CardMedia'
// import RemoteSignInOut from './RemoteSignInOut'
const Dialogbox = () => {
  // ** State
  const [open, setOpen] = useState(false)

  const handleClickOpen = () => {
    setOpen(true)
  }

  const handleClose = () => {
    setOpen(false)
  }

  return (
    <div>
      <Typography variant='h6' component='span'>
        <Button onClick={handleClickOpen}>+Add NOTE</Button>
      </Typography>

      <Dialog maxWidth='md' fullWidth open={open} onClose={handleClose}>
        <DialogTitle>Service Templates</DialogTitle>
        <IconButton
          aria-label='close'
          onClick={handleClose}
          sx={{ top: 10, right: 10, position: 'absolute', color: 'grey.800' }}
        >
          <Icon icon='mdi:close' />
        </IconButton>
        <Box sx={{ flexGrow: 1 }}>
          <Box sx={{ m: '5', border: '1px solid' }}>All Templates</Box>

          <Grid container>
            <Grid item xs={4}>
              <Item>
                <CardMedia
                  sx={{ height: 160, width: '255' }}
                  image='https://www.shutterstock.com/image-vector/mobile-phone-settings-gears-vector-260nw-1574738056.jpg'
                  title='green iguana'
                />
              </Item>
            </Grid>
          </Grid>
        </Box>
        <DialogActions>
          <Button onClick={handleClose} variant='outlined'>
            ok
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  )
}

export default Dialogbox