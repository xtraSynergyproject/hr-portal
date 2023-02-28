// ** React Imports
import { useState, Fragment,useEffect } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'
import Badge from '@mui/material/Badge'
import Button from '@mui/material/Button'
import IconButton from '@mui/material/IconButton'
import { styled } from '@mui/material/styles'
import useMediaQuery from '@mui/material/useMediaQuery'
import MuiMenu from '@mui/material/Menu'
import MuiMenuItem from '@mui/material/MenuItem'
import Typography from '@mui/material/Typography'
import Alert from 'src/@core/theme/overrides/alerts'
import axios from 'axios'
 //import{ BrowserRouter as Router, Link }from 'react-router-dom'
import { Divider, Link } from '@mui/material'
// ** Icon Imports
import Icon from 'src/@core/components/icon'

// ** Third Party Components
import PerfectScrollbarComponent from 'react-perfect-scrollbar'

// ** Custom Components Imports
import CustomChip from 'src/@core/components/mui/chip'
import CustomAvatar from 'src/@core/components/mui/avatar'

// ** Util Import
import { getInitials } from 'src/@core/utils/get-initials'
import { fontSize } from '@mui/system'
import { blue, grey } from '@mui/material/colors'


// ** Styled Menu component
const Menu = styled(MuiMenu)(({ theme }) => ({
  '& .MuiMenu-paper': {
    width: 280,
    height:470,
    
    overflow: 'hidden',
    marginTop: theme.spacing(4),
    [theme.breakpoints.down('sm')]: {
      width: '100%'
    }
  },
  '& .MuiMenu-list': {
    padding: 0
  }
}))

// ** Styled MenuItem component
const MenuItem = styled(MuiMenuItem)(({ theme }) => ({
  paddingTop: theme.spacing(3),
  paddingBottom: theme.spacing(3),
  // '&:not(:last-of-type)': {
  //   borderBottom: `1px solid ${theme.palette.divider}`
  // },
  '&:hover':{
    backgroundColor:'secondary.main',
    textColor:'white'
  }
  }
))

// ** Styled PerfectScrollbar component
const PerfectScrollbar = styled(PerfectScrollbarComponent)({
  MaxHeight: 349
})

// ** Styled Avatar component
const Avatar = styled(CustomAvatar)({
  width: 38,
  height: 38,
  fontSize: '1.125rem'
})

// ** Styled component for the title in MenuItems
const MenuItemTitle = styled(Typography)(({ theme }) => ({
  fontWeight: 1000,
  flex: '1 1 100%',
  overflow: 'hidden',
  fontSize: '0.875rem',
  whiteSpace: 'nowrap',
  textOverflow: 'ellipsis',
  marginBottom: theme.spacing(0.75)
}))
const MenuItemTitle1 = styled(Typography)(({ theme }) => ({
  fontWeight: 1000,
  flex: '1 1 100%',
  overflow: 'hidden',
  fontSize: '0.675rem',
  whiteSpace: 'nowrap',
  textOverflow: 'ellipsis',
  marginBottom: theme.spacing(0.65)
}))


// ** Styled component for the subtitle in MenuItems
const MenuItemSubtitle = styled(Typography)({
  flex: '1 1 90%',
  overflow: 'hidden',
  fontSize:'0.275',
  whiteSpace: 'nowrap',
  textOverflow: 'ellipsis'
})

const ScrollWrapper = ({ children, hidden }) => {
  if (hidden) {
    return <Box sx={{ maxHeight: 349, overflowY: 'auto', overflowX: 'hidden' }}>{children}</Box>
  } else {
    return <PerfectScrollbar options={{ wheelPropagation: false, suppressScrollX: true }}>{children}</PerfectScrollbar>
  }
} 

const MessageBox = props => {
  //const[message,setData]=useState([])
  // ** Props
  const { settings,message } = props

  // ** States
  const [anchorEl, setAnchorEl] = useState(null)

  // ** Hook
  const hidden = useMediaQuery(theme => theme.breakpoints.down('lg'))

  // ** Vars
  const { direction } = settings

  const handleDropdownOpen = event => {
    setAnchorEl(event.currentTarget)
  }

  const handleDropdownClose = () => {
    setAnchorEl(null)
  }
  const handleClick=()=>{
    <Alert variant='filled' severity='success'>
        This is an success alert — check it out!
      </Alert>
  }
  // useEffect(()=>
  //   axios
  //   .get("https://webapidev.aitalkx.com/portaladmin/notification/GetAllNotifications?userId=60da8f8f195197515042a1f2&portalId=c8dce908-74b1-4111-a809-a5e6995db660")
  //   .then((res)=> setData(res.data))
  //   },[])
 
  
  const RenderAvatar = ({ messages}) => {
    const { avatarAlt, avatarImg, avatarIcon, avatarText, avatarColor } = messages
    if (avatarImg) {
      return <Avatar alt={avatarAlt} src={avatarImg} />
    } else if (avatarIcon) {
      return (
        <Avatar skin='light' color={avatarColor}>
          {avatarIcon}
        </Avatar>
      )
    } else {
      return (
        <Avatar skin='light' color={avatarColor}>
          {/* {getInitials(avatarText)} */}
        </Avatar>
      )
    }
    // <Avatar alt="Remy Sharp" src="/static/images/avatar/1.jpg" />
  }

  return (
    <Fragment>
      <IconButton color='inherit' aria-haspopup='true' onClick={handleDropdownOpen} aria-controls='customized-menu'>
        <Badge
          color='error'
          variant='dot'
          invisible={!message.length}
          sx={{
            '& .MuiBadge-badge': { top: 4, right: 4, boxShadow: theme => `0 0 0 2px ${theme.palette.background.paper}` }
          }}
        >
          <Icon icon='mdi:email-outline' />
        </Badge>
      </IconButton>
      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleDropdownClose}
        anchorOrigin={{ vertical: 'bottom', horizontal: direction === 'ltr' ? 'right' : 'left' }}
        transformOrigin={{ vertical: 'top', horizontal: direction === 'ltr' ? 'right' : 'left' }}
      >
        <MenuItem
          disableRipple
          disableTouchRipple
          sx={{ cursor: 'default', userSelect: 'auto', backgroundColor: 'transparent !important' }}
        >
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', width: '100%' }}>
            <Typography sx={{ cursor: 'text', fontWeight: 600,fontSize:13 }}>Messages</Typography>
            {/* <CustomChip
              skin='light'
              size='small'
              color='primary'
              label={`${message.length} New`}
              sx={{ height: 20, fontSize: '0.75rem', fontWeight: 500, borderRadius: '10px' }}
            /> */}
            <Typography>
            <Link href="#" underline='hover'color={'darkgray'} fontSize={13} onClick={handleClick} component="button">Mark All As Read </Link>
            </Typography> 
          </Box>
        </MenuItem>
        <Divider/>
        {/* <ScrollWrapper hidden={!hidden}> */}
          {message.map((messages, index) => (
            <MenuItem key={index} onClick={handleDropdownClose}>
              <Box sx={{ width: '100%', display: 'flex', alignItems: 'center' }}>

              <Avatar alt="Remy Sharp" src='/images/avatars/4.png' />
                <Box sx={{ mx: 4,flex: '1 1', display: 'flex', overflow: 'hidden', flexDirection: 'column' }}>
                <MenuItemTitle>{messages.From}</MenuItemTitle>
                <MenuItemTitle1>{messages.Subject}</MenuItemTitle1>
                  <MenuItemSubtitle variant='body2'>{messages.DisplayCreatedDate}</MenuItemSubtitle>
                </Box>
                {/* <Typography variant='caption' sx={{ color: 'text.disabled' }}>
                  {messages.meta}
                </Typography> */}
              </Box>
            </MenuItem>
          ))}
          
       
         <Divider/>
         <Box align="center">
        
        
         <Link href="/dashboards/message-box1"> All Messages</Link>
         
         </Box>
      </Menu>
    </Fragment>
  )
}

export default MessageBox;
