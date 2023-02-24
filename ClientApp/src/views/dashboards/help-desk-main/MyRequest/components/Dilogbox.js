import CardMedia from '@mui/material/CardMedia'
import Card from '@mui/material/Card'
import Button from '@mui/material/Button'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import { useState } from 'react'
import DialogActions from '@mui/material/DialogActions'
import Dialog from '@mui/material/Dialog'
import DialogTitle from '@mui/material/DialogTitle'
import IconButton from '@mui/material/IconButton'
import Icon from 'src/@core/components/icon'
import Box from '@mui/material/Box'
import Grid from '@mui/material/Grid'
import Item from '@mui/material/Grid'
import RemoteSignInOut from './RemoteSignInOut'
import RequestSearchBar from './RequestSearchBar'
import Pagination from './Pagination'
import RequestMenu from './RequestMenu'
import { Divider } from '@mui/material'
const Dilogbox = () => {
  const [open, setOpen] = useState(false)

  const handleClickOpen = () => {
    setOpen(true)
  }

  const handleClose = () => {
    setOpen(false)
  }
  return (
    <Card>
      <CardContent>
        <Typography variant='body2'>
          <Grid variant='body2' sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
            <Button onClick={handleClickOpen}>+ Add Services</Button>
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', marginLeft: '285px' }}>
              <Box sx={{ border: '1px solid', borderRadius: 5 }}>
                <RequestSearchBar />
              </Box>
              <RequestMenu />
            </Box>
          </Grid>

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
              <Typography sx={{ border: '1px solid', my: '50px', mt: '5', pl: '20px' }}>All Templates</Typography>

              <Grid container>
                <Grid item xs={4}>
                  <Item>
                    <CardMedia
                      sx={{ height: 190, width: '255', ml: 40 }}
                      image='https:cdn.pixabay.com/photo/2021/03/05/05/12/man-6070329_960_720.png'
                    />
                    <RemoteSignInOut />
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
        </Typography>
      </CardContent>
      <Divider />
      <Box sx={{display: 'flex',alignItems: 'center', gap: '45%'}}>
        <Pagination />
        <Typography>No items to display</Typography>
      </Box>
    </Card>
  )
}

export default Dilogbox
