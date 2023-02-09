import React from 'react'
import { styled, alpha } from '@mui/material/styles'
import Button from '@mui/material/Button'
import Typography from '@mui/material/Typography'
import Menu from '@mui/material/Menu'
import MenuItem from '@mui/material/MenuItem'
import EditIcon from '@mui/icons-material/Edit'
import ArchiveIcon from '@mui/icons-material/Archive'
import FileCopyIcon from '@mui/icons-material/FileCopy'
import MoreHorizIcon from '@mui/icons-material/MoreHoriz'
import Icon from 'src/@core/components/icon'
import MenuTagModal from './menu_modals/MenuTagModal'
import MenuLogModal from './menu_modals/MenuLogModal'

const StyledMenu = styled(props => (
  <Menu
    elevation={0}
    anchorOrigin={{
      vertical: 'bottom',
      horizontal: 'right'
    }}
    transformOrigin={{
      vertical: 'top',
      horizontal: 'right'
    }}
    {...props}
  />
))(({ theme }) => ({
  '& .MuiPaper-root': {
    borderRadius: 6,
    // marginTop: theme.spacing(1),
    minWidth: 180,
    color: theme.palette.mode === 'light' ? 'rgb(55, 65, 81)' : theme.palette.grey[300],
    boxShadow:
      'rgb(255, 255, 255) 0px 0px 0px 0px, rgba(0, 0, 0, 0.05) 0px 0px 0px 1px, rgba(0, 0, 0, 0.1) 0px 10px 15px -3px, rgba(0, 0, 0, 0.05) 0px 4px 6px -2px',
    '& .MuiMenu-list': {
      padding: '4px 0'
    },
    '& .MuiMenuItem-root': {
      '& .MuiSvgIcon-root': {
        fontSize: 18,
        color: theme.palette.text.secondary,
        marginRight: theme.spacing(1.5)
      },
      '&:active': {
        backgroundColor: alpha(theme.palette.primary.main, theme.palette.action.selectedOpacity)
      }
    }
  }
}))

export default function MenuBtn() {
  const [anchorEl, setAnchorEl] = React.useState(null)
  const open = Boolean(anchorEl)
  const handleClick = event => {
    setAnchorEl(event.currentTarget)
  }
  const handleClose = () => {
    setAnchorEl(null)
  }

  return (
    <div>
      <Button
        id='demo-customized-button'
        aria-controls={open ? 'demo-customized-menu' : undefined}
        aria-haspopup='true'
        aria-expanded={open ? 'true' : undefined}
        // variant='contained'
        sx={{ height: '105px', borderRadius: '50px', display: 'flex', flexDirection: 'column' }}
        component='label'
        disableElevation
        onClick={handleClick}
      >
        <Icon icon='mdi:menu' />
        Menu
      </Button>

      <StyledMenu
        id='demo-customized-menu'
        MenuListProps={{
          'aria-labelledby': 'demo-customized-button'
        }}
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
      >
        <MenuItem onClick={handleClose} disableRipple>
        <Icon icon='mdi:lan' />
          Workflow
        </MenuItem>
        <MenuItem onClick={handleClose} disableRipple>
        <Icon icon='mdi:email' />
          {/* <Typography > <a href='gmail.com' target='-main' > */}
            Email
            {/* </a></Typography> */}
        </MenuItem>
        <MenuItem onClick={handleClose} disableRipple>
        <Icon icon='mdi:attachment-plus' />
          <MenuTagModal/>
        </MenuItem>
        <MenuItem onClick={handleClose} disableRipple>
        <Icon icon='mdi:attachment-plus' />
          <MenuLogModal/>
        </MenuItem>
      </StyledMenu>
    </div>
  )
}
