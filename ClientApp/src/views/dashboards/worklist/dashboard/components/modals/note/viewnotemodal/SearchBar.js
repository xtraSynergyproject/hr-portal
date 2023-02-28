import * as React from 'react'
import { styled, alpha } from '@mui/material/styles'
import Box from '@mui/material/Box'
import Toolbar from '@mui/material/Toolbar'
import IconButton from '@mui/material/IconButton'
import Typography from '@mui/material/Typography'
import InputBase from '@mui/material/InputBase'
import Divider from '@mui/material/Divider';
import ArrowDropDownIcon from '@mui/icons-material/ArrowDropDown';
import NoteTemplate from './NoteTemplate'
import MoreOptionModal from './MoreOptionModal'


const Search = styled('div')(({ theme }) => ({
  position: 'relative',
  borderRadius: theme.shape.borderRadius,
  backgroundColor: alpha(theme.palette.common.white, 0.15),
  '&:hover': {
    backgroundColor: alpha(theme.palette.common.white, 0.25)
  },
  marginLeft: 0,
  width: '100%',
  [theme.breakpoints.up('sm')]: {
    marginLeft: theme.spacing(1),
    width: 'auto'
  }
}))

const SearchIconWrapper = styled('div')(({ theme }) => ({
  padding: theme.spacing(0, 2),
  height: '100%',
  position: 'absolute',
  pointerEvents: 'none',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center'
}))

const StyledInputBase = styled(InputBase)(({ theme }) => ({
  color: 'inherit',
  '& .MuiInputBase-input': {
    padding: theme.spacing(1, 1, 1, 0),
    // vertical padding + font size from searchIcon
    paddingLeft: `calc(1em + ${theme.spacing(4)})`,
    transition: theme.transitions.create('width'),
    width: '100%'
  }
}))

export default function SearchBar() {
  return (
      <Toolbar>
      <IconButton size='small' edge='start' color='inherit' aria-label='open drawer' sx={{ mr: 10 }}>
        <NoteTemplate/>
      </IconButton>
      <Typography variant='h6' noWrap component='div' sx={{ flexGrow: 1, display: { xs: 'none' } }}></Typography>
      <Search>
               <StyledInputBase
          sx={{ width:'20rem', ml:55,border: '1px solid', display: 'flex',borderRadius:5 }}
          placeholder='Search…'
          inputProps={{ 'aria-label': 'search' }}
        />
      </Search>
      <Box sx={{ml:7,display:'flex'}}>
      {/* < MoreHorizIcon/> */}
      <MoreOptionModal/>
      <ArrowDropDownIcon/>

      </Box>
     
      <Divider/>
    </Toolbar>
 
  )
}