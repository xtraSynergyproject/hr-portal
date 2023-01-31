
// // ** React Imports
// import { Fragment } from 'react'

// // ** MUI Imports
// import Badge from '@mui/material/Badge'
// import MuiAvatar from '@mui/material/Avatar'
// import { styled } from '@mui/material/styles'
// import Typography from '@mui/material/Typography'
// import IconButton from '@mui/material/IconButton'
// import Box from '@mui/material/Box'

// // ** Icon Imports
// import Icon from 'src/@core/components/icon'

// // ** Custom Components Import
// import ChatLog from './ChatLog'
// import SendMsgForm from 'src/views/apps/chat/SendMsgForm'
// import CustomAvatar from 'src/@core/components/mui/avatar'
// import OptionsMenu from 'src/@core/components/option-menu'
// import UserProfileRight from 'src/views/apps/chat/UserProfileRight'

// // ** Styled Components
// const ChatWrapperStartChat = styled(Box)(({ theme }) => ({
//   flexGrow: 1,
//   height: '100%',
//   display: 'flex',
//   borderRadius: 1,
//   alignItems: 'center',
//   flexDirection: 'column',
//   justifyContent: 'center',
//   backgroundColor: theme.palette.action.hover
// }))

// const ChatContent = props => {
//   // ** Props
//   const {
//     store,
//     hidden,
//     sendMsg,
//     mdAbove,
//     dispatch,
//     statusObj,
//     getInitials,
//     sidebarWidth,
//     userProfileRightOpen,
//     handleLeftSidebarToggle,
//     handleUserProfileRightSidebarToggle
//   } = props

//   const handleStartConversation = () => {
//     if (!mdAbove) {
//       handleLeftSidebarToggle()
//     }
//   }

//   const renderContent = () => {
//     if (store) {
//       const selectedChat = store.selectedChat
//       if (!selectedChat) {
//         return (
//           <ChatWrapperStartChat
//             sx={{
//               ...(mdAbove ? { borderTopLeftRadius: 0, borderBottomLeftRadius: 0 } : {})
//             }}
//           >
//             <MuiAvatar
//               sx={{
//                 mb: 6,
//                 pt: 8,
//                 pb: 7,
//                 px: 7.5,
//                 width: 110,
//                 height: 110,
//                 boxShadow: 3,
//                 backgroundColor: 'background.paper'
//               }}
//             >
//               <Icon icon='mdi:message-outline' fontSize='3.125rem' />
//             </MuiAvatar>
//             <Box
//               onClick={handleStartConversation}
//               sx={{
//                 py: 2,
//                 px: 6,
//                 boxShadow: 3,
//                 borderRadius: 5,
//                 backgroundColor: 'background.paper',
//                 cursor: mdAbove ? 'default' : 'pointer'
//               }}
//             >
//               <Typography sx={{ fontWeight: 500, fontSize: '1.125rem', lineHeight: 'normal' }}>
//                 Start Conversation
//               </Typography>
//             </Box>
//           </ChatWrapperStartChat>
//         )
//       } else {
//         return (
//           <Box
//             sx={{
//               flexGrow: 1,
//               width: '100%',
//               height: '100%',
//               backgroundColor: 'action.hover'
//             }}
//           >
//             <Box
//               sx={{
//                 py: 3,
//                 px: 5,
//                 display: 'flex',
//                 alignItems: 'center',
//                 justifyContent: 'space-between',
//                 borderBottom: theme => `1px solid ${theme.palette.divider}`
//               }}
//             >
//               <Box sx={{ display: 'flex', alignItems: 'center' }}>
//                 {mdAbove ? null : (
//                   <IconButton onClick={handleLeftSidebarToggle} sx={{ mr: 2 }}>
//                     <Icon icon='mdi:menu' />
//                   </IconButton>
//                 )}
//                 <Box
//                   onClick={handleUserProfileRightSidebarToggle}
//                   sx={{ display: 'flex', alignItems: 'center', cursor: 'pointer' }}
//                 >
//                   <Badge
//                     overlap='circular'
//                     anchorOrigin={{
//                       vertical: 'bottom',
//                       horizontal: 'right'


// ** MUI Imports
import Box from '@mui/material/Box'
import TreeView from '@mui/lab/TreeView'
import { styled } from '@mui/material/styles'
import Typography from '@mui/material/Typography'
import TreeItem from '@mui/lab/TreeItem'

// ** Custom Icon Import
import Icon from 'src/@core/components/icon'

// Styled TreeItem component
const StyledTreeItemRoot = styled(TreeItem)(({ theme }) => ({
  '&:hover > .MuiTreeItem-content:not(.Mui-selected)': {
    backgroundColor: theme.palette.action.hover
  },
  '& .MuiTreeItem-content': {
    paddingRight: theme.spacing(3),
    borderTopRightRadius: theme.spacing(4),
    borderBottomRightRadius: theme.spacing(4),
    fontWeight: theme.typography.fontWeightMedium
  },
  '& .MuiTreeItem-label': {
    fontWeight: 'inherit',
    paddingRight: theme.spacing(3)
  },
  '& .MuiTreeItem-group': {
    marginLeft: 0,
    '& .MuiTreeItem-content': {
      paddingLeft: theme.spacing(4),
      fontWeight: theme.typography.fontWeightRegular
    }
  }
}))

const StyledTreeItem = props => {
  // ** Props
  const { labelText, labelIcon, labelInfo, ...other } = props

  return (
    <StyledTreeItemRoot
      {...other}
      label={
        <Box sx={{ py: 1, display: 'flex', alignItems: 'center', '& svg': { mr: 1 } }}>
          <Icon icon={labelIcon} color='inherit' />
          <Typography variant='body2' sx={{ flexGrow: 1, fontWeight: 'inherit' }}>
            {labelText}
          </Typography>
          {labelInfo ? (
            <Typography variant='caption' color='inherit'>
              {labelInfo}
            </Typography>
          ) : null}
        </Box>
      }
    />
  )
}



const DocumentsDashboard = ({ direction }) => {
  const ExpandIcon = <Icon icon={direction === 'rtl' ? 'mdi:chevron-left' : 'mdi:chevron-right'} />

  return (
    <TreeView
      sx={{ minHeight: 140 }}
      // defaultExpanded={['3']}
      defaultExpandIcon={ExpandIcon}
      defaultCollapseIcon={<Icon icon='mdi:chevron-down' />}
    >
      <StyledTreeItem nodeId='1' labelText='My workspace' labelIcon='mdi:folder'>
        <StyledTreeItem nodeId='8' labelInfo='' labelText='HR Management Issues' labelIcon='mdi:folder'
        onClick="" />
        <StyledTreeItem nodeId='9' labelInfo='' labelText='new25' labelIcon='mdi:folder' />
        <StyledTreeItem nodeId='10' labelInfo='' labelText='My Folder' labelIcon='mdi:folder' >
          <StyledTreeItem nodeId='11' labelInfo='' labelText='My Folder' labelIcon='mdi:folder' />
        </StyledTreeItem>
        <StyledTreeItem nodeId='12' labelInfo='' labelText='test' labelIcon='mdi:folder' />
        <StyledTreeItem nodeId='13' labelInfo='' labelText='Test' labelIcon='mdi:folder' />
      </StyledTreeItem>


      <StyledTreeItem nodeId='2' labelText='New Workspace' labelIcon='mdi:folder'>
        <StyledTreeItem nodeId='14' labelInfo='' labelText='newfk' labelIcon='mdi:folder' />
        <StyledTreeItem nodeId='15' labelInfo='' labelText='Test Workspace' labelIcon='mdi:folder' />
      </StyledTreeItem>
     
      <StyledTreeItem nodeId='3' labelText='Human Resources' labelIcon='mdi:folder'>
        <StyledTreeItem nodeId='16' labelInfo='' labelText='HR Documents' labelIcon='mdi:folder' />
        <StyledTreeItem nodeId='17' labelInfo='' labelText='Employee Documents' labelIcon='mdi:folder' />
        <StyledTreeItem nodeId='18' labelInfo='' labelText='VA Task' labelIcon='mdi:folder' />
        <StyledTreeItem nodeId='19' labelInfo='' labelText='inzi' labelIcon='mdi:folder' />
        <StyledTreeItem nodeId='20' labelInfo='' labelText='VA test 222' labelIcon='mdi:folder' />
        <StyledTreeItem nodeId='21' labelInfo='' labelText='VA HR test !' labelIcon='mdi:folder' />
        <StyledTreeItem nodeId='22' labelInfo='' labelText='new 27july' labelIcon='mdi:folder' />
      </StyledTreeItem>


      <StyledTreeItem nodeId='4' labelText='London' labelIcon='mdi:folder'>
        <StyledTreeItem nodeId='23' labelInfo='' labelText='Workspace' labelIcon='mdi:folder' />
        <StyledTreeItem nodeId='24' labelInfo='' labelText='newwsfkk' labelIcon='mdi:folder' />
        <StyledTreeItem nodeId='25' labelInfo='' labelText='VA 10' labelIcon='mdi:folder' />
        <StyledTreeItem nodeId='26' labelInfo='' labelText='new05' labelIcon='mdi:folder' />
      </StyledTreeItem>


      <StyledTreeItem nodeId='5' labelText='Withoutparentws' labelIcon='mdi:folder' />
      <StyledTreeItem nodeId='6' labelText='newtestparentws' labelIcon='mdi:folder' />
      <StyledTreeItem nodeId='7' labelText='VS Workspace' labelIcon='mdi:folder' />
    </TreeView>
  )
}

export default DocumentsDashboard