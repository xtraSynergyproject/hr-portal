// ** Icon Imports
import Icon from 'src/@core/components/icon'
import MoreHorizRoundedIcon from '@mui/icons-material/MoreHorizRounded';
// ** Custom Components Imports
import OptionsMenu from 'src/@core/components/option-menu'

const MoreDropdown = ({ settings, saveSettings }) => {
 

  const handleLangItemClick = lang => {
  
  }

  return (
    <OptionsMenu
      icon={<MoreHorizRoundedIcon />}
      menuProps={{ sx: { '& .MuiMenu-paper': { mt: 4, minWidth: 130 } } }}
      iconButtonProps={{ color: 'inherit', padding: 'inherit'}}
      options={[
        {
          text: 'Tag',
          menuItemProps: {
            sx: { py: 2 },
            onClick: () => {
              handleLangItemClick('en')
            }
          }
        },
        {
          text: 'Email',
          menuItemProps: {
            sx: { py: 2 },
            onClick: () => {
              handleLangItemClick('fr')
            }
          }
        },
        {
          text: 'Log',
          menuItemProps: {
            sx: { py: 2 },
            onClick: () => {
              handleLangItemClick('ar')
            }
          }
        },
        {
          text: 'Version',
          menuItemProps: {
            sx: { py: 2 },
            onClick: () => {
              handleLangItemClick('ar')
            }
          }
        }
      ]}
    />
  )
}

export default MoreDropdown
