import * as React from 'react'
import Popover from '@mui/material/Popover'
import Typography from '@mui/material/Typography'
import Grid from '@mui/material/Grid'
import PersonIcon from '@mui/icons-material/Person'
import TagIcon from '@mui/icons-material/Tag'
import DoneAllIcon from '@mui/icons-material/DoneAll'
import CameraAltIcon from '@mui/icons-material/CameraAlt'
import Card from '@mui/material/Card'
import Box from "@mui/material/Box";
import AddToPhotosIcon from '@mui/icons-material/AddToPhotos';
import IconButton from '@mui/material/IconButton';

const NoteHeader = props => {
  
  const { NoteNo, NoteStatus, UserName, VersionNo } = props
  const [anchorEl, setAnchorEl] = React.useState(null)

  const handlePopoverOpen = event => {
    setAnchorEl(event.currentTarget)
  }

  const handlePopoverClose = () => {
    setAnchorEl(null)
  }

  const open = Boolean(anchorEl)

  return (
    <div>
 <Box sx={{marginBottom: '10px'}}>
 <Grid container spacing={2}>

        <Grid item xs={3}>
<IconButton aria-label="Attach" sx={{ background: '#f7c46c', color:'white',p: 2,m:1 , borderRadius: 0,width: '-webkit-fill-available',fontSize: '1rem'}}>
 <PersonIcon /> {UserName}
</IconButton>
  </Grid>

        <Grid item xs={3}>
<IconButton aria-label="Share" sx={{ background: '#00a28a', color:'white',p: 2,m:1  , borderRadius: 0,width: '-webkit-fill-available',fontSize: '1rem'}}>
   <TagIcon /> {NoteNo}
</IconButton>
  </Grid>

        <Grid item xs={3}>
<IconButton aria-label="Comment" sx={{ background: '#0179a8', color:'white',p: 2 ,m:1 , borderRadius: 0,width: '-webkit-fill-available',fontSize: '1rem'}}>
    <DoneAllIcon /> {NoteStatus}
</IconButton>
  </Grid>

        <Grid item xs={3}>
<IconButton aria-label="More" sx={{ background: '#9155FD', color:'white' ,p: 2,m:1 , borderRadius: 0,width: '-webkit-fill-available',fontSize: '1rem'}}>
    <AddToPhotosIcon /> {VersionNo}
</IconButton>
  </Grid>
</Grid>
 </Box>

      <Popover
        id='mouse-over-popover'
        sx={{
          pointerEvents: 'none'
        }}
        open={open}
        anchorEl={anchorEl}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'left'
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'left'
        }}
        onClose={handlePopoverClose}
        disableRestoreFocus
        fullWidth
      >
        <Typography sx={{ p: 1 }}>{UserName}</Typography>
      </Popover>
    </div>
  )
}

  export default NoteHeader
